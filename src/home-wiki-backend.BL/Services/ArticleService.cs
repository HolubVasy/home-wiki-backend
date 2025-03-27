using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using home_wiki_backend.BL.Common.Contracts.Services;
using home_wiki_backend.BL.Common.Models.Requests;
using home_wiki_backend.DAL.Common.Contracts;
using home_wiki_backend.DAL.Common.Models.Entities;
using home_wiki_backend.Shared.Models;
using home_wiki_backend.Shared.Models.Results.Generic;
using home_wiki_backend.Shared.Enums;
using home_wiki_backend.Shared.Helpers;
using home_wiki_backend.Shared.Models.Results.Errors;
using home_wiki_backend.Shared.Extensions;
using home_wiki_backend.BL.Common.Models.Responses;
using home_wiki_backend.DAL.Extensions;
using home_wiki_backend.Shared.Models.Dtos;

namespace home_wiki_backend.BL.Services
{
    /// <inheritdoc/>
    public sealed class ArticleService : IArticleService
    {
        private readonly IGenericRepository<Article> _articleRepo;
        private readonly ILogger<ArticleService> _logger;

        public ArticleService(
            IGenericRepository<Article> articleRepo,
            ILogger<ArticleService> logger)
        {
            _articleRepo = articleRepo;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ResultModel<ArticleResponseDto>> CreateAsync(
            ArticleRequestDto article,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Creating article: {Name}",
                    article.Name);
                var newArticle = new Article
                {
                    Name = article.Name,
                    Description = article.Description,
                    CategoryId = article.CategoryId,
                    CreatedBy = article.CreatedBy,
                    CreatedAt = DateTime.UtcNow
                };
                var created = await _articleRepo.AddAsync(newArticle,
                    cancellationToken);
                _logger.LogInformation("Article created with ID: {Id}",
                    created.Id);
                return new ResultModel<ArticleResponseDto>
                {
                    Success = true,
                    Message = "Article created successfully",
                    Code = StatusCodes.Status201Created,
                    Data = new ArticleResponseDto
                    {
                        Id = created.Id,
                        Name = created.Name,
                        Description = created.Description,
                        Category = created.Category,
                        CreatedBy = created.CreatedBy,
                        CreatedAt = created.CreatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating article: {Name}",
                    article.Name);
                return new ResultModel<ArticleResponseDto>
                {
                    Success = false,
                    Message = "Error creating article",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message, ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<ArticleResponseDto>> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Getting article by ID: {Id}", id);
                var article = await _articleRepo.FirstOrDefaultAsync(id,
                    new ArticlesWithCategoryAndTagsSpecification(), cancellationToken);
                return GetById(id, article);
            }
            catch (Exception ex)
            {
                return ReturnFailedGetById(id, ex);
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<ArticleResponseDto>> GetByIdAsync(
            int id,
            ISpecification<Article> specification,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Getting article by ID: {Id}", id);
                var article = await _articleRepo.FirstOrDefaultAsync(id, specification, 
                    cancellationToken);
                return GetById(id, article);
            }
            catch (Exception ex)
            {
                return ReturnFailedGetById(id, ex);
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModels<ArticleResponseDto>> GetAsync(
            Expression<Func<ArticleRequestDto, bool>>? predicate = null,
            Func<IQueryable<ArticleRequestDto>, IOrderedQueryable<ArticleRequestDto>>?
                orderBy = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving articles with filters.");
                var artPred = predicate?.ConvertTo<ArticleRequestDto, Article>();
                var articles = await _articleRepo.GetAsync(
                    artPred,
                    orderBy?.ConvertTo<ArticleRequestDto, Article>(),
                    cancellationToken);
                var data = articles.Select(a => new ArticleResponseDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Category = a.Category,
                    CreatedBy = a.CreatedBy,
                    CreatedAt = a.CreatedAt,
                    ModifiedBy = a.ModifiedBy,
                    ModifiedAt = a.ModifiedAt
                }).ToList();
                return new ResultModels<ArticleResponseDto>
                {
                    Success = true,
                    Message = "Articles retrieved successfully",
                    Code = StatusCodes.Status200OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving articles.");
                return new ResultModels<ArticleResponseDto>
                {
                    Success = false,
                    Message = "Error retrieving articles",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message, ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<PagedList<ArticleResponseDto>>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<ArticleRequestDto, bool>>? predicate = null,
            Func<IQueryable<ArticleRequestDto>, IOrderedQueryable<ArticleRequestDto>>?
                orderBy = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching paged articles. " +
                    "Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);
                var artPred = predicate?.ConvertTo<ArticleRequestDto, Article>();
                var paged = await _articleRepo.GetPagedAsync(
                    pageNumber,
                    pageSize,
                    artPred,
                    orderBy?.ConvertTo<ArticleRequestDto, Article>(),
                    cancellationToken);
                return GetPaged(pageNumber, pageSize, paged);
            }
            catch (Exception ex)
            {
                return ReturnFailed(ex);
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<PagedList<ArticleResponseDto>>> GetPageAsync(
            ArticleFilterRequestDto filter,
            CancellationToken cancellationToken = default)
        {
            try
            {
                Log(filter);
                var predicate = filter.GetPredicate();
                var paged = await _articleRepo.GetPagedAsync(
                    filter.PageNumber,
                    filter.PageSize,
                    filter.GetPredicate(),
                    filter.GetOrderBy()
                    );
                return GetPaged(filter.PageNumber, filter.PageSize, paged);

            }
            catch (Exception ex)
            {
                return ReturnFailed(ex);
            }
        }

        private void Log(ArticleFilterRequestDto filter)
        {
            _logger.LogInformation("Fetching paged articles. " +
                                "Page: {PageNumber}, Size: {PageSize}, Sorting: {Sorting}",
                                filter.PageNumber, filter.PageSize,
                                filter.Sorting.GetStringRepresentation());
        }

        /// <inheritdoc/>
        public async Task<ResultModel<PagedList<ArticleResponseDto>>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            ISpecification<Article> specification,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var paged = await _articleRepo.GetPagedAsync(
                    pageNumber,
                    pageSize,
                    specification,
                    cancellationToken);
                return GetPaged(pageNumber, pageSize, paged);
            }
            catch (Exception ex)
            {
                return ReturnFailed(ex);
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<ArticleResponseDto>> UpdateAsync(
            ArticleRequestDto article,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating article ID: {Id}", article.Id);
                var existing = await _articleRepo.FirstOrDefaultAsync(
                    a => a.Id == article.Id, cancellationToken);
                if (existing == null)
                {
                    _logger.LogWarning("Article with ID {Id} not found.",
                        article.Id);
                    return new ResultModel<ArticleResponseDto>
                    {
                        Success = false,
                        Message = $"Article with ID {article.Id} not found.",
                        Code = StatusCodes.Status404NotFound,
                        Error = new ErrorResultModel("Not found",
                                                       ErrorCode.Unexpected)
                    };
                }
                var updated = new Article
                {
                    Id = article.Id,
                    Name = article.Name,
                    Description = article.Description,
                    CategoryId = article.CategoryId,
                    Tags = article.TagIds?.Select(t => new Tag { Id = t })
                        .ToHashSet(),
                    CreatedBy = existing.CreatedBy,
                    CreatedAt = existing.CreatedAt,
                    ModifiedBy = article.ModifiedBy,
                    ModifiedAt = DateTime.UtcNow
                };
                await _articleRepo.UpdateAsync(updated, cancellationToken);
                return new ResultModel<ArticleResponseDto>
                {
                    Success = true,
                    Message = "Article updated successfully",
                    Code = StatusCodes.Status200OK,
                    Data = new ArticleResponseDto
                    {
                        Id = updated.Id,
                        Name = updated.Name,
                        Description = updated.Description,
                        Category = updated.Category,
                        Tags = updated.Tags?.Cast<TagBase>()?.ToHashSet(),
                        CreatedBy = updated.CreatedBy,
                        CreatedAt = updated.CreatedAt,
                        ModifiedBy = updated.ModifiedBy,
                        ModifiedAt = updated.ModifiedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating article ID: {Id}", article.Id);
                return new ResultModel<ArticleResponseDto>
                {
                    Success = false,
                    Message = "Error updating article",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message, ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<ArticleResponseDto>> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Deleting article ID: {Id}", id);
                var exists = await _articleRepo.ExistsAsync(a => a.Id == id, cancellationToken);
                if (!exists)
                {
                    var errorMessage = $"Article with ID: {id} not exists";
                    _logger.LogInformation(errorMessage);
                    return new ResultModel<ArticleResponseDto>
                    {
                        Success = false,
                        Message = errorMessage,
                        Code = StatusCodes.Status404NotFound,
                        Data = null
                    };
                }
                await _articleRepo.RemoveAsync(a => a.Id == id, cancellationToken);
                return new ResultModel<ArticleResponseDto>
                {
                    Success = true,
                    Message = "Article deleted successfully",
                    Code = StatusCodes.Status200OK,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error deleting article ID: {id}";
                _logger.LogError(ex, errorMessage);
                return new ResultModel<ArticleResponseDto>
                {
                    Success = false,
                    Message = errorMessage,
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message, ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<ArticleResponseDto>> RemoveAsync(
            ArticleRequestDto article,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Removing article: {Name}", article.Name);
                await _articleRepo.RemoveAsync(
                    a => a.Name == article.Name, cancellationToken);
                return new ResultModel<ArticleResponseDto>
                {
                    Success = true,
                    Message = "Article removed successfully",
                    Code = StatusCodes.Status200OK,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing article: {Name}", article.Name);
                return new ResultModel<ArticleResponseDto>
                {
                    Success = false,
                    Message = "Error removing article",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message, ErrorCode.Unexpected)
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
                return await _articleRepo.ExistsAsync(a => a.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking article existence for ID: {Id}", id);
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> AnyAsync(
            Expression<Func<ArticleRequestDto, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var artPred = predicate?.ConvertTo<ArticleRequestDto, Article>();
                return await _articleRepo.ExistsAsync(artPred, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AnyAsync for articles.");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<ArticleResponseDto>> FirstOrDefault(
            Expression<Func<ArticleRequestDto, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var artPred = predicate?.ConvertTo<ArticleRequestDto, Article>();
                var article = await _articleRepo.FirstOrDefaultAsync(artPred, cancellationToken);
                if (article == null)
                {
                    return new ResultModel<ArticleResponseDto>
                    {
                        Success = false,
                        Message = "No matching article found",
                        Code = StatusCodes.Status404NotFound,
                        Error = new ErrorResultModel("Not found", ErrorCode.Unexpected)
                    };
                }
                return new ResultModel<ArticleResponseDto>
                {
                    Success = true,
                    Message = "Article retrieved successfully",
                    Code = StatusCodes.Status200OK,
                    Data = new ArticleResponseDto
                    {
                        Id = article.Id,
                        Name = article.Name,
                        Description = article.Description,
                        Category = article.Category,
                        CreatedBy = article.CreatedBy,
                        CreatedAt = article.CreatedAt,
                        ModifiedBy = article.ModifiedBy,
                        ModifiedAt = article.ModifiedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FirstOrDefault for articles.");
                return new ResultModel<ArticleResponseDto>
                {
                    Success = false,
                    Message = "Error retrieving first article",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message, ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModels<ArticleResponseDto>> GetListAsync(
            ISpecification<Article> specification,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving articles via specification.");
                var articles = await _articleRepo.ListAsync(specification, cancellationToken);
                var data = articles.Select(a => new ArticleResponseDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Category = a.Category,
                    Tags = a.Tags?.Select(t => (TagBase)t).ToHashSet(),
                    CreatedBy = a.CreatedBy,
                    CreatedAt = a.CreatedAt,
                    ModifiedBy = a.ModifiedBy,
                    ModifiedAt = a.ModifiedAt
                }).ToList();
                return new ResultModels<ArticleResponseDto>
                {
                    Success = true,
                    Message = "Articles retrieved via specification",
                    Code = StatusCodes.Status200OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving articles via specification.");
                return new ResultModels<ArticleResponseDto>
                {
                    Success = false,
                    Message = "Error retrieving articles by specification",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message, ErrorCode.Unexpected)
                };
            }
        }

        private ResultModel<PagedList<ArticleResponseDto>> GetPaged(
            int pageNumber,
            int pageSize,
            PagedList<Article> paged,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching paged articles. " +
                "Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);
            var data = paged.Items.Select(a => new ArticleResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Category = new CategoryResponseDto
                {
                    Name = a.Category.Name,
                    CreatedAt = a.Category.CreatedAt,
                    CreatedBy = a.Category.CreatedBy,
                    ModifiedAt = a.Category.ModifiedAt,
                    ModifiedBy = a.Category.ModifiedBy
                },
                CreatedBy = a.CreatedBy,
                CreatedAt = a.CreatedAt,
                ModifiedBy = a.ModifiedBy,
                ModifiedAt = a.ModifiedAt
            }).ToList();
            return new ResultModel<PagedList<ArticleResponseDto>>
            {
                Success = true,
                Message = "Paged articles retrieved successfully",
                Code = StatusCodes.Status200OK,
                Data = new PagedList<ArticleResponseDto>()
                {
                    Items = data,
                    PageNumber = paged.PageNumber,
                    PageSize = paged.PageSize,
                    TotalItemCount = paged.TotalItemCount,
                    HasNextPage = paged.HasNextPage,
                    HasPreviousPage = paged.HasPreviousPage,
                    PageCount = paged.PageCount
                }
            };
        }

        private ResultModel<PagedList<ArticleResponseDto>> ReturnFailed(Exception ex)
        {
            _logger.LogError(ex, "Error retrieving paged articles.");
            return new ResultModel<PagedList<ArticleResponseDto>>
            {
                Success = false,
                Message = "Error retrieving paged articles",
                Code = StatusCodes.Status500InternalServerError,
                Error = new ErrorResultModel(ex.Message, ErrorCode.Unexpected)
            };
        }

        public ResultModel<ArticleResponseDto> GetById(int id, Article? article)
        {
            if (article == null)
            {
                _logger.LogWarning("Article with ID {Id} not found.", id);
                return new ResultModel<ArticleResponseDto>
                {
                    Success = false,
                    Message = $"Article with ID {id} not found.",
                    Code = StatusCodes.Status404NotFound,
                    Error = new ErrorResultModel("Not found",
                                                   ErrorCode.Unexpected)
                };
            }
            return new ResultModel<ArticleResponseDto>
            {
                Success = true,
                Message = "Article retrieved successfully",
                Code = StatusCodes.Status200OK,
                Data = new ArticleResponseDto
                {
                    Id = article.Id,
                    Name = article.Name,
                    Description = article.Description,
                    Category = new CategoryResponseDto
                    {
                        Name = article.Category.Name,
                        CreatedAt = article.Category.CreatedAt,
                        CreatedBy = article.Category.CreatedBy,
                        ModifiedAt = article.Category.ModifiedAt,
                        ModifiedBy = article.Category.ModifiedBy
                    },
                    Tags = article.Tags?.Select(t => (TagBase)t).ToHashSet(),
                    CreatedBy = article.CreatedBy,
                    CreatedAt = article.CreatedAt,
                    ModifiedBy = article.ModifiedBy,
                    ModifiedAt = article.ModifiedAt
                }
            };
        }

        private ResultModel<ArticleResponseDto> ReturnFailedGetById(int id, Exception ex)
        {
            _logger.LogError(ex, "Error retrieving article by ID: {Id}", id);
            return new ResultModel<ArticleResponseDto>
            {
                Success = false,
                Message = "Error retrieving article by ID",
                Code = StatusCodes.Status500InternalServerError,
                Error = new ErrorResultModel(ex.Message, ErrorCode.Unexpected)
            };
        }
    }
}
