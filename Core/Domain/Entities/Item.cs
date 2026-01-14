namespace Elyssa.Core.Domain.Entities;

public class Item : BaseEntity
{
    public string ItemName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    
    public virtual ICollection<ItemDiagnostic> ItemDiagnostics { get; set; } = new List<ItemDiagnostic>();
}
