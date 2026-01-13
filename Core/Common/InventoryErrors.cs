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
}
