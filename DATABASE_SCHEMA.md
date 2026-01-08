# DATABASE SCHEMA - ELYSSA

Documentación completa del esquema de la base de datos PostgreSQL de Elyssa.

## ??? Tablas Principales

### **Company** (Empresas/Inmobiliarias)
Tabla principal que almacena información de las empresas inmobiliarias.

| Columna | Tipo | Nullable | Default | Descripción |
|---------|------|----------|---------|-------------|
| `CompanyId` | uuid | NO | - | **PK** - Identificador único |
| `BusinessName` | varchar | YES | - | Razón social |
| `Tin` | varchar | YES | - | NIT de la empresa |
| `Email` | varchar | YES | - | Email de contacto |
| `AddressNotification` | varchar | YES | - | Dirección para notificaciones |
| `Logo` | varchar | YES | - | URL del logo |
| `TradeName` | varchar | YES | - | Nombre comercial |
| `CityId` | varchar | YES | - | ID de la ciudad |
| `Phone` | varchar | YES | - | Teléfono |
| `PlanType` | integer | NO | - | Tipo de plan (1=Básico, 2=Estándar, 3=Premium) |
| `Status` | integer | YES | - | Estado (1=Activo, 0=Inactivo) |
| `Coins` | integer | YES | - | Monedas/créditos disponibles |
| `MaxRentedProperties` | integer | YES | - | Máximo de propiedades en renta |
| `CreationDate` | timestamptz | NO | -infinity | Fecha de creación |
| `UpdateDate` | timestamptz | YES | - | Fecha de última actualización |
| `CountryId` | uuid | NO | '908bc92a...' | **FK** a Country |
| `HasCenterRepair` | boolean | YES | - | Tiene centro de reparaciones |

**Foreign Keys:**
- `CountryId` ? `Country.CountryId`

**Campos de texto legal:**
- `LegalTextDelivery` (text): Texto legal para entrega
- `LegalTextRecruiment` (text): Texto legal para reclutamiento
- `LegalTextReturn` (text): Texto legal para devolución
- `LegalTextNews` (text): Texto legal para novedades

---

### **PlanType** (Tipos de Plan)
Catálogo de tipos de planes disponibles.

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `PlanTypeId` | ? | NO | **PK** - ID del tipo de plan |
| `CountryId` | uuid | ? | **FK** a Country |
| Otras columnas... | - | - | (Por confirmar) |

**Foreign Keys:**
- `CountryId` ? `Country.CountryId`

---

### **PlanCompany** (Relación Empresa-Plan)
Tabla intermedia que relaciona empresas con tipos de planes (relación N:M).

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `CompanyId` | uuid | NO | **FK** a Company |
| `PlanTypeId` | ? | NO | **FK** a PlanType |

**Foreign Keys:**
- `CompanyId` ? `Company.CompanyId`
- `PlanTypeId` ? `PlanType.PlanTypeId`

---

### **EstateAgentInCompany** (Usuarios/Agentes)
Tabla de usuarios/agentes inmobiliarios asociados a empresas.

| Columna | Tipo | Nullable | Default | Descripción |
|---------|------|----------|---------|-------------|
| `EstateAgentInCompanyId` | bigint | NO | - | **PK** - ID autoincremental |
| `RegistrationDate` | timestamptz | NO | - | Fecha de registro |
| `CompanyId` | uuid | YES | - | **FK** a Company |
| `IsActive` | boolean | NO | - | Usuario activo |
| `IsAdmin` | boolean | NO | - | Es administrador |
| `EmployeeName` | varchar | NO | '' | Nombre del empleado |
| `IdentificationNumber` | text | NO | '' | Número de documento |
| `DocumentType` | varchar | NO | '' | Tipo de documento |
| `Email` | varchar | NO | '' | Email |
| `Password` | varchar | NO | '' | Contraseña (hash) |
| `UrlImage` | text | YES | - | URL foto de perfil |
| `UrlSignatureImage` | text | YES | - | URL firma |
| `TokenRecovery` | text | NO | '' | Token recuperación |
| `TokenExpiration` | timestamptz | YES | - | Expiración token |
| `Phone` | varchar | YES | - | Teléfono |
| `RoleAliasId` | uuid | YES | - | **FK** a RoleAlias |

