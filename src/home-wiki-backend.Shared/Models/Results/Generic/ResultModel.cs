using home_wiki_backend.Shared.Models.Results.Errors;

namespace home_wiki_backend.Shared.Models.Results.Generic;

/// <summary>
/// Represents the result model with generic data type.
/// </summary>
/// <typeparam name="T">The type of the data.</typeparam>
public sealed class ResultModel<T> where T : class
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// Gets the message associated with the result.
    /// </summary>
    public string Message { get; init; } = null!;

    /// <summary>
    /// Gets the error details if the operation failed.
    /// </summary>
    public ErrorResultModel? Error { get; init; }

    /// <summary>
    /// Gets the status code associated with the result.
    /// </summary>
    public int Code { get; init; }

    /// <summary>
    /// Gets the data associated with the result.
    /// </summary>
    public T? Data { get; init; }
}
