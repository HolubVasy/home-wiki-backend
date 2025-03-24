using home_wiki_backend.BL.Common.Models.Requests;
using home_wiki_backend.Shared.Models;
using System.Linq.Expressions;

namespace home_wiki_backend.BL.Common.Contracts.Services
{
    /// <summary>
    /// Defines the contract for article service operations.
    /// </summary>
    public interface IArticleService
    {
        /// <summary>
        /// Creates a new article asynchronously.
        /// </summary>
        /// <param name="article">The article request model.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created article response.</returns>
        Task<ArticleResponse> CreateAsync(ArticleRequest article,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets an article by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the article.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the article response.</returns>
        Task<ArticleResponse> GetByIdAsync(int id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a collection of articles that match the specified criteria asynchronously.
        /// </summary>
        /// <param name="predicate">An expression to filter the articles.</param>
        /// <param name="orderBy">A function to order the articles.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of article responses.</returns>
        Task<IEnumerable<ArticleResponse>> GetAsync(
            Expression<Func<ArticleRequest, bool>>? predicate = default,
            Func<IQueryable<ArticleRequest>, IOrderedQueryable<ArticleRequest>>? orderBy = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a paged collection of articles that match the specified criteria asynchronously.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="predicate">An expression to filter the articles.</param>
        /// <param name="orderBy">A function to order the articles.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of article responses.</returns>
        Task<PagedList<ArticleResponse>> GetPagedAsync(
            int pageNumber, int pageSize,
            Expression<Func<ArticleRequest, bool>>? predicate = default,
            Func<IQueryable<ArticleRequest>, IOrderedQueryable<ArticleRequest>>? orderBy = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing article asynchronously.
        /// </summary>
        /// <param name="article">The article request model.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated article response.</returns>
        Task<ArticleResponse> UpdateAsync(ArticleRequest article,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an article by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the article.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(int id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an article asynchronously.
        /// </summary>
        /// <param name="article">The article request model.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveAsync(ArticleRequest article,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if an article exists by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the article.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the article exists.</returns>
        Task<bool> ExistsAsync(int id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if any articles match the specified criteria asynchronously.
        /// </summary>
        /// <param name="predicate">An expression to filter the articles.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether any articles match the criteria.</returns>
        Task<bool> AnyAsync(
            Expression<Func<ArticleRequest, bool>>? predicate = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the first article that matches the specified criteria asynchronously.
        /// </summary>
        /// <param name="predicate">An expression to filter the articles.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the first article response that matches the criteria or null if no article matches.</returns>
        Task<ArticleResponse?> FirstOrDefault(Expression<Func<ArticleRequest, bool>>? 
            predicate = default,
            CancellationToken cancellationToken = default);
            
    }
}
