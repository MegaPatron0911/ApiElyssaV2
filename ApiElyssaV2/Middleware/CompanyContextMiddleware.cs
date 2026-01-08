namespace Elyssa.PublicApi.Middleware;

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
                context.Items["CompanyId"] = companyId;
            }
        }

        await _next(context);
    }
}

public static class CompanyContextMiddlewareExtensions
{
    public static IApplicationBuilder UseCompanyContext(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CompanyContextMiddleware>();
    }
}
