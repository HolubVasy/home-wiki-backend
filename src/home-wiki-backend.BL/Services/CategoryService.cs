using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using home_wiki_backend.BL.Common.Contracts.Services;
using home_wiki_backend.BL.Common.Models.Requests;
using home_wiki_backend.DAL.Common.Contracts;
using home_wiki_backend.DAL.Common.Models.Entities;
using home_wiki_backend.DAL.Exceptions;
using home_wiki_backend.Shared.Helpers;

namespace home_wiki_backend.BL.Services
{
    public sealed class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _catRepo;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            IGenericRepository<Category> catRepo,
            ILogger<CategoryService> logger)
        {
            _catRepo = catRepo;
            _logger = logger;
        }

        public async Task<CategoryResponse> CreateAsync(
            CategoryRequest category,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Creating category: {Name}",
                    category.Name);
                var newCat = new Category
                {
                    Name = category.Name,
                    CreatedBy = "system", // Replace as needed.
                    CreatedAt = DateTime.UtcNow
                };
                var created = await _catRepo.AddAsync(newCat,
                    cancellationToken);
                _logger.LogInformation("Category created with ID: {Id}",
                    created.Id);
                return new CategoryResponse
                {
                    Id = created.Id,
                    Name = created.Name,
                    CreatedBy = created.CreatedBy,
                    CreatedAt = created.CreatedAt,
                    ModifiedBy = created.ModifiedBy,
                    ModifiedAt = created.ModifiedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category: {Name}",
                    category.Name);
                throw new CategoryServiceException("Error creating category", ex);
            }
        }

        public async Task<CategoryResponse> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Getting category by ID: {Id}", id);
                var cat = await _catRepo.FirstOrDefaultAsync(
                    c => c.Id == id, cancellationToken);
                if (cat == null)
                {
                    _logger.LogWarning("Category with ID {Id} not found.", id);
                    throw new KeyNotFoundException(
                        $"Category with ID {id} not found.");
                }
                return new CategoryResponse
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    CreatedBy = cat.CreatedBy,
                    CreatedAt = cat.CreatedAt,
                    ModifiedBy = cat.ModifiedBy,
                    ModifiedAt = cat.ModifiedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category by ID: {Id}", id);
                throw new CategoryServiceException("Error retrieving category", ex);
            }
        }

        public async Task<IEnumerable<CategoryResponse>> GetAsync(
            Expression<Func<CategoryRequest, bool>>? predicate = null,
            Func<IQueryable<CategoryRequest>,
                IOrderedQueryable<CategoryRequest>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Getting category list.");
                var pred = predicate?.ConvertTo<CategoryRequest, Category>();
                var cats = await _catRepo.GetAsync(
                    pred,
                    orderBy?.ConvertTo<CategoryRequest, Category>(),
                    cancellationToken);
                return cats.Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    CreatedBy = c.CreatedBy,
                    CreatedAt = c.CreatedAt,
                    ModifiedBy = c.ModifiedBy,
                    ModifiedAt = c.ModifiedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category list.");
                throw new CategoryServiceException("Error retrieving categories", ex);
            }
        }

        public async Task<IEnumerable<CategoryResponse>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<CategoryRequest, bool>>? predicate = null,
            Func<IQueryable<CategoryRequest>,
                IOrderedQueryable<CategoryRequest>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation(
                    "Paged categories: page {PageNumber}, size {PageSize}",
                    pageNumber, pageSize);
                var pred = predicate?.ConvertTo<CategoryRequest, Category>();
                var paged = await _catRepo.GetPagedAsync(
                    pageNumber,
                    pageSize,
                    pred,
                    orderBy?.ConvertTo<CategoryRequest, Category>(),
                    cancellationToken);
                return paged?.Items?.Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    CreatedBy = c.CreatedBy,
                    CreatedAt = c.CreatedAt,
                    ModifiedBy = c.ModifiedBy,
                    ModifiedAt = c.ModifiedAt
                }) ?? Array.Empty<CategoryResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged categories.");
                throw new CategoryServiceException("Error retrieving paged categories", ex);
            }
        }

        public async Task<CategoryResponse> UpdateAsync(
            CategoryRequest category,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating category ID: {Id}",
                    category.Id);
                var exist = await _catRepo.FirstOrDefaultAsync(
                    c => c.Id == category.Id, cancellationToken);
                if (exist == null)
                {
                    _logger.LogWarning("Category ID {Id} not found for update.",
                        category.Id);
                    throw new KeyNotFoundException(
                        $"Category with ID {category.Id} not found.");
                }
                var upd = new Category
                {
                    Id = category.Id,
                    Name = category.Name,
                    CreatedBy = exist.CreatedBy,
                    CreatedAt = exist.CreatedAt,
                    ModifiedBy = "system", // Replace as needed.
                    ModifiedAt = DateTime.UtcNow
                };
                await _catRepo.UpdateAsync(upd, cancellationToken);
                return new CategoryResponse
                {
                    Id = upd.Id,
                    Name = upd.Name,
                    CreatedBy = upd.CreatedBy,
                    CreatedAt = upd.CreatedAt,
                    ModifiedBy = upd.ModifiedBy,
                    ModifiedAt = upd.ModifiedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category ID: {Id}",
                    category.Id);
                throw new CategoryServiceException("Error updating category", ex);
            }
        }

        public async Task DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Deleting category ID: {Id}", id);
                await _catRepo.RemoveAsync(
                    c => c.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category ID: {Id}", id);
                throw new CategoryServiceException("Error deleting category", ex);
            }
        }

        public async Task RemoveAsync(
            CategoryRequest category,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Removing category: {Name}",
                    category.Name);
                await _catRepo.RemoveAsync(
                    c => c.Name == category.Name, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing category: {Name}",
                    category.Name);
                throw new CategoryServiceException("Error removing category", ex);
            }
        }

        public async Task<bool> ExistsAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Checking if category exists: ID {Id}",
                    id);
                return await _catRepo.AnyAsync(
                    c => c.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking existence for ID: {Id}",
                    id);
                throw new CategoryServiceException("Error checking category existence", ex);
            }
        }

        public async Task<bool> AnyAsync(
            Expression<Func<CategoryRequest, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Checking any category match.");
                var pred = predicate?.ConvertTo<CategoryRequest, Category>();
                return await _catRepo.AnyAsync(pred, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AnyAsync check for categories.");
                throw new CategoryServiceException("Error checking any category", ex);
            }
        }

        public async Task<CategoryResponse?> FirstOrDefault(
            Expression<Func<CategoryRequest, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Getting first matching category.");
                var pred = predicate?.ConvertTo<CategoryRequest, Category>();
                var cat = await _catRepo.FirstOrDefaultAsync(pred,
                    cancellationToken);
                if (cat == null)
                {
                    _logger.LogWarning("No matching category found.");
                    return null;
                }
                return new CategoryResponse
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    CreatedBy = cat.CreatedBy,
                    CreatedAt = cat.CreatedAt,
                    ModifiedBy = cat.ModifiedBy,
                    ModifiedAt = cat.ModifiedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FirstOrDefault for category.");
                throw new CategoryServiceException("Error getting first category",
                    ex);
            }
        }

        public async Task<IEnumerable<CategoryResponse>> GetListAsync(
            ISpecification<Category> specification,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Getting list via spec.");
                var cats = await _catRepo.ListAsync(specification,
                    cancellationToken);
                return cats.Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    CreatedBy = c.CreatedBy,
                    CreatedAt = c.CreatedAt,
                    ModifiedBy = c.ModifiedBy,
                    ModifiedAt = c.ModifiedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category list via spec.");
                throw new CategoryServiceException("Error retrieving category list by spec", ex);
            }
        }
    }
}
