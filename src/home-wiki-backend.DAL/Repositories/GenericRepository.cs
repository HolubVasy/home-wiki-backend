using System.Linq.Expressions;
using home_wiki_backend.DAL.Common.Contracts;
using home_wiki_backend.DAL.Common.Models.Bases;
using home_wiki_backend.DAL.Data;
using home_wiki_backend.DAL.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace home_wiki_backend.DAL.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> 
    where TEntity : ModelBase
{
    private readonly DbWikiContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(DbWikiContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbSet.AsNoTracking()
                .Where(predicate)
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
    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate, cancellationToken)
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
            await using var newContext = new DbWikiContext(_context.Options);
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
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbSet
                .AsNoTracking()
                .AnyAsync(predicate, cancellationToken)
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
            await _context
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            // Detach the entity after saving changes
            _context.Entry(entity).State = EntityState.Detached;

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
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(cancellationToken)
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
    public async Task RemoveAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        try
        {
            var entityToRemove = await _dbSet
                .FirstOrDefaultAsync(predicate, cancellationToken)
                .ConfigureAwait(false);
            if (entityToRemove != null)
            {
                if (_context.Entry(entityToRemove).State == EntityState.Detached)
                {
                    _dbSet.Attach(entityToRemove);
                    _dbSet.Remove(entityToRemove);
                    await _context.SaveChangesAsync(cancellationToken)
                        .ConfigureAwait(false);
                }
            }
        }
        catch (Exception ex)
        {
            throw new GenericRepositoryException(
                $"An error occurred while removing an entity of type `{typeof(TEntity).Name}`.",
                ex);
        }
    }

    /// <inheritdoc/>
    public IQueryable<TEntity> GetQueryable()
    {
        return _dbSet.AsNoTracking();
    }

    /// <inheritdoc/>
    public async Task<TEntity?> GetByIdAsync(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            await using var newContext = new DbWikiContext(_context.Options);
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
            await using var newContext = new DbWikiContext(_context.Options);
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
            await using var newContext = new DbWikiContext(_context.Options);
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
    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await using var newContext = new DbWikiContext(_context.Options);
            var newDbSet = newContext.Set<TEntity>();
            return await newDbSet.AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new GenericRepositoryException(
                $"An error occurred while retrieving all entities of type `{typeof(TEntity).Name}`.",
                ex);
        }
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<TEntity>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var newContext = new DbWikiContext(_context.Options);
            var newDbSet = newContext.Set<TEntity>();
            return await newDbSet
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
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
}
