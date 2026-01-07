using Elyssa.Core.DTOs;
using Elyssa.Core.Interfaces;
using Elyssa.PublicApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Elyssa.PublicApi.Controllers;

[Route("api/[controller]")]
public class CompaniesController : BaseController
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    /// <summary>
    /// Obtiene todas las compañías
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CompanyDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _companyService.GetAllAsync(cancellationToken);

        return result.Match(
            success => Ok(success),
            error => error.ToHttpResponse()
        );
    }

    /// <summary>
    /// Obtiene una compañía por ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _companyService.GetByIdAsync(id, cancellationToken);

        return result.Match(
            success => Ok(success),
            error => error.ToHttpResponse()
        );
    }

    /// <summary>
    /// Crea una nueva compañía
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CompanyDto companyDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _companyService.CreateAsync(companyDto, cancellationToken);

        return result.Match(
            success => CreatedAtAction(nameof(GetById), new { id = success.Id }, success),
            error => error.ToHttpResponse()
        );
    }

    /// <summary>
    /// Actualiza una compañía existente
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] CompanyDto companyDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _companyService.UpdateAsync(id, companyDto, cancellationToken);

        return result.Match(
            () => NoContent(),
            error => error.ToHttpResponse()
        );
    }

    /// <summary>
    /// Elimina una compañía
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _companyService.DeleteAsync(id, cancellationToken);

        return result.Match(
            () => NoContent(),
            error => error.ToHttpResponse()
        );
    }
}
