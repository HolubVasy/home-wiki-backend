using home_wiki_backend.Shared.Contracts;
using home_wiki_backend.Shared.Models;

namespace home_wiki_backend.BL.Common.Models.Requests
{
    public sealed class TagRequest : TagBase, IIdentifier
    {

        #region Identity

        /// <inheritdoc/>
        public int Id { get; init; }

        #endregion
    }
}
