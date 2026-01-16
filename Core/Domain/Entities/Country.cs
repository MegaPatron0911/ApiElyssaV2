using Elyssa.Core.Common.Constants;

namespace Elyssa.Core.Domain.Entities;

public class Country
{
    public Country()
    {
        Companies = new HashSet<Company>();
    } 

    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }
    public string? Currency { get; set; }
    public string? PrefixPhone { get; set; }
    public string? PhoneFormat { get; set; }
    public string? CurrencySymbol { get; set; }
    public virtual ICollection<Company> Companies { get; set; }
}
    