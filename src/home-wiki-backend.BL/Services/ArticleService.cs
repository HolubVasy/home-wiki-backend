using home_wiki_backend.BL.Common.Contracts.Services;
using home_wiki_backend.BL.Common.Models.Requests;
using home_wiki_backend.DAL.Common.Contracts;
using home_wiki_backend.DAL.Common.Models.Entities;
using home_wiki_backend.Shared.Models;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace home_wiki_backend.BL.Services
{
    /// <inheritdoc/>
    public sealed class ArticleService : IArticleService
    {
        private readonly IGenericRepository<Article> _articleRepository;
        private readonly ILogger<ArticleService> _logger;

        public ArticleService(
            IGenericRepository<Article> articleRepository,
            ILogger<ArticleService> logger)
        {
            _articleRepository = articleRepository;
            _logger = logger;
        }

        public async Task<bool> AnyAsync(Expression<Func<ArticleRequest, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            var articlePredicate = predicate?.ConvertTo<ArticleRequest, Article>();
            _logger.LogDebug("Checking if any article exists with the specified predicate.");
            return await _articleRepository.AnyAsync(articlePredicate, cancellationToken);
        }

        public async Task<ArticleResponse> CreateAsync(ArticleRequest articleRequest,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creating new article: {Name}", articleRequest.Name);

            var article = new Article
            {
                Name = articleRequest.Name,
                Description = articleRequest.Description,
                CategoryId = articleRequest.CategoryId,
                CreatedBy = articleRequest.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            var createdArticle = await _articleRepository.AddAsync(article, cancellationToken);

            _logger.LogInformation("Article created with ID: {Id}", createdArticle.Id);

            return new ArticleResponse
            {
                Id = createdArticle.Id,
                Name = createdArticle.Name,
                Description = createdArticle.Description,
                Category = createdArticle.Category,
                CreatedBy = createdArticle.CreatedBy,
                CreatedAt = createdArticle.CreatedAt
            };
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Deleting article with ID: {Id}", id);
            await _articleRepository.RemoveAsync(a => a.Id == id, cancellationToken);
        }

        public async Task RemoveAsync(ArticleRequest articleRequest,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Removing article with name: {Name}", articleRequest.Name);
            await _articleRepository.RemoveAsync(a => a.Name == articleRequest.Name, cancellationToken);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Checking existence of article ID: {Id}", id);
            return await _articleRepository.AnyAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<ArticleResponse?> FirstOrDefault(
            Expression<Func<ArticleRequest, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching first article matching specified condition...");
            var articlePredicate = predicate?.ConvertTo<ArticleRequest, Article>();
            var article = await _articleRepository.FirstOrDefaultAsync(articlePredicate, cancellationToken);

            if (article == null)
            {
                _logger.LogWarning("No article found matching the specified predicate.");
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

        public async Task<IEnumerable<ArticleResponse>> GetAsync(
            Expression<Func<ArticleRequest, bool>>? predicate = null,
            Func<IQueryable<ArticleRequest>, IOrderedQueryable<ArticleRequest>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving list of articles with optional filtering/sorting.");
            var articlePredicate = predicate?.ConvertTo<ArticleRequest, Article>();
            var articles = await _articleRepository.GetAsync(articlePredicate,
                orderBy?.ConvertTo<ArticleRequest, Article>(), cancellationToken);

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

        public async Task<ArticleResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving article by ID: {Id}", id);
            var article = await _articleRepository.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
            if (article == null)
            {
                _logger.LogWarning("Article with ID {Id} not found.", id);
                throw new KeyNotFoundException($"Article with ID {id} not found.");
            }

            return new ArticleResponse
            {
                Id = article.Id,
                Name = article.Name,
                Description = article.Description,
                Category = new CategoryResponse()
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

        public async Task<PagedList<ArticleResponse>> GetPagedAsync(int pageNumber, 
            int pageSize,
            Expression<Func<ArticleRequest, bool>>? predicate = null,
            Func<IQueryable<ArticleRequest>, IOrderedQueryable<ArticleRequest>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching paged articles. Page: {PageNumber}," +
                " Size: {PageSize}", 
                pageNumber, pageSize);

            var articlePredicate = predicate?.ConvertTo<ArticleRequest, Article>();
            var pagedArticles = await _articleRepository.GetPagedAsync(
                pageNumber, pageSize,
                articlePredicate,
                orderBy?.ConvertTo<ArticleRequest, Article>(),
                cancellationToken);

            var articleResponses = pagedArticles?.Items?.Select(a => new ArticleResponse
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Category = new CategoryResponse()
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
                PageNumber = pagedArticles!.PageNumber,
                PageSize = pagedArticles.PageSize,
                PageCount = pagedArticles.PageCount,
                HasNextPage = pagedArticles.HasNextPage,
                HasPreviousPage = pagedArticles.HasPreviousPage,
                Items = articleResponses,
                TotalItemCount = pagedArticles.TotalItemCount
            };
        }

        public async Task<ArticleResponse> UpdateAsync(ArticleRequest articleRequest,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Updating article with ID: {Id}", articleRequest.Id);

            var article = await _articleRepository.FirstOrDefaultAsync(a => a.Id == 
            articleRequest.Id, cancellationToken);
            if (article == null)
            {
                _logger.LogWarning("Article with ID {Id} not found for update.", articleRequest.Id);
                throw new KeyNotFoundException($"Article with ID {articleRequest.Id} not found.");
            }

            var updatedArticle = new Article
            {
                Id = articleRequest.Id,
                Name = articleRequest.Name,
                Description = articleRequest.Description,
                CategoryId = articleRequest.CategoryId,
                Tags = articleRequest.TagIds?.Select(t => new Tag() { Id = t }).ToHashSet(),
                CreatedBy = article.CreatedBy,
                CreatedAt = article.CreatedAt,
                ModifiedBy = articleRequest.ModifiedBy,
                ModifiedAt = DateTime.UtcNow
            };

            await _articleRepository.UpdateAsync(updatedArticle, cancellationToken);

            return new ArticleResponse
            {
                Id = updatedArticle.Id,
                Name = updatedArticle.Name,
                Description = updatedArticle.Description,
                Category = updatedArticle.Category,
                Tags = updatedArticle.Tags?.Cast<TagBase>()?.ToHashSet(),
                CreatedBy = updatedArticle.CreatedBy,
                CreatedAt = updatedArticle.CreatedAt,
                ModifiedBy = updatedArticle.ModifiedBy,
                ModifiedAt = updatedArticle.ModifiedAt
            };
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ArticleResponse>> GetListAsync(ISpecification<Article> 
            specification,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving articles using specification pattern.");

            // Retrieve the list of articles that satisfy the specification
            var articles = await _articleRepository.ListAsync(specification, cancellationToken);

            // Map the articles to the response model
            return articles.Select(a => new ArticleResponse
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Category = a.Category, // Depending on your mapping needs,
                                       // you might want to map this to a CategoryResponse
                Tags = a.Tags?.Select(t => (TagBase)t).ToHashSet(),
                CreatedBy = a.CreatedBy,
                CreatedAt = a.CreatedAt,
                ModifiedBy = a.ModifiedBy,
                ModifiedAt = a.ModifiedAt
            });
        }
    }

    public static class ExpressionExtensions
    {
        public static Expression<Func<TDestination, bool>>? ConvertTo<TSource, 
            TDestination>(this Expression<Func<TSource, bool>>? source)
        {
            if (source == null) return null;

            var parameter = Expression.Parameter(typeof(TDestination), source.Parameters[0].Name);
            var body = new ExpressionConverter<TSource, TDestination>(parameter).Visit(source.Body);
            return Expression.Lambda<Func<TDestination, bool>>(body!, parameter);
        }

        public static Func<IQueryable<TDestination>, IOrderedQueryable<TDestination>>? 
            ConvertTo<TSource, TDestination>(this Func<IQueryable<TSource>, 
                IOrderedQueryable<TSource>>? source)
        {
            if (source == null) return null;

            return query => (IOrderedQueryable<TDestination>)source(query.Cast<TSource>())
            .Cast<TDestination>();
        }
    }

    public class ExpressionConverter<TSource, TDestination> : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;

        public ExpressionConverter(ParameterExpression parameter)
        {
            _parameter = parameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _parameter;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.DeclaringType == typeof(TSource))
            {
                return Expression.PropertyOrField(_parameter, node.Member.Name);
            }

            return base.VisitMember(node);
        }
    }
}
