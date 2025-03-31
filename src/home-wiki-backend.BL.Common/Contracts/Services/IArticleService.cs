using home_wiki_backend.BL.Common.Models.Requests;
using home_wiki_backend.BL.Common.Models.Responses;
using home_wiki_backend.DAL.Common.Contracts.Specifications;
using home_wiki_backend.DAL.Common.Models.Entities;
using home_wiki_backend.Shared.Models;
using home_wiki_backend.Shared.Models.Dtos;
using home_wiki_backend.Shared.Models.Results.Generic;
using System.Linq.Expressions;

namespace home_wiki_backend.BL.Common.Contracts.Services
{
    /// <summary>
    /// Defines the contract for article-related operations.
    /// </summary>
    public interface IArticleService
    {
        /// <summary>
        /// Creates a new article asynchronously.
        /// </summary>
        /// <param name="article">The article request model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the result model with the created 
        /// article response.</returns>
        Task<ResultModel<ArticleResponseDto>> CreateAsync(
            ArticleRequestDto article,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets an article by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The article identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the result model with the article 
        /// response.</returns>
        Task<ResultModel<ArticleResponseDto>> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets articles based on the specified predicate and order 
        /// asynchronously.
        /// </summary>
        /// <param name="predicate">The predicate to filter articles.</param>
        /// <param name="orderBy">The function to order articles.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the result models with the article 
        /// responses.</returns>
        Task<ResultModels<ArticleResponseDto>> GetAsync(
            Expression<Func<ArticleRequestDto, bool>>? predicate = default,
            Func<IQueryable<ArticleRequestDto>, IOrderedQueryable<ArticleRequestDto>>?
            orderBy = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets paged articles based on the specified predicate and order
        /// asynchronously.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="predicate">The predicate to filter articles.</param>
        /// <param name="orderBy">The function to order articles.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the result models with the article 
        /// responses.</returns>
        Task<ResultModel<PagedList<ArticleResponseDto>>> GetPagedAsync(
            int pageNumber, int pageSize,
            Expression<Func<ArticleRequestDto, bool>>? predicate = default,
            Func<IQueryable<ArticleRequestDto>,
                IOrderedQueryable<ArticleRequestDto>>? orderBy = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an article asynchronously.
        /// </summary>
        /// <param name="article">The article request model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the result model with the updated 
        /// article response.</returns>
        Task<ResultModel<ArticleResponseDto>> UpdateAsync(
            ArticleRequestDto article,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an article by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The article identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the result model with the deletion 
        /// result.</returns>
        Task<ResultModel<ArticleResponseDto>> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an article asynchronously.
        /// </summary>
        /// <param name="article">The article request model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the result model with the removal
        /// result.</returns>
        Task<ResultModel<ArticleResponseDto>> RemoveAsync(
            ArticleRequestDto article,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if an article exists by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The article identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a boolean value indicating whether
        /// the article exists.</returns>
        Task<bool> ExistsAsync(
            int id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if any articles match the specified predicate asynchronously.
        /// </summary>
        /// <param name="predicate">The predicate to filter articles.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a boolean value indicating whether 
        /// any articles match the predicate.</returns>
        Task<bool> AnyAsync(
            Expression<Func<ArticleRequestDto, bool>>? predicate = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the first article that matches the specified predicate asynchronously.
        /// </summary>
        /// <param name="predicate">The predicate to filter articles.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the result model with the first 
        /// matching article response.</returns>
        Task<ResultModel<ArticleResponseDto>> FirstOrDefault(
            Expression<Func<ArticleRequestDto, bool>>? predicate = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of articles based on the specified specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification to filter articles.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the result models with the article responses.</returns>
        Task<ResultModels<ArticleResponseDto>> GetListAsync(
            ISpecification<Article> specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets paged articles based on the specified specification asynchronously.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="specification">The specification to filter articles.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the result models with the article responses.</returns>
        Task<ResultModel<PagedList<ArticleResponseDto>>> GetPagedAsync(
            int pageNumber, int pageSize,
            ISpecification<Article> specification,
            CancellationToken cancellationToken = default);
        Task<ResultModel<PagedList<ArticleResponseDto>>> GetPageAsync(ArticleFilterRequestDto filter, CancellationToken cancellationToken = default);
    }
}
