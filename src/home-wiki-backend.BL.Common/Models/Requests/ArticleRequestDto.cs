using home_wiki_backend.Shared.Contracts;
using home_wiki_backend.Shared.Models;

namespace home_wiki_backend.BL.Common.Models.Requests
{
    public sealed class ArticleRequestDto : ArticleBase, IIdentifier
    {
        public int CategoryId { get; init; }

        public string ModifiedBy { get; init; } = null!;

        public string CreatedBy { get; init; } = null!;

        /// <summary>
        /// Gets or sets the tags' identifiers, 
        /// associated with the article.
        /// </summary>
        public HashSet<int>? TagIds { get; init; }
    }
}
