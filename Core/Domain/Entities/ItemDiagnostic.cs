namespace Elyssa.Core.Domain.Entities;

public class ItemDiagnostic : BaseEntity
{
    public Guid EnvironmentDiagnosticId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string? State { get; set; }
    public int Quantity { get; set; }
    public string? Observations { get; set; }

    public EnvironmentDiagnostic? EnvironmentDiagnostic { get; set; }
}