**Foreign Keys:**
- `CompanyId` ? `Company.CompanyId`
- `RoleAliasId` ? `RoleAlias.RoleAliasId`

---

### **Property** (Propiedades)
Tabla de propiedades inmobiliarias.

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `PropertyId` | uuid | NO | **PK** - ID de la propiedad |
| `CompanyId` | uuid | NO | **FK** a Company |
| `PropertyTypeId` | ? | ? | **FK** a PropertyType |
| `EstateAgentInCompanyId` | bigint | ? | **FK** a EstateAgentInCompany |
| `Status` | integer | NO | Estado (1=Activo) |

**Foreign Keys:**
- `CompanyId` ? `Company.CompanyId`
- `PropertyTypeId` ? `PropertyType.PropertyTypeId`
- `EstateAgentInCompanyId` ? `EstateAgentInCompany.EstateAgentInCompanyId`

---

### **Inventory** (Inventarios)
Tabla de inventarios de propiedades.

| Columna | Tipo | Nullable | Default | Descripción |
|---------|------|----------|---------|-------------|
| `InventoryId` | uuid | NO | - | **PK** |
| `PropertyId` | uuid | NO | - | **FK** a Property |
| `CreationDate` | timestamptz | NO | - | Fecha de creación |
| `StakeHolderSignatureId` | uuid | YES | - | **FK** a StakeHolderSignature |
| `OwnerSignatureId` | uuid | YES | - | **FK** a OwnerSignature |
| `RentalPrice` | numeric | NO | - | Precio de renta |
| `InventoryType` | integer | NO | - | Tipo de inventario |
| `EstateAgentInCompanyId` | bigint | NO | - | **FK** a EstateAgentInCompany |
| `IsSigned` | boolean | NO | false | Está firmado |
| `PfdUrl` | text | YES | - | URL del PDF |
| `AgentSignatureDate` | timestamptz | YES | - | Fecha firma agente |
| `IsRemoteSigned` | boolean | NO | false | Firmado remotamente |
| `Approval_Code` | text | YES | - | Código de aprobación |
| `SignatureDate` | timestamptz | YES | - | Fecha de firma |
| `IsActive` | boolean | NO | true | Está activo |
| `Currency` | text | YES | - | Moneda |

**Foreign Keys:**
- `PropertyId` ? `Property.PropertyId`
- `StakeHolderSignatureId` ? `StakeHolderSignature.Id`
- `OwnerSignatureId` ? `OwnerSignature.Id`
- `EstateAgentInCompanyId` ? `EstateAgentInCompany.EstateAgentInCompanyId`

---

### **Environment** (Ambientes/Habitaciones)
Ambientes o habitaciones de una propiedad.

| Columna | Tipo | Nullable | Default | Descripción |
|---------|------|----------|---------|-------------|
| `EnvironmentId` | uuid | NO | - | **PK** |
| `Level` | integer | NO | - | Nivel/piso |
| `EnvironmentName` | text | NO | - | Nombre del ambiente |
| `EnvironmentTypeId` | uuid | NO | - | **FK** a EnvironmentType |
| `PropertyId` | uuid | NO | - | **FK** a Property |
| `IsActive` | boolean | NO | true | Está activo |
| `Order` | integer | NO | 0 | Orden de visualización |

**Foreign Keys:**
- `EnvironmentTypeId` ? `EnvironmentType.EnvironmentTypeId`
- `PropertyId` ? `Property.PropertyId`

---

### **EnvironmentDiagnostics** (Diagnósticos de Ambientes)
Diagnósticos de ambientes en inventarios.

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `EnvironmentDiagnosticId` | uuid | NO | **PK** |
| `EnvironmentId` | uuid | NO | **FK** a Environment |
| `InventoryId` | uuid | NO | **FK** a Inventory |
| Otras columnas... | - | - | (Por completar) |

**Foreign Keys:**
- `EnvironmentId` ? `Environment.EnvironmentId`
- `InventoryId` ? `Inventory.InventoryId`

---

