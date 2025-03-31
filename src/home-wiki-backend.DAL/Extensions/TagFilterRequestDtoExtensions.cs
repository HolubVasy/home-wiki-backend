using home_wiki_backend.BL.Models;
using home_wiki_backend.DAL.Common.Models.Entities;
using home_wiki_backend.Shared.Enums;
using home_wiki_backend.Shared.Helpers;
using System.Linq.Expressions;

namespace home_wiki_backend.BL.Extensions
{
    public static class TagFilterRequestDtoExtensions
    {
        public static Expression<Func<Tag, bool>> 
            GetPredicate(this TagFilterRequestDto filter)
        {
            Expression<Func<Tag, bool>> predicate = a => true;

            if (!string.IsNullOrEmpty(filter.PartName))
            {
                var partName = filter.PartName;
                predicate = predicate
                    .AndAlso(
                        a => a.Name.ToLower().Contains(partName.ToLower()));

            }

            return predicate;
        }

        public static Func<IQueryable<Tag>, IOrderedQueryable<Tag>>? 
            GetOrderBy(this
            TagFilterRequestDto filter) => filter.Sorting switch
            {
                Sorting.None => null,
                Sorting.Ascending => q => q.OrderBy(a => a.Name),
                Sorting.Descending => q => q.OrderByDescending(a => a.Name),
                _ => throw new ArgumentOutOfRangeException(nameof(filter.Sorting))
            };
    }

}
