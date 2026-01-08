namespace Elyssa.Core.DTOs;

/// <summary>
/// Response wrapper para endpoints públicos
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; } = true;
    public T? Data { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Response de error para endpoints públicos
/// </summary>
public class ApiErrorResponse
{
    public bool Success { get; set; } = false;
    public ErrorDetail Error { get; set; } = null!;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class ErrorDetail
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
}