### **EnvironmentType** (Tipos de Ambiente)
Catálogo de tipos de ambientes.

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `EnvironmentTypeId` | uuid | NO | **PK** |
| `EnvironmentTypeName` | text | NO | Nombre del tipo |

---

### **Item** (Items/Elementos)
Catálogo de items que pueden existir en ambientes.

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `ItemId` | uuid | NO | **PK** |
| `ItemName` | varchar | NO | Nombre del item |
| `ImageUrl` | text | YES | URL de la imagen |

---

### **ItemDiagnostic** (Diagnóstico de Items)
Estado y diagnóstico de items en inventarios.

| Columna | Tipo | Nullable | Default | Descripción |
|---------|------|----------|---------|-------------|
| `ItemDiagnosticId` | uuid | NO | - | **PK** |
| `MaterialId` | uuid | YES | - | **FK** a Material |
| `ItemId` | uuid | NO | - | **FK** a Item |
| `Description` | varchar | YES | - | Descripción |
| `Rating` | integer | YES | - | Calificación |
| `EnvironmentDiagnosticId` | uuid | NO | - | **FK** a EnvironmentDiagnostics |
| `ItemDiagnosticIdSelfReference` | uuid | YES | - | **FK** auto-referencia |
| `Subject` | text | YES | - | Asunto |
| `Anottation` | text | YES | - | Anotaciones |
| `IsANew` | boolean | NO | - | Es nuevo |
| `ToRepair` | boolean | NO | - | Requiere reparación |
| `Amount` | integer | NO | 0 | Cantidad |
| `Order` | integer | YES | - | Orden |

**Foreign Keys:**
- `MaterialId` ? `Material.MaterialId`
- `ItemId` ? `Item.ItemId`
- `EnvironmentDiagnosticId` ? `EnvironmentDiagnostics.EnvironmentDiagnosticId`
- `ItemDiagnosticIdSelfReference` ? `ItemDiagnostic.ItemDiagnosticId` (auto-referencia)

---

### **Photo** (Fotos)
Fotos asociadas a diagnósticos de items.

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `PhotoId` | bigint | NO | **PK** |
| `ItemDiagnosticId` | ? | ? | **FK** a ItemDiagnostic |

**Foreign Keys:**
- `ItemDiagnosticId` ? `ItemDiagnostic.ItemDiagnosticId`

---

### **Country** (Países)
Catálogo de países.

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `CountryId` | uuid | NO | **PK** |
| `CountryName` | varchar | YES | Nombre del país |
| `Currency` | varchar | YES | Moneda |
| `CurrencySymbol` | varchar | YES | Símbolo de moneda |
| `ISOCode` | varchar | YES | Código ISO |
| `PhoneFormat` | varchar | YES | Formato de teléfono |
| `PrefixPhone` | varchar | YES | Prefijo telefónico |

---

### **NuwweAuth** (Autenticación Nuwwe)
Tokens de autenticación para integración con Nuwwe.

| Columna | Tipo | Nullable | Default | Descripción |
|---------|------|----------|---------|-------------|
| `Id` | integer | NO | - | **PK** |
| `CompanyId` | uuid | NO | - | **FK** a Company |
| `Token` | text | NO | - | Token de acceso |
| `RefreshToken` | text | YES | - | Token de refresco |
| `NameIdNuwwe` | text | NO | - | ID en Nuwwe |
| `CreatedAt` | timestamptz | NO | now() | Fecha de creación |
| `UpdatedAt` | timestamptz | YES | - | Última actualización |
| `DomainUrl` | text | YES | - | URL del dominio |

**Foreign Keys:**
- `CompanyId` ? `Company.CompanyId`

---

### **NuwwePropertyDistribution** (Distribución de Propiedades Nuwwe)
Mapeo de distribuciones de propiedades con Nuwwe.

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `CodPropertyDistribution` | text | NO | **PK** - Código |
| `EnvironmentTypeId` | uuid | YES | **FK** a EnvironmentType |
| `CompanyId` | uuid | NO | **FK** a Company |

**Foreign Keys:**
- `EnvironmentTypeId` ? `EnvironmentType.EnvironmentTypeId`
- `CompanyId` ? `Company.CompanyId`

---

