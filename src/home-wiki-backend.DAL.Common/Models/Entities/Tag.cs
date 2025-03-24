using home_wiki_backend.Shared.Contracts;
using home_wiki_backend.Shared.Models;

namespace home_wiki_backend.DAL.Common.Models.Entities
{
    /// <summary>
    /// Represents a tag entity.
    /// </summary>
    public sealed class Tag : TagBase, IIdentifier, IAuditable
    {
        #region Entity relationships

        /// <summary>
        /// Gets the articles associated with the tag.
        /// </summary>
        public HashSet<Article>? Articles { get; init; }

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
