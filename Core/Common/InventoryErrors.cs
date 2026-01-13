namespace Elyssa.Core.Common;

public static class InventoryErrors
{
    public static Error NotFound(Guid inventoryId) => Error.NotFound(
        "INVENTORY_NOT_FOUND",
        "El inventario especificado no existe",
        "inventoryId");

    public static Error InvalidInventoryId => Error.Validation(
        "INVALID_INVENTORY_ID",
        "El ID del inventario no puede estar vacío",
        "inventoryId");

    public static Error ForbiddenResource(Guid inventoryId, Guid companyId) => Error.Forbidden(
        "FORBIDDEN_RESOURCE",
        $"El inventario {inventoryId} no pertenece a una propiedad de la compañía {companyId}",
        "inventoryId");

    public static Error InvalidPageNumber => Error.Validation(
        "INVALID_PAGE",
        "El número de página debe ser mayor o igual a 1",
        "page");

    public static Error InvalidPageSize => Error.Validation(
        "INVALID_PAGE_SIZE",
        "El tamaño de página debe estar entre 1 y 20",
        "pageSize");

    public static Error InvalidSortBy => Error.Validation(
        "INVALID_SORT_BY",
        "El campo de ordenamiento no es válido. Valores permitidos: createdAt, signatureDate, rentalPrice",
        "sortBy");

    public static Error InvalidSortOrder => Error.Validation(
        "INVALID_SORT_ORDER",
        "El orden debe ser 'asc' o 'desc'",
        "sortOrder");

    public static Error InvalidInventoryType => Error.Validation(
        "INVALID_INVENTORY_TYPE",
        "El tipo de inventario debe estar entre 0 y 3",
        "inventoryType");
}
