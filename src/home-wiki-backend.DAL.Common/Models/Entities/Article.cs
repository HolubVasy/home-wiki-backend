using home_wiki_backend.DAL.Common.Models.Base;

namespace home_wiki_backend.DAL.Models.Entities
{
    public sealed class Article : ModelBase
    {
        #region Entity relationships

        public int CategoryId { get; init; }

        public Category Category { get; init; } = null!;

        public HashSet<Tag>? Tags { get; init; }

        #endregion
    }
}
