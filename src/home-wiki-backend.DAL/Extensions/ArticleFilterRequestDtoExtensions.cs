using home_wiki_backend.BL.Models;
using home_wiki_backend.DAL.Common.Models.Entities;
using home_wiki_backend.Shared.Enums;
using home_wiki_backend.Shared.Helpers;
using System.Linq.Expressions;

namespace home_wiki_backend.BL.Extensions
{
    public static class ArticleFilterRequestDtoExtensions
    {
        public static Expression<Func<Article, bool>> 
            GetPredicate(this ArticleFilterRequestDto filter)
        {
            Expression<Func<Article, bool>> predicate = a => true;

            if (!string.IsNullOrEmpty(filter.PartName))
            {
                var partName = filter.PartName;
                predicate = predicate
                    .AndAlso(
                        a => a.Name.Contains(partName, 
                            StringComparison.OrdinalIgnoreCase));
            }

            if (filter.CategoryIds.Any())
            {
                var categoryIds = filter.CategoryIds;
                predicate = predicate
                        .AndAlso(
                            a => categoryIds.Contains(a.CategoryId));
            }

            if (filter.TagIds.Any())
            {
                var tagIds = filter.TagIds;
                predicate = predicate
                    .AndAlso(a => a.Tags!
                        .Any(t => tagIds.Contains(t.Id)));
            }

            return predicate;
        }

        public static Func<IQueryable<Article>, IOrderedQueryable<Article>>? 
            GetOrderBy(this
            ArticleFilterRequestDto filter) => filter.Sorting switch
            {
                Sorting.None => null,
                Sorting.Ascending => q => q.OrderBy(a => a.Name),
                Sorting.Descending => q => q.OrderByDescending(a => a.Name),
                _ => throw new ArgumentOutOfRangeException(nameof(filter.Sorting))
            };
    }

}
