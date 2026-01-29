using System.Globalization;

namespace Dfe.EarlyYearsQualification.Web.Helpers;

public static class StringDateHelper
{
    // date is in the format MM/YYYY
    public static (int? startMonth, int? startYear) SplitDate(string date)
    {
        int? month = null;
        int? year = null;
        string[] dateSplit = date.Split('/');

        // ReSharper disable once InvertIf
        if (dateSplit.Length == 2
            && int.TryParse(dateSplit[0], out var parsedStartMonth)
            && int.TryParse(dateSplit[1], out var parsedStartYear))
        {
            month = parsedStartMonth;
            year = parsedStartYear;
        }

        return (month, year);
    }

    // date is in the format MMM-yy e.g Sep-19
    public static (int startMonth, int startYear)? ConvertDate(string date)
    {
        if (DateTime.TryParseExact(
                date,
                "MMM-yy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var converted))
        {
            return (converted.Month, converted.Year);
        }

        return null;
    }

    public static string ConvertToDateString(int? dateMonth, int? dateYear, string prefixValue)
    {
        if (dateMonth is null || dateYear is null) return string.Empty;
        var date = new DateOnly(dateYear.Value, dateMonth.Value, 1);
        return $"{prefixValue} {date.ToString("MMMM", CultureInfo.InvariantCulture)} {dateYear.Value}";
    }
}