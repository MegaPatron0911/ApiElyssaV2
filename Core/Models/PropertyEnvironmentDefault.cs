namespace Core.Models
{
    public class PropertyEnvironmentDefault
    {
        public Guid Id { get; set; }
        public Guid PropertyTypeId { get; set; }
        public Guid EnvironmentTypeId { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual PropertyType PropertyType { get; set; }
        public virtual EnvironmentType EnvironmentType { get; set; }
        public virtual Company Company { get; set; }
    }
}
