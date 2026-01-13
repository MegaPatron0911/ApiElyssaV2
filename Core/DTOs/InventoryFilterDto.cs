namespace Elyssa.Core.DTOs;

public class InventoryFilterDto
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public int? InventoryType { get; set; }
    public bool? IsSigned { get; set; }
    public string SortBy { get; set; } = "createdAt";
    public string SortOrder { get; set; } = "desc";
}

public class InventoryListResponse
{
    public List<InventoryResponse> Inventories { get; set; } = new();
    public PageInfo Pagination { get; set; } = new();
}

public class InventoryResponse
{
    public Guid InventoryId { get; set; }
    public InventoryPropertyInfoDto Property { get; set; } = new();
    public int InventoryType { get; set; }
    public string InventoryTypeName { get; set; } = string.Empty;
    public bool IsSigned { get; set; }
    public bool IsRemoteSigned { get; set; }
    public decimal RentalPrice { get; set; }
    public string? Currency { get; set; }
    public string? PdfDownloadUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? SignatureDate { get; set; }
}

public class InventoryPropertyInfoDto
{
    public Guid PropertyId { get; set; }
    public string? Code { get; set; }
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}
