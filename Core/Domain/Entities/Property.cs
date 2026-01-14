namespace Elyssa.Core.Domain.Entities;

public class Property : BaseEntity
{
    public string Address { get; set; } = string.Empty;
    public Guid CompanyId { get; set; }
    public Guid PropertyTypeId { get; set; }
    public decimal BuiltArea { get; set; }
    public decimal? LotArea { get; set; }
    public string Neighborhood { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string City { get; set; } = string.Empty;
    public bool IsRented { get; set; }
    public long? EstateAgentInCompanyId { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? Detail { get; set; }
    public int Levels { get; set; }
    public string Country { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    public virtual PropertyType? PropertyType { get; set; }
    public virtual Company? Company { get; set; }
    public virtual EstateAgentInCompany? EstateAgent { get; set; }
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    public virtual ICollection<Environment> Environments { get; set; } = new List<Environment>();
}
