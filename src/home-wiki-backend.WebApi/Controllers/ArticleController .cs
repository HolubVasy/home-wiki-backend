using Microsoft.AspNetCore.Mvc;
using home_wiki_backend.BL.Common.Contracts.Services;
using home_wiki_backend.BL.Common.Models.Requests;

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
        [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ArticleResponse>> Create([FromBody] ArticleRequest article)
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
        [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ArticleResponse>> GetById(int id)
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
        [ProducesResponseType(typeof(IList<ArticleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<ArticleResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IList<ArticleResponse>>> GetAll()
        {
            // Here we call GetAsync without a predicate.
            var result = await _articleService.GetAsync();
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
        [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ArticleResponse>> Update([FromBody] ArticleRequest article)
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
        [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ArticleResponse>> Delete(int id)
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
        [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<ArticleResponse>> Remove([FromBody] ArticleRequest article)
        {
            var result = await _articleService.RemoveAsync(article);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }
    }
}
