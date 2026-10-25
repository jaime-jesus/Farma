namespace PharmaCareBlazor.Common;

public class Result
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public static Result Success(string message = "Operación completada con éxito.") => new(true, message);
    public static Result Failure(string message) => new(false, message);
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, string message) : base(isSuccess, message)
    {
        Value = value;
    }

    public static Result<T> Success(T value, string message = "Operación completada con éxito.") => new(true, value, message);
    public static new Result<T> Failure(string message) => new(false, default, message);
}
