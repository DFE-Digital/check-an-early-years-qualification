using Dfe.EarlyYearsQualification.Web.Models;

namespace Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

public interface IUserJourneyCookieService
{
    public void SetWhereWasQualificationAwarded(string location);
    public void SetWhenWasQualificationAwarded(string date);
    public void SetLevelOfQualification(string level);
    public UserJourneyModel GetUserJourneyModelFromCookie();
    public void ResetUserJourneyCookie();
}
