namespace home_wiki_backend.Shared.Models
{
    /// <summary>  
    /// Represents a paginated list of items.  
    /// </summary>  
    /// <typeparam name="T">The type of items in the list.</typeparam>  
    public sealed class PagedList<T> where T : class
    {
        /// <summary>  
        /// Gets the total number of pages.  
        /// </summary>  
        public int PageCount { get; init; }

        /// <summary>  
        /// Gets the total number of items.  
        /// </summary>  
        public int TotalItemCount { get; init; }

        /// <summary>  
        /// Gets the current page number.  
        /// </summary>  
        public int PageNumber { get; init; }

        /// <summary>  
        /// Gets the number of items per page.  
        /// </summary>  
        public int PageSize { get; init; }

        /// <summary>  
        /// Gets a value indicating whether there is a previous page.  
        /// </summary>  
        public bool HasPreviousPage { get; init; }

        /// <summary>  
        /// Gets a value indicating whether there is a next page.  
        /// </summary>  
        public bool HasNextPage { get; init; }

        /// <summary>  
        /// Gets the list of items.  
        /// </summary>  
        public IEnumerable<T> Items { get; init; } = null!;

        /// <summary>
        /// Gets an empty instance of <see cref="PagedList{T}"/>.
        /// </summary>
        public static PagedList<T> Empty =>
            new PagedList<T>
            {
                PageCount = 0,
                TotalItemCount = 0,
                PageNumber = 0,
                PageSize = 0,
                HasPreviousPage = false,
                HasNextPage = false,
                Items = Array.Empty<T>()
            };
    }
}
