namespace Battleship.Application.Common;

/// <summary>
/// Represents the result of an operation, containing a value on success or an error message on failure.
/// </summary>
/// <typeparam name="T">The type of the value returned on success.</typeparam>
public readonly record struct Result<T>(
    bool IsSuccess,
    T? Value = default,
    string? Error = null
)
{
    /// <summary>
    /// Gets a value indicating whether the result is a failure.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Creates a successful result containing the specified value.
    /// </summary>
    /// <param name="value">The value to return on success.</param>
    /// <returns>A successful <see cref="Result{T}"/>.</returns>
    public static Result<T> Success(T value) =>
        new(true, value);

    /// <summary>
    /// Creates a failed result with the specified error message.
    /// </summary>
    /// <param name="error">The error message.</param>
    /// <returns>A failed <see cref="Result{T}"/>.</returns>
    public static Result<T> Failure(string error) =>
        new(false, default, error);

    /// <summary>
    /// Implicitly converts the result to a boolean indicating success.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    public static implicit operator bool(Result<T> result) =>
        result.IsSuccess;

    /// <summary>
    /// Returns a string representation of the result.
    /// </summary>
    /// <returns>A string describing the result.</returns>
    public override string ToString() =>
        IsSuccess
            ? $"Success({Value})"
            : $"Failure({Error})";
}

/// <summary>
/// Represents the result of an operation that does not return a value, containing an error message on failure.
/// </summary>
public readonly record struct Result(
    bool IsSuccess,
    string? Error = null
)
{
    /// <summary>
    /// Gets a value indicating whether the result is a failure.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <returns>A successful <see cref="Result"/>.</returns>
    public static Result Success() =>
        new(true);

    /// <summary>
    /// Creates a failed result with the specified error message.
    /// </summary>
    /// <param name="error">The error message.</param>
    /// <returns>A failed <see cref="Result"/>.</returns>
    public static Result Failure(string error) =>
        new(false, error);

    /// <summary>
    /// Implicitly converts the result to a boolean indicating success.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    public static implicit operator bool(Result result) =>
        result.IsSuccess;

    /// <summary>
    /// Returns a string representation of the result.
    /// </summary>
    /// <returns>A string describing the result.</returns>
    public override string ToString() =>
        IsSuccess ? "Success" : $"Failure({Error})";
}
