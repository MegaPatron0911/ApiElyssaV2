namespace Elyssa.Core.Domain.Entities;

public class Item : BaseEntity
{
    public string ItemName { get; set; }
    public string? ImageUrl { get; set; }
    
    public virtual ICollection<ItemDiagnostic> ItemDiagnostics { get; set; }
}
