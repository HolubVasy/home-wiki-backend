namespace home_wiki_backend.DAL.Common.Resources;
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
    /// Restriction on column length of 500 characters.
    /// </summary>
    public const short Long500 = 500;

    /// <summary>
    /// Restriction on column length of 30 characters.
    /// </summary>
    public const short Short30 = 30;

    /// <summary>
    /// Restriction on column length of 50 characters.
    /// </summary>
    public const short Short50 = 50;

    /// <summary>
    /// Restriction on column length of 5000 characters.
    /// </summary>
    public const short Huge = 5000;
}