namespace Elyssa.Core.Domain.Entities;

public class StakeHolderSignature : BaseEntity
{
    public Guid? CompanyUserId { get; set; }
    public DateTime? SignatureDate { get; set; }
    
    public virtual ICollection<Inventory> Inventories { get; set; }
}
