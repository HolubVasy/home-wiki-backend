namespace home_wiki_backend.Contracts;

/// <summary>
/// Contract for seeding database.
/// </summary>
public interface ISeeder
{
    /// <summary>
    /// Checks of data already seeded.
    /// </summary>
    /// <returns></returns>
    bool AlreadySeeded();

    /// <summary>
    /// Seeds data.
    /// </summary>
    void Seed();
}