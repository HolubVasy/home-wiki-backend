namespace home_wiki_backend.Shared.Enums
{
    /// <summary>  
    /// Represents error codes used in the application.  
    /// </summary>  
    [Flags]
    public enum ErrorCode
    {
        /// <summary>
        /// No error.
        /// </summary>
        None = 0,

        /// <summary>
        /// An unexpected error occurred.
        /// </summary>
        Unexpected = 1 << 0,

        /// <summary>
        /// A database exception occurred.
        /// </summary>
        DatabaseException = 1 << 1,
    }
}
