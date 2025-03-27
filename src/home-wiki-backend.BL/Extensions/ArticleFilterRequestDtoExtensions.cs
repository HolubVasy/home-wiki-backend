using home_wiki_backend.BL.Common.Enums;
using home_wiki_backend.BL.Models;
using home_wiki_backend.DAL.Common.Models.Entities;
using System.Linq.Expressions;

namespace home_wiki_backend.BL.Extensions
{
    internal static class ArticleFilterRequestDtoExtensions
    {
        internal static Expression<Func<Article, bool>> GetPredicate(this
            ArticleFilterRequestDto filter)
        {
            Expression<Func<Article, bool>> predicate = a => true;
            if (!string.IsNullOrEmpty(filter.PartName))
            {
                predicate = a => a.Name.Contains(filter.PartName, 
                    StringComparison.OrdinalIgnoreCase);
            }
            if (filter.CategoryIds.Any())
            {
                predicate = a => filter.CategoryIds.Contains(a.CategoryId);
            }
            if (filter.TagIds.Any())
            {
                predicate = a => a.Tags!.Any(t => filter.TagIds.Contains(t.Id));
            }
            return predicate;
        }

        internal static Func<IQueryable<Article>, IOrderedQueryable<Article>>? 
            GetOrderBy(this
            ArticleFilterRequestDto filter) => filter.Sorting switch
            {
                Sorting.None => null,
                Sorting.Ascending => q => q.OrderBy(a => a.Name),
                Sorting.Descending => q => q.OrderByDescending(a => a.Name),
                _ => throw new ArgumentOutOfRangeException(nameof(filter.Sorting))
            };

        internal IEnumerable<Expression<Func<Article, object>>[]>? GetIndludes(this
            ArticleFilterRequestDto filter)
        {

        }
    }
}
