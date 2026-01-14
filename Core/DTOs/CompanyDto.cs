namespace Elyssa.Core.DTOs;

public class CompanyDto
{
    public Guid Id { get; set; }
    public string? TradeName { get; set; }
    public string? BusinessName { get; set; }
    public string? Tin { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Logo { get; set; }
    public string? AddressNotification { get; set; }
    public string? CityId { get; set; }
    public int PlanType { get; set; }
    public int? Status { get; set; }
    public int? Coins { get; set; }
    public int? MaxRentedProperties { get; set; }
    public Guid CountryId { get; set; }
    public bool? HasCenterRepair { get; set; }
    
    public CountryInfoDto? Country { get; set; }
}

public class CountryInfoDto
{
    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }
    public string? Currency { get; set; }
    public string? CurrencySymbol { get; set; }
}
