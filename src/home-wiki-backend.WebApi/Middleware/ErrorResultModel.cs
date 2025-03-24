using home_wiki_backend.Shared.Enums;

namespace Andersen.Infrastructure.API
{
    /// <summary>
    /// Represents the model for error results.
    /// </summary>
    public sealed class ErrorResultModel
    {
        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public ErrorCode Code { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResultModel"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="code">The error code.</param>
        public ErrorResultModel(string message, ErrorCode code)
        {
            Message = message;
            Code = code;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Error code: {Code.ToString()}, message: {Message}.";
        }
    }
}