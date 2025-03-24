using System.Linq.Expressions;

namespace home_wiki_backend.Shared.Helpers
{
    public static class ExpressionExtensionsHelper
    {
        public static Expression<Func<TDestination, bool>>? ConvertTo<TSource,
            TDestination>(this Expression<Func<TSource, bool>>? source)
        {
            if (source == null) return null;

            var parameter = Expression.Parameter(typeof(TDestination), source.Parameters[0].Name);
            var body = new ExpressionConverterHelper<TSource, TDestination>(parameter).Visit(source.Body);
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
}
