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
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CategoryResponseDto>> Create([FromBody] CategoryRequestDto category)
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
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponseDto>> GetById(int id)
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
        [ProducesResponseType(typeof(IList<CategoryResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<CategoryResponseDto>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IList<CategoryResponseDto>>> GetAll()
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
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponseDto>> Update([FromBody] CategoryRequestDto category)
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
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponseDto>> Delete(int id)
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
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CategoryResponseDto>> Remove([FromBody] CategoryRequestDto category)
        {
            var result = await _categoryService.RemoveAsync(category);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);
        }

        /// <summary>
        /// Searches for categories by name.
        /// </summary>
        /// <param name="name">The partial name to search for.</param>
        /// <param name="pageNumber">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of categories per page (default is 10).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A result model containing a paginated list of category responses.</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ResultModel<PagedList<CategoryResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModel<PagedList<CategoryResponseDto>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchCategories(
            [FromQuery] string? name,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            // Create a filter request and set the name to the provided value.
            var filter = new CategoryFilterRequestDto(
                pageNumber,
                pageSize,
                Shared.Enums.Sorting.Ascending,
                name!);

            var result = await _categoryService.GetPagedAsync(pageNumber,
                                                             pageSize,
                                                             filter,
                                                             cancellationToken);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Code, result.Data);

        }
    }
}
