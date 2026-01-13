using Asp.Versioning;
using Elyssa.Core.DTOs;
using Elyssa.Core.Interfaces;
using Elyssa.PublicApi.Extensions;
using Elyssa.PublicApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Elyssa.PublicApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/properties")]
[ApiVersion("1.0")]
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

    /// <summary>
    /// Obtiene la información detallada de una propiedad específica
    /// </summary>
    /// <param name="companyId">ID de la compañía (desde header x-company-id)</param>
    /// <param name="request">Request con el ID de la propiedad</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Información detallada de la propiedad incluyendo ubicación, tipo y estadísticas</returns>
    /// <remarks>
    /// La propiedad debe pertenecer a la misma empresa del header x-company-id.
    /// 
    /// Ejemplo de request:
    /// 
    ///     GET /api/v1/properties/details?propertyId=123e4567-e89b-12d3-a456-426614174000
    ///     Headers:
    ///       x-company-id: 456e4567-e89b-12d3-a456-426614174000
    /// 
    /// Respuestas:
    /// - 200 OK: Propiedad encontrada y pertenece a la compañía
    /// - 400 Bad Request: propertyId inválido o vacío (código: INVALID_PROPERTY_ID)
    /// - 403 Forbidden: Propiedad existe pero no pertenece a la compañía (código: FORBIDDEN_RESOURCE)
    /// - 404 Not Found: Propiedad no existe (código: PROPERTY_NOT_FOUND)
    /// </remarks>
    /// <response code="200">Devuelve la información detallada de la propiedad</response>
    /// <response code="400">Si el propertyId es inválido o vacío (INVALID_PROPERTY_ID)</response>
    /// <response code="403">Si la propiedad no pertenece a la compañía especificada (FORBIDDEN_RESOURCE)</response>
    /// <response code="404">Si la propiedad no existe o no está activa (PROPERTY_NOT_FOUND)</response>
    [HttpGet("details")]
    [ProducesResponseType(typeof(ApiResponse<PropertyDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPropertyDetail(
        [FromHeader(Name = "x-company-id")] Guid companyId,
        [FromQuery] PropertyDetailRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Processing property detail request for property {PropertyId} and company {CompanyId}", 
            request.PropertyId, companyId);

        var result = await _propertyService.GetDetailAsync(
            request.PropertyId,
            companyId,
            cancellationToken);

        return result.Match<IActionResult>(
            success => Ok(new ApiResponse<PropertyDetailResponse>
            {
                Success = true,
                Data = success,
                Timestamp = DateTime.UtcNow
            }),
            error => error.ToApiErrorResponse());
    }
}
