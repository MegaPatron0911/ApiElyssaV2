namespace Core.Models
{
    public class PlanType
    {
        public PlanType() => PlanCompanies = new HashSet<PlanCompany>();

        public Guid PlanTypeId { get; set; }
        public string? PlanTypeName { get; set; }
        public decimal FixedFee { get; set; }
        public decimal PropertyFee { get; set; }
        public decimal AdditionalUserFee { get; set; }
        public int QuantityUsers { get; set; }
        public int QuantityProperties { get; set; }
        public decimal Discount { get; set; }
        public Guid CountryId { get; set; }

        public virtual Country? Country { get; set; }
        public virtual IEnumerable<PlanCompany> PlanCompanies { get; set; }
    }
}
