using home_wiki_backend.BL.Common.Enums;

namespace home_wiki_backend.BL.Extensions
{
    internal static class SortingExtensions
    {
        internal static string GetStringRepresentation(this Sorting sorting)
            => sorting == Sorting.None
                    ? "without sorting" :
                        sorting == Sorting.Ascending
                            ? "Ascending" : "Descending";
        
    }
}
