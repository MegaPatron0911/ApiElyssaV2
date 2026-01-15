namespace Elyssa.Core.Domain.Entities;

public class OwnerSignature : BaseEntity
{
    public DateTime? SignatureDate { get; set; }
    
    public virtual ICollection<Inventory> Inventories { get; set; }
}
