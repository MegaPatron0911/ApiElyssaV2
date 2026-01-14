namespace Elyssa.Core.Domain.Entities;

public class EnvironmentType : BaseEntity
{
    public string EnvironmentTypeName { get; set; } = string.Empty;
    
    public virtual ICollection<Environment> Environments { get; set; } = new List<Environment>();
}
