using Asp.Versioning;
using Elyssa.Core.DTOs;
using Elyssa.Core.Interfaces;
using Elyssa.PublicApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Elyssa.PublicApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Produces("application/json")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    /// <summary>
    /// Obtiene información básica de la empresa autenticada
    /// </summary>
    [HttpGet("info")]
    [ProducesResponseType(typeof(ApiResponseDto<CompanyBasicInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCompanyBasicInfo(
        [FromHeader(Name = "x-company-id")] Guid companyId,
        CancellationToken cancellationToken)
    {
        var result = await _companyService.GetBasicInfoAsync(companyId, cancellationToken);

        return result.Match(
            success => Ok(new ApiResponseDto<CompanyBasicInfoDto>
            {
                Success = true,
                Data = success,
                Timestamp = DateTime.UtcNow
            }),
            error => error.ToApiErrorResponse());
    }
}
