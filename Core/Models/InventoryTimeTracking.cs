namespace Core.Models
{
    public class InventoryTimeTracking
    {
        public Guid Id { get; set; }
        public Guid InventoryId { get; set; }
        public long EstateAgentInCompanyId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Inventory? Inventory { get; set; }
        public virtual EstateAgentInCompany? EstateAgentInCompany{ get; set; }
    }
}