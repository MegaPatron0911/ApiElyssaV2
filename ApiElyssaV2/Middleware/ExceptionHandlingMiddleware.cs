using Elyssa.Core.DTOs;
using System.Net;
using System.Text.Json;

namespace Elyssa.PublicApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var isDevelopment = context.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() ?? false;
        
        var errorResponse = new ApiErrorResponseDto
        {
            Success = false,
            Error = new ErrorDetailDto
            {
                Code = "INTERNAL_SERVER_ERROR",
                Message = "Ocurrió un error interno en el servidor",
                Details = isDevelopment 
                    ? $"ERROR: {exception.Message}\n\nSTACK TRACE:\n{exception.StackTrace}\n\nINNER EXCEPTION:\n{exception.InnerException?.Message}\n{exception.InnerException?.StackTrace}"
                    : $"TraceId: {context.TraceIdentifier}. Por favor contacte al administrador."
            },
            Timestamp = DateTime.UtcNow
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(errorResponse, options);
        return context.Response.WriteAsync(json);
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
