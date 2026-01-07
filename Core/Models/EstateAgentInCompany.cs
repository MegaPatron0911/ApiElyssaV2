namespace Core.Models
{
    public partial class EstateAgentInCompany
    {
        public EstateAgentInCompany()
        {
            Inventories = new HashSet<Inventory>();
            Properties = new HashSet<Property>();
            CustomerSuccessRatings = new HashSet<CustomerSuccessRating>();
        }

        public long EstateAgentInCompanyId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? RoleAliasId { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public string EmployeeName { get; set; }
        public string? UrlImage { get; set; }
        public string? UrlSignatureImage { get; set; }
        public string IdentificationNumber { get; set; }
        public string DocumentType { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TokenRecovery { get; set; } = string.Empty;
        public DateTime? TokenExpiration { get; set; }
        public string Phone { get; set; }

        public virtual Company? Company { get; set; }
        public virtual RoleAlias? RoleAlias { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<Property> Properties { get; set; }
        public virtual ICollection<CustomerSuccessRating> CustomerSuccessRatings { get; set; }
    }
}
