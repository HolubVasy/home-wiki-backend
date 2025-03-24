using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using home_wiki_backend.BL.Common.Contracts.Services;
using home_wiki_backend.BL.Common.Models.Requests;
using home_wiki_backend.DAL.Common.Contracts;
using home_wiki_backend.DAL.Common.Models.Entities;
using home_wiki_backend.Shared.Models.Results.Generic;
using home_wiki_backend.Shared.Enums;
using home_wiki_backend.Shared.Helpers;
using home_wiki_backend.Shared.Models.Results.Errors;

namespace home_wiki_backend.BL.Services
{
    /// <inheritdoc/>
    public sealed class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _catRepo;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            IGenericRepository<Category> catRepo,
            ILogger<CategoryService> logger)
        {
            _catRepo = catRepo;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ResultModel<CategoryResponse>> CreateAsync(
            CategoryRequest category,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Creating category: {Name}",
                    category.Name);
                var newCat = new Category
                {
                    Name = category.Name,
                    CreatedBy = "system",
                    CreatedAt = DateTime.UtcNow
                };
                var created = await _catRepo.AddAsync(newCat,
                    cancellationToken);
                _logger.LogInformation("Category created with ID: {Id}",
                    created.Id);
                return new ResultModel<CategoryResponse>
                {
                    Success = true,
                    Message = "Category created successfully",
                    Code = StatusCodes.Status201Created,
                    Data = new CategoryResponse
                    {
                        Id = created.Id,
                        Name = created.Name,
                        CreatedBy = created.CreatedBy,
                        CreatedAt = created.CreatedAt,
                        ModifiedBy = created.ModifiedBy,
                        ModifiedAt = created.ModifiedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category: {Name}",
                    category.Name);
                return new ResultModel<CategoryResponse>
                {
                    Success = false,
                    Message = "Error creating category",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message,
                    ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<CategoryResponse>> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Getting category by ID: {Id}", id);
                var cat = await _catRepo.FirstOrDefaultAsync(c => c.Id == id,
                    cancellationToken);
                if (cat == null)
                {
                    _logger.LogWarning("Category with ID {Id} " +
                        "not found.", id);
                    return new ResultModel<CategoryResponse>
                    {
                        Success = false,
                        Message = $"Category with ID {id} not found.",
                        Code = StatusCodes.Status404NotFound,
                        Error = new ErrorResultModel("Not found",
                        ErrorCode.Unexpected)
                    };
                }
                return new ResultModel<CategoryResponse>
                {
                    Success = true,
                    Message = "Category retrieved successfully",
                    Code = StatusCodes.Status200OK,
                    Data = new CategoryResponse
                    {
                        Id = cat.Id,
                        Name = cat.Name,
                        CreatedBy = cat.CreatedBy,
                        CreatedAt = cat.CreatedAt,
                        ModifiedBy = cat.ModifiedBy,
                        ModifiedAt = cat.ModifiedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category by ID:" +
                    " {Id}", id);
                return new ResultModel<CategoryResponse>
                {
                    Success = false,
                    Message = "Error retrieving category by ID",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message,
                    ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModels<CategoryResponse>> GetAsync(
            Expression<Func<CategoryRequest, bool>>? predicate = null,
            Func<IQueryable<CategoryRequest>,
                IOrderedQueryable<CategoryRequest>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Getting category list.");
                var pred = predicate?.ConvertTo<CategoryRequest,
                    Category>();
                var cats = await _catRepo.GetAsync(pred,
                    orderBy?.ConvertTo<CategoryRequest, Category>(),
                    cancellationToken);
                var data = cats.Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    CreatedBy = c.CreatedBy,
                    CreatedAt = c.CreatedAt,
                    ModifiedBy = c.ModifiedBy,
                    ModifiedAt = c.ModifiedAt
                }).ToList();
                return new ResultModels<CategoryResponse>
                {
                    Success = true,
                    Message = "Categories retrieved successfully",
                    Code = StatusCodes.Status200OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories.");
                return new ResultModels<CategoryResponse>
                {
                    Success = false,
                    Message = "Error retrieving categories",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message,
                    ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModels<CategoryResponse>>
            GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<CategoryRequest,
                bool>>? predicate = null,
            Func<IQueryable<CategoryRequest>,
                IOrderedQueryable<CategoryRequest>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching paged categories. " +
                    "Page: {PageNumber}, Size: {PageSize}",
                    pageNumber, pageSize);
                var pred = predicate?.ConvertTo<CategoryRequest, Category>();
                var paged = await _catRepo.GetPagedAsync(pageNumber,
                    pageSize, pred,
                    orderBy?.ConvertTo<CategoryRequest,
                    Category>(), cancellationToken);
                var data = paged.Items.Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    CreatedBy = c.CreatedBy,
                    CreatedAt = c.CreatedAt,
                    ModifiedBy = c.ModifiedBy,
                    ModifiedAt = c.ModifiedAt
                }).ToList();
                return new ResultModels<CategoryResponse>
                {
                    Success = true,
                    Message = "Paged categories retrieved successfully",
                    Code = StatusCodes.Status200OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged categories.");
                return new ResultModels<CategoryResponse>
                {
                    Success = false,
                    Message = "Error retrieving paged categories",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message,
                    ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<CategoryResponse>> UpdateAsync(
            CategoryRequest category,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating category ID: {Id}",
                    category.Id);
                var exist = await _catRepo.FirstOrDefaultAsync(
                    c => c.Id == category.Id, cancellationToken);
                if (exist == null)
                {
                    _logger.LogWarning("Category with ID {Id} not" +
                        " found.", category.Id);
                    return new ResultModel<CategoryResponse>
                    {
                        Success = false,
                        Message = $"Category with ID {category.Id} " +
                        $"not found.",
                        Code = StatusCodes.Status404NotFound,
                        Error = new ErrorResultModel("Not found",
                        ErrorCode.Unexpected)
                    };
                }
                var upd = new Category
                {
                    Id = category.Id,
                    Name = category.Name,
                    CreatedBy = exist.CreatedBy,
                    CreatedAt = exist.CreatedAt,
                    ModifiedBy = "system",
                    ModifiedAt = DateTime.UtcNow
                };
                await _catRepo.UpdateAsync(upd, cancellationToken);
                return new ResultModel<CategoryResponse>
                {
                    Success = true,
                    Message = "Category updated successfully",
                    Code = StatusCodes.Status200OK,
                    Data = new CategoryResponse
                    {
                        Id = upd.Id,
                        Name = upd.Name,
                        CreatedBy = upd.CreatedBy,
                        CreatedAt = upd.CreatedAt,
                        ModifiedBy = upd.ModifiedBy,
                        ModifiedAt = upd.ModifiedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category ID: {Id}",
                    category.Id);
                return new ResultModel<CategoryResponse>
                {
                    Success = false,
                    Message = "Error updating category",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message,
                    ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<CategoryResponse>> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Deleting category ID: " +
                    "{Id}", id);
                await _catRepo.RemoveAsync(c => c.Id == id,
                    cancellationToken);
                return new ResultModel<CategoryResponse>
                {
                    Success = true,
                    Message = "Category deleted successfully",
                    Code = StatusCodes.Status200OK,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category ID: " +
                    "{Id}", id);
                return new ResultModel<CategoryResponse>
                {
                    Success = false,
                    Message = "Error deleting category",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message,
                    ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<CategoryResponse>> RemoveAsync(
            CategoryRequest category,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Removing category: {Name}",
                    category.Name);
                await _catRepo.RemoveAsync(c => c.Name == category.Name,
                    cancellationToken);
                return new ResultModel<CategoryResponse>
                {
                    Success = true,
                    Message = "Category removed successfully",
                    Code = StatusCodes.Status200OK,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing category: {Name}",
                    category.Name);
                return new ResultModel<CategoryResponse>
                {
                    Success = false,
                    Message = "Error removing category",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message,
                    ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _catRepo.ExistsAsync(c => c.Id == id,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking category " +
                    "existence for ID: " +
                    "{Id}", id);
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> AnyAsync(
            Expression<Func<CategoryRequest, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var pred = predicate?.ConvertTo<CategoryRequest, Category>();
                return await _catRepo.ExistsAsync(pred, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AnyAsync for categories.");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<CategoryResponse>> FirstOrDefault(
            Expression<Func<CategoryRequest, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var pred = predicate?.ConvertTo<CategoryRequest,
                    Category>();
                var cat = await _catRepo.FirstOrDefaultAsync(pred,
                    cancellationToken);
                if (cat == null)
                {
                    return new ResultModel<CategoryResponse>
                    {
                        Success = false,
                        Message = "No matching category found",
                        Code = StatusCodes.Status404NotFound,
                        Error = new ErrorResultModel("Not found",
                        ErrorCode.Unexpected)
                    };
                }
                return new ResultModel<CategoryResponse>
                {
                    Success = true,
                    Message = "Category retrieved successfully",
                    Code = StatusCodes.Status200OK,
                    Data = new CategoryResponse
                    {
                        Id = cat.Id,
                        Name = cat.Name,
                        CreatedBy = cat.CreatedBy,
                        CreatedAt = cat.CreatedAt,
                        ModifiedBy = cat.ModifiedBy,
                        ModifiedAt = cat.ModifiedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FirstOrDefault for " +
                    "categories.");
                return new ResultModel<CategoryResponse>
                {
                    Success = false,
                    Message = "Error retrieving first category",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message,
                    ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModels<CategoryResponse>> GetListAsync(
            ISpecification<Category> specification,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving categories " +
                    "via specification.");
                var cats = await _catRepo.ListAsync(specification,
                    cancellationToken);
                var data = cats.Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    CreatedBy = c.CreatedBy,
                    CreatedAt = c.CreatedAt,
                    ModifiedBy = c.ModifiedBy,
                    ModifiedAt = c.ModifiedAt
                }).ToList();
                return new ResultModels<CategoryResponse>
                {
                    Success = true,
                    Message = "Categories retrieved via specification",
                    Code = StatusCodes.Status200OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories via" +
                    " specification.");
                return new ResultModels<CategoryResponse>
                {
                    Success = false,
                    Message = "Error retrieving categories by specification",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message,
                    ErrorCode.Unexpected)
                };
            }
        }
    }
}
