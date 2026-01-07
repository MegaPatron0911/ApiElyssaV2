using Microsoft.AspNetCore.Mvc;

namespace Elyssa.PublicApi.Controllers;

/// <summary>
/// Controlador base con funcionalidades comunes
/// </summary>
[ApiController]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Obtiene el Company ID del contexto HTTP
    /// </summary>
    protected Guid? CompanyId
    {
        get
        {
            if (HttpContext.Items.TryGetValue("CompanyId", out var companyId) && companyId is Guid id)
            {
                return id;
            }
            return null;
        }
    }

    /// <summary>
    /// Obtiene el User ID del contexto HTTP (si está autenticado)
    /// </summary>
    protected string? UserId => User?.FindFirst("sub")?.Value;
}
