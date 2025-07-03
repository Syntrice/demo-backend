namespace DemoBackend.Common.Results;

public sealed record Error(ErrorCode Code, string Description)
{
    // Convenience methods for quickly creating errors
    public static Error Validation(string desc) => new(ErrorCode.Validation, desc);
    public static Error NotFound(string desc) => new(ErrorCode.NotFound, desc);
    public static Error Conflict(string desc) => new(ErrorCode.Conflict, desc);
    public static Error Unauthorized(string desc) => new(ErrorCode.Unauthorized, desc);
    public static Error Forbidden(string desc) => new(ErrorCode.Forbidden, desc);
    public static Error Unexpected(string desc) => new(ErrorCode.Unexpected, desc);
}