namespace GolMetrics.API.Core.Results;

public class Result
{
    protected Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public Error? Error { get; }

    public static Result Success() => new(true, null);
    public static Result Failure(Error error) => new(false, error);
}

public class Result<T> : Result
{
    private Result(T value) : base(true, null)
    {
        Value = value;
    }

    private Result(Error error) : base(false, error)
    {
        Value = default;
    }

    public T? Value { get; }

    public new static Result<T> Failure(Error error) => new(error);

    public static implicit operator Result<T>(T value) => new(value);
}