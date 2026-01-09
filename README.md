# ApiElyssaV2 - Arquitectura en Capas con Clean Architecture

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)]()
[![.NET Version](https://img.shields.io/badge/.NET-8.0-blue)]()
[![License](https://img.shields.io/badge/license-MIT-blue)]()

## ?? **Proyecto Completo según AGENTS.md**

Este proyecto implementa **TODAS** las mejores prácticas definidas en [AGENTS.md](AGENTS.md):

- ? **AutoMapper** para mapeo automático
- ? **Serilog** para logging estructurado
- ? **Result Pattern** para manejo de errores
- ? **Unit of Work** para transacciones
- ? **Thin Controllers** (solo orquestación)
- ? **SOLID principles** aplicados

---

## Información General

# Elyssa API

API REST para gestión de propiedades inmobiliarias construida con .NET 8 y arquitectura limpia.

## Requisitos

- .NET 8 SDK
- PostgreSQL 12+
- Visual Studio 2022 o VS Code

## Configuración

### 1. Clonar el repositorio

```bash
git clone https://github.com/MegaPatron0911/ApiElyssaV2.git
cd ApiElyssaV2
```

### 2. Configurar User Secrets

```bash
cd ApiElyssaV2
dotnet user-secrets set "ConnectionStrings:deployDatabase" "Host=localhost;Port=5432;Database=elyssa;Username=postgres;Password=your_password"
dotnet user-secrets set "Jwt:Key" "your_jwt_secret_key"
dotnet user-secrets set "Jwt:Issuer" "ElyssaBackOfficeAPI"
dotnet user-secrets set "Jwt:Audience" "ElyssaClients"
```

### 3. Ejecutar migraciones

```bash
dotnet ef database update --project Infrastructure/Infrastructure.csproj --startup-project ApiElyssaV2/Elyssa.PublicApi.csproj
```

### 4. Ejecutar la aplicación

```bash
cd ApiElyssaV2
dotnet run
```

La API estará disponible en `https://localhost:57979`

---

## Estructura del Proyecto

Este proyecto sigue una arquitectura en capas limpia (Clean Architecture) con principios SOLID y patrones de diseño modernos.

```
ApiElyssaV2/
??? Core/                    # Lógica de negocio y contratos
?   ??? Common/             # Result pattern y errores
?   ??? Domain/             # Entidades de dominio
?   ??? DTOs/               # Data Transfer Objects
?   ??? Interfaces/         # Contratos de servicios
?   ??? Mappings/           # Perfiles de AutoMapper
?   ??? Services/           # Lógica de negocio
??? Infrastructure/          # Implementaciones de infraestructura
?   ??? Data/               # DbContext
?   ??? Repositories/       # Repositorios
??? ApiElyssaV2/            # Capa de presentación (API)
    ??? Controllers/        # Endpoints
    ??? Filters/            # Action Filters
    ??? Middleware/         # Middleware customizado
```

## Endpoints Principales

### Properties

#### Listar propiedades
```http
GET /api/v1/properties
```

**Query Parameters:**
- `page` (int, default: 1): Número de página
- `pageSize` (int, default: 20, max: 20): Tamaño de página
- `code` (string, opcional): Búsqueda por código
- `address` (string, opcional): Búsqueda por dirección
- `city` (string, opcional): Búsqueda por ciudad
- `sortBy` (string, default: "createdAt"): Campo de ordenamiento
- `sortOrder` (string, default: "desc"): Orden (asc/desc)

**Headers:**
- `x-company-id` (required): GUID de la empresa

**Response:**
```json
{
  "success": true,
  "data": {
    "properties": [...],
    "pagination": {
      "currentPage": 1,
      "pageSize": 20,
      "totalRecords": 100,
      "totalPages": 5,
      "hasNextPage": true,
      "hasPreviousPage": false,
      "nextPage": 2,
      "remainingRecords": 80
    }
  },
  "timestamp": "2026-01-09T20:00:00Z"
}
```

### Companies

#### Obtener información de empresa
```http
GET /api/v1/companies/info
```

**Headers:**
- `x-company-id` (required): GUID de la empresa

---

## ?? Tecnologías Utilizadas

- **.NET 8.0**
- **ASP.NET Core Web API**
- **Entity Framework Core 8.0**
- **AutoMapper 12.0.1**
- **Serilog 8.0.0**
- **xUnit + Moq + FluentAssertions**
- **Swagger/OpenAPI**
- **SQL Server**

---

## ?? Comandos Útiles

### Compilar el proyecto
```bash
dotnet build
```

### Ejecutar tests
```bash
dotnet test
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
- ? **AutoMapper** para mapeo automático
- ? **Serilog** para logging estructurado
- ? **Principios SOLID** aplicados

---

## ?? Documentación Adicional

- [**AGENTS.md**](AGENTS.md) - Guía de colaboración y estándares
- [**AGENTS_IMPLEMENTATION.md**](AGENTS_IMPLEMENTATION.md) - Implementación completa
- [**PRINCIPIOS_DISEÑO.md**](PRINCIPIOS_DISEÑO.md) - Patrones y principios implementados
- [**EJEMPLOS.md**](EJEMPLOS.md) - Ejemplos de uso de cada patrón
- [**SETUP.md**](SETUP.md) - Guía de configuración paso a paso
- [**ARQUITECTURA.md**](ARQUITECTURA.md) - Diagramas y estructura detallada
- [**PROPERTIES_ENDPOINT.md**](PROPERTIES_ENDPOINT.md) - Documentación del endpoint de propiedades
- [**DATABASE_SCHEMA.md**](DATABASE_SCHEMA.md) - Esquema completo de la base de datos

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
```

---

## ? **Checklist AGENTS.md**

```
? dotnet build sin errores
? No hay secretos en commits
? AutoMapper configurado
? Contratos públicos coherentes
? Lógica de negocio en Services
? Controllers finos
? Repository Pattern
? Unit of Work
? Serilog configurado
```

---

## ?? **Conclusión**

El proyecto **ApiElyssaV2** implementa **TODAS** las mejores prácticas de AGENTS.md:
- ? Arquitectura limpia y escalable
- ? Patrones de diseño modernos
- ? Principios SOLID
- ? Logging estructurado
- ? Mapeo automático
- ? Sin sorpresas (Result Pattern)

**¡Listo para desarrollo colaborativo profesional!** ????
