using Microsoft.AspNetCore.Mvc;
using home_wiki_backend.BL.Common.Contracts.Services;
using home_wiki_backend.BL.Common.Models.Requests;

namespace home_wiki_backend.Controllers
{
    /// <summary>
    /// Controller for managing category-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="category">The category request model.</param>
        /// <returns>A result model containing the created category response.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CategoryResponse>> Create([FromBody] CategoryRequest category)
        {
            var result = await _categoryService.CreateAsync(category);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Retrieves a category by its identifier.
        /// </summary>
        /// <param name="id">The category identifier.</param>
        /// <returns>A result model containing the category response.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponse>> GetById(int id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Data);
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A result model containing a list of category responses.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IList<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<CategoryResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IList<CategoryResponse>>> GetAll()
        {
            // Call GetAsync without predicate for all categories.
            var result = await _categoryService.GetAsync();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="category">The category request model.</param>
        /// <returns>A result model containing the updated category response.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponse>> Update([FromBody] CategoryRequest category)
        {
            var result = await _categoryService.UpdateAsync(category);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Deletes a category by its identifier.
        /// </summary>
        /// <param name="id">The category identifier.</param>
        /// <returns>A result model indicating the deletion result.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponse>> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Removes a category.
        /// </summary>
        /// <param name="category">The category request model.</param>
        /// <returns>A result model indicating the removal result.</returns>
        [HttpPost("remove")]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<CategoryResponse>> Remove([FromBody] CategoryRequest category)
        {
            var result = await _categoryService.RemoveAsync(category);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }
    }
}
