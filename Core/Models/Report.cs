namespace Core.Models
{
    public class Report
    {
        public Guid Id { get; set; }
        public int ReportCount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? PropertyId { get; set; }
        public Guid? CompanyId { get; set; }
        public long? EstateId { get; set; }
        public long? EstateAssignedId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? StateId { get; set; }
        public string? PriorityId { get; set; }
        public int? Position { get; set; }
        public string? NotificationEmail { get; set; }
        public string? NotificationName { get; set; }

        public virtual Property? Property { get; set; }
        public virtual Company? Company { get; set; }
        public virtual EstateAgentInCompany? Estate { get; set; }
        public virtual EstateAgentInCompany? EstateAssigned { get; set; }
    }
}
