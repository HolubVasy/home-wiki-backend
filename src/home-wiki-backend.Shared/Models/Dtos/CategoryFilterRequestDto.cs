using home_wiki_backend.Shared.Enums;
using home_wiki_backend.Shared.Models.Dtos.Common;

namespace home_wiki_backend.Shared.Models.Dtos
{
    public sealed class CategoryFilterRequestDto : FilterRequestDtoBase
    {
        public CategoryFilterRequestDto(
            int pageNumber,
            int pageSize,
            Sorting sorting,
            string partName) : base(pageNumber, pageSize, sorting, partName)
        {
        }

        public CategoryFilterRequestDto(
            int pageNumber,
            int pageSize,
            string partName) : base(pageNumber, pageSize, partName)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            PartName = partName;
        }

        public CategoryFilterRequestDto(
            int pageNumber,
            int pageSize) : base(pageNumber, pageSize)
        {
        }

    }
}
