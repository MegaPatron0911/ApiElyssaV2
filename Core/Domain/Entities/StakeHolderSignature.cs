namespace Elyssa.Core.Domain.Entities;

public class StakeHolderSignature 
{
    public StakeHolderSignature()
    {
        Inventories = new HashSet<Inventory>();
    }
    public Guid? Id { get; set; }
    public DateTime? SignatureDate { get; set; }
    
    public virtual ICollection<Inventory> Inventories { get; set; }
}
