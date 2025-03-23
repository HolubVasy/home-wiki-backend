using home_wiki_backend.DAL.Common.Models.Base;

namespace home_wiki_backend.DAL.Models.Entities
{
    public sealed class Category : ModelBase
    {
        #region Entity relationships

        public HashSet<Article>? Articles { get; init; }

        #endregion
    }
}
