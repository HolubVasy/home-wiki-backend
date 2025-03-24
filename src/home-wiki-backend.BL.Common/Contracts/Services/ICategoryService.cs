using home_wiki_backend.BL.Common.Models.Requests;
using home_wiki_backend.DAL.Common.Models.Entities;
using System.Linq.Expressions;

namespace home_wiki_backend.BL.Common.Contracts.Services
{
    /// <summary>
    /// Defines the contract for category service operations.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Creates a new category asynchronously.
        /// </summary>
        /// <param name="category">The category request model.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created category response.</returns>
        Task<CategoryResponse> CreateAsync(CategoryRequest category,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets an category by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the category.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the category response.</returns>
        Task<CategoryResponse> GetByIdAsync(int id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a collection of categories that match the specified criteria asynchronously.
        /// </summary>
        /// <param name="predicate">An expression to filter the categories.</param>
        /// <param name="orderBy">A function to order the categories.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of category responses.</returns>
        Task<IEnumerable<CategoryResponse>> GetAsync(
            Expression<Func<CategoryRequest, bool>>? predicate = default,
            Func<IQueryable<CategoryRequest>, IOrderedQueryable<CategoryRequest>>? orderBy = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a paged collection of categories that match the specified criteria asynchronously.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="predicate">An expression to filter the categories.</param>
        /// <param name="orderBy">A function to order the categories.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of category responses.</returns>
        Task<IEnumerable<CategoryResponse>> GetPagedAsync(
            int pageNumber, int pageSize,
            Expression<Func<CategoryRequest, bool>>? predicate = default,
            Func<IQueryable<CategoryRequest>, IOrderedQueryable<CategoryRequest>>? orderBy = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing category asynchronously.
        /// </summary>
        /// <param name="category">The category request model.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated category response.</returns>
        Task<CategoryResponse> UpdateAsync(CategoryRequest category,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an category by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the category.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(int id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an category asynchronously.
        /// </summary>
        /// <param name="category">The category request model.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveAsync(CategoryRequest category,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if an category exists by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the category.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the category exists.</returns>
        Task<bool> ExistsAsync(int id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if any categories match the specified criteria asynchronously.
        /// </summary>
        /// <param name="predicate">An expression to filter the categories.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether any categories match the criteria.</returns>
        Task<bool> AnyAsync(
            Expression<Func<CategoryRequest, bool>>? predicate = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the first category that matches the specified criteria asynchronously.
        /// </summary>
        /// <param name="predicate">An expression to filter the categories.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the first category response that matches the criteria or null if no category matches.</returns>
        Task<CategoryResponse?> FirstOrDefault(Expression<Func<CategoryRequest, bool>>? 
            predicate = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a collection of articles based on the given specification.
        /// </summary>
        /// <param name="specification">The specification to filter and include related data.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation that returns
        /// a collection of article responses.</returns>
        Task<IEnumerable<CategoryResponse>> GetListAsync(
            ISpecification<Category> specification,
            CancellationToken cancellationToken = default);

    }
}
