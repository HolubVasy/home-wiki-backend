namespace home_wiki_backend.DAL.Configurations;
/// <summary>
/// Constant set, used for making length restriction for a
/// string type column in a database.
/// </summary>
public static class StringMaxLengths
{

    /// <summary>
    /// Restriction on column length of 150 characters.
    /// </summary>
    public const short Long = 150;

    /// <summary>
    /// Restriction on column length of 30 characters.
    /// </summary>
    public const short Short30 = 30;

    /// <summary>
    /// Restriction on column length of 30 characters.
    /// </summary>
    public const short Short50 = 50;
}