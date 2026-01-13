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

    public InventoryService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<InventoryService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
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

            var environmentsTask = _unitOfWork.Inventories
                .CountEnvironmentsByInventoryAsync(inventoryId, cancellationToken);
            
            var itemsTask = _unitOfWork.Inventories
                .CountItemsByInventoryAsync(inventoryId, cancellationToken);

            await Task.WhenAll(environmentsTask, itemsTask).ConfigureAwait(false);

            var totalEnvironments = await environmentsTask;
            var totalItems = await itemsTask;

            var inventoryTypeName = inventory.InventoryType switch
            {
                InventoryType.PLACEMENT => InventoryTypeNames.PLACEMENT,
                InventoryType.WITHDRAWAL => InventoryTypeNames.WITHDRAWAL,
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
}
