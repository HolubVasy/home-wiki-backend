namespace home_wiki_backend.Shared.Contracts;

/// <summary>
///     Represents the name of the entity.
/// </summary>
public interface IName
{
    /// <summary>
    ///     Gets or sets the name of the entity.
    /// </summary>
    public string Name { get; init; }
}