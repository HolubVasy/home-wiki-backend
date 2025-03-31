using System.Linq.Expressions;
using home_wiki_backend.BL.Common.Models.Requests;
using home_wiki_backend.Shared.Models.Results.Generic;
using home_wiki_backend.DAL.Common.Models.Entities;
using home_wiki_backend.Shared.Models;
using home_wiki_backend.DAL.Common.Contracts.Specifications;
using home_wiki_backend.BL.Common.Models.Responses;
using home_wiki_backend.Shared.Models.Dtos;

namespace home_wiki_backend.BL.Common.Contracts.Services
{
    /// <summary>
    /// Defines the contract for category-related operations.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Creates a new category asynchronously.
        /// </summary>
        /// <param name="category">The category request model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result model containing the created category response.</returns>
        Task<ResultModel<CategoryResponseDto>> CreateAsync(
            CategoryRequestDto category,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a category by its ID asynchronously.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result model containing the category response.</returns>
        Task<ResultModel<CategoryResponseDto>> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets categories based on a predicate and order asynchronously.
        /// </summary>
        /// <param name="predicate">The predicate to filter categories.</param>
        /// <param name="orderBy">The function to order categories.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result model containing the list of category responses.
        /// </returns>
        Task<ResultModels<CategoryResponseDto>> GetAsync(
            Expression<Func<CategoryRequestDto, bool>>? predicate = default,
            Func<IQueryable<CategoryRequestDto>,
                IOrderedQueryable<CategoryRequestDto>>? orderBy = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets paged categories based on a predicate and order asynchronously.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="predicate">The predicate to filter categories.</param>
        /// <param name="orderBy">The function to order categories.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result model containing the list of paged category 
        /// responses.</returns>
        Task<ResultModel<PagedList<CategoryResponseDto>>> GetPagedAsync(
            int pageNumber, int pageSize,
            Expression<Func<CategoryRequestDto, bool>>? predicate = default,
            Func<IQueryable<CategoryRequestDto>,
                IOrderedQueryable<CategoryRequestDto>>? orderBy = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a category asynchronously.
        /// </summary>
        /// <param name="category">The category request model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result model containing the updated category 
        /// response.</returns>
        Task<ResultModel<CategoryResponseDto>> UpdateAsync(
            CategoryRequestDto category,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a category by its ID asynchronously.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result model containing the deleted category response
        /// .</returns>
        Task<ResultModel<CategoryResponseDto>> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a category asynchronously.
        /// </summary>
        /// <param name="category">The category request model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result model containing the removed category
        /// response.</returns>
        Task<ResultModel<CategoryResponseDto>> RemoveAsync(
            CategoryRequestDto category,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a category exists by its ID asynchronously.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A boolean indicating whether the category
        /// exists.</returns>
        Task<bool> ExistsAsync(
            int id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if any category matches the predicate asynchronously.
        /// </summary>
        /// <param name="predicate">The predicate to filter categories.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A boolean indicating whether any category 
        /// matches the predicate.</returns>
        Task<bool> AnyAsync(
            Expression<Func<CategoryRequestDto, bool>>? predicate = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the first category that matches the predicate asynchronously.
        /// </summary>
        /// <param name="predicate">The predicate to filter categories.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result model containing the first matching
        /// category response.</returns>
        Task<ResultModel<CategoryResponseDto>> FirstOrDefault(
            Expression<Func<CategoryRequestDto, bool>>? predicate = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of categories based on a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification to filter 
        /// categories.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result model containing the list of category 
        /// responses.</returns>
        Task<ResultModels<CategoryResponseDto>> GetListAsync(
            ISpecification<Category> specification,
            CancellationToken cancellationToken = default);
        Task<ResultModel<PagedList<CategoryResponseDto>>> GetPagedAsync(int pageNumber, int pageSize, CategoryFilterRequestDto filterSpecification, CancellationToken cancellationToken = default);
    }
}
