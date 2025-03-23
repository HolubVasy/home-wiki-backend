using home_wiki_backend.BL.Common.Contracts.Services;
using home_wiki_backend.BL.Common.Models.Requests;
using home_wiki_backend.DAL.Common.Contracts;
using home_wiki_backend.DAL.Common.Models.Entities;
using System.Linq.Expressions;

namespace home_wiki_backend.BL.Services
{
    public sealed class ArticleService : IArticleService
    {
        private readonly IGenericRepository<Article> _articleRepository;

        public ArticleService(IGenericRepository<Article> articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<bool> AnyAsync(Expression<Func<ArticleRequest, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            var articlePredicate = predicate?.ConvertTo<ArticleRequest, Article>();
            return await _articleRepository.AnyAsync(articlePredicate, cancellationToken);
        }

        public async Task<ArticleResponse> CreateAsync(ArticleRequest articleRequest,
            CancellationToken cancellationToken = default)
        {
            var article = new Article
            {
                Name = articleRequest.Name,
                Description = articleRequest.Description,
                CategoryId = articleRequest.CategoryId,
                CreatedBy = articleRequest.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            var createdArticle = await _articleRepository.AddAsync(article, cancellationToken);
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
            await _articleRepository
                .RemoveAsync(a => a.Id == id, cancellationToken);
        }

        public async Task DeleteAsync(ArticleRequest articleRequest, CancellationToken cancellationToken = default)
        {
            await _articleRepository.RemoveAsync(a => a.Id == articleRequest.Id, cancellationToken);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _articleRepository.AnyAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<ArticleResponse>> GetAsync(Expression<Func<ArticleRequest, bool>>? predicate = null, Func<IQueryable<ArticleRequest>, IOrderedQueryable<ArticleRequest>>? orderBy = null, CancellationToken cancellationToken = default)
        {
            var articlePredicate = predicate?.ConvertTo<ArticleRequest, Article>();
            var articles = await _articleRepository.GetAsync(articlePredicate, orderBy?.ConvertTo<ArticleRequest, Article>(), cancellationToken);

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
            var article = await _articleRepository.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
            if (article == null)
            {
                throw new KeyNotFoundException($"Article with ID {id} not found.");
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

        public async Task<IEnumerable<ArticleResponse>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<ArticleRequest, bool>>? predicate = null, Func<IQueryable<ArticleRequest>, IOrderedQueryable<ArticleRequest>>? orderBy = null, CancellationToken cancellationToken = default)
        {
            var articlePredicate = predicate?.ConvertTo<ArticleRequest, Article>();
            var pagedArticles = await _articleRepository.GetPagedAsync(pageNumber, pageSize, articlePredicate, orderBy?.ConvertTo<ArticleRequest, Article>(), cancellationToken);

            return pagedArticles.Items.Select(a => new ArticleResponse
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

        public async Task<ArticleResponse> UpdateAsync(ArticleRequest articleRequest, CancellationToken cancellationToken = default)
        {
            var article = await _articleRepository.FirstOrDefaultAsync(a => a.Id == articleRequest.Id, cancellationToken);
            if (article == null)
            {
                throw new KeyNotFoundException($"Article with ID {articleRequest.Id} not found.");
            }

            article.Name = articleRequest.Name;
            article.Description = articleRequest.Description;
            article.CategoryId = articleRequest.CategoryId;
            article.ModifiedBy = articleRequest.ModifiedBy;
            article.ModifiedAt = DateTime.UtcNow;

            await _articleRepository.UpdateAsync(article, cancellationToken);

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
    }

    public static class ExpressionExtensions
    {
        public static Expression<Func<TDestination, bool>>? ConvertTo<TSource, TDestination>(this Expression<Func<TSource, bool>>? source)
        {
            if (source == null) return null;

            var parameter = Expression.Parameter(typeof(TDestination), source.Parameters[0].Name);
            var body = new ExpressionConverter<TSource, TDestination>(parameter).Visit(source.Body);
            return Expression.Lambda<Func<TDestination, bool>>(body!, parameter);
        }

        public static Func<IQueryable<TDestination>, IOrderedQueryable<TDestination>>? ConvertTo<TSource, TDestination>(this Func<IQueryable<TSource>, IOrderedQueryable<TSource>>? source)
        {
            if (source == null) return null;

            return query => source(query.Cast<TSource>()).Cast<TDestination>();
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
