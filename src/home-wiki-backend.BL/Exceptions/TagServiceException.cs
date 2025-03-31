using home_wiki_backend.DAL.Common.Models.Exceptions;

namespace home_wiki_backend.BL.Exceptions
{
    /// <summary>
    /// Represents errors that occur during repository operations.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class TagServiceException : ExceptionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagServiceException"/> class.
        /// </summary>
        public TagServiceException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagServiceException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TagServiceException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagServiceException"/> class
        /// with a specified error message and a reference to the inner exception
        /// that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public TagServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