### **NuwwePropertyType** (Tipos de Propiedad Nuwwe)
Mapeo de tipos de propiedad con Nuwwe.

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `CodPropertyType` | text | NO | **PK** - Código |
| `CompanyId` | uuid | NO | **FK** a Company |

**Foreign Keys:**
- `CompanyId` ? `Company.CompanyId`

---

### **NuwweRecordType** (Tipos de Registro Nuwwe)
Mapeo de tipos de registro con Nuwwe.

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `CodRecordType` | text | NO | **PK** |
| `CompanyId` | uuid | YES | **FK** a Company |

**Foreign Keys:**
- `CompanyId` ? `Company.CompanyId`

---

### **CustomerSuccessRating** (Calificaciones)
Calificaciones de satisfacción del cliente.

| Columna | Tipo | Nullable | Default | Descripción |
|---------|------|----------|---------|-------------|
| `Id` | uuid | NO | - | **PK** |
| `CompanyId` | uuid | NO | - | **FK** a Company |
| `InventoryId` | uuid | NO | - | **FK** a Inventory |
| `CustomerName` | varchar(200) | NO | - | Nombre del cliente |
| `Email` | varchar(320) | NO | - | Email del cliente |
| `Rating` | double precision | NO | - | Calificación |
| `CreatedAt` | timestamptz | NO | now() | Fecha de creación |
| `Message` | text | YES | - | Mensaje/comentario |
| `EstateAgentInCompanyId` | bigint | NO | 0 | **FK** a EstateAgentInCompany |

**Foreign Keys:**
- `CompanyId` ? `Company.CompanyId`
- `InventoryId` ? `Inventory.InventoryId`
- `EstateAgentInCompanyId` ? `EstateAgentInCompany.EstateAgentInCompanyId`

---

### **DefaultItems** (Items por Defecto)
Items por defecto para tipos de ambiente.

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `Id` | uuid | NO | **PK** |
| `EnvironmentTypeId` | uuid | NO | **FK** a EnvironmentType |
| `ItemId` | uuid | NO | **FK** a Item |
| `CompanyId` | uuid | YES | **FK** a Company |

**Foreign Keys:**
- `EnvironmentTypeId` ? `EnvironmentType.EnvironmentTypeId`
- `ItemId` ? `Item.ItemId`
- `CompanyId` ? `Company.CompanyId`

---

### **PropertyEnvironmentDefault** (Ambientes por Defecto por Tipo de Propiedad)
Configuración de ambientes por defecto según tipo de propiedad.

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| Columnas... | - | - | (Por confirmar) |
| `PropertyTypeId` | ? | ? | **FK** a PropertyType |
| `CompanyId` | uuid | ? | **FK** a Company |
| `EnvironmentTypeId` | uuid | ? | **FK** a EnvironmentType |

**Foreign Keys:**
- `PropertyTypeId` ? `PropertyType.PropertyTypeId`
- `CompanyId` ? `Company.CompanyId`
- `EnvironmentTypeId` ? `EnvironmentType.EnvironmentTypeId`

---

### **CommercialRegistry** (Registro Comercial)
Información de registro comercial de empresas.

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `Id` | uuid | NO | **PK** |
| `CompanyId` | uuid | NO | **FK** a Company |

**Foreign Keys:**
- `CompanyId` ? `Company.CompanyId`

---

### **AuditSourceProperty** (Auditoría de Origen de Propiedad)
Auditoría de propiedades importadas.

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `Id` | uuid | NO | **PK** |
| `PropertyId` | uuid | NO | **FK** a Property |

**Foreign Keys:**
- `PropertyId` ? `Property.PropertyId`

---

### **InventoryTimeTracking** (Seguimiento de Tiempo de Inventarios)
Tracking de tiempo dedicado a inventarios.

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `Id` | uuid | NO | **PK** |
| `InventoryId` | uuid | NO | **FK** a Inventory |
| `EstateAgentInCompanyId` | bigint | NO | **FK** a EstateAgentInCompany |

**Foreign Keys:**
- `InventoryId` ? `Inventory.InventoryId`
- `EstateAgentInCompanyId` ? `EstateAgentInCompany.EstateAgentInCompanyId`

---

