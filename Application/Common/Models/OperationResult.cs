namespace Application.Common.Models;

/// <summary>
/// Represents the result of an operation with success/failure status and optional data
/// </summary>
/// <typeparam name="T">The type of data returned on success</typeparam>
public class OperationResult<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public List<string> Errors { get; set; } = new();

    public static OperationResult<T> Success(T data)
    {
        return new OperationResult<T>
        {
            IsSuccess = true,
            Data = data
        };
    }

    public static OperationResult<T> Failure(string errorMessage)
    {
        return new OperationResult<T>
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            Errors = new List<string> { errorMessage }
        };
    }

    public static OperationResult<T> Failure(List<string> errors)
    {
        return new OperationResult<T>
        {
            IsSuccess = false,
            ErrorMessage = string.Join(", ", errors),
            Errors = errors
        };
    }
}

/// <summary>
/// Operation result without data (for void operations like Delete)
/// </summary>
public class OperationResult
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public List<string> Errors { get; set; } = new();

    public static OperationResult Success()
    {
        return new OperationResult { IsSuccess = true };
    }

    public static OperationResult Failure(string errorMessage)
    {
        return new OperationResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            Errors = new List<string> { errorMessage }
        };
    }

    public static OperationResult Failure(List<string> errors)
    {
        return new OperationResult
        {
            IsSuccess = false,
            ErrorMessage = string.Join(", ", errors),
            Errors = errors
        };
    }
}