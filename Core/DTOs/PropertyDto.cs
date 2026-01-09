namespace Elyssa.Core.DTOs;

public class PropertyResponse
{
    public Guid PropertyId { get; set; }
    public string? Code { get; set; }
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public bool IsRented { get; set; }
    public decimal BuiltArea { get; set; }
    public decimal LotArea { get; set; }
    public int Levels { get; set; }
    public PropertyTypeResponse PropertyType { get; set; } = new();
    public bool HasInventories { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}

public class PropertyTypeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
