using System.ComponentModel;

namespace home_wiki_backend.Shared.Enums
{
    public enum Sorting
    {
        [Description("without sorting")]
        None = 0,

        [Description("Ascending")]
        Ascending = 1,

        [Description("Descending")]
        Descending = 2,
    }
}
