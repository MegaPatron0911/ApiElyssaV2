namespace Elyssa.Core.Domain.Entities;

public class PropertyType
{
    public PropertyType()
    {
        Properties = new HashSet<Property>();
    }
    public Guid PropertyTypeId { set; get; }
    public string typeName { get; set; }
    
    
    public virtual ICollection<Property> Properties { get; set; }
}
