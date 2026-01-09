using Elyssa.Core.DTOs;
using Elyssa.Core.Interfaces;
using Elyssa.PublicApi.Extensions;
using Elyssa.PublicApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Elyssa.PublicApi.Controllers;

[ApiController]
[Route("api/v1/properties")]
[ValidateCompany]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService _propertyService;
    private readonly ILogger<PropertiesController> _logger;

    public PropertiesController(
        IPropertyService propertyService,
        ILogger<PropertiesController> logger)
    {
        _propertyService = propertyService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene un listado paginado de propiedades con opciones de filtrado y ordenamiento
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PropertyListResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProperties(
        [FromHeader(Name = "x-company-id")] Guid companyId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? code = null,
        [FromQuery] string? address = null,
        [FromQuery] string? city = null,
        [FromQuery] string sortBy = "createdAt",
        [FromQuery] string sortOrder = "desc",
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Processing properties request for company {CompanyId}", companyId);

        var filter = new PropertyFilterDto
        {
            Page = page,
            PageSize = pageSize,
            Code = code,
            Address = address,
            City = city,
            SortBy = sortBy,
            SortOrder = sortOrder
        };

        var result = await _propertyService.GetPagedPropertiesAsync(
            companyId,
            filter,
            cancellationToken);

        return result.Match<IActionResult>(
            success => Ok(new ApiResponse<PropertyListResponse>
            {
                Success = true,
                Data = success,
                Timestamp = DateTime.UtcNow
            }),
            error => error.ToApiErrorResponse());
    }
}
