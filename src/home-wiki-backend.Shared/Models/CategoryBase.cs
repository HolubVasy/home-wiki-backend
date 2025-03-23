using home_wiki_backend.Shared.Contracts;

namespace home_wiki_backend.Shared.Models
{
    /// <summary>
    ///     Represents the base class for an category.
    /// </summary>
    public abstract class CategoryBase : IName
    {

        /// <summary>
        ///     Gets the name of the category.
        /// </summary>
        public string Name { get; init; } = null!;
    }
}
