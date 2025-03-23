namespace home_wiki_backend.DAL.Common.Models.Exceptions
{
    /// <summary>
    /// Represents the base class for all custom exceptions in the domain.
    /// This class is intended to be inherited by specific exception classes
    /// and should not be instantiated directly.
    /// </summary>
    [Serializable]
    public abstract class ExceptionBase : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionBase"/> class.
        /// This constructor is protected to prevent direct instantiation of
        /// this class.
        /// </summary>
        protected ExceptionBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionBase"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        protected ExceptionBase(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionBase"/> class
        /// with a specified error message and a reference to the inner exception
        /// that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for
        /// the exception.</param>
        /// <param name="inner">The exception that is the cause of the current
        /// exception, or a null reference if no inner exception is specified.
        /// </param>
        protected ExceptionBase(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
