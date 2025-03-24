﻿using Microsoft.AspNetCore.Mvc;
using home_wiki_backend.BL.Common.Contracts.Services;
using home_wiki_backend.BL.Common.Models.Requests;
using home_wiki_backend.Shared.Models.Results.Generic;

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
        [ProducesResponseType(typeof(ResultModel<CategoryResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResultModel<CategoryResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CategoryRequest category)
        {
            var result = await _categoryService.CreateAsync(category);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
            }
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Retrieves a category by its identifier.
        /// </summary>
        /// <param name="id">The category identifier.</param>
        /// <returns>A result model containing the category response.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResultModel<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModel<CategoryResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A result model containing a list of category responses.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ResultModels<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModels<CategoryResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            // Call GetAsync without predicate for all categories.
            var result = await _categoryService.GetAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="category">The category request model.</param>
        /// <returns>A result model containing the updated category response.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResultModel<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModel<CategoryResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] CategoryRequest category)
        {
            var result = await _categoryService.UpdateAsync(category);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Deletes a category by its identifier.
        /// </summary>
        /// <param name="id">The category identifier.</param>
        /// <returns>A result model indicating the deletion result.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResultModel<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultModel<CategoryResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Removes a category.
        /// </summary>
        /// <param name="category">The category request model.</param>
        /// <returns>A result model indicating the removal result.</returns>
        [HttpPost("remove")]
        [ProducesResponseType(typeof(ResultModel<CategoryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromBody] CategoryRequest category)
        {
            var result = await _categoryService.RemoveAsync(category);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.Code, result);
        }
    }
}
