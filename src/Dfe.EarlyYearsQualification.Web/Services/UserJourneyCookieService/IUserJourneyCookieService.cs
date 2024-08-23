using Dfe.EarlyYearsQualification.Web.Models;

namespace Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

public interface IUserJourneyCookieService
{
    public void SetWhereWasQualificationAwarded(string location);
    public void SetWhenWasQualificationStarted(string date);
    public void SetLevelOfQualification(string level);
    public void SetAwardingOrganisation(string awardingOrganisation);
    public void SetAdditionalQuestionsAnswers(Dictionary<string, string> additionalQuestionsAnswers);

    public void SetQualificationNameSearchCriteria(string searchCriteria);
    public UserJourneyModel GetUserJourneyModelFromCookie();
    public void SetUserJourneyModelCookie(UserJourneyModel model);
    public void ResetUserJourneyCookie();

    public string? GetWhereWasQualificationAwarded();

    public (int? startMonth, int? startYear) GetWhenWasQualificationStarted();
    bool WasStartedBeforeSeptember2014();
    bool WasStartedBetweenSeptember2014AndAugust2019();

    public int? GetLevelOfQualification();
    public string? GetAwardingOrganisation();

    public string? GetSearchCriteria();
    public Dictionary<string, string>? GetAdditionalQuestionsAnswers();
    public bool UserHasAnsweredAdditionalQuestions();
}