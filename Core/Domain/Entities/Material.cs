namespace Elyssa.Core.Domain.Entities;

public class Material : BaseEntity
{
    public string MaterialName { get; set; }
    public string? Description { get; set; }
    
    public virtual ICollection<ItemDiagnostic> ItemDiagnostics { get; set; }
}
