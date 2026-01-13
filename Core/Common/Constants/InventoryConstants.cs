namespace Elyssa.Core.Common.Constants;

public static class InventoryType
{
    public const int Captacion = 0;
    public const int Colocación = 1;
    public const int PreVisita = 2;
    public const int Desocupación = 3;
}

public static class InventoryTypeNames
{
    public const string Captacion = "Captación";
    public const string Colocacion = "Colocación";
    public const string PreVisita = "PreVisita";
    public const string Desocupación = "Desocupación";
}

public static class InventoryConstants
{
    public const int MIN_PAGE_NUMBER = 1;
    public const int MIN_PAGE_SIZE = 1;
    public const int MAX_PAGE_SIZE = 20;
    public const int DEFAULT_PAGE_SIZE = 20;
}

public static class InventorySortFields
{
    public const string CREATED_AT = "createdAt";
    public const string SIGNATURE_DATE = "signatureDate";
    public const string RENTAL_PRICE = "rentalPrice";
}
