# AGENTS.md

Guía esencial para el desarrollo y colaboración en la API Elyssa V2. Este documento define las normas, arquitectura y flujos de trabajo estrictos para mantener la calidad y coherencia del proyecto.

## 1. Contexto del Proyecto

El proyecto es una solución **ASP.NET Core** que implementa una arquitectura limpia (Clean Architecture).

- **Solución Principal**: `ApiElyssaV2.sln`
- **Versión .NET**: .NET 10 (Target Framework `net10.0`)
- **Estructura de Directorios**:
  - `ApiElyssaV2/` (Interfaz Web API): Controladores, Middlewares, Filtros y Configuración de inicio.
  - `Core/` (Núcleo): Entidades de Dominio, Interfaces, DTOs, Validaciones, Servicios de Dominio y Especificaciones. **Sin dependencias externas.**
  - `Infrastructure/` (Infraestructura): Implementación de repositorios, EF Core, y adaptadores para servicios externos.
  - `Tests/`: Pruebas unitarias e integración. `Core.Tests`, `Infrastructure.Tests`, `Integration.Tests`.

## 2. Tecnologías y Librerías Clave

- **ORM**: Entity Framework Core (con PostgreSQL npgsql).
- **Mapeo**: AutoMapper (ver `Core/Mappings`).
- **Validación**: FluentValidation (ver `Core/Validators`).
- **Patrones**: Repository Pattern, Specification Pattern (Ardalis.Specification), Dependency Injection.
- **Logging**: Serilog.
- **Documentación API**: Swagger/OpenAPI.

## 3. Flujo de Trabajo

### Compilación y Ejecución
- **Restaurar paquetes**: `dotnet restore`
- **Compilar solución**: `dotnet build ApiElyssaV2.sln`
- **Ejecutar API**: `dotnet run --project ApiElyssaV2/ApiElyssaV2/Elyssa.PublicApi.csproj` (o desde Visual Studio/IDE).

### Pruebas (Obligatorio)
No se acepta código sin pruebas verdes.
- **Ejecutar todos los tests**: `dotnet test ApiElyssaV2.sln`
- **Cobertura requerida**:
  - Lógica de negocio en `Core`: Unit Tests.
  - Repositorios/Integraciones en `Infrastructure`: Integration/Unit Tests.

## 4. Guías de Estilo y Arquitectura

### Clean Architecture
1.  **Dependencias**: `Core` no debe depender de nadie. `Infrastructure` y `ApiElyssaV2` dependen de `Core`.
2.  **Lógica de Negocio**: Debe residir EXCLUSIVAMENTE en `Core` (Servicios o Entidades), nunca en los Controladores.
3.  **Controladores**: Deben ser "finos" (Thin Controllers). Su única responsabilidad es recibir peticiones, validar (delegando), llamar a servicios/mediadores y devolver una respuesta HTTP adecuada.

### Convenciones de Código
- **Naming**: `PascalCase` para clases y métodos públicos, `camelCase` para variables locales y campos privados.
- **Async**: Todo I/O (Base de datos, HTTP calls) debe ser asíncrono (`async/await`) usando `CancellationToken`.
- **Inyección de Dependencias**: Usar interfaces para inyectar servicios. No instanciar clases concretas de infraestructura en la lógica de negocio.

### Manejo de Datos
- **DTOs vs Entidades**: Nunca exponer Entidades de dominio directamente en la API. Usar DTOs definidos en `Core/DTOs`.
- **AutoMapper**: Usar perfiles de mapeo para transformar Entidades <-> DTOs. Actualizar `Core/Mappings` al crear nuevos modelos.
- **Secretos**: **PROHIBIDO** commitear claves, tokens o cadenas de conexión reales. Usar `User Secrets` en desarrollo y variables de entorno en producción.

## 5. Checklist para Agentes

Antes de dar una tarea por finalizada, verifica:

1.  [ ] La solución compila sin errores (`dotnet build ApiElyssaV2.sln`).
2.  [ ] Las nuevas funcionalidades tienen pruebas unitarias o de integración y TODAS pasan (`dotnet test`).
3.  [ ] Se han respetado las capas de arquitectura (no hay lógica de DB en controladores, etc.).
4.  [ ] No se han introducido "Magic Strings" o secretos en el código fuente.
5.  [ ] Si se cambió el modelo de datos, se han actualizado los DTOs y Mappings correspondientes.
6.  [ ] El código es limpio, formateado y sigue las convenciones de nombres de C#.

---
*Este documento es la fuente de verdad para el estilo y normas del proyecto. Síguelo estrictamente.*
