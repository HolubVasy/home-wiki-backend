using home_wiki_backend.Shared.Contracts;
using home_wiki_backend.Shared.Models;

namespace home_wiki_backend.BL.Common.Models.Requests
{
    public sealed class CategoryResponse : CategoryBase, IAuditable, IIdentifier
    {
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
