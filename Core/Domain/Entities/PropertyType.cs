namespace Elyssa.Core.Domain.Entities;

public class PropertyType : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    
    
    public ICollection<Property> Properties { get; set; } = new List<Property>();
}
