using Elyssa.Core.DTOs;
using Elyssa.Core.Interfaces;
using Elyssa.PublicApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Elyssa.PublicApi.Controllers;

[Route("api/v1/[controller]")]
[Produces("application/json")]
[ApiController]
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
    [ProducesResponseType(typeof(ApiResponse<CompanyBasicInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCompanyBasicInfo(
        [FromHeader(Name = "x-company-id")] Guid companyId,
        CancellationToken cancellationToken)
    {
        var result = await _companyService.GetBasicInfoAsync(companyId, cancellationToken);

        return result.Match(
            success => Ok(new ApiResponse<CompanyBasicInfoDto>
            {
                Success = true,
                Data = success,
                Timestamp = DateTime.UtcNow
            }),
            error => error.ToApiErrorResponse());
    }
}
