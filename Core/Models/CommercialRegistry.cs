namespace Core.Models
{
    public class CommercialRegistry
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string City { get; set; } = string.Empty;
        public string Register { get; set; } = string.Empty;
        public string? GoogleMyBusinessURL { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Company Company { get; set; } = null!;
    }
}
