using home_wiki_backend.DAL.Common.Models.Bases;
using System.Linq.Expressions;

namespace home_wiki_backend.DAL.Common.Contracts
{
    public interface IGenericRepository<TEntity> where TEntity : ModelBase
    {
        /// <summary>
        /// Asynchronously gets a collection of entities that satisfy the specified
        /// predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.
        /// </param>
        /// <param name="cancellationToken">A cancellation token that can be used to
        /// cancel the work.</param>
        /// <returns>A task that represents the asynchronous operation. The task result
        /// contains a collection of entities.</returns>
        Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously gets the first entity that satisfies the specified predicate
        /// or null if no such entity is found.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.
        /// </param>
        /// <param name="cancellationToken">A cancellation token that can be used to
        /// cancel the work.</param>
        /// <returns>A task that represents the asynchronous operation. The task result
        /// contains the first entity or null.</returns>
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously determines whether any entity satisfies the specified
        /// predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.
        /// </param>
        /// <param name="cancellationToken">A cancellation token that can be used to
        /// cancel the work.</param>
        /// <returns>A task that represents the asynchronous operation. The task result
        /// contains true if any entity satisfies the predicate; otherwise, false.
        /// </returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to
        /// cancel the work.</param>
        /// <returns>A task that represents the asynchronous operation. The task result
        /// contains the added entity.</returns>
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to
        /// cancel the work.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously gets a collection of entities that satisfy the specified
        /// predicate, including specified navigation properties.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.
        /// </param>
        /// <param name="includes">A collection of lambda expressions representing the
        /// paths to the navigation properties to be included.</param>
        /// <returns>A task that represents the asynchronous operation. The task result
        /// contains a collection of entities.</returns>
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Asynchronously gets an entity by its identifier, including specified
        /// navigation properties.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.
        /// </param>
        /// <param name="includes">A collection of lambda expressions representing the
        /// paths to the navigation properties to be included.</param>
        /// <returns>A task that represents the asynchronous operation. The task result
        /// contains the entity.</returns>
        Task<TEntity?> GetByIdAsync(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Asynchronously removes entities that satisfy the specified predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.
        /// </param>
        /// <param name="cancellationToken">A cancellation token that can be used to
        /// cancel the work.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously gets a collection of entities that satisfy the specified
        /// predicate, including nested navigation properties.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.
        /// </param>
        /// <param name="navigationPropertyPaths">A collection of lambda expressions
        /// representing the paths to the navigation properties to be included.
        /// </param>
        /// <param name="cancellationToken">A cancellation token that can be used to
        /// cancel the work.</param>
        /// <returns>A task that represents the asynchronous operation. The task result
        /// contains a collection of entities.</returns>
        Task<IEnumerable<TEntity>> GetWithNestedIncludesAsync(Expression<Func<TEntity, bool>> predicate,
            IEnumerable<Expression<Func<TEntity, object>>[]> navigationPropertyPaths,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a queryable collection of entities.
        /// </summary>
        /// <returns>A queryable collection of entities.</returns>
        IQueryable<TEntity> GetQueryable();

        /// <summary>
        /// Asynchronously gets all entities.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to
        /// cancel the work.</param>
        /// <returns>A task that represents the asynchronous operation. The task result
        /// contains a collection of all entities.</returns>
        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken 
            = default);

        /// <summary>
        /// Asynchronously gets a paginated collection of entities.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of entities per page.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to
        /// cancel the work.</param>
        /// <returns>A task that represents the asynchronous operation. The task result
        /// contains a paginated collection of entities.</returns>
        Task<IReadOnlyList<TEntity>> GetPagedAsync(int pageNumber, int pageSize,
            CancellationToken cancellationToken = default);
    }
}
