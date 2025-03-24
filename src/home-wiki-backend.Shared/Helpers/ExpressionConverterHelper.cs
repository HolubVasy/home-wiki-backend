using System.Linq.Expressions;

namespace home_wiki_backend.Shared.Helpers
{
    public class ExpressionConverterHelper<TSource, TDestination> : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;

        public ExpressionConverterHelper(ParameterExpression parameter)
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
