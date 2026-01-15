namespace Elyssa.Core.Domain.Entities;

public class Country : BaseEntity
{
    public string? CountryName { get; set; }
    public string? Currency { get; set; }
    public string? CurrencySymbol { get; set; }
    public string? ISOCode { get; set; }
    public string? PhoneFormat { get; set; }
    public string? PrefixPhone { get; set; }
    
    public virtual ICollection<Company> Companies { get; set; }
}
