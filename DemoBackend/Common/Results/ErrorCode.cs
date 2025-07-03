namespace DemoBackend.Common.Results;

// Error types
public enum ErrorCode
{
    None = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    Unauthorized = 4,
    Forbidden = 5,
    Unexpected = 6
}