namespace Core.Models
{
    public class ActivityLog
    {
        public long Id { get; set; } 
        public long EstateAgentInCompanyId { get; set; }
        public string? EstateAgentName { get; set; }
        public string Action { get; set; }
        public Guid PropertyId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? InventoryId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
