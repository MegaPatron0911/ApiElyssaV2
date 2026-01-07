using Core.Enums;

namespace Core.Models
{
    public class NuwweRecordType
    {
        public int Id { get; set; }
        public string CodRecordType { get; set; }
        public InventoryType InventoryType { get; set; }
        public string Name { get; set; }  
        public Guid? CompanyId { get; set; }
        public DateTime CreatedAt { get; set; }  
        public DateTime? UpdatedAt { get; set; }

        public virtual Company Company { get; set; }
    }
}
