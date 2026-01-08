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
        var timestamp = DateTime.UtcNow;
        var result = await _companyService.GetBasicInfoAsync(companyId, cancellationToken);

        return result.Match(
            success => Ok(new ApiResponse<CompanyBasicInfoDto>
            {
                Success = true,
                Data = success,
                Timestamp = timestamp
            }),
            error => error.Type switch
            {
                Core.Common.ErrorType.NotFound => NotFound(new ApiErrorResponse
                {
                    Success = false,
                    Error = new ErrorDetail
                    {
                        Code = "COMPANY_NOT_FOUND",
                        Message = "La empresa especificada no existe",
                        Details = "No se encontró ninguna empresa con el ID proporcionado en el header x-company-id"
                    },
                    Timestamp = timestamp
                }),
                Core.Common.ErrorType.Validation => BadRequest(new ApiErrorResponse
                {
                    Success = false,
                    Error = new ErrorDetail
                    {
                        Code = "COMPANY_INACTIVE",
                        Message = error.Message,
                        Details = "La empresa no está activa para consultar su información"
                    },
                    Timestamp = timestamp
                }),
                _ => StatusCode(500, new ApiErrorResponse
                {
                    Success = false,
                    Error = new ErrorDetail
                    {
                        Code = "INTERNAL_ERROR",
                        Message = "Error interno del servidor",
                        Details = error.Message
                    },
                    Timestamp = timestamp
                })
            }
        );
    }
}
