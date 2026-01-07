namespace Core.Models
{
    public class AuditSourceProperty
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public string NuwweCode { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Property Property { get; set; }
    }
}
