namespace Core.Models
{
    public class CustomerSuccessRating
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid InventoryId { get; set; }
        public long EstateAgentInCompanyId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Message { get; set; }

        public Company Company { get; set; } = null!;
        public Inventory Inventory { get; set; } = null!;
        public EstateAgentInCompany EstateAgentInCompany { get; set; } = null!;
    }
}
