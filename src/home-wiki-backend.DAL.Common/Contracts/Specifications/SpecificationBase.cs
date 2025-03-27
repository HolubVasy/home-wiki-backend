using System.Linq.Expressions;

namespace home_wiki_backend.DAL.Common.Contracts.Specifications;

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

    public Func<IQueryable<T>, IOrderedQueryable<T>>? Sorting { get; private set; }

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
    /// <param name="sorting">Sorting data.</param>
    protected void ApplySorting(Func<IQueryable<T>, IOrderedQueryable<T>>? sorting)
    {
        Sorting = sorting;
    }

}
