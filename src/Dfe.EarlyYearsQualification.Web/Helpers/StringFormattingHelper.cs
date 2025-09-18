using System.Text.RegularExpressions;

namespace Dfe.EarlyYearsQualification.Web.Helpers;

public static class StringFormattingHelper
{
    public static string? FormatSlashedNumbers(string? input)
    {
        if (input is null)
        {
            return null;
        }

        return Regex.Replace(input, @"\s*/\s*", " / ");
    }
}