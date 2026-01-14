namespace Elyssa.Core.Domain.Entities;

public class EnvironmentDiagnostic : BaseEntity
{
    public Guid EnvironmentId { get; set; }
    public Guid InventoryId { get; set; }
    public string? Observations { get; set; }
    public string? State { get; set; }

    public virtual Environment? Environment { get; set; }
    public virtual Inventory? Inventory { get; set; }
    public virtual ICollection<ItemDiagnostic> ItemDiagnostics { get; set; } = new List<ItemDiagnostic>();
}
