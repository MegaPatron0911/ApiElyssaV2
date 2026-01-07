namespace Core.Models
{
    public partial class OwnerSignature
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string IdentificationNumber { get; set; }
        public string DocumentType { get; set; }
        public DateTime? SignatureDate { get; set; }
        public string? SignaturePhoto { get; set; }
        public string? FacePhoto { get; set; }

        public virtual Inventory Inventory { get; set; }
    }
}