### **StakeHolderSignature** (Firma de Interesados)
Firmas de stakeholders en inventarios.

| Columna | Tipo | Nullable | Descripción |
|---------|------|----------|-------------|
| `Id` | uuid | NO | **PK** |
| `CompanyUserId` | uuid | ? | **FK** a CompanyUser |

**Foreign Keys:**
- `CompanyUserId` ? `CompanyUser.Id`

---

## ?? Relaciones Principales (Foreign Keys)

### **Company (Centro del Sistema)**
- **Company (1) ? (N) EstateAgentInCompany**: Una empresa tiene múltiples agentes
- **Company (1) ? (N) Property**: Una empresa gestiona múltiples propiedades
- **Company (1) ? (N) PlanCompany**: Relación con planes (N:M via tabla intermedia)
- **Company (1) ? (N) DefaultItems**: Items por defecto de la empresa
- **Company (1) ? (N) NuwweAuth**: Tokens de integración Nuwwe
- **Company (1) ? (N) CommercialRegistry**: Registro comercial
- **Company (1) ? (N) CustomerSuccessRating**: Calificaciones recibidas
- **Company (N) ? (1) Country**: Empresa pertenece a un país

### **Property (Propiedades)**
- **Property (N) ? (1) Company**: Propiedad pertenece a una empresa
- **Property (N) ? (1) PropertyType**: Tipo de propiedad
- **Property (N) ? (1) EstateAgentInCompany**: Agente asignado
- **Property (1) ? (N) Inventory**: Múltiples inventarios por propiedad
- **Property (1) ? (N) Environment**: Múltiples ambientes por propiedad
- **Property (1) ? (N) AuditSourceProperty**: Auditoría de origen

### **Inventory (Inventarios)**
- **Inventory (N) ? (1) Property**: Inventario de una propiedad
- **Inventory (N) ? (1) EstateAgentInCompany**: Agente que crea el inventario
- **Inventory (N) ? (1) StakeHolderSignature**: Firma del interesado
- **Inventory (N) ? (1) OwnerSignature**: Firma del propietario
- **Inventory (1) ? (N) EnvironmentDiagnostics**: Diagnósticos de ambientes
- **Inventory (1) ? (N) CustomerSuccessRating**: Calificaciones del inventario
- **Inventory (1) ? (N) InventoryTimeTracking**: Seguimiento de tiempo

### **Environment (Ambientes)**
- **Environment (N) ? (1) Property**: Ambiente de una propiedad
- **Environment (N) ? (1) EnvironmentType**: Tipo de ambiente
- **Environment (1) ? (N) EnvironmentDiagnostics**: Diagnósticos del ambiente

### **EnvironmentDiagnostics (Diagnósticos de Ambientes)**
- **EnvironmentDiagnostics (N) ? (1) Environment**: Diagnóstico de un ambiente
- **EnvironmentDiagnostics (N) ? (1) Inventory**: Diagnóstico en un inventario
- **EnvironmentDiagnostics (1) ? (N) ItemDiagnostic**: Diagnósticos de items

### **ItemDiagnostic (Diagnóstico de Items)**
- **ItemDiagnostic (N) ? (1) Item**: Item diagnosticado
- **ItemDiagnostic (N) ? (1) Material**: Material del item
- **ItemDiagnostic (N) ? (1) EnvironmentDiagnostics**: Pertenece a un diagnóstico
- **ItemDiagnostic (N) ? (1) ItemDiagnostic**: Auto-referencia (items relacionados)
- **ItemDiagnostic (1) ? (N) Photo**: Fotos del diagnóstico

### **Catálogos y Referencias**
- **EnvironmentType (1) ? (N) Environment**: Tipo usado en ambientes
- **EnvironmentType (1) ? (N) DefaultItems**: Items por defecto del tipo
- **Item (1) ? (N) ItemDiagnostic**: Item usado en diagnósticos
- **Item (1) ? (N) DefaultItems**: Item como default
- **Country (1) ? (N) Company**: País de la empresa
- **Country (1) ? (N) PlanType**: Planes por país

---

## ?? Convenciones

### **Nombres de Tablas:**
- PascalCase con primera letra mayúscula
- Singular (Company, Property, Item)
- Algunas tablas usan nombres compuestos (EstateAgentInCompany, ItemDiagnostic)

