using home_wiki_backend.Shared.Enums;
using home_wiki_backend.Shared.Models.Dtos.Common;

namespace home_wiki_backend.Shared.Models.Dtos
{
    public sealed class TagFilterRequestDto : FilterRequestDtoBase
    {
        public TagFilterRequestDto(
            int pageNumber,
            int pageSize,
            Sorting sorting,
            string partName) : base(pageNumber, pageSize, sorting, partName)
        {
        }

        public TagFilterRequestDto(
            int pageNumber,
            int pageSize,
            string partName) : base(pageNumber, pageSize, partName)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            PartName = partName;
        }

        public TagFilterRequestDto(
            int pageNumber,
            int pageSize) : base(pageNumber, pageSize)
        {
        }

    }
}
