using home_wiki_backend.DAL.Common.Contracts.Specifications;
using Microsoft.EntityFrameworkCore;

namespace home_wiki_backend.DAL.Common.Helpers.Specifications;

public static class SpecificationEvaluator<T> where T : class
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery,
                                         ISpecification<T> specification)
    {
        var query = inputQuery;

        // Apply filtering criteria if present
        if (specification.Criteria != null)
            query = query.Where(specification.Criteria);

        // Apply include expressions
        query = specification.Includes.Aggregate(query, static (current, include) =>
        current.Include(include));

        // Apply ordering if provided
        if (specification.Sorting is not null)
        {
            query = specification.Sorting(query);
        }

        return query;
    }
}
