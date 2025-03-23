using home_wiki_backend.Shared.Contracts;

namespace home_wiki_backend.Shared.Models
{
    /// <summary>
    ///     Represents the base class for an tag.
    /// </summary>
    public abstract class TagBase : IName
    {

        /// <summary>
        ///     Gets the name of the tag.
        /// </summary>
        public string Name { get; init; } = null!;
    }
}
