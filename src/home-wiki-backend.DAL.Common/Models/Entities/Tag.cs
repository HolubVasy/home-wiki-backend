using home_wiki_backend.DAL.Common.Models.Bases;

namespace home_wiki_backend.DAL.Common.Models.Entities
{
    /// <summary>
    /// Represents a tag entity.
    /// </summary>
    public sealed class Tag : ModelBase
    {
        #region Entity relationships

        /// <summary>
        /// Gets the articles associated with the tag.
        /// </summary>
        public HashSet<Article>? Articles { get; init; }

        #endregion
    }
}
