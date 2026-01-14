namespace Elyssa.Core.Domain.Entities;

public class Environment : BaseEntity
{
    public int Level { get; set; }
    public string EnvironmentName { get; set; } = string.Empty;
    public Guid EnvironmentTypeId { get; set; }
    public Guid PropertyId { get; set; }
    public bool IsActive { get; set; } = true;
    public int Order { get; set; }
    public int NuwweDistributionId { get; set; }
    public int NuwweInmuebleId { get; set; }
    public int NuwweOrden { get; set; }
    
    public virtual EnvironmentType? EnvironmentType { get; set; }
    public virtual Property? Property { get; set; }
    public virtual ICollection<EnvironmentDiagnostic> EnvironmentDiagnostics { get; set; } = new List<EnvironmentDiagnostic>();
}
