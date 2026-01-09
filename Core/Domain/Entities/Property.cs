namespace Elyssa.Core.Domain.Entities;

public class Property : BaseEntity
{
    public string? Code { get; set; }
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public bool IsRented { get; set; }
    public decimal BuiltArea { get; set; }
    public decimal LotArea { get; set; }
    public int Levels { get; set; }
    public Guid PropertyTypeId { get; set; }
    public Guid CompanyId { get; set; }
    public bool IsActive { get; set; } = true;
    
    public PropertyType? PropertyType { get; set; }
    public Company? Company { get; set; }
}
