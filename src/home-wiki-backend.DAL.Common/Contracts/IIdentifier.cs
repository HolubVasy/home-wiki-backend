namespace home_wiki_backend.DAL.Common.Contracts;

/// <summary>
/// Enforces a model to have an identifier in a shape of integer value.
/// </summary>
public interface IIdentifier
{
    /// <summary>
    /// Identifier of a model.
    /// </summary>
    public int Id { get; init; }
}