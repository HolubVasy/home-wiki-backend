using System.Linq.Expressions;

public abstract class SpecificationBase<T> : ISpecification<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpecificationBase{T}"/> class.
    /// </summary>
    /// <param name="criteria">The filter criteria for the specification.</param>
    public SpecificationBase(Expression<Func<T, bool>>? criteria = null)
    {
        Criteria = criteria;
    }

    /// <summary>
    /// Gets the filter criteria for the specification.
    /// </summary>
    public Expression<Func<T, bool>>? Criteria { get; }

    /// <summary>
    /// Gets the list of include expressions for related entities.
    /// </summary>
    public List<Expression<Func<T, object>>> Includes { get; } =
        new List<Expression<Func<T, object>>>();

    /// <summary>
    /// Gets the expression for ordering the results in ascending order.
    /// </summary>
    public Expression<Func<T, object>>? OrderBy { get; private set; }

    /// <summary>
    /// Gets the expression for ordering the results in descending order.
    /// </summary>
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    /// <summary>
    /// Adds an include expression for related entities.
    /// </summary>
    /// <param name="includeExpression">The include expression.</param>
    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    /// <summary>
    /// Applies an ascending order by expression.
    /// </summary>
    /// <param name="orderByExpression">The order by expression.</param>
    protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    /// <summary>
    /// Applies a descending order by expression.
    /// </summary>
    /// <param name="orderByDescendingExpression">The order by descending expression.</param>
    protected void ApplyOrderByDescending(Expression<Func<T, object>> 
        orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
    }
}
