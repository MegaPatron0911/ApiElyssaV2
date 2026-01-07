namespace Core.Models
{
    public partial class Company
    {
        public Company()
        {
            EstateAgentInCompanies = new HashSet<EstateAgentInCompany>();
            DefaultItems = new HashSet<DefaultItem>();
            PlanCompanies = new HashSet<PlanCompany>();
            PropertyEnvironmentDefaults = new HashSet<PropertyEnvironmentDefault>();
            CustomerSuccessRatings = new HashSet<CustomerSuccessRating>();
            CommercialRegistries = new HashSet<CommercialRegistry>();
        }

        public Guid CompanyId { get; set; }
        public string? BusinessName { get; set; }
        public string? Tin { get; set; }
        public string? Email { get; set; }
        public string? AddressNotification { get; set; }
        public string? Logo { get; set; }
        public string? TradeName { get; set; }
        public string? CityId { get; set; }
        public string? Phone { get; set; }
        /// <summary>
        /// Could be:
        /// 1: Administración
        /// 2: Uso
        /// </summary>
        public int PlanType { get; set; }
        /// <summary>
        /// Could be:
        /// 0: Inactiva
        /// 1: Activa
        /// </summary>
        public int? Status { get; set; }

        public int? Coins { get; set; }

        public int? MaxRentedProperties { get; set; }

        public string LegalTextRecruiment { get; set; }

        public string LegalTextNews { get; set; }
        public string LegalTextDelivery { get; set; }

        public string LegalTextReturn { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid CountryId { get; set; }
        public bool? HasCenterRepair { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<EstateAgentInCompany> EstateAgentInCompanies { get; set; }

        public virtual ICollection<DefaultItem> DefaultItems { get; set; }

        public virtual ICollection<PlanCompany> PlanCompanies { get; set; }
        public virtual ICollection<PropertyEnvironmentDefault> PropertyEnvironmentDefaults { get; set; }
        public virtual ICollection<CustomerSuccessRating> CustomerSuccessRatings { get; set; }
        public virtual ICollection<CommercialRegistry> CommercialRegistries { get; set; }
    }
}
