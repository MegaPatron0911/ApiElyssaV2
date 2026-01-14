namespace Elyssa.Core.Domain.Entities;

public class PropertyEnvironment : BaseEntity
{
    public Guid PropertyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public Property? Property { get; set; }
    public ICollection<EnvironmentDiagnostic> EnvironmentDiagnostics { get; set; } = new List<EnvironmentDiagnostic>();
}
