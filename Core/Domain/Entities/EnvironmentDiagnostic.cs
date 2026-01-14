namespace Elyssa.Core.Domain.Entities;

public class EnvironmentDiagnostic : BaseEntity
{
    public Guid InventoryId { get; set; }
    public Guid EnvironmentDiagnosticId { get; set; }
    public string? Observations { get; set; }
    public string? State { get; set; }

    public Inventory? Inventory { get; set; }
    public ICollection<ItemDiagnostic> ItemDiagnostics { get; set; } = new List<ItemDiagnostic>();
}
