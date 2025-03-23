using home_wiki_backend.Shared.Contracts;

namespace home_wiki_backend.Shared.Models
{
    /// <summary>
    ///     Represents the base class for an article.
    /// </summary>
    public abstract class ArticleBase : IName
    {
        /// <summary>
        ///     Gets the description of the article.
        /// </summary>
        public string Description { get; init; } = null!;

        /// <summary>
        ///     Gets the name of the article.
        /// </summary>
        public string Name { get; init; } = null!;
    }
}
