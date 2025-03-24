using Microsoft.AspNetCore.Mvc;
using home_wiki_backend.BL.Common.Contracts.Services;
using home_wiki_backend.BL.Common.Models.Requests;
using home_wiki_backend.Shared.Models.Results.Generic;

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
        [ProducesResponseType(typeof(ResultModel<ArticleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResultModel<ArticleResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ArticleRequest article)
        {
            var result = await _articleService.CreateAsync(article);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
            }
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Retrieves an article by its identifier.
        /// </summary>
        /// <param name="id">The article identifier.</param>
        /// <returns>A result model containing the article response.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResultModel<ArticleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModel<ArticleResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _articleService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        /// <summary>
        /// Retrieves all articles based on optional filters.
        /// </summary>
        /// <remarks>
        /// Note: For simplicity, filtering is not implemented here.
        /// </remarks>
        /// <returns>A result model containing a list of article responses.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ResultModels<ArticleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModels<ArticleResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            // Here we call GetAsync without a predicate.
            var result = await _articleService.GetAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Updates an existing article.
        /// </summary>
        /// <param name="article">The article request model.</param>
        /// <returns>A result model containing the updated article response.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResultModel<ArticleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModel<ArticleResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] ArticleRequest article)
        {
            var result = await _articleService.UpdateAsync(article);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Deletes an article by its identifier.
        /// </summary>
        /// <param name="id">The article identifier.</param>
        /// <returns>A result model indicating the deletion result.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResultModel<ArticleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModel<ArticleResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _articleService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Removes an article.
        /// </summary>
        /// <param name="article">The article request model.</param>
        /// <returns>A result model indicating the removal result.</returns>
        [HttpPost("remove")]
        [ProducesResponseType(typeof(ResultModel<ArticleResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromBody] ArticleRequest article)
        {
            var result = await _articleService.RemoveAsync(article);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.Code, result);
        }
    }
}
