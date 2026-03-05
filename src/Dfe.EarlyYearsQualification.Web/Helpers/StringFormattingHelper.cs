using System.Text.RegularExpressions;

namespace Dfe.EarlyYearsQualification.Web.Helpers;

public static partial class StringFormattingHelper
{
    [GeneratedRegex(@"\s*/\s*")]
    private static partial Regex MatchSpacesAndSlashesRegex();

    public static string? FormatSlashedNumbers(string? input)
    {
        if (input is null)
        {
            return null;
        }

        return MatchSpacesAndSlashesRegex().Replace(input, " / ");
    }
    
    public static string ToHtmlId(string input)
    {
        return Regex.Replace(input, @"[^a-zA-Z0-9_-]", "-").ToLower();
    }
}