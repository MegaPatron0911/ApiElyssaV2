namespace Elyssa.Core.Domain.Entities;

public class Material 
{
    public Material()
    {
        ItemDiagnostics = new HashSet<ItemDiagnostic>(); 
    }
    public Guid MaterialId { get; set; }
    public string MaterialName { get; set; }
    public string? Description { get; set; }
    
    public virtual ICollection<ItemDiagnostic> ItemDiagnostics { get; set; }
}
