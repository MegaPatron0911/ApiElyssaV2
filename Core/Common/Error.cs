namespace Elyssa.Core.Common;

public sealed record Error
{
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }
    public string? Field { get; }

    private Error(string code, string message, ErrorType type, string? field = null)
    {
        Code = code;
        Message = message;
        Type = type;
        Field = field;
    }

    public static Error NotFound(string code, string message, string? field = null) =>
        new(code, message, ErrorType.NotFound, field);

    public static Error Validation(string code, string message, string? field = null) =>
        new(code, message, ErrorType.Validation, field);

    public static Error Conflict(string code, string message, string? field = null) =>
        new(code, message, ErrorType.Conflict, field);

    public static Error Failure(string code, string message, string? field = null) =>
        new(code, message, ErrorType.Failure, field);

    public static Error Unauthorized(string code, string message, string? field = null) =>
        new(code, message, ErrorType.Unauthorized, field);

    public static Error Forbidden(string code, string message, string? field = null) =>
        new(code, message, ErrorType.Forbidden, field);

    public static Error UnprocessableEntity(string code, string message, string? field = null) =>
        new(code, message, ErrorType.UnprocessableEntity, field);

    public static Error ServiceUnavailable(string code, string message, string? field = null) =>
        new(code, message, ErrorType.ServiceUnavailable, field);
}

public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    Unauthorized = 4,
    Forbidden = 5,
    UnprocessableEntity = 6,
    ServiceUnavailable = 7
}
