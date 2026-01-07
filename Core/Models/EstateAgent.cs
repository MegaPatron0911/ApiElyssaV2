namespace Core.Models
{
    public partial class EstateAgent
    {
        public Guid EstateAgentId { get; set; }
        public string EmployeeName { get; set; }
        public string? UrlImage { get; set; }
        public string? UrlSignatureImage { get; set; }
        public string IdentificationNumber { get; set; }
        public string DocumentType { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TokenRecovery { get; set; } = string.Empty;
        public DateTime? TokenExpiration { get; set; }
        public bool TermsAccepted { get; set; }
        public DateTime DateTermsAccepted { get; set; }
        public string Phone { get; set; }
    }
}
