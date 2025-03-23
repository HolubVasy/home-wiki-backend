using home_wiki_backend.DAL.Common.Models.Bases;

namespace home_wiki_backend.DAL.Common.Models.Entities
{
    public sealed class Article : ModelBase
    {
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;

        #region Entity relationships

        public int CategoryId { get; init; }

        public Category Category { get; init; } = null!;

        public HashSet<Tag>? Tags { get; init; }

        #endregion
    }
}
