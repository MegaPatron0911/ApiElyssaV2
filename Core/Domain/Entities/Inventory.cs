namespace Elyssa.Core.Domain.Entities;

public class Inventory : BaseEntity
{
    public Guid PropertyId { get; set; }
    public Guid? StakeHolderSignatureId { get; set; }
    public decimal RentalPrice { get; set; }
    public int InventoryType { get; set; }
    public long EstateAgentInCompanyId { get; set; }
    public bool IsSigned { get; set; }
    public string? PfdUrl { get; set; }
    public DateTime? AgentSignatureDate { get; set; }
    public bool IsRemoteSigned { get; set; }
    public string? ApprovalCode { get; set; }
    public DateTime? SignatureDate { get; set; }
    public string? PdfUrlRef { get; set; }
    public DateTime? OwnerSignatureDate { get; set; }
    public Guid? OwnerSignatureId { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Currency { get; set; }
    
    public virtual Property? Property { get; set; }
    public virtual EstateAgentInCompany? EstateAgent { get; set; }
    public virtual StakeHolderSignature? StakeHolderSignature { get; set; }
    public virtual OwnerSignature? OwnerSignature { get; set; }
    public virtual ICollection<EnvironmentDiagnostic> EnvironmentDiagnostics { get; set; } = new List<EnvironmentDiagnostic>();
}
