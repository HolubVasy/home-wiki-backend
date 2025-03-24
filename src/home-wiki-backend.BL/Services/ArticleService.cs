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
        public async Task<ResultModel<ArticleResponse>> CreateAsync(
            ArticleRequest article,
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
                return new ResultModel<ArticleResponse>
                {
                    Success = true,
                    Message = "Article created successfully",
                    Code = StatusCodes.Status201Created,
                    Data = new ArticleResponse
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
                return new ResultModel<ArticleResponse>
                {
                    Success = false,
                    Message = "Error creating article",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message, ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<ArticleResponse>> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Getting article by ID: {Id}", id);
                var article = await _articleRepo.FirstOrDefaultAsync(
                    a => a.Id == id, cancellationToken);
                if (article == null)
                {
                    _logger.LogWarning("Article with ID {Id} not found.", id);
                    return new ResultModel<ArticleResponse>
                    {
                        Success = false,
                        Message = $"Article with ID {id} not found.",
                        Code = StatusCodes.Status404NotFound,
                        Error = new ErrorResultModel("Not found",
                                                       ErrorCode.Unexpected)
                    };
                }
                return new ResultModel<ArticleResponse>
                {
                    Success = true,
                    Message = "Article retrieved successfully",
                    Code = StatusCodes.Status200OK,
                    Data = new ArticleResponse
                    {
                        Id = article.Id,
                        Name = article.Name,
                        Description = article.Description,
                        Category = new CategoryResponse
                        {
                            Name = article.Category.Name,
                            CreatedAt = article.Category.CreatedAt,
                            CreatedBy = article.Category.CreatedBy,
                            ModifiedAt = article.Category.ModifiedAt,
                            ModifiedBy = article.Category.ModifiedBy
                        },
                        CreatedBy = article.CreatedBy,
                        CreatedAt = article.CreatedAt,
                        ModifiedBy = article.ModifiedBy,
                        ModifiedAt = article.ModifiedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving article by ID: {Id}", id);
                return new ResultModel<ArticleResponse>
                {
                    Success = false,
                    Message = "Error retrieving article by ID",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message, ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModels<ArticleResponse>> GetAsync(
            Expression<Func<ArticleRequest, bool>>? predicate = null,
            Func<IQueryable<ArticleRequest>, IOrderedQueryable<ArticleRequest>>?
                orderBy = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving articles with filters.");
                var artPred = predicate?.ConvertTo<ArticleRequest, Article>();
                var articles = await _articleRepo.GetAsync(
                    artPred,
                    orderBy?.ConvertTo<ArticleRequest, Article>(),
                    cancellationToken);
                var data = articles.Select(a => new ArticleResponse
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
                return new ResultModels<ArticleResponse>
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
                return new ResultModels<ArticleResponse>
                {
                    Success = false,
                    Message = "Error retrieving articles",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message, ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModels<ArticleResponse>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<ArticleRequest, bool>>? predicate = null,
            Func<IQueryable<ArticleRequest>, IOrderedQueryable<ArticleRequest>>?
                orderBy = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching paged articles. " +
                    "Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);
                var artPred = predicate?.ConvertTo<ArticleRequest, Article>();
                var paged = await _articleRepo.GetPagedAsync(
                    pageNumber,
                    pageSize,
                    artPred,
                    orderBy?.ConvertTo<ArticleRequest, Article>(),
                    cancellationToken);
                var data = paged.Items.Select(a => new ArticleResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Category = new CategoryResponse
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
                return new ResultModels<ArticleResponse>
                {
                    Success = true,
                    Message = "Paged articles retrieved successfully",
                    Code = StatusCodes.Status200OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged articles.");
                return new ResultModels<ArticleResponse>
                {
                    Success = false,
                    Message = "Error retrieving paged articles",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message, ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<ArticleResponse>> UpdateAsync(
            ArticleRequest article,
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
                    return new ResultModel<ArticleResponse>
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
                return new ResultModel<ArticleResponse>
                {
                    Success = true,
                    Message = "Article updated successfully",
                    Code = StatusCodes.Status200OK,
                    Data = new ArticleResponse
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
                return new ResultModel<ArticleResponse>
                {
                    Success = false,
                    Message = "Error updating article",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message, ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<ArticleResponse>> DeleteAsync(
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
                    return new ResultModel<ArticleResponse>
                    {
                        Success = false,
                        Message = errorMessage,
                        Code = StatusCodes.Status404NotFound,
                        Data = null
                    };
                }
                await _articleRepo.RemoveAsync(a => a.Id == id, cancellationToken);
                return new ResultModel<ArticleResponse>
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
                return new ResultModel<ArticleResponse>
                {
                    Success = false,
                    Message = errorMessage,
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message, ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<ArticleResponse>> RemoveAsync(
            ArticleRequest article,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Removing article: {Name}", article.Name);
                await _articleRepo.RemoveAsync(
                    a => a.Name == article.Name, cancellationToken);
                return new ResultModel<ArticleResponse>
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
                return new ResultModel<ArticleResponse>
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
            Expression<Func<ArticleRequest, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var artPred = predicate?.ConvertTo<ArticleRequest, Article>();
                return await _articleRepo.ExistsAsync(artPred, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AnyAsync for articles.");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<ArticleResponse>> FirstOrDefault(
            Expression<Func<ArticleRequest, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var artPred = predicate?.ConvertTo<ArticleRequest, Article>();
                var article = await _articleRepo.FirstOrDefaultAsync(artPred, cancellationToken);
                if (article == null)
                {
                    return new ResultModel<ArticleResponse>
                    {
                        Success = false,
                        Message = "No matching article found",
                        Code = StatusCodes.Status404NotFound,
                        Error = new ErrorResultModel("Not found", ErrorCode.Unexpected)
                    };
                }
                return new ResultModel<ArticleResponse>
                {
                    Success = true,
                    Message = "Article retrieved successfully",
                    Code = StatusCodes.Status200OK,
                    Data = new ArticleResponse
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
                return new ResultModel<ArticleResponse>
                {
                    Success = false,
                    Message = "Error retrieving first article",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message, ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModels<ArticleResponse>> GetListAsync(
            ISpecification<Article> specification,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving articles via specification.");
                var articles = await _articleRepo.ListAsync(specification, cancellationToken);
                var data = articles.Select(a => new ArticleResponse
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
                return new ResultModels<ArticleResponse>
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
                return new ResultModels<ArticleResponse>
                {
                    Success = false,
                    Message = "Error retrieving articles by specification",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message, ErrorCode.Unexpected)
                };
            }
        }
    }
}
