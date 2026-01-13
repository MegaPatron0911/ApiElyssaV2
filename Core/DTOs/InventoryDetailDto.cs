namespace Elyssa.Core.DTOs;

public class InventoryDetailRequest
{
    public Guid InventoryId { get; set; }
}

public class InventoryDetailResponse
{
    public Guid InventoryId { get; set; }
    public InventoryPropertyDto Property { get; set; } = new();
    public int InventoryType { get; set; }
    public string InventoryTypeName { get; set; } = string.Empty;
    public bool IsSigned { get; set; }
    public bool IsRemoteSigned { get; set; }
    public decimal RentalPrice { get; set; }
    public string? Currency { get; set; } = string.Empty;
    public string? ApprovalCode { get; set; }
    public InventorySignaturesDto Signatures { get; set; } = new();
    public string? PdfDownloadUrl { get; set; }
    public InventoryStatsDto Stats { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class InventoryPropertyDto
{
    public Guid PropertyId { get; set; }
    public string? Code { get; set; }
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
}

public class InventorySignaturesDto
{
    public DateTime? AgentSignatureDate { get; set; }
    public DateTime? OwnerSignatureDate { get; set; }
    public DateTime? SignatureDate { get; set; }
}

public class InventoryStatsDto
{
    public int TotalEnvironments { get; set; }
    public int TotalItems { get; set; }
}
