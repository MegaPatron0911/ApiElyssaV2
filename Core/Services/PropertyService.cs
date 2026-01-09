using AutoMapper;
using Elyssa.Core.Common;
using Elyssa.Core.DTOs;
using Elyssa.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Elyssa.Core.Services;

public class PropertyService : IPropertyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PropertyService> _logger;

    private static readonly HashSet<string> ValidSortFields = new(StringComparer.OrdinalIgnoreCase)
    {
        "createdAt", "code", "address", "city"
    };

    public PropertyService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<PropertyService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<PropertyListResponse>> GetPagedPropertiesAsync(
        Guid companyId,
        PropertyFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching properties for company {CompanyId}", companyId);

        var validationResult = ValidateFilter(filter);
        if (!validationResult.IsSuccess)
        {
            return Result<PropertyListResponse>.Failure(validationResult.Error!);
        }

        var pageSize = Math.Min(filter.PageSize, 20);
        var page = Math.Max(filter.Page, 1);

        try
        {
            var (properties, totalCount) = await _unitOfWork.Properties.GetPagedAsync(
                companyId,
                page,
                pageSize,
                filter.Code,
                filter.Address,
                filter.City,
                filter.SortBy,
                filter.SortOrder.ToLowerInvariant(),
                cancellationToken);

            var propertyList = _mapper.Map<List<PropertyResponse>>(properties);

            if (propertyList.Any())
            {
                var propertyIds = propertyList.Select(p => p.PropertyId).ToList();
                var inventoryMap = await _unitOfWork.Properties.GetInventoriesExistenceAsync(
                    propertyIds,
                    cancellationToken);

                foreach (var property in propertyList)
                {
                    property.HasInventories = inventoryMap.TryGetValue(property.PropertyId, out var hasInventories) 
                        && hasInventories;
                }
            }

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var hasNextPage = page < totalPages;
            var hasPreviousPage = page > 1;
            var remainingRecords = totalCount - (page * pageSize);

            var response = new PropertyListResponse
            {
                Properties = propertyList,
                Pagination = new PageInfo
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalRecords = totalCount,
                    TotalPages = totalPages,
                    HasNextPage = hasNextPage,
                    HasPreviousPage = hasPreviousPage,
                    NextPage = hasNextPage ? page + 1 : null,
                    RemainingRecords = Math.Max(0, remainingRecords)
                }
            };

            _logger.LogDebug("Retrieved {Count} properties, page {Page}/{TotalPages}",
                propertyList.Count, page, totalPages);

            return Result<PropertyListResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching properties for company {CompanyId}", companyId);
            throw;
        }
    }

    private static Result ValidateFilter(PropertyFilterDto filter)
    {
        if (filter.Page < 1)
        {
            return Result.Failure(PropertyErrors.InvalidPageNumber);
        }

        if (filter.PageSize < 1 || filter.PageSize > 20)
        {
            return Result.Failure(PropertyErrors.InvalidPageSize);
        }

        if (!ValidSortFields.Contains(filter.SortBy))
        {
            return Result.Failure(PropertyErrors.InvalidSortBy);
        }

        var sortOrder = filter.SortOrder.ToLowerInvariant();
        if (sortOrder != "asc" && sortOrder != "desc")
        {
            return Result.Failure(PropertyErrors.InvalidSortOrder);
        }

        return Result.Success();
    }
}
