using Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.Web.Helpers;

public class PlaceholderUpdater(IDateTimeAdapter dateTimeAdapter, IUserJourneyCookieService userJourneyCookieService) : IPlaceholderUpdater
{
    private const string ActualYearPlaceholder = "$[actual-year]$";
    private const string LevelForSept14ToAug19Placeholder = "$[level-for-Sept14-to-Aug19]$";
    private const string StartDatePlaceholder = "$[start-date]$";

    public string Replace(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        var result = text;
        if (text.Contains(ActualYearPlaceholder))
        {
            result = result.Replace(ActualYearPlaceholder, dateTimeAdapter.Now().Year.ToString());
        }

        if (text.Contains(LevelForSept14ToAug19Placeholder))
        {
            var levelBeingChecked = userJourneyCookieService.GetLevelOfQualification();
            if (levelBeingChecked != null)
            {
                result = result.Replace(LevelForSept14ToAug19Placeholder, levelBeingChecked <= 5 ? "level 3" : "level 3 or level 6");
            }
        }

        if (text.Contains(StartDatePlaceholder))
        {
            var (startedMonth, startedYear) = userJourneyCookieService.GetWhenWasQualificationStarted();

            if (startedMonth is not null && startedYear is not null)
            {
                result = result.Replace(StartDatePlaceholder, new DateOnly(startedYear.Value, startedMonth.Value, 1).ToString("MM-yyyy"));
            }
        }

        return result;
    }
}