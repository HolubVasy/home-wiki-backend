using home_wiki_backend.Shared.Contracts;
using home_wiki_backend.Shared.Models;

namespace home_wiki_backend.BL.Common.Models.Requests
{
    public sealed class ArticleResponse : ArticleBase, IAuditable
    {
        #region Entity relationships

        /// <summary>
        /// Gets the category of the article.
        /// </summary>
        public CategoryBase Category { get; init; } = null!;

        /// <summary>
        /// Gets the tags associated with the article.
        /// </summary>
        public HashSet<TagBase>? Tags { get; init; }

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
