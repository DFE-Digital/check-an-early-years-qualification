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
}