# AGENTS.md

Guía para colaborar en `refactoring-elyssa` con buenas prácticas, foco en calidad y sin sorpresas.

## Contexto técnico

- Solución .NET:
  - API BackOffice ASP.NET Core 6 (`BackOfficeElyssa/`)
  - Librería de dominio (`Core/`)
  - Infraestructura EF Core PostgreSQL (`Infraestructure/`)
  - Cliente Nuwwe (`NuwweService/`)
  - Azure Functions .NET 8 (`ProcessPropertyFunction/`, `UpdatePropertyFunction/`, `NuwweTokenRefresher/`)
- SDKs:
  - .NET 6 para API / librerías / tests
  - .NET 8 para Functions v4
- Principales librerías: AutoMapper, Ardalis.Specification, EF Core Npgsql, Serilog, Durable Functions.
- Config sensible: `appsettings*.json` y `local.settings.json` contienen claves de BD, SendGrid, Google Geocoding y Nuwwe. **No versionar valores reales.**

## Flujo de trabajo

- Build inicial:
  - `dotnet restore`
  - `dotnet build BackOfficeElyssa.sln`
- Pruebas locales:
  - `dotnet test BackOfficeElyssa.sln`
  - Añade / actualiza tests al modificar lógica de dominio o servicios.
- Estilo C#:
  - PascalCase para públicos, camelCase para privados.
  - Respeta nullability, DI y patrones de especificación/repositorio existentes.
- API y contratos:
  - Si cambias contratos, actualiza DTOs, perfiles AutoMapper y swagger (en `Program.cs`).
- Azure Functions:
  - Prueba triggers HTTP/locales con `func start` cuando aplique.
- Datos y migraciones:
  - EF Core apunta a PostgreSQL.
  - No ejecutar migraciones contra producción desde local.
  - No dejar cadenas de conexión ni claves en el código.

## Buenas prácticas de código

- Arquitectura:
  - Controllers finos: validación + orquestación.
  - Lógica de negocio en servicios / dominio (`Core/`).
  - Infraestructura (repos, EF, Nuwwe, etc.) desacoplada de dominio.
- SOLID y Clean Code:
  - Single Responsibility: métodos y clases con una responsabilidad clara.
  - Dependency Inversion: programar contra interfaces en `Core/Interfaces`.
  - Métodos cortos, nombres descriptivos, sin “magia”.
  - DRY: extraer lógica repetida a servicios / helpers / extensiones.
- Async/await:
  - Preferir APIs async, evitar `.Result` / `.Wait()`.
  - Usar `CancellationToken` cuando esté disponible.
- Validación y errores:
  - Validar entradas en controladores y servicios.
  - Respuestas HTTP correctas para casos felices y de error.
  - Para datos externos (Nuwwe), manejar fallos transitorios con reintentos controlados.
- Logging:
  - Usar `ILogger` / Serilog.
  - Mensajes claros, sin PII ni secretos.
  - Mantener coherencia con la telemetría existente (Application Insights).

## Testing (obligatorio)

- Ubicación:
  - Tests de dominio: `Tests/Core.Tests`
  - Tests de infraestructura: `Tests/Infraestructure.Tests`
- Enfoque:
  - Unit tests para lógica de dominio y servicios (xUnit + Moq).
  - Tests de integración para endpoints (WebApplicationFactory) cuando tenga sentido.
  - Cubre casos felices, de error y bordes relevantes.
- Comando estándar:
  - `dotnet test BackOfficeElyssa.sln`
- Regla:
  - No dar una tarea por terminada si hay tests fallando.

## Convenciones específicas

- AutoMapper:
  - Al añadir entidades / DTOs, actualizar perfiles en `Core/Mappings`.
- Especificaciones / repos:
  - Para consultas complejas, crear especificaciones Ardalis en lugar de lógica ad-hoc.
  - Mantener coherencia en repos genéricos y patrones de consulta.
- Azure Functions:
  - Bindings y settings en `local.settings.json` de cada Function.
  - Mantener configuración para .NET 8 Functions v4 (por ejemplo, flags necesarios en config).
- Controladores:
  - Rutas REST claras.
  - Validan modelos y delegan a servicios (`Core/Services`).
  - No mezclar lógica de negocio en controladores.

## Seguridad y datos

- Secrets:
  - Nunca subir secretos, dumps de BD ni tokens.
  - Usar `dotnet user-secrets` o variables de entorno para desarrollo local.
- Config:
  - Revisar usos de SendGrid, Google, Nuwwe y JWT.
  - Claves solo a través de configuración segura (user-secrets, KeyVault, env vars).
- Logs compartidos:
  - Redactar PII, tokens y datos sensibles antes de compartir.

## Checklist antes de finalizar

Antes de considerar una tarea como “lista”:

- `dotnet build BackOfficeElyssa.sln` sin errores relevantes.
- `dotnet test BackOfficeElyssa.sln` pasa todos los tests relevantes.
- No se han introducido secretos ni cadenas sensibles en commits.
- Mapeos AutoMapper, contratos públicos y migraciones de BD siguen siendo coherentes.
- Si el cambio afecta a integraciones externas, se ha probado al menos el flujo principal en local.
