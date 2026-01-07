using Core.DTO;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BackOfficeElyssa.Extensions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackOfficeElyssa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ICompanyQueryService _companyQueryService;

        public CompanyController(ICompanyService companyService, ICompanyQueryService companyQueryService)
        {
            _companyService = companyService;
            _companyQueryService = companyQueryService;
        }

        /// <summary>
        /// Obtiene información básica de la empresa autenticada
        /// </summary>
        /// <param name="companyId">Id de la empresa desde el header x-company-id</param>
        [HttpGet("info")]
        [Authorize]
        public async Task<ActionResult<CompanyBasicInfoResponseDto>> GetCompanyBasicInfo([FromHeader(Name = "x-company-id")] Guid companyId)
        {
            var companyInfo = await _companyQueryService.GetCompanyBasicInfoAsync(companyId);

            if (companyInfo == null)
            {
                return NotFound(new ErrorResponseDto
                {
                    Error = new ErrorDetailDto
                    {
                        Code = "COMPANY_NOT_FOUND",
                        Message = "La empresa especificada no existe",
                        Details = "No se encontró ninguna empresa con el ID proporcionado en el header x-company-id"
                    }
                });
            }

            return Ok(new CompanyBasicInfoResponseDto
            {
                Data = companyInfo
            });
        }

        /// <summary>
        /// Este metodo obtiene la informacion de una empresa dado el Id
        /// </summary>
        /// <param name="companyId">Id de la empresa</param>
        /// <returns></returns>
        [HttpGet("CompanyInformation/{companyId}")]
        public async Task<ActionResult<CompanyInformationDTO>> GetCompanyInformationAsync(Guid companyId)
        {
            var companyInformation = await _companyQueryService.GetCompanyInformationAsync(companyId);

            if (companyInformation == null)
            {
                return NotFound();
            }

            return Ok(companyInformation);
        }

        /// <summary>
        /// Este metodo crea una nueva empresa
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        // POST api/<EmpresaController>
        [HttpPost("CreateCompany")]
        public async Task<IActionResult> Post([FromForm] CreateCompanyDTO company)
        {
            var companyId = await _companyService.CreateCompany(company);
            return Ok(new { CompanyId = companyId });
        }

        [HttpPut("UpdateCompany/{companyId}")]
        public async Task<IActionResult> UpdateCompany(Guid companyId, UpdateCompanyDTO updateCompany)
        {
            try
            {
                await _companyService.UpdateCompany(companyId, updateCompany);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Permite actualizar el logo de una empresa
        /// </summary>
        /// <param name="updateLogo"></param>
        /// <returns></returns>
        [HttpPut("UpdateLogoCompany")]
        public async Task<IActionResult> UpdateLogoCompany([FromForm] UpdateLogoDto updateLogo)
        {
            try
            {
                var logoResponse = await _companyService.UpdateLogoCompany(updateLogo);
                return Ok(logoResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Este metodo permite actualizar los textos legales de una empresa
        /// </summary>
        /// <param name="updateLegalText"></param>
        /// <returns></returns>
        [HttpPut("UpdateLegalText")]
        public async Task<IActionResult> UpdateLegalText(UpdateLegalTextDto updateLegalText)
        {
            try
            {
                await _companyService.UpdateLegalText(updateLegalText);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
