using System.ComponentModel.DataAnnotations.Schema;

namespace Elyssa.Core.Domain.Entities;

public class Property
{
    public Property()
    {
        Inventories = new HashSet<Inventory>();
        Environments = new HashSet<Environment>();
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PropertyId { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? ModificationDate { get; set; }
    public string Address { get; set; }
    public Guid CompanyId { get; set; }
    public Guid PropertyTypeId { get; set; }
    public decimal BuiltArea { get; set; }
    public decimal? LotArea { get; set; }
    public string Neighborhood { get; set; }
    public string? Code { get; set; }
    public string City { get; set; }
    public bool IsRented { get; set; }
    public long? EstateAgentInCompanyId { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? Detail { get; set; }
    public int Levels { get; set; }
    public string Country { get; set; }
    public bool IsActive { get; set; }

    public virtual PropertyType? PropertyType { get; set; }
    public virtual Company? Company { get; set; }
    public virtual EstateAgentInCompany? EstateAgent { get; set; }
    public virtual ICollection<Inventory> Inventories { get; set; }
    public virtual ICollection<Environment> Environments { get; set; }
}
