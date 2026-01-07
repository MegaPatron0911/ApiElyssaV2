# Guía de Configuración Rápida

## ?? Primeros Pasos

### 1. Restaurar Paquetes NuGet
```bash
dotnet restore
```

### 2. Configurar la Base de Datos

Edita el archivo `ApiElyssaV2/appsettings.json` con tu cadena de conexión:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=ElyssaDb;User Id=TU_USUARIO;Password=TU_PASSWORD;TrustServerCertificate=True"
  }
}
```

### 3. Crear la Base de Datos con Entity Framework

```bash
# Instalar herramientas de EF Core (si no las tienes)
dotnet tool install --global dotnet-ef

# Crear la migración inicial
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project ApiElyssaV2

# Aplicar la migración a la base de datos
dotnet ef database update --project Infrastructure --startup-project ApiElyssaV2
```

### 4. Ejecutar la Aplicación

```bash
dotnet run --project ApiElyssaV2/Elyssa.PublicApi.csproj
```

La API estará disponible en:
- **HTTPS**: https://localhost:7xxx
- **HTTP**: http://localhost:5xxx
- **Swagger UI**: https://localhost:7xxx/swagger

---

## ?? Endpoints Disponibles

### Companies API

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/companies` | Obtener todas las compañías |
| GET | `/api/companies/{id}` | Obtener una compañía por ID |
| POST | `/api/companies` | Crear una nueva compañía |
| PUT | `/api/companies/{id}` | Actualizar una compañía |
| DELETE | `/api/companies/{id}` | Eliminar una compañía |

### Ejemplo de Request (POST)

```json
{
  "name": "Acme Corporation",
  "description": "Una empresa de tecnología",
  "email": "contact@acme.com",
  "phone": "+1234567890",
  "isActive": true
}
```

---

## ??? Comandos Útiles de Entity Framework

### Crear una nueva migración
```bash
dotnet ef migrations add NombreDeLaMigracion --project Infrastructure --startup-project ApiElyssaV2
```

### Aplicar migraciones pendientes
```bash
dotnet ef database update --project Infrastructure --startup-project ApiElyssaV2
```

### Revertir a una migración específica
```bash
dotnet ef database update NombreDeLaMigracion --project Infrastructure --startup-project ApiElyssaV2
```

### Eliminar la última migración
```bash
dotnet ef migrations remove --project Infrastructure --startup-project ApiElyssaV2
```

### Ver el script SQL de una migración
```bash
dotnet ef migrations script --project Infrastructure --startup-project ApiElyssaV2
```

---

## ?? Paquetes NuGet Instalados

### Elyssa.PublicApi
- Microsoft.AspNetCore.OpenApi (8.0.0)
- Microsoft.EntityFrameworkCore.Design (8.0.0)
- Swashbuckle.AspNetCore (6.5.0)

### Elyssa.Infrastructure
- Microsoft.EntityFrameworkCore (8.0.0)
- Microsoft.EntityFrameworkCore.SqlServer (8.0.0)
- Microsoft.AspNetCore.Cryptography.KeyDerivation (8.0.0)

---

## ?? Seguridad

El proyecto incluye un `PasswordHasher` en la capa de Infrastructure para hash seguro de contraseñas:

```csharp
var hashedPassword = PasswordHasher.HashPassword("miPassword123");
var isValid = PasswordHasher.VerifyPassword(hashedPassword, "miPassword123");
```

---

## ?? Próximas Mejoras Sugeridas

- [ ] Implementar autenticación JWT
- [ ] Agregar AutoMapper para mapeo de objetos
- [ ] Implementar paginación en GetAll
- [ ] Agregar filtros y búsqueda
- [ ] Implementar logging con Serilog
- [ ] Agregar Unit Tests
- [ ] Implementar Response Wrappers
- [ ] Agregar Health Checks
- [ ] Implementar Rate Limiting
- [ ] Agregar API Versioning

---

## ?? Solución de Problemas

### Error de conexión a la base de datos
Verifica que:
1. SQL Server esté corriendo
2. La cadena de conexión sea correcta
3. El usuario tenga permisos suficientes

### Error al crear migraciones
Asegúrate de estar en el directorio raíz del proyecto y que todos los paquetes estén restaurados.

### Puerto ocupado
Cambia el puerto en `Properties/launchSettings.json`
