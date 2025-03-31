using home_wiki_backend.Shared.Enums;
using home_wiki_backend.Shared.Models.Dtos.Common;
using System.Collections.Immutable;

namespace home_wiki_backend.Shared.Models.Dtos
{
    public sealed class ArticleFilterRequestDto : FilterRequestDtoBase
    {
        public ImmutableHashSet<int> CategoryIds { get; } = ImmutableHashSet<int>.Empty;
        public IImmutableSet<int> TagIds { get; } = ImmutableHashSet<int>.Empty;

        public ArticleFilterRequestDto(
            int pageNumber,
            int pageSize,
            Sorting sorting,
            string partName,
            ImmutableHashSet<int> categoryIds,
            ImmutableHashSet<int> tagIds) : base(pageNumber, pageSize, sorting, partName)
        {
            CategoryIds = categoryIds;
            TagIds = tagIds;
            CategoryIds = categoryIds;
            TagIds = tagIds;
            PartName = partName;
        }

        public ArticleFilterRequestDto(
            int pageNumber,
            int pageSize,
            string partName) : base(pageNumber, pageSize, partName)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            PartName = partName;
        }

        public ArticleFilterRequestDto(
            int pageNumber,
            int pageSize) : base(pageNumber, pageSize)
        {
        }

    }
}
