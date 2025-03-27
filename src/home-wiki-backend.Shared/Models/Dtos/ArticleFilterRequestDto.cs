using home_wiki_backend.Shared.Enums;
using System.Collections.Immutable;

namespace home_wiki_backend.BL.Models
{
    public sealed class ArticleFilterRequestDto
    {
        public int PageNumber { get; }
        public int PageSize { get; }
        public string PartName { get; }
        public Sorting Sorting { get; } = Sorting.None;

        public ImmutableHashSet<int> CategoryIds { get; } = ImmutableHashSet<int>.Empty;
        public IImmutableSet<int> TagIds { get; } = ImmutableHashSet<int>.Empty;

        public ArticleFilterRequestDto(
            int pageNumber,
            int pageSize,
            Sorting sorting,
            string partName,
            ImmutableHashSet<int> categoryIds,
            ImmutableHashSet<int> tagIds)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Sorting = sorting;
            PartName = partName;
            CategoryIds = categoryIds;
            TagIds = tagIds;
        }

        public ArticleFilterRequestDto(
            int pageNumber,
            int pageSize,
            string partName)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            PartName = partName;
        }

        public ArticleFilterRequestDto(
            int pageNumber,
            int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            PartName = string.Empty;
        }

    }
}
