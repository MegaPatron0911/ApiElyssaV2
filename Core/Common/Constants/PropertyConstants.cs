namespace Elyssa.Core.Common.Constants;

/// <summary>
/// Constantes relacionadas con propiedades y paginación
/// </summary>
public static class PropertyConstants
{
    public const int MAX_PAGE_SIZE = 20;
    public const int DEFAULT_PAGE_SIZE = 20;
    public const int DEFAULT_PAGE_NUMBER = 1;
    public const int MIN_PAGE_NUMBER = 1;
    public const int MIN_PAGE_SIZE = 1;
}

/// <summary>
/// Constantes para campos de ordenamiento de propiedades
/// </summary>
public static class PropertySortFields
{
    public const string CREATED_AT = "createdAt";
    public const string CODE = "code";
    public const string ADDRESS = "address";
    public const string CITY = "city";
}

/// <summary>
/// Constantes para orden de clasificación
/// </summary>
public static class SortOrder
{
    public const string ASCENDING = "asc";
    public const string DESCENDING = "desc";
}
