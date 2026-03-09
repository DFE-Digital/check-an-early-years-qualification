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

    public static string ConvertToDateString(int? dateMonth, int? dateYear)
    {
        if (dateMonth is null || dateYear is null) return string.Empty;
        var date = new DateOnly(dateYear.Value, dateMonth.Value, 1);
        return $"{date.ToString("MMMM", CultureInfo.InvariantCulture)} {dateYear.Value}";
    }
}