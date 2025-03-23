using home_wiki_backend.DAL.Common.Models.Bases;

namespace home_wiki_backend.DAL.Common.Models.Entities
{
    /// <summary>
    /// Represents a category entity.
    /// </summary>
    public sealed class Category : ModelBase
    {
        #region Entity relationships

        /// <summary>
        /// Gets the articles associated with the category.
        /// </summary>
        public HashSet<Article>? Articles { get; init; }

        #endregion
    }
}
