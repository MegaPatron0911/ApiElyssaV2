namespace Elyssa.PublicApi.Middleware;

/// <summary>
/// Middleware para extraer el Company ID del header y agregarlo al contexto
/// </summary>
public class CompanyContextMiddleware
{
    private readonly RequestDelegate _next;
    private const string CompanyIdHeader = "X-Company-Id";

    public CompanyContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(CompanyIdHeader, out var companyIdValue))
        {
            if (Guid.TryParse(companyIdValue, out var companyId))
            {
                // Almacenar en HttpContext.Items para uso en toda la petición
                context.Items["CompanyId"] = companyId;
            }
        }

        await _next(context);
    }
}

/// <summary>
/// Extensión para registrar el middleware
/// </summary>
public static class CompanyContextMiddlewareExtensions
{
    public static IApplicationBuilder UseCompanyContext(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CompanyContextMiddleware>();
    }
}
