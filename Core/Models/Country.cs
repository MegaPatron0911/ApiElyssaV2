namespace Core.Models
{
    public class Country
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? Currency { get; set; }
        public string? ISOCode { get; set; }
        public string? PrefixPhone { get; set; }
        public string? PhoneFormat { get; set; }
        public string? CurrencySymbol { get; set; }

        public virtual IEnumerable<PlanType>? PlanTypes { get; set; }
    }
}
