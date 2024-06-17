using Dfe.EarlyYearsQualification.Web.Models;

namespace Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

public interface IUserJourneyCookieService
{
    public void SetWhereWasQualificationAwarded(string location);

    public void SetWhenWasQualificationAwarded(DateTime date);

    public void SetLevelOfQualification(int? level);
    public UserJourneyModel GetUserJourneyCookie();
}
