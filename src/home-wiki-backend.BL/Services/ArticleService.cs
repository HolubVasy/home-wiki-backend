using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using home_wiki_backend.BL.Common.Contracts.Services;
using home_wiki_backend.BL.Common.Models.Requests;
using home_wiki_backend.DAL.Common.Contracts;
using home_wiki_backend.DAL.Common.Models.Entities;
using home_wiki_backend.DAL.Exceptions;
using home_wiki_backend.Shared.Models;
using home_wiki_backend.Shared.Helpers;

namespace home_wiki_backend.BL.Services
{
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

        public async Task<ArticleResponse> CreateAsync(
            ArticleRequest articleRequest,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Creating article: {Name}",
                    articleRequest.Name);
                var article = new Article
                {
                    Name = articleRequest.Name,
                    Description = articleRequest.Description,
                    CategoryId = articleRequest.CategoryId,
                    CreatedBy = articleRequest.CreatedBy,
                    CreatedAt = DateTime.UtcNow
                };
                var created = await _articleRepo.AddAsync(article,
                    cancellationToken);
                _logger.LogInformation("Article created with ID: {Id}",
                    created.Id);
                return new ArticleResponse
                {
                    Id = created.Id,
                    Name = created.Name,
                    Description = created.Description,
                    Category = created.Category,
                    CreatedBy = created.CreatedBy,
                    CreatedAt = created.CreatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating article: {Name}",
                    articleRequest.Name);
                throw new ArticleServiceException(
                    "Error creating article", ex);
            }
        }

        public async Task<ArticleResponse> GetByIdAsync(
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
                    throw new KeyNotFoundException(
                        $"Article with ID {id} not found.");
                }
                return new ArticleResponse
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
                        ModifiedBy = article.Category.ModifiedBy,
                    },
                    CreatedBy = article.CreatedBy,
                    CreatedAt = article.CreatedAt,
                    ModifiedBy = article.ModifiedBy,
                    ModifiedAt = article.ModifiedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting article by ID: {Id}", id);
                throw new ArticleServiceException(
                    "Error retrieving article by ID", ex);
            }
        }

        public async Task<IEnumerable<ArticleResponse>> GetAsync(
            Expression<Func<ArticleRequest, bool>>? predicate = null,
            Func<IQueryable<ArticleRequest>,
                IOrderedQueryable<ArticleRequest>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation(
                    "Retrieving list of articles with filtering/sorting.");
                var artPred = predicate?.ConvertTo<ArticleRequest, Article>();
                var articles = await _articleRepo.GetAsync(
                    artPred,
                    orderBy?.ConvertTo<ArticleRequest, Article>(),
                    cancellationToken);
                return articles.Select(a => new ArticleResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Category = a.Category,
                    CreatedBy = a.CreatedBy,
                    CreatedAt = a.CreatedAt,
                    ModifiedBy = a.ModifiedBy,
                    ModifiedAt = a.ModifiedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error retrieving list of articles.");
                throw new ArticleServiceException(
                    "Error retrieving articles", ex);
            }
        }

        public async Task<PagedList<ArticleResponse>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<ArticleRequest, bool>>? predicate = null,
            Func<IQueryable<ArticleRequest>,
                IOrderedQueryable<ArticleRequest>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching paged articles. Page: {PageNumber}, Size: {PageSize}",
                    pageNumber, pageSize);
                var artPred = predicate?.ConvertTo<ArticleRequest, Article>();
                var paged = await _articleRepo.GetPagedAsync(
                    pageNumber,
                    pageSize,
                    artPred,
                    orderBy?.ConvertTo<ArticleRequest, Article>(),
                    cancellationToken);
                var responses = paged?.Items?.Select(a => new ArticleResponse
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
                        ModifiedBy = a.Category.ModifiedBy,
                    },
                    CreatedBy = a.CreatedBy,
                    CreatedAt = a.CreatedAt,
                    ModifiedBy = a.ModifiedBy,
                    ModifiedAt = a.ModifiedAt
                }) ?? Array.Empty<ArticleResponse>();
                return new PagedList<ArticleResponse>
                {
                    PageNumber = paged!.PageNumber,
                    PageSize = paged.PageSize,
                    PageCount = paged.PageCount,
                    HasNextPage = paged.HasNextPage,
                    HasPreviousPage = paged.HasPreviousPage,
                    Items = responses,
                    TotalItemCount = paged.TotalItemCount
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching paged articles.");
                throw new ArticleServiceException(
                    "Error retrieving paged articles", ex);
            }
        }

        public async Task<ArticleResponse> UpdateAsync(
            ArticleRequest articleRequest,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating article with ID: {Id}",
                    articleRequest.Id);
                var article = await _articleRepo.FirstOrDefaultAsync(
                    a => a.Id == articleRequest.Id, cancellationToken);
                if (article == null)
                {
                    _logger.LogWarning("Article with ID {Id} not found for update.",
                        articleRequest.Id);
                    throw new KeyNotFoundException(
                        $"Article with ID {articleRequest.Id} not found.");
                }
                var updated = new Article
                {
                    Id = articleRequest.Id,
                    Name = articleRequest.Name,
                    Description = articleRequest.Description,
                    CategoryId = articleRequest.CategoryId,
                    Tags = articleRequest.TagIds?.Select(t => new Tag { Id = t })
                        .ToHashSet(),
                    CreatedBy = article.CreatedBy,
                    CreatedAt = article.CreatedAt,
                    ModifiedBy = articleRequest.ModifiedBy,
                    ModifiedAt = DateTime.UtcNow
                };
                await _articleRepo.UpdateAsync(updated, cancellationToken);
                return new ArticleResponse
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
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating article with ID: {Id}",
                    articleRequest.Id);
                throw new ArticleServiceException(
                    "Error updating article", ex);
            }
        }

        public async Task DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Deleting article with ID: {Id}", id);
                await _articleRepo.RemoveAsync(a => a.Id == id,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting article with ID: {Id}", id);
                throw new ArticleServiceException(
                    "Error deleting article", ex);
            }
        }

        public async Task RemoveAsync(
            ArticleRequest articleRequest,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Removing article with name: {Name}",
                    articleRequest.Name);
                await _articleRepo.RemoveAsync(
                    a => a.Name == articleRequest.Name, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing article: {Name}",
                    articleRequest.Name);
                throw new ArticleServiceException(
                    "Error removing article", ex);
            }
        }

        public async Task<bool> ExistsAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Checking existence of article ID: {Id}", id);
                return await _articleRepo.AnyAsync(
                    a => a.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking existence of article ID: {Id}",
                    id);
                throw new ArticleServiceException(
                    "Error checking article existence", ex);
            }
        }

        public async Task<bool> AnyAsync(
            Expression<Func<ArticleRequest, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Checking any article match.");
                var artPred = predicate?.ConvertTo<ArticleRequest, Article>();
                return await _articleRepo.AnyAsync(artPred, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AnyAsync for articles.");
                throw new ArticleServiceException(
                    "Error in AnyAsync for articles", ex);
            }
        }

        public async Task<ArticleResponse?> FirstOrDefault(
            Expression<Func<ArticleRequest, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching first matching article.");
                var artPred = predicate?.ConvertTo<ArticleRequest, Article>();
                var article = await _articleRepo.FirstOrDefaultAsync(artPred,
                    cancellationToken);
                if (article == null)
                {
                    _logger.LogWarning("No article found matching criteria.");
                    return null;
                }
                return new ArticleResponse
                {
                    Id = article.Id,
                    Name = article.Name,
                    Description = article.Description,
                    Category = article.Category,
                    CreatedBy = article.CreatedBy,
                    CreatedAt = article.CreatedAt,
                    ModifiedBy = article.ModifiedBy,
                    ModifiedAt = article.ModifiedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FirstOrDefault for articles.");
                throw new ArticleServiceException(
                    "Error fetching first matching article", ex);
            }
        }

        public async Task<IEnumerable<ArticleResponse>> GetListAsync(
            ISpecification<Article> specification,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving articles via spec.");
                var articles = await _articleRepo.ListAsync(specification,
                    cancellationToken);
                return articles.Select(a => new ArticleResponse
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
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving articles via spec.");
                throw new ArticleServiceException(
                    "Error retrieving articles by specification", ex);
            }
        }
    }
}