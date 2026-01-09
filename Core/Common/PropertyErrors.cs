namespace Elyssa.Core.Common;

public static class PropertyErrors
{
    public static Error InvalidPageNumber => Error.Validation(
        "INVALID_PAGE",
        "La página debe ser mayor o igual a 1");

    public static Error InvalidPageSize => Error.Validation(
        "INVALID_PAGE_SIZE",
        "El tamaño de página debe estar entre 1 y 20");

    public static Error InvalidSortBy => Error.Validation(
        "INVALID_SORT_BY",
        "Campo de orden inválido. Valores permitidos: createdAt, code, address, city"
    );

    public static Error InvalidSortOrder => Error.Validation(
        "INVALID_SORT_ORDER",
        "El orden debe ser 'asc' o 'desc'"
    );
}
