namespace Elyssa.Core.Domain.Entities;

public class EnvironmentDiagnostic
{
    public EnvironmentDiagnostic()
    {
        ItemDiagnostics = new HashSet<ItemDiagnostic>();
    }
    public Guid EnvinronmentDiagnosticId { get; set; }
    public Guid EnvironmentId { get; set; }
    public Guid InventoryId { get; set; }
    public string? Observations { get; set; }
    public string? State { get; set; }

    public virtual Environment? Environment { get; set; }
    public virtual Inventory? Inventory { get; set; }
    public virtual ICollection<ItemDiagnostic> ItemDiagnostics { get; set; }
}
