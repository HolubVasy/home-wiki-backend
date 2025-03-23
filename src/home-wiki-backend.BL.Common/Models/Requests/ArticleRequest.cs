using home_wiki_backend.Shared.Models;

namespace home_wiki_backend.BL.Common.Models.Requests
{
    public sealed class ArticleRequest : ArticleBase
    {
        public int CategoryId { get; init; }
        public string CreatedBy { get; init; } = null!;
    }
}
