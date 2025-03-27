using home_wiki_backend.Shared.Enums;

namespace home_wiki_backend.Shared.Extensions
{
    public static class SortingExtensions
    {
        public static string GetStringRepresentation(this Sorting sorting)
            => sorting == Sorting.None
                    ? "without sorting" :
                        sorting == Sorting.Ascending
                            ? "Ascending" : "Descending";

    }
}
