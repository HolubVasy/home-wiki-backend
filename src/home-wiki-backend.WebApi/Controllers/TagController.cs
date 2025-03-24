using Microsoft.AspNetCore.Mvc;
using home_wiki_backend.BL.Common.Contracts.Services;
using home_wiki_backend.BL.Common.Models.Requests;
using home_wiki_backend.Shared.Models.Results.Generic;

namespace home_wiki_backend.Controllers
{
    /// <summary>
    /// Controller for managing tag-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagController"/> class.
        /// </summary>
        /// <param name="tagService">The tag service.</param>
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// Creates a new tag.
        /// </summary>
        /// <param name="tag">The tag request model.</param>
        /// <returns>A result model containing the created tag response.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResultModel<TagResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResultModel<TagResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] TagRequest tag)
        {
            var result = await _tagService.CreateAsync(tag);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
            }
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Retrieves a tag by its identifier.
        /// </summary>
        /// <param name="id">The tag identifier.</param>
        /// <returns>A result model containing the tag response.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResultModel<TagResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModel<TagResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _tagService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        /// <summary>
        /// Retrieves all tags based on optional filters.
        /// </summary>
        /// <returns>A result model containing a list of tag responses.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ResultModels<TagResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModels<TagResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _tagService.GetAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Updates an existing tag.
        /// </summary>
        /// <param name="tag">The tag request model.</param>
        /// <returns>A result model containing the updated tag response.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResultModel<TagResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModel<TagResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] TagRequest tag)
        {
            var result = await _tagService.UpdateAsync(tag);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Deletes a tag by its identifier.
        /// </summary>
        /// <param name="id">The tag identifier.</param>
        /// <returns>A result model indicating the deletion result.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResultModel<TagResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModel<TagResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _tagService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Removes a tag.
        /// </summary>
        /// <param name="tag">The tag request model.</param>
        /// <returns>A result model indicating the removal result.</returns>
        [HttpPost("remove")]
        [ProducesResponseType(typeof(ResultModel<TagResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromBody] TagRequest tag)
        {
            var result = await _tagService.RemoveAsync(tag);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.Code, result);
        }
    }
}
