using System.Linq.Expressions;
using home_wiki_backend.BL.Common.Models.Requests;
using home_wiki_backend.Shared.Models.Results.Generic;
using home_wiki_backend.DAL.Common.Models.Entities;

namespace home_wiki_backend.BL.Common.Contracts.Services
{
    /// <summary>
    /// Provides methods for managing tags.
    /// </summary>
    public interface ITagService
    {
        /// <summary>
        /// Creates a new tag.
        /// </summary>
        /// <param name="tag">The tag request model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result model containing the created tag response.
        /// </returns>
        Task<ResultModel<TagResponse>> CreateAsync(
            TagRequest tag,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a tag by its identifier.
        /// </summary>
        /// <param name="id">The tag identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result model containing the tag response.</returns>
        Task<ResultModel<TagResponse>> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets tags based on a predicate and order.
        /// </summary>
        /// <param name="predicate">The predicate to filter tags.</param>
        /// <param name="orderBy">The function to order tags.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result models containing the tag responses.
        /// </returns>
        Task<ResultModels<TagResponse>> GetAsync(
            Expression<Func<TagRequest, bool>>? predicate = default,
            Func<IQueryable<TagRequest>,
                IOrderedQueryable<TagRequest>>? orderBy = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets paged tags based on a predicate and order.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="predicate">The predicate to filter tags.</param>
        /// <param name="orderBy">The function to order tags.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result models containing the tag responses.</returns>
        Task<ResultModels<TagResponse>> GetPagedAsync(
            int pageNumber, int pageSize,
            Expression<Func<TagRequest, bool>>? predicate = default,
            Func<IQueryable<TagRequest>,
                IOrderedQueryable<TagRequest>>? orderBy = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing tag.
        /// </summary>
        /// <param name="tag">The tag request model.</param>
        /// <param name="cancellationToken">The cancellation token.
        /// </param>
        /// <returns>The result model containing the updated tag 
        /// response.</returns>
        Task<ResultModel<TagResponse>> UpdateAsync(
            TagRequest tag,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a tag by its identifier.
        /// </summary>
        /// <param name="id">The tag identifier.</param>
        /// <param name="cancellationToken">The cancellation token.
        /// </param>
        /// <returns>The result model containing the deleted tag 
        /// response.</returns>
        Task<ResultModel<TagResponse>> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a tag.
        /// </summary>
        /// <param name="tag">The tag request model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result model containing the removed tag 
        /// response.</returns>
        Task<ResultModel<TagResponse>> RemoveAsync(
            TagRequest tag,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a tag exists by its identifier.
        /// </summary>
        /// <param name="id">The tag identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the tag exists, otherwise false.</returns>
        Task<bool> ExistsAsync(
            int id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if any tags match the given predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter tags.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if any tags match the predicate, otherwise
        /// false.</returns>
        Task<bool> AnyAsync(
            Expression<Func<TagRequest, bool>>? predicate = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the first tag that matches the given predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter tags.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result model containing the first matching
        /// tag response.</returns>
        Task<ResultModel<TagResponse>> FirstOrDefault(
            Expression<Func<TagRequest, bool>>? predicate = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of tags based on a specification.
        /// </summary>
        /// <param name="specification">The specification to filter 
        /// tags.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result models containing the tag responses.</returns>
        Task<ResultModels<TagResponse>> GetListAsync(
            ISpecification<Tag> specification,
            CancellationToken cancellationToken = default);
    }
}
