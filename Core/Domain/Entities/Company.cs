using System.ComponentModel.DataAnnotations.Schema;

namespace Elyssa.Core.Domain.Entities;

public class Company
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CompanyId { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string? BusinessName { get; set; }
    public string? Tin { get; set; }
    public string? Email { get; set; }
    public string? AddressNotification { get; set; }
    public string? Logo { get; set; }
    public string? TradeName { get; set; }
    public string? CityId { get; set; }
    public string? Phone { get; set; }
    public int PlanType { get; set; }
    public int? Status { get; set; }
    public int? Coins { get; set; }
    public int? MaxRentedProperties { get; set; }
    public Guid CountryId { get; set; }
    public bool? HasCenterRepair { get; set; }

    public virtual Country? Country { get; set; }
}