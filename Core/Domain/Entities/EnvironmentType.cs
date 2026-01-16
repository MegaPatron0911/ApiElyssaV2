namespace Elyssa.Core.Domain.Entities;

public class EnvironmentType 
{
    public EnvironmentType()
    {
        Environments = new HashSet<Environment>();
    }
    public Guid EnvironmentTypeId { get; set; }
    public string EnvironmentTypeName { get; set; }
    
    public virtual ICollection<Environment> Environments { get; set; }
}
