using home_wiki_backend.Shared.Contracts;
using home_wiki_backend.Shared.Models;

namespace home_wiki_backend.BL.Common.Models.Requests
{
    public sealed class ArticleRequest : ArticleBase, IIdentifier
    {

        #region Identity

        /// <inheritdoc/>
        public int Id { get; init; }

        #endregion

        public int CategoryId { get; init; }

        public string ModifiedBy { get; init; } = null!;

        public string CreatedBy { get; init; } = null!;
    }
}
