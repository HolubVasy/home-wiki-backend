using System.Text.RegularExpressions;

namespace home_wiki_backend.DAL.Common.Extensions;

/// <summary>
/// Format entity property name into the name of database names.
/// </summary>
internal static class StringExtensions
{
    internal static string SplitComplexNameAndFormat(this string complexName)
    {
        var regex = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])");
        return regex.Replace(complexName, "_").ToLower();
    }
}