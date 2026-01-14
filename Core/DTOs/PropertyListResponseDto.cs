namespace Elyssa.Core.DTOs;

public class PageInfoDto
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
    public int? NextPage { get; set; }
    public int RemainingRecords { get; set; }
}

public class PropertyListResponseDto
{
    public List<PropertyResponseDto> Properties { get; set; } = new();
    public PageInfoDto Pagination { get; set; } = new();
}
