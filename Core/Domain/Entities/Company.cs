using Elyssa.Core.Common.Constants;

namespace Elyssa.Core.Domain.Entities;

public class Company : BaseEntity
{
    public string? BusinessName { get; set; }
    public string? Tin { get; set; }
    public string? Email { get; set; }
    public string? AddressNotification { get; set; }
    public string? Logo { get; set; }
    public string? TradeName { get; set; }
    public string? CityId { get; set; }
    public string? Phone { get; set; }
    public int PlanType { get; set; }
    public int? Status { get; set; } = CompanyStatus.ACTIVE;
    public string LegalTextDelivery { get; set; } = string.Empty;
    public string LegalTextRecruiment { get; set; } = string.Empty;
    public string LegalTextReturn { get; set; } = string.Empty;
    public string LegalTextNews { get; set; } = string.Empty;
    public int? Coins { get; set; }
    public int? MaxRentedProperties { get; set; }
    public Guid CountryId { get; set; }
    public bool? HasCenterRepair { get; set; }
    
    public virtual Country? Country { get; set; }
}
