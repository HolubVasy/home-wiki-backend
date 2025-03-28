using home_wiki_backend.Shared.Enums;
using System.Collections.Immutable;

namespace home_wiki_backend.BL.Models
{
    public abstract class FilterRequestDtoBase
    {
        public int PageNumber { get; protected set; }
        public int PageSize { get; protected set; }
        public string PartName { get; protected set; }
        public Sorting Sorting { get; protected set; } = Sorting.None;


        protected FilterRequestDtoBase(
            int pageNumber,
            int pageSize,
            Sorting sorting,
            string partName)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Sorting = sorting;
            PartName = partName;
        }

        protected FilterRequestDtoBase(
            int pageNumber,
            int pageSize,
            string partName)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            PartName = partName;
        }

        protected FilterRequestDtoBase(
            int pageNumber,
            int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            PartName = string.Empty;
        }

    }
}
