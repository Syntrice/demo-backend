namespace DemoBackend.Common.Results;

public sealed record Error(ErrorCode Code, string Description)
{
    // Convenience methods for quickly creating errors
    public static Error Validation(string desc)
    {
        return new Error(ErrorCode.Validation, desc);
    }

    public static Error NotFound(string desc)
    {
        return new Error(ErrorCode.NotFound, desc);
    }

    public static Error Conflict(string desc)
    {
        return new Error(ErrorCode.Conflict, desc);
    }

    public static Error Unauthorized(string desc)
    {
        return new Error(ErrorCode.Unauthorized, desc);
    }

    public static Error Forbidden(string desc)
    {
        return new Error(ErrorCode.Forbidden, desc);
    }

    public static Error Unexpected(string desc)
    {
        return new Error(ErrorCode.Unexpected, desc);
    }
}