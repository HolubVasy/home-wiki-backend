using home_wiki_backend.DAL.Common.Models.Bases;

namespace home_wiki_backend.DAL.Common.Models.Entities
{
    /// <summary>
    /// Represents an article entity.
    /// </summary>
    public sealed class Article : ModelBase
    {
        /// <summary>
        /// Gets the description of the article.
        /// </summary>
        public string Description { get; init; } = null!;

        #region Entity relationships

        /// <summary>
        /// Gets the category ID of the article.
        /// </summary>
        public int CategoryId { get; init; }

        /// <summary>
        /// Gets the category of the article.
        /// </summary>
        public Category Category { get; init; } = null!;

        /// <summary>
        /// Gets the tags associated with the article.
        /// </summary>
        public HashSet<Tag>? Tags { get; init; }

        #endregion
    }
}
