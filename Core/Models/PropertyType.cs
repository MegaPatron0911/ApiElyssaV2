namespace Core.Models
{
    public partial class PropertyType
    {
        public PropertyType()
        {
            Properties = new HashSet<Property>();
            PropertyEnvironmentDefaults = new HashSet<PropertyEnvironmentDefault>();
        }

        public Guid PropertyTypeId { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
        public virtual NuwwePropertyType NuwwePropertyType { get; set; }
        public virtual ICollection<PropertyEnvironmentDefault> PropertyEnvironmentDefaults { get; set; }
    }
}
