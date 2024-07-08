using Dfe.EarlyYearsQualification.Web.Models;

namespace Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

public interface IUserJourneyCookieService
{
    public void SetWhereWasQualificationAwarded(string location);
    public void SetWhenWasQualificationAwarded(string date);
    public void SetLevelOfQualification(string level);
    public void SetAwardingOrganisation(string awardingOrganisation);
    
    public void SetNameSearchCriteria(string searchCriteria);
    public UserJourneyModel GetUserJourneyModelFromCookie();
    public void ResetUserJourneyCookie();

    public string? GetWhereWasQualificationAwarded();
    public (int? startMonth, int? startYear) GetWhenWasQualificationAwarded();
    public int? GetLevelOfQualification();
    public string? GetAwardingOrganisation();

    public string? GetSearchCriteria();
}