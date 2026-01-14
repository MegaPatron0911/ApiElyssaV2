namespace Elyssa.Core.DTOs;

public class ApiResponseDto
{
    public bool Success { get; set; } = true;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class ApiResponseDto<T> : ApiResponseDto
{
    public T Data { get; set; } = default!;
}

public class ApiErrorResponseDto
{
    public bool Success { get; set; } = false;
    public ErrorDetailDto Error { get; set; } = null!;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class ErrorDetailDto
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string? Field { get; set; }
}
