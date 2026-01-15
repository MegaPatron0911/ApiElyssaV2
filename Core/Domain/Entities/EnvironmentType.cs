namespace Elyssa.Core.Domain.Entities;

public class EnvironmentType : BaseEntity
{
    public string EnvironmentTypeName { get; set; }
    
    public virtual ICollection<Environment> Environments { get; set; }
}
