using AutoMapper;
using Elyssa.Core.Common;
using Elyssa.Core.Common.Constants;
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
        PropertySortFields.CREATED_AT,
        PropertySortFields.CODE,
        PropertySortFields.ADDRESS,
        PropertySortFields.CITY
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

        var pageSize = Math.Min(filter.PageSize, PropertyConstants.MAX_PAGE_SIZE);
        var page = Math.Max(filter.Page, PropertyConstants.MIN_PAGE_NUMBER);

        try
        {
            var (properties, totalCount) = await _unitOfWork.Properties
                .GetPagedAsync(
                    companyId,
                    page,
                    pageSize,
                    filter.Code,
                    filter.Address,
                    filter.City,
                    filter.SortBy,
                    filter.SortOrder.ToLowerInvariant(),
                    cancellationToken)
                .ConfigureAwait(false);

            var propertyList = _mapper.Map<List<PropertyResponse>>(properties);

            if (propertyList.Any())
            {
                var propertyIds = propertyList.Select(p => p.PropertyId).ToList();
                var inventoryMap = await _unitOfWork.Properties
                    .GetInventoriesExistenceAsync(propertyIds, cancellationToken)
                    .ConfigureAwait(false);

                foreach (var property in propertyList)
                {
                    property.HasInventories = inventoryMap.TryGetValue(property.PropertyId, out var hasInventories) 
                        && hasInventories;
                }
            }

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var hasNextPage = page < totalPages;
            var hasPreviousPage = page > PropertyConstants.MIN_PAGE_NUMBER;
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

    public async Task<Result<PropertyDetailResponse>> GetDetailAsync(
        Guid propertyId,
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching property detail {PropertyId} for company {CompanyId}", 
            propertyId, companyId);

        try
        {
            var propertyWithoutFilter = await _unitOfWork.Properties
                .GetByIdWithoutCompanyFilterAsync(propertyId, cancellationToken)
                .ConfigureAwait(false);

            if (propertyWithoutFilter == null)
            {
                _logger.LogWarning("Property {PropertyId} not found", propertyId);
                return Result<PropertyDetailResponse>.Failure(PropertyErrors.NotFound(propertyId));
            }

            if (propertyWithoutFilter.CompanyId != companyId)
            {
                _logger.LogWarning("Property {PropertyId} does not belong to company {CompanyId}. Actual company: {ActualCompanyId}", 
                    propertyId, companyId, propertyWithoutFilter.CompanyId);
                return Result<PropertyDetailResponse>.Failure(PropertyErrors.ForbiddenResource(propertyId, companyId));
            }

            var property = propertyWithoutFilter;

            var totalEnvironments = await _unitOfWork.Properties
                .CountEnvironmentsByPropertyAsync(propertyId, cancellationToken)
                .ConfigureAwait(false);
            
            var totalInventories = await _unitOfWork.Properties
                .CountInventoriesByPropertyAsync(propertyId, cancellationToken)
                .ConfigureAwait(false);

            var response = _mapper.Map<PropertyDetailResponse>(property);
            
            response.Stats = new PropertyStatsDto
            {
                TotalEnvironments = totalEnvironments,
                TotalInventories = totalInventories
            };

            _logger.LogDebug("Property detail retrieved successfully for {PropertyId}", propertyId);
            return Result<PropertyDetailResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching property detail {PropertyId} for company {CompanyId}", 
                propertyId, companyId);
            throw;
        }
    }

    private static Result ValidateFilter(PropertyFilterDto filter)
    {
        if (filter.Page < PropertyConstants.MIN_PAGE_NUMBER)
        {
            return Result.Failure(PropertyErrors.InvalidPageNumber);
        }

        if (filter.PageSize < PropertyConstants.MIN_PAGE_SIZE || filter.PageSize > PropertyConstants.MAX_PAGE_SIZE)
        {
            return Result.Failure(PropertyErrors.InvalidPageSize);
        }

        if (!ValidSortFields.Contains(filter.SortBy))
        {
            return Result.Failure(PropertyErrors.InvalidSortBy);
        }

        var sortOrder = filter.SortOrder.ToLowerInvariant();
        if (sortOrder != SortOrder.ASCENDING && sortOrder != SortOrder.DESCENDING)
        {
            return Result.Failure(PropertyErrors.InvalidSortOrder);
        }

        return Result.Success();
    }
}
