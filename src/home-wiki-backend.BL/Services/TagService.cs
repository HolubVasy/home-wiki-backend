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
    public sealed class TagService : ITagService
    {
        private readonly IGenericRepository<Tag> _tagRepo;
        private readonly ILogger<TagService> _logger;

        public TagService(
            IGenericRepository<Tag> tagRepo,
            ILogger<TagService> logger)
        {
            _tagRepo = tagRepo;
            _logger = logger;
        }

        public async Task<TagResponse> CreateAsync(
            TagRequest tag,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Creating tag: {Name}", tag.Name);
                var newTag = new Tag
                {
                    Name = tag.Name,
                    CreatedBy = "system", // Replace as needed.
                    CreatedAt = DateTime.UtcNow
                };
                var created = await _tagRepo.AddAsync(newTag,
                    cancellationToken);
                _logger.LogInformation("Tag created with ID: {Id}", created.Id);
                return new TagResponse
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
                _logger.LogError(ex, "Error creating tag: {Name}", tag.Name);
                throw new TagServiceException("Error creating tag", ex);
            }
        }

        public async Task<TagResponse> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Getting tag by ID: {Id}", id);
                var tag = await _tagRepo.FirstOrDefaultAsync(
                    t => t.Id == id, cancellationToken);
                if (tag == null)
                {
                    _logger.LogWarning("Tag with ID {Id} not found.", id);
                    throw new KeyNotFoundException(
                        $"Tag with ID {id} not found.");
                }
                return new TagResponse
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    CreatedBy = tag.CreatedBy,
                    CreatedAt = tag.CreatedAt,
                    ModifiedBy = tag.ModifiedBy,
                    ModifiedAt = tag.ModifiedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tag by ID: {Id}", id);
                throw new TagServiceException("Error retrieving tag", ex);
            }
        }

        public async Task<IEnumerable<TagResponse>> GetAsync(
            Expression<Func<TagRequest, bool>>? predicate = null,
            Func<IQueryable<TagRequest>,
                IOrderedQueryable<TagRequest>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Getting tag list.");
                var pred = predicate?.ConvertTo<TagRequest, Tag>();
                var tags = await _tagRepo.GetAsync(
                    pred,
                    orderBy?.ConvertTo<TagRequest, Tag>(),
                    cancellationToken);
                return tags.Select(t => new TagResponse
                {
                    Id = t.Id,
                    Name = t.Name,
                    CreatedBy = t.CreatedBy,
                    CreatedAt = t.CreatedAt,
                    ModifiedBy = t.ModifiedBy,
                    ModifiedAt = t.ModifiedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tag list.");
                throw new TagServiceException("Error retrieving tags", ex);
            }
        }

        public async Task<IEnumerable<TagResponse>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<TagRequest, bool>>? predicate = null,
            Func<IQueryable<TagRequest>,
                IOrderedQueryable<TagRequest>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Paged tags: page {PageNumber}, size {PageSize}",
                    pageNumber, pageSize);
                var pred = predicate?.ConvertTo<TagRequest, Tag>();
                var paged = await _tagRepo.GetPagedAsync(
                    pageNumber,
                    pageSize,
                    pred,
                    orderBy?.ConvertTo<TagRequest, Tag>(),
                    cancellationToken);
                return paged?.Items?.Select(t => new TagResponse
                {
                    Id = t.Id,
                    Name = t.Name,
                    CreatedBy = t.CreatedBy,
                    CreatedAt = t.CreatedAt,
                    ModifiedBy = t.ModifiedBy,
                    ModifiedAt = t.ModifiedAt
                }) ?? Array.Empty<TagResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged tags.");
                throw new TagServiceException("Error retrieving paged tags", ex);
            }
        }

        public async Task<TagResponse> UpdateAsync(
            TagRequest tag,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating tag ID: {Id}", tag.Id);
                var exist = await _tagRepo.FirstOrDefaultAsync(
                    t => t.Id == tag.Id, cancellationToken);
                if (exist == null)
                {
                    _logger.LogWarning("Tag ID {Id} not found for update.",
                        tag.Id);
                    throw new KeyNotFoundException(
                        $"Tag with ID {tag.Id} not found.");
                }
                var upd = new Tag
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    CreatedBy = exist.CreatedBy,
                    CreatedAt = exist.CreatedAt,
                    ModifiedBy = "system", // Replace as needed.
                    ModifiedAt = DateTime.UtcNow
                };
                await _tagRepo.UpdateAsync(upd, cancellationToken);
                return new TagResponse
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
                _logger.LogError(ex, "Error updating tag ID: {Id}", tag.Id);
                throw new TagServiceException("Error updating tag", ex);
            }
        }

        public async Task DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Deleting tag ID: {Id}", id);
                await _tagRepo.RemoveAsync(
                    t => t.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tag ID: {Id}", id);
                throw new TagServiceException("Error deleting tag", ex);
            }
        }

        public async Task RemoveAsync(
            TagRequest tag,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Removing tag: {Name}", tag.Name);
                await _tagRepo.RemoveAsync(
                    t => t.Name == tag.Name, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing tag: {Name}", tag.Name);
                throw new TagServiceException("Error removing tag", ex);
            }
        }

        public async Task<bool> ExistsAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Checking tag exists: ID {Id}", id);
                return await _tagRepo.AnyAsync(
                    t => t.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking tag existence for ID: {Id}",
                    id);
                throw new TagServiceException("Error checking tag existence", ex);
            }
        }

        public async Task<bool> AnyAsync(
            Expression<Func<TagRequest, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Checking any tag match.");
                var pred = predicate?.ConvertTo<TagRequest, Tag>();
                return await _tagRepo.AnyAsync(pred, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AnyAsync for tags.");
                throw new TagServiceException("Error checking any tag match", ex);
            }
        }

        public async Task<TagResponse?> FirstOrDefault(
            Expression<Func<TagRequest, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Getting first matching tag.");
                var pred = predicate?.ConvertTo<TagRequest, Tag>();
                var tag = await _tagRepo.FirstOrDefaultAsync(
                    pred, cancellationToken);
                if (tag == null)
                {
                    _logger.LogWarning("No matching tag found.");
                    return null;
                }
                return new TagResponse
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    CreatedBy = tag.CreatedBy,
                    CreatedAt = tag.CreatedAt,
                    ModifiedBy = tag.ModifiedBy,
                    ModifiedAt = tag.ModifiedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FirstOrDefault for tag.");
                throw new TagServiceException("Error getting first matching tag", ex);
            }
        }

        public async Task<IEnumerable<TagResponse>> GetListAsync(
            ISpecification<Tag> specification,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Getting list via spec.");
                var tags = await _tagRepo.ListAsync(specification,
                    cancellationToken);
                return tags.Select(t => new TagResponse
                {
                    Id = t.Id,
                    Name = t.Name,
                    CreatedBy = t.CreatedBy,
                    CreatedAt = t.CreatedAt,
                    ModifiedBy = t.ModifiedBy,
                    ModifiedAt = t.ModifiedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tag list via spec.");
                throw new TagServiceException("Error retrieving tag list by spec", ex);
            }
        }
    }
}
