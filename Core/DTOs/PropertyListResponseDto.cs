namespace Elyssa.Core.DTOs;

public class PageInfo
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

public class PropertyListResponse
{
    public List<PropertyResponse> Properties { get; set; } = new();
    public PageInfo Pagination { get; set; } = new();
}
