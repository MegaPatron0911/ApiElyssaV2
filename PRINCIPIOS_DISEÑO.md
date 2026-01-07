# ?? Principios de Diseño Implementados

## 1. **Controllers Finos (Thin Controllers)** ?

### Implementación

Los controladores en `Elyssa.PublicApi` siguen el principio de responsabilidad única:

```csharp
[HttpGet("{id:guid}")]
public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
{
    var result = await _companyService.GetByIdAsync(id, cancellationToken);

    return result.Match(
        success => Ok(success),
        error => error.ToHttpResponse()
    );
}
```

**Responsabilidades:**
- ? Validación básica (model binding, route constraints)
- ? Orquestación de servicios
- ? Conversión de Result a respuesta HTTP
- ? Sin lógica de negocio

---

## 2. **Result Pattern** ?

### Ventajas
- Manejo explícito de errores
- No se usan excepciones para flujo de control
- Type-safe error handling
- Mejor experiencia en debugging

### Uso en Servicios

```csharp
public async Task<Result<CompanyDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
{
    var company = await _unitOfWork.Companies.GetByIdAsync(id, cancellationToken);
    
    if (company == null)
        return Result<CompanyDto>.Failure(CompanyErrors.NotFound(id));

    return Result<CompanyDto>.Success(MapToDto(company));
}
```

### Errores Tipados

```csharp
public static class CompanyErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Company.NotFound", $"La compañía con ID '{id}' no fue encontrada");
}
```

---

## 3. **Unit of Work Pattern** ?

### Implementación

```csharp
public interface IUnitOfWork : IDisposable
{
    IRepository<Company> Companies { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
```

### Ventajas
- ? Transacciones consistentes
- ? Un solo `SaveChanges` al final
- ? Múltiples operaciones en una transacción
- ? Rollback automático en caso de error

### Ejemplo de Uso con Transacciones

```csharp
public async Task<Result> ProcessCompanyBatchAsync(List<CompanyDto> companies)
{
    await _unitOfWork.BeginTransactionAsync();
    
    try
    {
        foreach (var dto in companies)
        {
            var company = MapToEntity(dto);
            await _unitOfWork.Companies.AddAsync(company);
        }
        
        await _unitOfWork.CommitTransactionAsync();
        return Result.Success();
    }
    catch
    {
        await _unitOfWork.RollbackTransactionAsync();
        return Result.Failure(Error.Failure("Batch.Failed", "Error al procesar lote"));
    }
}
```

---

## 4. **Repository Pattern** ?

### Interfaz Genérica

```csharp
public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
```

### Ventajas
- ? Abstracción del acceso a datos
- ? Fácil de testear (mocking)
- ? Cambio de ORM sin afectar la lógica de negocio

---

## 5. **Lógica de Negocio en Servicios** ?

### ? Antes (Lógica en Controller)

```csharp
[HttpPost]
public async Task<IActionResult> Create([FromBody] CompanyDto dto)
{
    // ? Validación de negocio en el controller
    if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length < 3)
        return BadRequest("Name too short");
    
    // ? Acceso directo a DbContext
    var company = new Company { ... };
    await _context.Companies.AddAsync(company);
    await _context.SaveChangesAsync();
    
    return Ok(company);
}
```

### ? Ahora (Lógica en Service)

**Controller:**
```csharp
[HttpPost]
public async Task<IActionResult> Create([FromBody] CompanyDto dto, CancellationToken ct)
{
    var result = await _companyService.CreateAsync(dto, ct);
    return result.Match(success => Created(...), error => error.ToHttpResponse());
}
```

**Service:**
```csharp
public async Task<Result<CompanyDto>> CreateAsync(CompanyDto dto, CancellationToken ct)
{
    // ? Validación de negocio
    if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length < 3)
        return Result<CompanyDto>.Failure(CompanyErrors.NameTooShort);

    // ? Lógica de negocio
    var company = new Company { ... };
    
    // ? Uso de Unit of Work
    await _unitOfWork.Companies.AddAsync(company, ct);
    await _unitOfWork.SaveChangesAsync(ct);

    return Result<CompanyDto>.Success(MapToDto(company));
}
```

---

## 6. **Dependency Inversion** ?

### Configuración en Program.cs

```csharp
// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddScoped<ICompanyService, CompanyService>();
```

### Ventajas
- ? Las capas superiores dependen de abstracciones
- ? Fácil cambio de implementaciones
- ? Testeable con mocks

---

## 7. **Middleware Pattern** ?

### Exception Handling Middleware

```csharp
public class ExceptionHandlingMiddleware
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción no controlada");
            await HandleExceptionAsync(context, ex);
        }
    }
}
```

### Company Context Middleware

```csharp
public class CompanyContextMiddleware
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("X-Company-Id", out var value))
        {
            context.Items["CompanyId"] = Guid.Parse(value);
        }
        await _next(context);
    }
}
```

**Uso en BaseController:**
```csharp
protected Guid? CompanyId => HttpContext.Items["CompanyId"] as Guid?;
```

---

## 8. **CancellationToken Support** ?

### Implementación en toda la pila

**Controller:**
```csharp
public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
```

**Service:**
```csharp
public async Task<Result<IEnumerable<CompanyDto>>> GetAllAsync(CancellationToken ct)
```

**Repository:**
```csharp
public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct)
```

**Ventajas:**
- ? Cancelación de requests largos
- ? Liberación de recursos
- ? Mejor performance

---

## 9. **BaseController Pattern** ?

```csharp
public abstract class BaseController : ControllerBase
{
    protected Guid? CompanyId => HttpContext.Items["CompanyId"] as Guid?;
    protected string? UserId => User?.FindFirst("sub")?.Value;
}
```

**Uso:**
```csharp
public class CompaniesController : BaseController
{
    // Acceso directo a CompanyId y UserId
}
```

---

## ?? Comparación: Antes vs Ahora

| Aspecto | ? Antes | ? Ahora |
|---------|----------|----------|
| **Manejo de errores** | try-catch, excepciones | Result Pattern |
| **Acceso a datos** | DbContext directo | Unit of Work + Repository |
| **Lógica de negocio** | En Controllers | En Services |
| **Transacciones** | Manual, dispersas | Unit of Work |
| **Respuestas HTTP** | StatusCode manual | ErrorExtensions + Match |
| **Cancelación** | No soportada | CancellationToken en toda la pila |
| **Contexto global** | Request.Headers | HttpContext.Items + Middleware |

---

## ?? Flujo de una Request

```
1. HTTP Request ? CompanyContextMiddleware
   ? (Extrae X-Company-Id)
   
2. ExceptionHandlingMiddleware
   ? (try-catch global)
   
3. Controller (Thin)
   ? (Solo orquestación)
   
4. Service (Business Logic)
   ? (Validaciones, transformaciones)
   
5. Unit of Work
   ? (Coordinación de repositorios)
   
6. Repository
   ? (Acceso a datos)
   
7. DbContext ? Database
```

---

## ? Checklist de Principios

- [x] **Thin Controllers** - Solo orquestación
- [x] **Result Pattern** - Manejo explícito de errores
- [x] **Unit of Work** - Transacciones consistentes
- [x] **Repository Pattern** - Abstracción de datos
- [x] **Business Logic en Services** - Separación de responsabilidades
- [x] **Dependency Inversion** - Interfaces sobre implementaciones
- [x] **Middleware Pattern** - Cross-cutting concerns
- [x] **CancellationToken** - Soporte de cancelación
- [x] **BaseController** - Reutilización de código común
- [x] **Error Extensions** - Conversión automática a HTTP
