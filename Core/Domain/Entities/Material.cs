namespace Elyssa.Core.Domain.Entities;

public class Material : BaseEntity
{
    public string MaterialName { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public virtual ICollection<ItemDiagnostic> ItemDiagnostics { get; set; } = new List<ItemDiagnostic>();
}
