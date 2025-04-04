﻿using Microsoft.AspNetCore.Mvc;
using home_wiki_backend.BL.Common.Contracts.Services;
using home_wiki_backend.BL.Common.Models.Requests;
using home_wiki_backend.BL.Common.Models.Responses;
using home_wiki_backend.Shared.Models.Results.Generic;
using home_wiki_backend.Shared.Models;
using home_wiki_backend.Shared.Models.Dtos;

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
        [ProducesResponseType(typeof(TagResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(TagResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TagResponseDto>> Create([FromBody] TagRequestDto tag)
        {
            var result = await _tagService.CreateAsync(tag);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Retrieves a tag by its identifier.
        /// </summary>
        /// <param name="id">The tag identifier.</param>
        /// <returns>A result model containing the tag response.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TagResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(TagResponseDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TagResponseDto>> GetById(int id)
        {
            var result = await _tagService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Data);
        }

        /// <summary>
        /// Retrieves all tags based on optional filters.
        /// </summary>
        /// <returns>A result model containing a list of tag responses.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IList<TagResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<TagResponseDto>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IList<TagResponseDto>>> GetAll()
        {
            var result = await _tagService.GetAsync();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Updates an existing tag.
        /// </summary>
        /// <param name="tag">The tag request model.</param>
        /// <returns>A result model containing the updated tag response.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(TagResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(TagResponseDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TagResponseDto>> Update([FromBody] TagRequestDto tag)
        {
            var result = await _tagService.UpdateAsync(tag);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Deletes a tag by its identifier.
        /// </summary>
        /// <param name="id">The tag identifier.</param>
        /// <returns>A result model indicating the deletion result.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(TagResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(TagResponseDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TagResponseDto>> Delete(int id)
        {
            var result = await _tagService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Removes a tag.
        /// </summary>
        /// <param name="tag">The tag request model.</param>
        /// <returns>A result model indicating the removal result.</returns>
        [HttpPost("remove")]
        [ProducesResponseType(typeof(TagResponseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<TagResponseDto>> Remove([FromBody] TagRequestDto tag)
        {
            var result = await _tagService.RemoveAsync(tag);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Searches for tags by name.
        /// </summary>
        /// <param name="name">The partial name to search for.</param>
        /// <param name="pageNumber">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of tags per page (default is 10).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result model containing a paginated list of tag responses.</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ResultModel<PagedList<ArticleResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModel<PagedList<ArticleResponseDto>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchTags(
            [FromQuery] string? name,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            // Create a filter request and set the name to the provided value.
            // The remaining filter properties (categories, tags) are set to empty.
            var filter = new TagFilterRequestDto(
            pageNumber,
            pageSize,
                Shared.Enums.Sorting.Ascending,
                name!);

            var result = await _tagService.GetPagedAsync(
                pageNumber, pageSize, filter, cancellationToken);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);

        }
    }
}
