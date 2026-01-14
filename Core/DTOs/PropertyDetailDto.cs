namespace Elyssa.Core.DTOs;

public class PropertyDetailResponseDto
{
    public Guid PropertyId { get; set; }
    public string? Code { get; set; }
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public bool IsRented { get; set; }
    public decimal BuiltArea { get; set; }
    public decimal LotArea { get; set; }
    public int Levels { get; set; }
    public string? Detail { get; set; }
    public PropertyLocationDto Location { get; set; } = new();
    public PropertyTypeDetailDto PropertyType { get; set; } = new();
    public PropertyStatsDto Stats { get; set; } = new();
    public string? EstateAgentName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}

public class PropertyLocationDto
{
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}

public class PropertyTypeDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class PropertyStatsDto
{
    public int TotalEnvironments { get; set; }
    public int TotalInventories { get; set; }
}
