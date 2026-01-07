namespace Core.Models
{
    public class DefaultItemAudit
    {
        public int Id { get; set; } 
        public Guid EstateAgentId { get; set; }
        public long EstateAgentInCompanyId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid EnvironmentId { get; set; }
        public string ItemIds { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
