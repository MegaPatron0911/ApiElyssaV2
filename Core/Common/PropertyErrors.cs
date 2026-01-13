namespace Elyssa.Core.Common;

public static class PropertyErrors
{
    public static Error InvalidPageNumber => Error.Validation(
        "INVALID_PAGE",
        "La página debe ser mayor o igual a 1",
        "page");
        
    public static Error InvalidPageSize => Error.Validation(
        "INVALID_PAGE_SIZE",
        "El tamaño de página debe estar entre 1 y 20",
        "pageSize");

    public static Error InvalidSortBy => Error.Validation(
        "INVALID_SORT_BY",
        "Campo de orden inválido. Valores permitidos: createdAt, code, address, city",
        "sortBy"
    );

    public static Error InvalidSortOrder => Error.Validation(
        "INVALID_SORT_ORDER",
        "El orden debe ser 'asc' o 'desc'",
        "sortOrder"
    );

    public static Error NotFound(Guid propertyId) => Error.NotFound(
        "PROPERTY_NOT_FOUND",
        "La propiedad especificada no existe",
        "propertyId");

    public static Error InvalidPropertyId => Error.Validation(
        "INVALID_PROPERTY_ID",
        "El ID de la propiedad no puede estar vacío",
        "propertyId");

    public static Error ForbiddenResource(Guid propertyId, Guid companyId) => Error.Forbidden(
        "FORBIDDEN_RESOURCE",
        $"La propiedad {propertyId} no pertenece a la compañía {companyId}",
        "propertyId");
}
