namespace home_wiki_backend.DAL.Common.Contracts
{
    /// <summary>
    /// Defines a contract for auditable entities.
    /// </summary>
    public interface IAuditable
    {
        /// <summary>
        /// Gets the user who created this record.
        /// </summary>
        string CreatedBy { get; init; }

        /// <summary>
        /// Gets the date and time when this record was created.
        /// </summary>
        DateTime CreatedAt { get; init; }

        /// <summary>
        /// Gets or sets the user who last modified this record.
        /// </summary>
        string? ModifiedBy { get; init; }

        /// <summary>
        /// Gets or sets the date and time when this record was last modified.
        /// </summary>
        DateTime? ModifiedAt { get; init; }
    }
}

