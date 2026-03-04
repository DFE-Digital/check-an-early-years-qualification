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
    
    public static bool DateIsBeforeSeptember2014(int? month, int? year)
    {
        if (month is null || year is null)
        {
            throw new
                InvalidOperationException("Unable to determine whether date was started before 09-2014");
        }

        var date = new DateOnly(year.Value, month.Value, 1);
        return date < new DateOnly(2014, 9, 1);
    }
}