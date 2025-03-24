using home_wiki_backend.Shared.Models;

namespace home_wiki_backend.BL.Common.Models.Requests
{
    public sealed class CategoryRequest : CategoryBase
    {
        #region Identity

        /// <inheritdoc/>
        public int Id { get; init; }

        #endregion
    }
}
