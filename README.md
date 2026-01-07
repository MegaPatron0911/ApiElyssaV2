# ApiElyssaV2 - Arquitectura en Capas con Clean Architecture

## Estructura del Proyecto

Este proyecto sigue una arquitectura en capas limpia (Clean Architecture) con principios SOLID y patrones de diseño modernos.

### ?? Elyssa.PublicApi
**Capa de Presentación** - API Web que expone los endpoints HTTP

```
Elyssa.PublicApi/
??? Controllers/          # Thin Controllers (solo orquestación)
?   ??? BaseController.cs
?   ??? CompaniesController.cs
?   ??? WeatherForecastController.cs
??? Middleware/          # Cross-cutting concerns
?   ??? ExceptionHandlingMiddleware.cs
?   ??? CompanyContextMiddleware.cs
??? Extensions/          # Extensiones HTTP
?   ??? ErrorExtensions.cs
??? Shared/              # Utilidades compartidas
?   ??? Constants/
?   ??? Extensions/
?   ??? Helpers/
??? Program.cs           # Configuración y DI
```

**Responsabilidades:**
- Manejar requests y responses HTTP
- Validación de entrada (DataAnnotations)
- Orquestación de servicios
- Conversión de Result a IActionResult

---

### ?? Elyssa.Core
**Capa de Dominio** - Lógica de negocio y reglas del dominio

```
Core/
??? Domain/              # Entidades y objetos de valor
?   ??? Entities/        # Entidades del dominio
?   ?   ??? BaseEntity.cs
?   ?   ??? Company.cs
?   ??? ValueObjects/    # Objetos de valor inmutables
?       ??? Address.cs
??? Services/            # Servicios de aplicación (Lógica de Negocio)
?   ??? CompanyService.cs
??? Interfaces/          # Contratos (abstracciones)
?   ??? IRepository.cs
?   ??? ICompanyService.cs
?   ??? IUnitOfWork.cs
??? DTOs/                # Data Transfer Objects
?   ??? CompanyDto.cs
??? Common/              # Result Pattern y Errores
    ??? Result.cs
    ??? Error.cs
    ??? CompanyErrors.cs
```

**Responsabilidades:**
- Definir entidades del dominio
- Lógica de negocio y validaciones
- Interfaces para inversión de dependencias
- DTOs para transferencia de datos
- Result Pattern para manejo de errores

---

### ?? Elyssa.Infrastructure
**Capa de Infraestructura** - Implementaciones técnicas

```
Infrastructure/
??? Data/                # Contexto de Entity Framework Core
?   ??? ApplicationDbContext.cs
??? Repositories/        # Implementaciones de repositorios
?   ??? Repository.cs (genérico)
?   ??? CompanyRepository.cs
?   ??? UnitOfWork.cs
??? External/            # Servicios externos de terceros
?   ??? ExternalApiClient.cs
??? Security/            # Autenticación y autorización
    ??? PasswordHasher.cs
```

**Responsabilidades:**
- Acceso a datos (Entity Framework Core)
- Implementación de repositorios y Unit of Work
- Integración con APIs externas
- Seguridad y criptografía

---

## ?? Dependencias entre Capas

```
Elyssa.PublicApi
    ?
    ??? Elyssa.Core (Interfaces y DTOs)
    ??? Elyssa.Infrastructure (Implementaciones)
            ?
        Elyssa.Core (Interfaces)
```

**Principios:**
- ? PublicApi depende de Core e Infrastructure
- ? Infrastructure depende de Core (solo interfaces)
- ? Core no depende de nadie (capa independiente)
- ? Dependency Inversion Principle aplicado

---

## ?? Patrones de Diseño Implementados

### 1. **Result Pattern**
Manejo explícito de errores sin excepciones:
```csharp
var result = await _companyService.GetByIdAsync(id, cancellationToken);
return result.Match(
    success => Ok(success),
    error => error.ToHttpResponse()
);
```

### 2. **Unit of Work Pattern**
Transacciones consistentes y coordinación de repositorios:
```csharp
await _unitOfWork.BeginTransactionAsync();
await _unitOfWork.Companies.AddAsync(company);
await _unitOfWork.CommitTransactionAsync();
```

### 3. **Repository Pattern**
Abstracción del acceso a datos:
```csharp
public interface IRepository<T> where T : BaseEntity { }
```

### 4. **Middleware Pattern**
Cross-cutting concerns (logging, error handling):
```csharp
app.UseExceptionHandlingMiddleware();
app.UseCompanyContext();
```

### 5. **Thin Controllers**
Controllers solo para orquestación, sin lógica de negocio.

---

## ?? Tecnologías Utilizadas

- **.NET 8.0**
- **ASP.NET Core Web API**
- **Entity Framework Core 8.0**
- **Swagger/OpenAPI**
- **SQL Server**

---

## ?? Comandos Útiles

### Restaurar dependencias
```bash
dotnet restore
```

### Compilar el proyecto
```bash
dotnet build
```

### Ejecutar la API
```bash
dotnet run --project ApiElyssaV2/Elyssa.PublicApi.csproj
```

### Agregar migraciones de EF Core
```bash
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project ApiElyssaV2
```

### Actualizar base de datos
```bash
dotnet ef database update --project Infrastructure --startup-project ApiElyssaV2
```

---

## ?? Características Implementadas

- ? **Result Pattern** para manejo de errores
- ? **Unit of Work** para transacciones
- ? **Repository Pattern** genérico
- ? **Thin Controllers** (sin lógica de negocio)
- ? **Middleware** para exception handling
- ? **CancellationToken** en toda la pila
- ? **BaseController** con contexto compartido
- ? **Error Extensions** para conversión HTTP
- ? **Principios SOLID** aplicados

---

## ?? Documentación Adicional

- [PRINCIPIOS_DISEÑO.md](PRINCIPIOS_DISEÑO.md) - Patrones y principios implementados
- [SETUP.md](SETUP.md) - Guía de configuración paso a paso
- [ARQUITECTURA.md](ARQUITECTURA.md) - Diagramas y estructura detallada

---

## ??? Principios SOLID Aplicados

### Single Responsibility Principle (SRP) ?
- Cada capa tiene una responsabilidad específica
- Controllers: Solo orquestación
- Services: Solo lógica de negocio
- Repositories: Solo acceso a datos

### Open/Closed Principle (OCP) ?
- Uso de interfaces para extensibilidad
- Result Pattern extensible con nuevos tipos de error

### Liskov Substitution Principle (LSP) ?
- Repository genérico con BaseEntity
- Cualquier implementación de IRepository es intercambiable

### Interface Segregation Principle (ISP) ?
- Interfaces específicas y cohesivas
- ICompanyService, IRepository, IUnitOfWork

### Dependency Inversion Principle (DIP) ?
- Las capas superiores dependen de abstracciones
- Inyección de dependencias en toda la aplicación

---

## ?? Flujo de una Request

```
HTTP Request
    ?
CompanyContextMiddleware (extrae headers)
    ?
ExceptionHandlingMiddleware (try-catch global)
    ?
Controller (orquestación)
    ?
Service (lógica de negocio)
    ?
Unit of Work (coordinación)
    ?
Repository (acceso a datos)
    ?
DbContext ? Database
