using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using home_wiki_backend.BL.Common.Contracts.Services;
using home_wiki_backend.BL.Common.Models.Requests;
using home_wiki_backend.DAL.Common.Contracts;
using home_wiki_backend.DAL.Common.Models.Entities;
using home_wiki_backend.Shared.Models.Results.Generic;
using home_wiki_backend.Shared.Enums;
using home_wiki_backend.Shared.Helpers;
using home_wiki_backend.Shared.Models.Results.Errors;
using home_wiki_backend.Shared.Models;
using home_wiki_backend.BL.Common.Models.Responses;

namespace home_wiki_backend.BL.Services
{
    /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<ResultModel<TagResponseDto>> CreateAsync(
            TagRequestDto tag,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Creating tag: {Name}", tag.Name);
                var newTag = new Tag
                {
                    Name = tag.Name,
                    CreatedBy = "system",
                    CreatedAt = DateTime.UtcNow
                };
                var created = await _tagRepo.AddAsync(newTag,
                    cancellationToken);
                _logger.LogInformation("Tag created with ID: {Id}",
                    created.Id);
                return new ResultModel<TagResponseDto>
                {
                    Success = true,
                    Message = "Tag created successfully",
                    Code = StatusCodes.Status201Created,
                    Data = new TagResponseDto
                    {
                        Id = created.Id,
                        Name = created.Name,
                        CreatedBy = created.CreatedBy,
                        CreatedAt = created.CreatedAt,
                        ModifiedBy = created.ModifiedBy,
                        ModifiedAt = created.ModifiedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating tag: {Name}", tag.Name);
                return new ResultModel<TagResponseDto>
                {
                    Success = false,
                    Message = "Error creating tag",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message,
                    ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<TagResponseDto>> GetByIdAsync(
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
                    return new ResultModel<TagResponseDto>
                    {
                        Success = false,
                        Message = $"Tag with ID {id} not found.",
                        Code = StatusCodes.Status404NotFound,
                        Error = new ErrorResultModel("Not found",
                        ErrorCode.Unexpected)
                    };
                }
                return new ResultModel<TagResponseDto>
                {
                    Success = true,
                    Message = "Tag retrieved successfully",
                    Code = StatusCodes.Status200OK,
                    Data = new TagResponseDto
                    {
                        Id = tag.Id,
                        Name = tag.Name,
                        CreatedBy = tag.CreatedBy,
                        CreatedAt = tag.CreatedAt,
                        ModifiedBy = tag.ModifiedBy,
                        ModifiedAt = tag.ModifiedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tag by ID: " +
                    "{Id}", id);
                return new ResultModel<TagResponseDto>
                {
                    Success = false,
                    Message = "Error retrieving tag by ID",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message,
                    ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModels<TagResponseDto>> GetAsync(
            Expression<Func<TagRequestDto, bool>>? predicate = null,
            Func<IQueryable<TagRequestDto>,
                IOrderedQueryable<TagRequestDto>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving tags with filters.");
                var pred = predicate?.ConvertTo<TagRequestDto, Tag>();
                var tags = await _tagRepo.GetAsync(pred,
                    orderBy?.ConvertTo<TagRequestDto, Tag>(),
                    cancellationToken);
                var data = tags.Select(t => new TagResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    CreatedBy = t.CreatedBy,
                    CreatedAt = t.CreatedAt,
                    ModifiedBy = t.ModifiedBy,
                    ModifiedAt = t.ModifiedAt
                }).ToList();
                return new ResultModels<TagResponseDto>
                {
                    Success = true,
                    Message = "Tags retrieved successfully",
                    Code = StatusCodes.Status200OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tags.");
                return new ResultModels<TagResponseDto>
                {
                    Success = false,
                    Message = "Error retrieving tags",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message,
                    ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<PagedList<TagResponseDto>>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<TagRequestDto, bool>>? predicate = null,
            Func<IQueryable<TagRequestDto>,
                IOrderedQueryable<TagRequestDto>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching paged tags. Page: " +
                    "{PageNumber}, Size: {PageSize}",
                    pageNumber, pageSize);
                var pred = predicate?.ConvertTo<TagRequestDto, Tag>();
                var paged = await _tagRepo.GetPagedAsync(pageNumber,
                    pageSize, pred,
                    orderBy?.ConvertTo<TagRequestDto, Tag>(), cancellationToken);
                var data = paged.Items.Select(t => new TagResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    CreatedBy = t.CreatedBy,
                    CreatedAt = t.CreatedAt,
                    ModifiedBy = t.ModifiedBy,
                    ModifiedAt = t.ModifiedAt
                }).ToList();
                return new ResultModel<PagedList<TagResponseDto>>
                {
                    Success = true,
                    Message = "Paged tags retrieved successfully",
                    Code = StatusCodes.Status200OK,
                    Data = new PagedList<TagResponseDto>()
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        PageCount = paged.PageCount,
                        HasNextPage = paged.HasNextPage,
                        HasPreviousPage = paged.HasPreviousPage,
                        TotalItemCount = paged.TotalItemCount,
                        Items = data,
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged tags.");
                return new ResultModel<PagedList<TagResponseDto>>
                {
                    Success = false,
                    Message = "Error retrieving paged tags",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message,
                    ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<TagResponseDto>> UpdateAsync(
            TagRequestDto tag,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating tag ID: {Id}", tag.Id);
                var existing = await _tagRepo.FirstOrDefaultAsync(
                    t => t.Id == tag.Id, cancellationToken);
                if (existing == null)
                {
                    _logger.LogWarning("Tag with ID {Id} not found.", tag.Id);
                    return new ResultModel<TagResponseDto>
                    {
                        Success = false,
                        Message = $"Tag with ID {tag.Id} not found.",
                        Code = StatusCodes.Status404NotFound,
                        Error = new ErrorResultModel("Not found",
                        ErrorCode.Unexpected)
                    };
                }
                var updated = new Tag
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    CreatedBy = existing.CreatedBy,
                    CreatedAt = existing.CreatedAt,
                    ModifiedBy = "system",
                    ModifiedAt = DateTime.UtcNow
                };
                await _tagRepo.UpdateAsync(updated, cancellationToken);
                return new ResultModel<TagResponseDto>
                {
                    Success = true,
                    Message = "Tag updated successfully",
                    Code = StatusCodes.Status200OK,
                    Data = new TagResponseDto
                    {
                        Id = updated.Id,
                        Name = updated.Name,
                        CreatedBy = updated.CreatedBy,
                        CreatedAt = updated.CreatedAt,
                        ModifiedBy = updated.ModifiedBy,
                        ModifiedAt = updated.ModifiedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tag ID: {Id}", tag.Id);
                return new ResultModel<TagResponseDto>
                {
                    Success = false,
                    Message = "Error updating tag",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message,
                    ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<TagResponseDto>> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Deleting tag ID: {Id}", id);
                await _tagRepo.RemoveAsync(t => t.Id == id,
                    cancellationToken);
                return new ResultModel<TagResponseDto>
                {
                    Success = true,
                    Message = "Tag deleted successfully",
                    Code = StatusCodes.Status200OK,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tag ID: {Id}", id);
                return new ResultModel<TagResponseDto>
                {
                    Success = false,
                    Message = "Error deleting tag",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message,
                    ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<TagResponseDto>> RemoveAsync(
            TagRequestDto tag,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Removing tag: {Name}",
                    tag.Name);
                await _tagRepo.RemoveAsync(t => t.Name == tag.Name,
                    cancellationToken);
                return new ResultModel<TagResponseDto>
                {
                    Success = true,
                    Message = "Tag removed successfully",
                    Code = StatusCodes.Status200OK,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing tag: " +
                    "{Name}", tag.Name);
                return new ResultModel<TagResponseDto>
                {
                    Success = false,
                    Message = "Error removing tag",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message,
                    ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _tagRepo.ExistsAsync(t => t.Id == id,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking tag " +
                    "existence for ID: {Id}", id);
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> AnyAsync(
            Expression<Func<TagRequestDto, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var pred = predicate?.ConvertTo<TagRequestDto, Tag>();
                return await _tagRepo.ExistsAsync(pred, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AnyAsync for tags.");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModel<TagResponseDto>> FirstOrDefault(
            Expression<Func<TagRequestDto, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var pred = predicate?.ConvertTo<TagRequestDto, Tag>();
                var tag = await _tagRepo.FirstOrDefaultAsync(pred,
                    cancellationToken);
                if (tag == null)
                {
                    return new ResultModel<TagResponseDto>
                    {
                        Success = false,
                        Message = "No matching tag found",
                        Code = StatusCodes.Status404NotFound,
                        Error = new ErrorResultModel("Not found",
                        ErrorCode.Unexpected)
                    };
                }
                return new ResultModel<TagResponseDto>
                {
                    Success = true,
                    Message = "Tag retrieved successfully",
                    Code = StatusCodes.Status200OK,
                    Data = new TagResponseDto
                    {
                        Id = tag.Id,
                        Name = tag.Name,
                        CreatedBy = tag.CreatedBy,
                        CreatedAt = tag.CreatedAt,
                        ModifiedBy = tag.ModifiedBy,
                        ModifiedAt = tag.ModifiedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FirstOrDefault for tags.");
                return new ResultModel<TagResponseDto>
                {
                    Success = false,
                    Message = "Error retrieving first tag",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message,
                    ErrorCode.Unexpected)
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultModels<TagResponseDto>> GetListAsync(
            ISpecification<Tag> specification,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving tags " +
                    "via specification.");
                var tags = await _tagRepo.ListAsync(specification,
                    cancellationToken);
                var data = tags.Select(t => new TagResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    CreatedBy = t.CreatedBy,
                    CreatedAt = t.CreatedAt,
                    ModifiedBy = t.ModifiedBy,
                    ModifiedAt = t.ModifiedAt
                }).ToList();
                return new ResultModels<TagResponseDto>
                {
                    Success = true,
                    Message = "Tags retrieved via specification",
                    Code = StatusCodes.Status200OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tags " +
                    "via specification.");
                return new ResultModels<TagResponseDto>
                {
                    Success = false,
                    Message = "Error retrieving tags by specification",
                    Code = StatusCodes.Status500InternalServerError,
                    Error = new ErrorResultModel(ex.Message,
                    ErrorCode.Unexpected)
                };
            }
        }
    }
}
