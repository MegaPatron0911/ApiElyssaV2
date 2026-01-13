using Elyssa.Core.Common;
using Elyssa.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Elyssa.PublicApi.Extensions;

public static class ErrorExtensions
{
    public static IActionResult ToApiErrorResponse(this Error error)
    {
        var response = new ApiErrorResponse
        {
            Success = false,
            Error = new ErrorDetail
            {
                Code = error.Code,
                Message = error.Message,
                Details = GetErrorDetails(error),
                Field = error.Field
            },
            Timestamp = DateTime.UtcNow
        };

        return error.Type switch
        {
            ErrorType.Validation => new BadRequestObjectResult(response),
            ErrorType.NotFound => new NotFoundObjectResult(response),
            ErrorType.Conflict => new ConflictObjectResult(response),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(response),
            ErrorType.Forbidden => new ObjectResult(response) { StatusCode = 403 },
            ErrorType.UnprocessableEntity => new UnprocessableEntityObjectResult(response),
            ErrorType.ServiceUnavailable => new ObjectResult(response) { StatusCode = 503 },
            _ => new ObjectResult(response) { StatusCode = 500 }
        };
    }

    private static string GetErrorDetails(Error error)
    {
        return error.Type switch
        {
            ErrorType.NotFound => $"El recurso solicitado no fue encontrado: {error.Message}",
            ErrorType.Validation => $"Validación fallida: {error.Message}",
            ErrorType.Conflict => $"Conflicto de recursos: {error.Message}",
            ErrorType.Unauthorized => "No autorizado para realizar esta operación",
            ErrorType.Forbidden => "Acceso denegado a este recurso",
            ErrorType.UnprocessableEntity => $"La entidad no se puede procesar: {error.Message}",
            ErrorType.ServiceUnavailable => $"Servicio no disponible: {error.Message}",
            _ => "Error interno del servidor"
        };
    }
}
