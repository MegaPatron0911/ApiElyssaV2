# ?? Ejemplos de Uso

## Tabla de Contenido
- [Controllers](#controllers)
- [Services](#services)
- [Unit of Work](#unit-of-work)
- [Result Pattern](#result-pattern)
- [Middleware](#middleware)

---

## Controllers

### ? Ejemplo Correcto - Thin Controller

```csharp
[HttpGet("{id:guid}")]
public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
{
    // ? Solo orquestación, sin lógica de negocio
    var result = await _companyService.GetByIdAsync(id, cancellationToken);

    // ? Pattern matching para conversión automática
    return result.Match(
        success => Ok(success),
        error => error.ToHttpResponse()
    );
}
```

### ? Ejemplo Incorrecto - Fat Controller

```csharp
[HttpGet("{id}")]
public async Task<IActionResult> GetById(Guid id)
{
    // ? Acceso directo a DbContext
    var company = await _context.Companies.FindAsync(id);
    
    // ? Lógica de negocio en el controller
    if (company == null) return NotFound();
    if (!company.IsActive) return Forbid();
    
    // ? Transformación manual
    var dto = new CompanyDto { ... };
    
    return Ok(dto);
}
```

---

## Services

### ? Lógica de Negocio con Validaciones

```csharp
public async Task<Result<CompanyDto>> CreateAsync(
    CompanyDto companyDto, 
    CancellationToken cancellationToken)
{
    // ? Validación de negocio específica
    if (string.IsNullOrWhiteSpace(companyDto.Name) || companyDto.Name.Length < 3)
        return Result<CompanyDto>.Failure(CompanyErrors.NameTooShort);

    // ? Validación de duplicados
    var existingCompany = await _unitOfWork.Companies
        .GetAllAsync(cancellationToken)
        .FirstOrDefaultAsync(c => c.Email == companyDto.Email);
    
    if (existingCompany != null)
        return Result<CompanyDto>.Failure(CompanyErrors.EmailAlreadyExists(companyDto.Email));

    // ? Creación de entidad
    var company = new Company
    {
        Id = Guid.NewGuid(),
        Name = companyDto.Name.Trim(),
        Description = companyDto.Description,
        Email = companyDto.Email.ToLowerInvariant(),
        Phone = companyDto.Phone,
        IsActive = true
    };

    // ? Uso de Unit of Work
    await _unitOfWork.Companies.AddAsync(company, cancellationToken);
    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return Result<CompanyDto>.Success(MapToDto(company));
}
```

---

## Unit of Work

### ? Operaciones con Transacciones

```csharp
public async Task<Result> ImportCompaniesAsync(
    List<CompanyDto> companies, 
    CancellationToken cancellationToken)
{
    // Iniciar transacción
    await _unitOfWork.BeginTransactionAsync(cancellationToken);
    
    try
    {
        foreach (var dto in companies)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(dto.Name))
                return Result.Failure(CompanyErrors.NameTooShort);

            var company = new Company
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                IsActive = true
            };

            await _unitOfWork.Companies.AddAsync(company, cancellationToken);
        }

        // Commit solo si todo fue exitoso
        await _unitOfWork.CommitTransactionAsync(cancellationToken);
        
        return Result.Success();
    }
    catch (Exception ex)
    {
        // Rollback automático en caso de error
        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        return Result.Failure(Error.Failure("Import.Failed", ex.Message));
    }
}
```

### ? Múltiples Repositorios en una Transacción

```csharp
public async Task<Result> CreateCompanyWithUsersAsync(
    CompanyDto companyDto,
    List<UserDto> users,
    CancellationToken cancellationToken)
{
    await _unitOfWork.BeginTransactionAsync(cancellationToken);
    
    try
    {
        // Crear compañía
        var company = new Company { ... };
        await _unitOfWork.Companies.AddAsync(company, cancellationToken);

        // Crear usuarios asociados
        foreach (var userDto in users)
        {
            var user = new User 
            { 
                CompanyId = company.Id,
                ...
            };
            await _unitOfWork.Users.AddAsync(user, cancellationToken);
        }

        // Commit de todo junto
        await _unitOfWork.CommitTransactionAsync(cancellationToken);
        
        return Result.Success();
    }
    catch
    {
        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        throw;
    }
}
```

---

## Result Pattern

### ? Creación de Errores Personalizados

```csharp
public static class CompanyErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound(
            "Company.NotFound",
            $"La compañía con ID '{id}' no fue encontrada");

    public static Error EmailAlreadyExists(string email) =>
        Error.Conflict(
            "Company.EmailConflict",
            $"Ya existe una compañía con el email '{email}'");

    public static Error InvalidEmail(string email) =>
        Error.Validation(
            "Company.InvalidEmail",
            $"El email '{email}' no es válido");

    public static Error NameTooShort =>
        Error.Validation(
            "Company.NameTooShort",
            "El nombre debe tener al menos 3 caracteres");
}
```

### ? Uso de Match para Transformaciones

```csharp
// En Controller
var result = await _companyService.GetByIdAsync(id, cancellationToken);

return result.Match(
    success => Ok(success),
    error => error.ToHttpResponse()
);

// En Service (transformación de resultado)
var companyResult = await GetCompanyAsync(id);

return companyResult.Match(
    company => Result<CompanyDetailsDto>.Success(MapToDetails(company)),
    error => Result<CompanyDetailsDto>.Failure(error)
);
```

### ? Composición de Results

```csharp
public async Task<Result<CompanyWithStatsDto>> GetCompanyWithStatsAsync(
    Guid id,
    CancellationToken cancellationToken)
{
    // Obtener compañía
    var companyResult = await GetByIdAsync(id, cancellationToken);
    
    if (!companyResult.IsSuccess)
        return Result<CompanyWithStatsDto>.Failure(companyResult.Error!);

    var company = companyResult.Value!;

    // Calcular estadísticas
    var stats = await CalculateStatsAsync(id, cancellationToken);

    var dto = new CompanyWithStatsDto
    {
        Company = company,
        TotalUsers = stats.UserCount,
        ActiveProjects = stats.ProjectCount
    };

    return Result<CompanyWithStatsDto>.Success(dto);
}
```

---

## Middleware

### ? Middleware de Contexto

```csharp
public class CompanyContextMiddleware
{
    private readonly RequestDelegate _next;
    private const string CompanyIdHeader = "X-Company-Id";

    public CompanyContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Extraer Company ID del header
        if (context.Request.Headers.TryGetValue(CompanyIdHeader, out var companyIdValue))
        {
            if (Guid.TryParse(companyIdValue, out var companyId))
            {
                // Almacenar en HttpContext.Items
                context.Items["CompanyId"] = companyId;
            }
        }

        await _next(context);
    }
}
```

### ? Uso en BaseController

```csharp
public abstract class BaseController : ControllerBase
{
    protected Guid? CompanyId
    {
        get
        {
            if (HttpContext.Items.TryGetValue("CompanyId", out var companyId) 
                && companyId is Guid id)
            {
                return id;
            }
            return null;
        }
    }

    protected string? UserId => User?.FindFirst("sub")?.Value;
}

// Uso en controller derivado
public class CompaniesController : BaseController
{
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrent(CancellationToken cancellationToken)
    {
        if (CompanyId == null)
            return BadRequest("Company ID no proporcionado");

        var result = await _companyService.GetByIdAsync(CompanyId.Value, cancellationToken);
        
        return result.Match(
            success => Ok(success),
            error => error.ToHttpResponse()
        );
    }
}
```

---

## Validaciones

### ? Validaciones en DTO (DataAnnotations)

```csharp
public class CompanyDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    public string Email { get; set; } = string.Empty;
    
    [Phone(ErrorMessage = "El teléfono no es válido")]
    public string Phone { get; set; } = string.Empty;
}
```

### ? Validaciones de Negocio en Service

```csharp
public async Task<Result<CompanyDto>> CreateAsync(
    CompanyDto companyDto,
    CancellationToken cancellationToken)
{
    // Validación de negocio: nombre único
    var existingByName = await _unitOfWork.Companies
        .GetAllAsync(cancellationToken)
        .FirstOrDefaultAsync(c => c.Name.ToLower() == companyDto.Name.ToLower());

    if (existingByName != null)
        return Result<CompanyDto>.Failure(
            Error.Conflict("Company.NameExists", "Ya existe una compañía con ese nombre"));

    // Validación de negocio: email único
    var existingByEmail = await _unitOfWork.Companies
        .GetAllAsync(cancellationToken)
        .FirstOrDefaultAsync(c => c.Email == companyDto.Email);

    if (existingByEmail != null)
        return Result<CompanyDto>.Failure(CompanyErrors.EmailAlreadyExists(companyDto.Email));

    // Continuar con la creación...
}
```

---

## CancellationToken

### ? Propagación en toda la pila

```csharp
// Controller
[HttpGet]
public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
{
    var result = await _companyService.GetAllAsync(cancellationToken);
    return result.Match(success => Ok(success), error => error.ToHttpResponse());
}

// Service
public async Task<Result<IEnumerable<CompanyDto>>> GetAllAsync(
    CancellationToken cancellationToken)
{
    var companies = await _unitOfWork.Companies.GetAllAsync(cancellationToken);
    return Result<IEnumerable<CompanyDto>>.Success(companies.Select(MapToDto));
}

// Repository
public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
{
    return await _dbSet.ToListAsync(cancellationToken);
}
```

### ? Operaciones largas con progreso

```csharp
public async Task<Result> ProcessLargeBatchAsync(
    List<CompanyDto> companies,
    IProgress<int> progress,
    CancellationToken cancellationToken)
{
    var total = companies.Count;
    var processed = 0;

    foreach (var dto in companies)
    {
        // Verificar cancelación
        cancellationToken.ThrowIfCancellationRequested();

        await _unitOfWork.Companies.AddAsync(MapToEntity(dto), cancellationToken);
        
        processed++;
        progress?.Report((processed * 100) / total);
    }

    await _unitOfWork.SaveChangesAsync(cancellationToken);
    return Result.Success();
}
```

---

## ?? Testing

### ? Mock de IUnitOfWork

```csharp
[Test]
public async Task GetByIdAsync_WhenCompanyExists_ReturnsSuccess()
{
    // Arrange
    var mockUnitOfWork = new Mock<IUnitOfWork>();
    var company = new Company { Id = Guid.NewGuid(), Name = "Test" };
    
    mockUnitOfWork
        .Setup(x => x.Companies.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(company);

    var service = new CompanyService(mockUnitOfWork.Object);

    // Act
    var result = await service.GetByIdAsync(company.Id, CancellationToken.None);

    // Assert
    Assert.IsTrue(result.IsSuccess);
    Assert.AreEqual("Test", result.Value.Name);
}
```
