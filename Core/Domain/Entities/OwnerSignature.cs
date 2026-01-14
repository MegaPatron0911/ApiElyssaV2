namespace Elyssa.Core.Domain.Entities;

public class OwnerSignature : BaseEntity
{
    public string? SignatureUrl { get; set; }
    public DateTime? SignatureDate { get; set; }
    
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
