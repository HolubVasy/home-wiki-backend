using home_wiki_backend.BL.Common.Models.Requests;
using home_wiki_backend.DAL.Common.Models.Entities;
using System.Linq.Expressions;

namespace home_wiki_backend.BL.Common.Contracts.Services
{
    /// <summary>
    /// Defines the contract for tag service operations.
    /// </summary>
    public interface ITagService
    {
        /// <summary>
        /// Creates a new tag asynchronously.
        /// </summary>
        /// <param name="tag">The tag request model.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created tag response.</returns>
        Task<TagResponse> CreateAsync(TagRequest tag,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets an tag by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the tag.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the tag response.</returns>
        Task<TagResponse> GetByIdAsync(int id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a collection of tags that match the specified criteria asynchronously.
        /// </summary>
        /// <param name="predicate">An expression to filter the tags.</param>
        /// <param name="orderBy">A function to order the tags.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of tag responses.</returns>
        Task<IEnumerable<TagResponse>> GetAsync(
            Expression<Func<TagRequest, bool>>? predicate = default,
            Func<IQueryable<TagRequest>, IOrderedQueryable<TagRequest>>? orderBy = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a paged collection of tags that match the specified criteria asynchronously.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="predicate">An expression to filter the tags.</param>
        /// <param name="orderBy">A function to order the tags.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of tag responses.</returns>
        Task<IEnumerable<TagResponse>> GetPagedAsync(
            int pageNumber, int pageSize,
            Expression<Func<TagRequest, bool>>? predicate = default,
            Func<IQueryable<TagRequest>, IOrderedQueryable<TagRequest>>? orderBy = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing tag asynchronously.
        /// </summary>
        /// <param name="tag">The tag request model.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated tag response.</returns>
        Task<TagResponse> UpdateAsync(TagRequest tag,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an tag by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the tag.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(int id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an tag asynchronously.
        /// </summary>
        /// <param name="tag">The tag request model.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveAsync(TagRequest tag,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if an tag exists by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the tag.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the tag exists.</returns>
        Task<bool> ExistsAsync(int id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if any tags match the specified criteria asynchronously.
        /// </summary>
        /// <param name="predicate">An expression to filter the tags.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether any tags match the criteria.</returns>
        Task<bool> AnyAsync(
            Expression<Func<TagRequest, bool>>? predicate = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the first tag that matches the specified criteria asynchronously.
        /// </summary>
        /// <param name="predicate">An expression to filter the tags.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the first tag response that matches the criteria or null if no tag matches.</returns>
        Task<TagResponse?> FirstOrDefault(Expression<Func<TagRequest, bool>>? 
            predicate = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a collection of tags based on the given specification.
        /// </summary>
        /// <param name="specification">The specification to filter and include related data.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation that returns a collection of tags responses.</returns>
        Task<IEnumerable<TagResponse>> GetListAsync(
            ISpecification<Tag> specification,
            CancellationToken cancellationToken = default);

    }
}