### **Nombres de Columnas:**
- PascalCase con primera letra mayúscula
- IDs terminan en "Id" (CompanyId, PropertyId)
- Fechas usan sufijos Date/At (CreationDate, CreatedAt, UpdatedAt)

### **Primary Keys:**
- Mayoría usan UUID (uuid)
- Algunas tablas usan bigint autoincremental (EstateAgentInCompany, Photo)
- Algunas usan integer autoincremental (Permissions, NuwweAuth)

### **Foreign Keys:**
- Prefijo FK_ seguido de las tablas relacionadas
- Formato: `FK_TablaOrigen_TablaDestino_ColumnaFK`
- Ejemplo: `FK_Company_Country_CountryId`

### **Estados/Status:**
- Generalmente integer: 1 = Activo, 0 = Inactivo
- Algunos usan boolean: IsActive, IsSigned, IsRemoteSigned

### **Timestamps:**
- `timestamp with time zone` (timestamptz)
- Convención: CreationDate/CreatedAt para creación, UpdateDate/UpdatedAt para última modificación

---

## ?? Queries Útiles

### Contar usuarios activos de una empresa:
```sql
SELECT COUNT(*) 
FROM "EstateAgentInCompany" 
WHERE "CompanyId" = {companyId} AND "IsActive" = true
```

### Contar propiedades activas de una empresa:
```sql
SELECT COUNT(*) 
FROM "Property" 
WHERE "CompanyId" = {companyId} AND "Status" = 1
```

### Obtener plan de una empresa (via tabla intermedia):
```sql
SELECT pt.*
FROM "PlanType" pt
INNER JOIN "PlanCompany" pc ON pt."PlanTypeId" = pc."PlanTypeId"
WHERE pc."CompanyId" = {companyId}
```

### Obtener inventarios de una propiedad:
```sql
SELECT * 
FROM "Inventory" 
WHERE "PropertyId" = {propertyId} AND "IsActive" = true
ORDER BY "CreationDate" DESC
```

### Obtener ambientes con items de un inventario:
```sql
SELECT 
    e."EnvironmentName",
    et."EnvironmentTypeName",
    i."ItemName",
    id."Rating",
    id."Description"
FROM "EnvironmentDiagnostics" ed
INNER JOIN "Environment" e ON ed."EnvironmentId" = e."EnvironmentId"
INNER JOIN "EnvironmentType" et ON e."EnvironmentTypeId" = et."EnvironmentTypeId"
INNER JOIN "ItemDiagnostic" id ON ed."EnvironmentDiagnosticId" = id."EnvironmentDiagnosticId"
INNER JOIN "Item" i ON id."ItemId" = i."ItemId"
WHERE ed."InventoryId" = {inventoryId}
ORDER BY e."Order", id."Order"
```

---

## ?? Notas Importantes

1. **PostgreSQL es case-sensitive** cuando se usan comillas dobles en nombres.
2. Usar siempre comillas dobles para nombres de tablas/columnas: `"Company"`, `"CompanyId"`
3. Los tipos de plan están en una tabla separada `PlanType`, relacionada via `PlanCompany`
4. El campo `PlanType` en `Company` es un integer simple, NO es FK
5. El status de Company/Property: 1=Activo, otros valores=Inactivo
6. Las fechas están en formato UTC (timestamptz)
7. La tabla de usuarios reales es `EstateAgentInCompany`, NO `Users`
8. Existe una relación N:M entre `Company` y `PlanType` via `PlanCompany`
9. Las firmas tienen dos tablas: `StakeHolderSignature` (inquilinos) y `OwnerSignature` (propietarios)
10. `ItemDiagnostic` tiene auto-referencia para relacionar items entre sí

---

## ?? Tablas Intermedias (N:M)

### **PlanCompany**
Relaciona empresas con tipos de planes (muchos a muchos).
- Una empresa puede tener múltiples planes históricos
- Un plan puede estar asignado a múltiples empresas

---

**Última actualización:** 2026-01-08
**Base de datos:** Azure PostgreSQL - elyssa.postgres.database.azure.com
**Esquema:** public
