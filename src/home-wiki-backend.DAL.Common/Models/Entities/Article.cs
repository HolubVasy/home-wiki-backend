using home_wiki_backend.Shared.Contracts;
using home_wiki_backend.Shared.Models;

namespace home_wiki_backend.DAL.Common.Models.Entities
{
    /// <summary>
    /// Represents an article entity.
    /// </summary>
    public sealed class Article : ArticleBase, IIdentifier, IAuditable
    {
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

        #region Auditable properties

        /// <inheritdoc/>
        public string CreatedBy { get; init; } = null!;

        /// <inheritdoc/>
        public DateTime CreatedAt { get; init; }

        /// <inheritdoc/>
        public string? ModifiedBy { get; set; }

        /// <inheritdoc/>
        public DateTime? ModifiedAt { get; set; }

        #endregion
    }
}
