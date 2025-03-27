using System.Linq.Expressions;

namespace home_wiki_backend.Shared.Helpers
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> AndAlso<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var left = Expression.Invoke(expr1, parameter);
            var right = Expression.Invoke(expr2, parameter);
            var body = Expression.AndAlso(left, right);

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
