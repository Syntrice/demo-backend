namespace DemoBackend.Common.Results;

public readonly struct Result<T>
{
    private readonly T _value;
    private readonly List<Error> _errors;

    public T Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("Result is Failure and so no value is provided.");

    public IReadOnlyList<Error> Errors => _errors;
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private Result(T value)
    {
        _value = value;
        _errors = [];
        IsSuccess = true;
    }

    private Result(List<Error> errors)
    {
        _value = default!;
        _errors = errors;
        IsSuccess = false;
    }

    // Convenience methods
    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    public static Result<T> Failure(params Error[] errs)
    {
        return new Result<T>(errs.ToList());
    }

    // Define implicit conversion operators
    public static implicit operator Result<T>(T value)
    {
        return Success(value);
    }

    public static implicit operator Result<T>(Error err)
    {
        return Failure(err);
    }
}