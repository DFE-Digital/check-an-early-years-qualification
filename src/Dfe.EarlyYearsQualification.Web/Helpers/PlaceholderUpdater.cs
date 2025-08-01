using Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.Web.Helpers;

public class PlaceholderUpdater(IDateTimeAdapter dateTimeAdapter, IUserJourneyCookieService userJourneyCookieService) : IPlaceholderUpdater
{
    private const string ActualYearPlaceholder = "$[actual-year]$";
    private const string LevelForSept14ToAug19 = "$[level-for-Aug14-to-Sept19]$";

    public string Replace(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        var result = text;
        if (text.Contains(ActualYearPlaceholder))
        {
            result = result.Replace(ActualYearPlaceholder, dateTimeAdapter.Now().Year.ToString());
        }

        if (text.Contains(LevelForSept14ToAug19))
        {
            var levelBeingChecked = userJourneyCookieService.GetLevelOfQualification();
            if (levelBeingChecked != null)
            {
                result = result.Replace(LevelForSept14ToAug19, levelBeingChecked <= 5 ? "level 3" : "level 3 or level 6");
            }
        }

        return result;
    }
}