using Elyssa.Core.Common;
using Microsoft.AspNetCore.Mvc;

namespace Elyssa.PublicApi.Extensions;

public static class ErrorExtensions
{
    public static IActionResult ToHttpResponse(this Error error)
    {
        var problemDetails = new ProblemDetails
        {
            Title = GetTitle(error.Type),
            Status = GetStatusCode(error.Type),
            Detail = error.Message,
            Type = error.Code
        };

        return error.Type switch
        {
            ErrorType.Validation => new BadRequestObjectResult(problemDetails),
            ErrorType.NotFound => new NotFoundObjectResult(problemDetails),
            ErrorType.Conflict => new ConflictObjectResult(problemDetails),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(problemDetails),
            ErrorType.Forbidden => new ObjectResult(problemDetails) { StatusCode = 403 },
            _ => new ObjectResult(problemDetails) { StatusCode = 500 }
        };
    }

    private static string GetTitle(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => "Validation Error",
        ErrorType.NotFound => "Resource Not Found",
        ErrorType.Conflict => "Conflict",
        ErrorType.Unauthorized => "Unauthorized",
        ErrorType.Forbidden => "Forbidden",
        _ => "Internal Server Error"
    };

    private static int GetStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        _ => StatusCodes.Status500InternalServerError
    };
}
