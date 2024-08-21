using Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;

namespace Dfe.EarlyYearsQualification.Web.Helpers;

public class PlaceholderUpdater(IDateTimeAdapter dateTimeAdapter) : IPlaceholderUpdater
{
    private const string ActualYearPlaceholder = "$[actual-year]$";
    public string Replace(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;
        
        var result = text;
        if (text.Contains(ActualYearPlaceholder))
        {
            result = result.Replace(ActualYearPlaceholder, dateTimeAdapter.Now().Year.ToString());
        }

        return result;
    }
}