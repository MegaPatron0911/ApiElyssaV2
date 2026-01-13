using Elyssa.Core.Common.Constants;

namespace Elyssa.Core.DTOs;

public class PropertyFilterDto
{
    public int Page { get; set; } = PropertyConstants.DEFAULT_PAGE_NUMBER;
    public int PageSize { get; set; } = PropertyConstants.DEFAULT_PAGE_SIZE;
    public string? Code { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string SortBy { get; set; } = PropertySortFields.CREATED_AT;
    public string SortOrder { get; set; } = Common.Constants.SortOrder.DESCENDING;
}
