# ApiElyssaV2 - Arquitectura en Capas

## Estructura del Proyecto

Este proyecto sigue una arquitectura en capas limpia (Clean Architecture) con las siguientes bibliotecas de clases:

### ?? Elyssa.PublicApi
**Capa de Presentación** - API Web que expone los endpoints HTTP

```
Elyssa.PublicApi/
??? Controllers/          # Controladores API REST
?   ??? WeatherForecastController.cs
??? Shared/              # Utilidades compartidas
?   ??? Constants/       # Constantes de la aplicación
?   ?   ??? AppConstants.cs
?   ??? Extensions/      # Métodos de extensión
?   ?   ??? DateTimeExtensions.cs
?   ??? Helpers/         # Clases auxiliares
?       ??? StringHelper.cs
??? Program.cs           # Punto de entrada de la aplicación
```

**Responsabilidades:**
- Manejar requests y responses HTTP
- Validación de entrada
- Configuración de middleware
- Swagger/OpenAPI documentation

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
??? Services/            # Servicios de aplicación
?   ??? CompanyService.cs
??? Interfaces/          # Contratos (abstracciones)
?   ??? IRepository.cs
?   ??? ICompanyService.cs
??? DTOs/                # Data Transfer Objects
    ??? CompanyDto.cs
```

**Responsabilidades:**
- Definir entidades del dominio
- Lógica de negocio
- Interfaces para inversión de dependencias
- DTOs para transferencia de datos

---

### ?? Elyssa.Infrastructure
**Capa de Infraestructura** - Implementaciones técnicas

```
Infrastructure/
??? Data/                # Contexto de Entity Framework Core
?   ??? ApplicationDbContext.cs
??? Repositories/        # Implementaciones de repositorios
?   ??? Repository.cs
?   ??? CompanyRepository.cs
??? External/            # Servicios externos de terceros
?   ??? ExternalApiClient.cs
??? Security/            # Autenticación y autorización
    ??? PasswordHasher.cs
```

**Responsabilidades:**
- Acceso a datos (Entity Framework Core)
- Implementación de repositorios
- Integración con APIs externas
- Seguridad y criptografía

---

## ?? Dependencias entre Capas

```
Elyssa.PublicApi
    ?
    ??? Elyssa.Core
    ??? Elyssa.Infrastructure
            ?
        Elyssa.Core
```

**Principios:**
- ? PublicApi depende de Core e Infrastructure
- ? Infrastructure depende de Core
- ? Core no depende de nadie (capa independiente)

---

## ?? Tecnologías Utilizadas

- **.NET 8.0**
- **ASP.NET Core Web API**
- **Entity Framework Core 8.0**
- **Swagger/OpenAPI**
- **SQL Server** (como proveedor de base de datos)

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

## ?? Próximos Pasos

1. Configurar la cadena de conexión en `appsettings.json`
2. Implementar la lógica en `CompanyService`
3. Crear controladores para las entidades del dominio
4. Agregar autenticación JWT
5. Implementar middleware de manejo de errores
6. Agregar logging
7. Configurar CORS

---

## ??? Patrones Implementados

- **Repository Pattern**: Abstracción del acceso a datos
- **Dependency Injection**: Inversión de control
- **DTO Pattern**: Separación entre entidades y modelos de API
- **Clean Architecture**: Separación de responsabilidades por capas
