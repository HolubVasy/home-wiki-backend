using home_wiki_backend.Shared.Enums;
using home_wiki_backend.Shared.Models.Dtos.Common;
using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace home_wiki_backend.Shared.Models.Dtos
{
    public sealed class ArticleFilterRequestDto : FilterRequestDtoBase
    {
        public ImmutableHashSet<int> CategoryIds { get; init; } = ImmutableHashSet<int>.Empty;
        public ImmutableHashSet<int> TagIds { get; init; } = ImmutableHashSet<int>.Empty;

        [JsonConstructor]
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
        }

        public ArticleFilterRequestDto(
            int pageNumber,
            int pageSize,
            string partName) : base(pageNumber, pageSize, partName)
        {
        }

        public ArticleFilterRequestDto(
            int pageNumber,
            int pageSize) : base(pageNumber, pageSize)
        {
        }
    }
}
