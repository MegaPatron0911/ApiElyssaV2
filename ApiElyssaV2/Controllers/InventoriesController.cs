using Asp.Versioning;
using Elyssa.Core.DTOs;
using Elyssa.Core.Interfaces;
using Elyssa.PublicApi.Extensions;
using Elyssa.PublicApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Elyssa.PublicApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/inventories")]
[ApiVersion("1.0")]
[ValidateCompany]
public class InventoriesController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    private readonly ILogger<InventoriesController> _logger;

    public InventoriesController(
        IInventoryService inventoryService,
        ILogger<InventoriesController> logger)
    {
        _inventoryService = inventoryService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene la información detallada de un inventario específico
    /// </summary>
    [HttpGet("details")]
    [ProducesResponseType(typeof(ApiResponse<InventoryDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetInventoryDetail(
        [FromHeader(Name = "x-company-id")] Guid companyId,
        [FromQuery] InventoryDetailRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Processing inventory detail request for inventory {InventoryId} and company {CompanyId}", 
            request.InventoryId, companyId);

        var result = await _inventoryService.GetDetailAsync(
            request.InventoryId,
            companyId,
            cancellationToken);

        return result.Match<IActionResult>(
            success => Ok(new ApiResponse<InventoryDetailResponse>
            {
                Success = true,
                Data = success,
                Timestamp = DateTime.UtcNow
            }),
            error => error.ToApiErrorResponse());
    }
}
