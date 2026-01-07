namespace Core.Models
{
    public class NuwwePropertyType
    {
        public string CodPropertyType { get; set; }
        public Guid PropertyTypeId { get; set; }
        public string Name { get; set; }
        public Guid? CompanyId { get; set; }
        public DateTime CreatedAt { get; set; }  
        public DateTime? UpdatedAt { get; set; } 

        public virtual PropertyType PropertyType { get; set; }
        public virtual Company Company { get; set; }
    }
}
