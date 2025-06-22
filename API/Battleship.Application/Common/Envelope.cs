namespace Battleship.Application.Common;

/// <summary>
/// Represents a standardized envelope for API responses, encapsulating the result, errors, timestamp, and success status.
/// </summary>
/// <typeparam name="T">The type of the result contained in the envelope.</typeparam>
public readonly record struct Envelope<T>(
    T? Result,
    IReadOnlyList<string> Errors,
    DateTime TimeGenerated,
    bool Success)
{
    /// <summary>
    /// Creates a successful envelope containing the specified result.
    /// </summary>
    /// <param name="result">The result to include in the envelope.</param>
    /// <returns>An <see cref="Envelope{T}"/> indicating success.</returns>
    public static Envelope<T> Ok(T result) =>
        new(result, [], DateTime.UtcNow, true);

    /// <summary>
    /// Creates an error envelope containing a single error message.
    /// </summary>
    /// <param name="errorMessage">The error message to include.</param>
    /// <returns>An <see cref="Envelope{T}"/> indicating failure.</returns>
    public static Envelope<T?> Error(string errorMessage) =>
        new(default, [errorMessage], DateTime.UtcNow, false);

    /// <summary>
    /// Creates an error envelope containing multiple error messages.
    /// </summary>
    /// <param name="errorMessages">The collection of error messages to include.</param>
    /// <returns>An <see cref="Envelope{T}"/> indicating failure.</returns>
    public static Envelope<T?> Error(IEnumerable<string> errorMessages) =>
        new(default, errorMessages is IReadOnlyList<string> list ? list : errorMessages.ToArray(), DateTime.UtcNow, false);
}

/// <summary>
/// Provides non-generic helper methods for creating envelopes with string results.
/// </summary>
public static class Envelope
{
    /// <summary>
    /// Creates a successful envelope with an empty string result.
    /// </summary>
    /// <returns>An <see cref="Envelope{String}"/> indicating success.</returns>
    public static Envelope<string> Ok() =>
        Envelope<string>.Ok(string.Empty);

    /// <summary>
    /// Creates an error envelope containing a single error message.
    /// </summary>
    /// <param name="errorMessage">The error message to include.</param>
    /// <returns>An <see cref="Envelope{String}"/> indicating failure.</returns>
    public static Envelope<string?> Error(string errorMessage) =>
        Envelope<string?>.Error(errorMessage);

    /// <summary>
    /// Creates an error envelope containing multiple error messages.
    /// </summary>
    /// <param name="errorMessages">The collection of error messages to include.</param>
    /// <returns>An <see cref="Envelope{String}"/> indicating failure.</returns>
    public static Envelope<string?> Error(IEnumerable<string> errorMessages) =>
        Envelope<string?>.Error(errorMessages);
}
