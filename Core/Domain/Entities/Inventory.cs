namespace Elyssa.Core.Domain.Entities;

public class Inventory : BaseEntity
{
    public Guid PropertyId { get; set; }
    public int InventoryType { get; set; }
    public bool IsSigned { get; set; }
    public bool IsRemoteSigned { get; set; }
    public decimal RentalPrice { get; set; }
    public string? Currency { get; set; } = "COP";
    public string? ApprovalCode { get; set; }
    public DateTime? AgentSignatureDate { get; set; }
    public DateTime? OwnerSignatureDate { get; set; }
    public DateTime? SignatureDate { get; set; }
    public string? PdfUrl { get; set; }
    public bool IsActive { get; set; } = true;
    
    public Property? Property { get; set; }
}
