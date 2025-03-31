using System.Linq.Expressions;
using home_wiki_backend.DAL.Common.Contracts;
using home_wiki_backend.DAL.Common.Contracts.Specifications;
using home_wiki_backend.DAL.Common.Data;
using home_wiki_backend.DAL.Common.Helpers.Specifications;
using home_wiki_backend.DAL.Exceptions;
using home_wiki_backend.Shared.Contracts;
using home_wiki_backend.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace home_wiki_backend.DAL.Repositories;

public sealed class GenericRepository<TEntity> : IGenericRepository<TEntity> 
    where TEntity : class, IIdentifier, IName
{
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(DbWikiContext context)
    {
        DbContext = context;
        _dbSet = context.Set<TEntity>();
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? predicate = default,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _dbSet.AsNoTracking();
            if (predicate is not null)
            {
                query = query.Where(predicate);
            }
            if (orderBy is not null)
            {
                query = orderBy(query);
            }

            return await query
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new GenericRepositoryException(
                $"An error occurred while retrieving entities of type `{typeof(TEntity).Name}`.",
                ex);
        }
    }

    /// <inheritdoc/>
    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _dbSet.AsNoTracking();

            if (predicate is not null)
            {
                return await query
                    .FirstOrDefaultAsync(predicate, cancellationToken)
                    .ConfigureAwait(false);
            }

            return await query
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new GenericRepositoryException(
                $"An error occurred while retrieving the first entity of type `{typeof(TEntity).Name}`.",
                ex);
        }
    }

    /// <inheritdoc/>
    public async Task<TEntity?> FirstOrDefaultAsync(
        int id,
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var newContext = new DbWikiContext(DbContext.Options);
            var newDbSet = newContext.Set<TEntity>();
            var query = GetQuery(newDbSet, specification);

            return await query
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new GenericRepositoryException(
                $"An error occurred while retrieving the first entity of type `{typeof(TEntity).Name}`.",
                ex);
        }
    }

    /// <inheritdoc/>
    public async Task<TEntity?> FirstOrDefaultWithNewContextAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var newContext = new DbWikiContext(DbContext.Options);
            var newDbSet = newContext.Set<TEntity>();
            return await newDbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new GenericRepositoryException(
                $"An error occurred while retrieving the first entity of type `{typeof(TEntity).Name}` with a new context.",
                ex);
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? predicate, CancellationToken cancellationToken = default)
    {
        try
        {

            var query = _dbSet.AsNoTracking();

            if (predicate is not null)
            { 
                query = query.Where(predicate);
            }

            return await query
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new GenericRepositoryException(
                $"An error occurred while checking for the existence of entities of type `{typeof(TEntity).Name}`.",
                ex);
        }
    }

    /// <inheritdoc/>
    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _dbSet.Add(entity);
            await DbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            // Detach the entity after saving changes
            DbContext.Entry(entity).State = EntityState.Detached;

            return entity;
        }
        catch (Exception ex)
        {
            throw new GenericRepositoryException(
                $"An error occurred while adding an entity of type `{typeof(TEntity).Name}`.",
                ex);
        }
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            var beforeUpdate = await _dbSet.FirstOrDefaultAsync(e => e.Id == entity.Id);
            if (beforeUpdate is null)
            {
                throw new InvalidOperationException($"Entity with ID: `{entity.Id}` " +
                    $"not found in the database.");
            }

            DbContext.Entry(beforeUpdate).State = EntityState.Detached;
            DbContext.Entry(entity).State = EntityState.Modified;
            _dbSet.Update(entity);
            await DbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new GenericRepositoryException(
                $"An error occurred while updating an entity of type `{typeof(TEntity).Name}`.",
                ex);
        }
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(Expression<Func<TEntity, bool>>? predicate = default,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        var entityToRemove = await query
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
        await RemoveEntityAsync(entityToRemove, cancellationToken);
    }

    /// <inheritdoc/>
    public IQueryable<TEntity> GetQueryable()
    {
        return _dbSet;
    }

    /// <inheritdoc/>
    public async Task<TEntity?> GetByIdAsync(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            await using var newContext = new DbWikiContext(DbContext.Options);
            var newDbSet = newContext.Set<TEntity>();
            var query = newDbSet
                .AsNoTracking()
                .AsQueryable() ??
                    throw new InvalidOperationException();
            foreach (var include in includes) query = query.Include(include);

            return await query.FirstOrDefaultAsync(predicate)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new GenericRepositoryException(
                $"An error occurred while retrieving an entity of type `{typeof(TEntity).Name}` by ID.",
                ex);
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            await using var newContext = new DbWikiContext(DbContext.Options);
            var newDbSet = newContext.Set<TEntity>();
            var query = newDbSet
                .AsNoTracking()
                .AsQueryable();
            foreach (var include in includes) query = query.Include(include);

            return await query
                .Where(predicate)
                .ToListAsync()
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new GenericRepositoryException(
                $"An error occurred while retrieving entities of type `{typeof(TEntity).Name}` with includes.",
                ex);
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> GetWithNestedIncludesAsync(
        Expression<Func<TEntity, bool>> predicate,
        IEnumerable<Expression<Func<TEntity, object>>[]> navigationPropertyPaths,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var newContext = new DbWikiContext(DbContext.Options);
            var newDbSet = newContext.Set<TEntity>();
            var query = newDbSet.AsNoTracking();

            foreach (var propertyPath in navigationPropertyPaths)
            {
                var propertyNames = propertyPath.Select(GetPropertyName).ToArray();
                query = propertyNames.Aggregate(query, (current, property)
                    => current.Include(property));
            }

            return await query
                .Where(predicate)
                .ToListAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new GenericRepositoryException(
                $"An error occurred while retrieving entities of type `{typeof(TEntity).Name}` with nested includes.",
                ex);
        }
    }

    /// <inheritdoc/>
    public async Task<PagedList<TEntity>> GetPagedAsync(
        int pageNumber, int pageSize,
        Expression<Func<TEntity, bool>>? predicate = default,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var newContext = new DbWikiContext(DbContext.Options);
            var newDbSet = newContext.Set<TEntity>();
            var newQuery = newDbSet.AsNoTracking();
            if (predicate is not null)
            {
                newQuery = newQuery.Where(predicate);
            }
            if (orderBy is not null)
            {
                newQuery = orderBy(newQuery);
            }

            var totalItemCountTask = newQuery.CountAsync(cancellationToken);
            var query = _dbSet.AsNoTracking();
            var elementsTask = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            await Task.WhenAll(totalItemCountTask, elementsTask)
                .ConfigureAwait(false);

            var totalItemCount = await totalItemCountTask;
            var elements = await elementsTask;

            return new PagedList<TEntity>
            {
                PageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize),
                TotalItemCount = totalItemCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                HasPreviousPage = pageNumber > 1,
                HasNextPage = pageNumber < (int)Math.Ceiling(totalItemCount / (double)pageSize),
                Items = elements is null ? Array.Empty<TEntity>() : elements.AsReadOnly()
            };
        }
        catch (Exception ex)
        {
            throw new GenericRepositoryException(
                $"An error occurred while retrieving paginated entities of type `{typeof(TEntity).Name}`.",
                ex);
        }
    }

    /// <inheritdoc/>
    public async Task<PagedList<TEntity>> GetPagedAsync(
        int pageNumber, int pageSize,
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var newContext = new DbWikiContext(DbContext.Options);
            var newDbSet = newContext.Set<TEntity>();
            var queryNew = GetQuery(newDbSet, specification);

            var totalItemCountTask = queryNew.CountAsync(cancellationToken);

            var query = GetQuery(_dbSet, specification);
            var elementsTask = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            await Task.WhenAll(totalItemCountTask, elementsTask)
                .ConfigureAwait(false);

            var totalItemCount = await totalItemCountTask;
            var elements = await elementsTask;

            return new PagedList<TEntity>
            {
                PageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize),
                TotalItemCount = totalItemCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                HasPreviousPage = pageNumber > 1,
                HasNextPage = pageNumber < (int)Math.Ceiling(totalItemCount / (double)pageSize),
                Items = elements is null ? Array.Empty<TEntity>() : elements.AsReadOnly()
            };
        }
        catch (Exception ex)
        {
            throw new GenericRepositoryException(
                $"An error occurred while retrieving paginated entities of type `{typeof(TEntity).Name}`.",
                ex);
        }
    }

    private static string GetPropertyName<T>(Expression<Func<T, object>> expression)
    {
        return expression.Body switch
        {
            MemberExpression memberExpression => memberExpression.Member.Name,
            UnaryExpression { Operand: MemberExpression } unaryExpression =>
                ((MemberExpression)unaryExpression.Operand).Member.Name,
            _ => throw new ArgumentException("Expression is not a member access", nameof(expression))
        };
    }

    public async Task<TEntity?> FindAsync(
        int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task RemoveAsync(TEntity? entity, CancellationToken cancellationToken = default)
    {
        await RemoveEntityAsync(entity, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<TEntity>> ListAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = GetQuery(_dbSet, specification);
            return await query.ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new GenericRepositoryException(
                $"An error occurred while retrieving entities of type" +
                $" `{typeof(TEntity).Name}` using a specification.", ex);
        }
    }


    public DbWikiContext DbContext { get; }

    private async Task RemoveEntityAsync(TEntity? entity,
       CancellationToken cancellationToken = default)
    {
        try
        {
            if (entity is not null && DbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
                _dbSet.Remove(entity);
                await DbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            throw new GenericRepositoryException(
                $"An error occurred while removing an entity of type `{typeof(TEntity).Name}`.",
                ex);
        }
    }

    private IQueryable<TEntity> GetQuery(DbSet<TEntity> _dbSet, ISpecification<TEntity> specification)
    {
        return SpecificationEvaluator<TEntity>.GetQuery(_dbSet.AsNoTracking(), specification);
    }
}
