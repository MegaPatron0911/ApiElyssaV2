namespace Elyssa.Core.DTOs;

public class PropertyFilterDto
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Code { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string SortBy { get; set; } = "createdAt";
    public string SortOrder { get; set; } = "desc";
}
