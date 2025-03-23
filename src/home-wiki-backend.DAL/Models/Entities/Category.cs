using home_wiki_backend.DAL.Common.Models;

namespace home_wiki_backend.DAL.Models.Entities
{
    public sealed class Category : ModelBase
    {
        public HashSet<Article>? Articles { get; init; }
    }
}
