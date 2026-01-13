using AutoMapper;
using Elyssa.Core.Common;
using Elyssa.Core.Common.Constants;
using Elyssa.Core.DTOs;
using Elyssa.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Elyssa.Core.Services;

public class InventoryService : IInventoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<InventoryService> _logger;

    private static readonly HashSet<string> ValidSortFields = new(StringComparer.OrdinalIgnoreCase)
    {
        InventorySortFields.CREATED_AT,
        InventorySortFields.SIGNATURE_DATE,
        InventorySortFields.RENTAL_PRICE
    };

    public InventoryService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<InventoryService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<InventoryListResponse>> GetPagedInventoriesAsync(
        Guid companyId,
        InventoryFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching inventories for company {CompanyId}", companyId);

        var validationResult = ValidateFilter(filter);
        if (!validationResult.IsSuccess)
        {
            return Result<InventoryListResponse>.Failure(validationResult.Error!);
        }

        var pageSize = Math.Min(filter.PageSize, InventoryConstants.MAX_PAGE_SIZE);
        var page = Math.Max(filter.Page, InventoryConstants.MIN_PAGE_NUMBER);

        try
        {
            var (inventories, totalCount) = await _unitOfWork.Inventories
                .GetPagedAsync(
                    companyId,
                    page,
                    pageSize,
                    filter.InventoryType,
                    filter.IsSigned,
                    filter.SortBy,
                    filter.SortOrder.ToLowerInvariant(),
                    cancellationToken)
                .ConfigureAwait(false);

            var inventoryList = inventories.Select(inventory =>
            {
                var inventoryTypeName = inventory.InventoryType switch
                {
                    InventoryType.Captacion => InventoryTypeNames.Captacion,
                    InventoryType.Colocación => InventoryTypeNames.Colocacion,
                    InventoryType.PreVisita => InventoryTypeNames.PreVisita,
                    InventoryType.Desocupación => InventoryTypeNames.Desocupación,
                    _ => "Desconocido"
                };

                string? pdfDownloadUrl = null;
                if (inventory.IsSigned && !string.IsNullOrWhiteSpace(inventory.PdfUrl))
                {
                    pdfDownloadUrl = inventory.PdfUrl;
                }

                return new InventoryResponse
                {
                    InventoryId = inventory.Id,
                    Property = new InventoryPropertyInfoDto
                    {
                        PropertyId = inventory.Property!.Id,
                        Code = inventory.Property.Code,
                        Address = inventory.Property.Address,
                        City = inventory.Property.City
                    },
                    InventoryType = inventory.InventoryType,
                    InventoryTypeName = inventoryTypeName,
                    IsSigned = inventory.IsSigned,
                    IsRemoteSigned = inventory.IsRemoteSigned,
                    RentalPrice = inventory.RentalPrice,
                    Currency = inventory.Currency,
                    PdfDownloadUrl = pdfDownloadUrl,
                    CreatedAt = inventory.CreatedAt,
                    SignatureDate = inventory.SignatureDate
                };
            }).ToList();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var hasNextPage = page < totalPages;
            var hasPreviousPage = page > InventoryConstants.MIN_PAGE_NUMBER;
            var remainingRecords = totalCount - (page * pageSize);

            var response = new InventoryListResponse
            {
                Inventories = inventoryList,
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

            _logger.LogDebug("Retrieved {Count} inventories, page {Page}/{TotalPages}",
                inventoryList.Count, page, totalPages);

            return Result<InventoryListResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching inventories for company {CompanyId}", companyId);
            throw;
        }
    }

    public async Task<Result<InventoryDetailResponse>> GetDetailAsync(
        Guid inventoryId,
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching inventory detail {InventoryId} for company {CompanyId}", 
            inventoryId, companyId);

        try
        {
            var inventory = await _unitOfWork.Inventories
                .GetDetailByIdAsync(inventoryId, cancellationToken)
                .ConfigureAwait(false);

            if (inventory == null)
            {
                _logger.LogWarning("Inventory {InventoryId} not found", inventoryId);
                return Result<InventoryDetailResponse>.Failure(InventoryErrors.NotFound(inventoryId));
            }

            if (inventory.Property == null || inventory.Property.CompanyId != companyId)
            {
                _logger.LogWarning("Inventory {InventoryId} does not belong to a property of company {CompanyId}", 
                    inventoryId, companyId);
                return Result<InventoryDetailResponse>.Failure(InventoryErrors.ForbiddenResource(inventoryId, companyId));
            }

            var totalEnvironments = await _unitOfWork.Inventories
                .CountEnvironmentsByInventoryAsync(inventoryId, cancellationToken)
                .ConfigureAwait(false);
            
            var totalItems = await _unitOfWork.Inventories
                .CountItemsByInventoryAsync(inventoryId, cancellationToken)
                .ConfigureAwait(false);

            var inventoryTypeName = inventory.InventoryType switch
            {
                InventoryType.Captacion => InventoryTypeNames.Captacion,
                InventoryType.Colocación => InventoryTypeNames.Colocacion,
                InventoryType.PreVisita => InventoryTypeNames.PreVisita,
                InventoryType.Desocupación => InventoryTypeNames.Desocupación,
                _ => "Desconocido"
            };

            var response = new InventoryDetailResponse
            {
                InventoryId = inventory.Id,
                Property = new InventoryPropertyDto
                {
                    PropertyId = inventory.Property.Id,
                    Code = inventory.Property.Code,
                    Address = inventory.Property.Address,
                    City = inventory.Property.City,
                    Neighborhood = inventory.Property.Neighborhood
                },
                InventoryType = inventory.InventoryType,
                InventoryTypeName = inventoryTypeName,
                IsSigned = inventory.IsSigned,
                IsRemoteSigned = inventory.IsRemoteSigned,
                RentalPrice = inventory.RentalPrice,
                Currency = inventory.Currency,
                ApprovalCode = inventory.ApprovalCode,
                Signatures = new InventorySignaturesDto
                {
                    AgentSignatureDate = inventory.AgentSignatureDate,
                    OwnerSignatureDate = inventory.OwnerSignatureDate,
                    SignatureDate = inventory.SignatureDate
                },
                PdfDownloadUrl = inventory.PdfUrl,
                Stats = new InventoryStatsDto
                {
                    TotalEnvironments = totalEnvironments,
                    TotalItems = totalItems
                },
                CreatedAt = inventory.CreatedAt
            };

            _logger.LogDebug("Inventory detail retrieved successfully for {InventoryId}", inventoryId);
            return Result<InventoryDetailResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching inventory detail {InventoryId} for company {CompanyId}", 
                inventoryId, companyId);
            throw;
        }
    }

    private static Result ValidateFilter(InventoryFilterDto filter)
    {
        if (filter.Page < InventoryConstants.MIN_PAGE_NUMBER)
        {
            return Result.Failure(InventoryErrors.InvalidPageNumber);
        }

        if (filter.PageSize < InventoryConstants.MIN_PAGE_SIZE || filter.PageSize > InventoryConstants.MAX_PAGE_SIZE)
        {
            return Result.Failure(InventoryErrors.InvalidPageSize);
        }

        if (!ValidSortFields.Contains(filter.SortBy))
        {
            return Result.Failure(InventoryErrors.InvalidSortBy);
        }

        var sortOrder = filter.SortOrder.ToLowerInvariant();
        if (sortOrder != SortOrder.ASCENDING && sortOrder != SortOrder.DESCENDING)
        {
            return Result.Failure(InventoryErrors.InvalidSortOrder);
        }

        if (filter.InventoryType.HasValue && (filter.InventoryType.Value < 0 || filter.InventoryType.Value > 3))
        {
            return Result.Failure(InventoryErrors.InvalidInventoryType);
        }

        return Result.Success();
    }
}
