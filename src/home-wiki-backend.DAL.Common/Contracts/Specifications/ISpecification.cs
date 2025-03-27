using System.Linq.Expressions;

namespace home_wiki_backend.DAL.Common.Contracts.Specifications;

/// <summary>
/// Represents a specification pattern that encapsulates the
/// criteria and logic for querying entities.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Gets the filter criteria for the specification.
    /// </summary>
    Expression<Func<T, bool>>? Criteria { get; }

    /// <summary>
    /// Gets the list of include expressions for related entities.
    /// </summary>
    List<Expression<Func<T, object>>> Includes { get; }

    /// <summary>
    /// Gets the expression for sorting the results in ascending order.
    /// </summary>
    Func<IQueryable<T>, IOrderedQueryable<T>>? Sorting { get; }
}
