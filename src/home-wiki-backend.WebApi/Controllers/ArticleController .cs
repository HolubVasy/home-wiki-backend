using Microsoft.AspNetCore.Mvc;
using home_wiki_backend.BL.Common.Contracts.Services;
using home_wiki_backend.BL.Common.Models.Requests;
using home_wiki_backend.Shared.Models;
using home_wiki_backend.BL.Common.Models.Responses;
using home_wiki_backend.DAL.Specifications;
using home_wiki_backend.BL.Models;
using home_wiki_backend.Shared.Models.Results.Generic;
using System.Collections.Immutable;

namespace home_wiki_backend.Controllers
{
    /// <summary>
    /// Controller for managing article-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleController"/> class.
        /// </summary>
        /// <param name="articleService">The article service.</param>
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// Creates a new article.
        /// </summary>
        /// <param name="article">The article request model.</param>
        /// <returns>A result model containing the created article response.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ArticleResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ArticleResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ArticleResponseDto>> Create([FromBody] ArticleRequestDto article)
        {
            var result = await _articleService.CreateAsync(article);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Retrieves an article by its identifier.
        /// </summary>
        /// <param name="id">The article identifier.</param>
        /// <returns>A result model containing the article response.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ArticleResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ArticleResponseDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ArticleResponseDto>> GetById(int id)
        {
            var result = await _articleService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Data);
        }

        /// <summary>
        /// Retrieves all articles based on optional filters.
        /// </summary>
        /// <remarks>
        /// Note: For simplicity, filtering is not implemented here.
        /// </remarks>
        /// <returns>A result model containing a list of article responses.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IList<ArticleResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<ArticleResponseDto>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IList<ArticleResponseDto>>> GetAll()
        {
            // Here we call GetAsync without a predicate.
            var result = await _articleService.GetListAsync(new ArticlesWithCategoryAndTagsSpecification());
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Updates an existing article.
        /// </summary>
        /// <param name="article">The article request model.</param>
        /// <returns>A result model containing the updated article response.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(ArticleResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ArticleResponseDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ArticleResponseDto>> Update([FromBody] ArticleRequestDto article)
        {
            var result = await _articleService.UpdateAsync(article);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Deletes an article by its identifier.
        /// </summary>
        /// <param name="id">The article identifier.</param>
        /// <returns>A result model indicating the deletion result.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ArticleResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ArticleResponseDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ArticleResponseDto>> Delete(int id)
        {
            var result = await _articleService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Removes an article.
        /// </summary>
        /// <param name="article">The article request model.</param>
        /// <returns>A result model indicating the removal result.</returns>
        [HttpPost("remove")]
        [ProducesResponseType(typeof(ArticleResponseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<ArticleResponseDto>> Remove([FromBody] ArticleRequestDto article)
        {
            var result = await _articleService.RemoveAsync(article);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Retrieves paged articles.
        /// </summary>
        /// <param name="pageNumber">The page number (default is 1).</param>
        /// <param name="pageSize">The page size (default is 10).</param>
        /// <returns>A result model containing a paged list of article responses.</returns>
        [HttpGet("paged")]
        [ProducesResponseType(typeof(PagedList<ArticleResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PagedList<ArticleResponseDto>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedList<ArticleResponseDto>>> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _articleService.GetPagedAsync(pageNumber, pageSize,
                new ArticlesWithCategoryAndTagsSpecification());
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Retrieves paginated articles by category ID.
        /// </summary>
        /// <param name="categoryId">The category ID to filter articles by.</param>
        /// <param name="name">The partial name to search for.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of articles per page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result model containing a paginated list of article responses.</returns>
        [HttpGet("category/{categoryId}")]
        [ProducesResponseType(typeof(ResultModel<PagedList<ArticleResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModel<PagedList<ArticleResponseDto>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetArticlesByCategory(
            [FromRoute] int categoryId,
            [FromQuery] string? name,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var filter = new ArticleFilterRequestDto(pageNumber, pageSize,
                Shared.Enums.Sorting.Ascending, name!,
                new HashSet<int>() { categoryId }.ToImmutableHashSet(),
                ImmutableHashSet<int>.Empty);

            var result = await _articleService.GetPageAsync(filter, cancellationToken);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Retrieves paginated articles by tag ID.
        /// </summary>
        /// <param name="tagId">The tag ID to filter articles by.</param>
        /// <param name="name">The partial name to search for.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of articles per page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result model containing a paginated list of article responses.</returns>
        [HttpGet("tag/{tagId}")]
        [ProducesResponseType(typeof(ResultModel<PagedList<ArticleResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModel<PagedList<ArticleResponseDto>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetArticlesByTag(
            [FromRoute] int tagId,
            [FromQuery] string? name,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var filter = new ArticleFilterRequestDto(pageNumber, pageSize,
                Shared.Enums.Sorting.Ascending, name!,
                ImmutableHashSet<int>.Empty,
                new HashSet<int>() { tagId }.ToImmutableHashSet());

            var result = await _articleService.GetPageAsync(filter, cancellationToken);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Retrieves paginated articles based on the provided filter.
        /// </summary>
        /// <param name="filter">The article request filter model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result model containing a paginated list of article responses.</returns>
        [HttpPost("filter")]
        [ProducesResponseType(typeof(ResultModel<PagedList<ArticleResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModel<PagedList<ArticleResponseDto>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetArticlesByFilter(
            [FromBody] ArticleFilterRequestDto filter,
            CancellationToken cancellationToken = default)
        {
            var result = await _articleService.GetPageAsync(filter, cancellationToken);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Searches for articles by name.
        /// </summary>
        /// <param name="name">The partial name to search for.</param>
        /// <param name="pageNumber">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of articles per page (default is 10).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result model containing a paginated list of article responses.</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ResultModel<PagedList<ArticleResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModel<PagedList<ArticleResponseDto>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchArticles(
            [FromQuery] string name,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            // Create a filter request and set the name to the provided value.
            // The remaining filter properties (categories, tags) are set to empty.
            var filter = new ArticleFilterRequestDto(
                pageNumber,
                pageSize,
                Shared.Enums.Sorting.Ascending,
                name, // This is the partial name filter.
                ImmutableHashSet<int>.Empty,
                ImmutableHashSet<int>.Empty);

            var result = await _articleService.GetPageAsync(filter, cancellationToken);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);

        }
    }
}
