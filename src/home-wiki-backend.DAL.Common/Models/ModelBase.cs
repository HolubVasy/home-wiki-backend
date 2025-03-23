using home_wiki_backend.DAL.Common.Contracts;

namespace home_wiki_backend.DAL.Common.Models;

/// <summary>
///     Represents the base entity in the application.
/// </summary>
public abstract class ModelBase : IEquatable<ModelBase>, IIdentifier,
    IAuditable, IName
{
    /// <inheritdoc/>
    public int Id { get; init; }

    /// <inheritdoc/>
    public string Name { get; init; } = null!;

    /// <inheritdoc/>
    public string CreatedBy { get; init; } = null!;

    /// <inheritdoc/>
    public DateTime CreatedAt { get; init; }

    /// <inheritdoc/>
    public string? ModifiedBy { get; init; }

    /// <inheritdoc/>
    public DateTime? ModifiedAt { get; init; }

    /// <summary>
    ///     Determines whether the specified ModelBase is 
    ///     equal to the current ModelBase.
    /// </summary>
    /// <param name="other">The ModelBase to compare with the
    /// current ModelBase.</param>
    /// <returns>true if the specified ModelBase is equal to 
    /// the current ModelBase; otherwise, false.</returns>
    public bool Equals(ModelBase? other)
    {
        return Id.Equals(other?.Id);
    }

    /// <summary>
    ///     Determines whether the specified object is equal 
    ///     to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.
    /// </param>
    /// <returns>true if the specified object is equal to the 
    /// current object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not ModelBase other)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        if (Id == default || other.Id == default)
        {
            return false;
        }

        return Id == other.Id;
    }

    /// <summary>
    ///     Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }

    /// <summary>
    ///     Determines whether two specified instances of ModelBase
    ///     are equal.
    /// </summary>
    /// <param name="a">The first instance to compare.</param>
    /// <param name="b">The second instance to compare.</param>
    /// <returns>true if a and b represent the same value; otherwise, 
    /// false.</returns>
    public static bool operator ==(ModelBase? a, ModelBase? b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
        {
            return true;
        }

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
        {
            return false;
        }

        return a.Equals(b);
    }

    /// <summary>
    ///     Determines whether two specified instances of ModelBase 
    ///     are not equal.
    /// </summary>
    /// <param name="a">The first instance to compare.</param>
    /// <param name="b">The second instance to compare.</param>
    /// <returns>true if a and b do not represent the same value; otherwise,
    /// false.</returns>
    public static bool operator !=(ModelBase? a, ModelBase? b)
    {
        return !(a == b);
    }
}