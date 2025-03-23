using home_wiki_backend.DAL.Common.Models;

namespace home_wiki_backend.DAL.Models.Entities
{
    public sealed class Article : ModelBase
    {
        public Category Category { get; init; } = null!;

        public HashSet<Tag>? Tags { get; init; }
    }
}
