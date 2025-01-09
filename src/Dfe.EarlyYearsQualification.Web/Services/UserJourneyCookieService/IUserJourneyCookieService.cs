namespace Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

public interface IUserJourneyCookieService
{
    void SetWhereWasQualificationAwarded(string location);
    void SetWhenWasQualificationStarted(string date);
    void SetLevelOfQualification(string level);
    void SetAwardingOrganisation(string awardingOrganisation);
    void SetAwardingOrganisationNotOnList(bool isOnList);
    void SetUserSelectedQualificationFromList(YesOrNo yesOrNo);
    void SetAdditionalQuestionsAnswers(IDictionary<string, string> additionalQuestionsAnswers);
    void ClearAdditionalQuestionsAnswers();
    void SetQualificationNameSearchCriteria(string searchCriteria);
    void ResetUserJourneyCookie();
    string? GetWhereWasQualificationAwarded();
    (int? startMonth, int? startYear) GetWhenWasQualificationStarted();
    bool WasStartedBeforeSeptember2014();
    bool WasStartedBetweenSeptember2014AndAugust2019();
    bool WasStartedOnOrAfterSeptember2019();
    int? GetLevelOfQualification();
    string? GetAwardingOrganisation();
    bool GetAwardingOrganisationIsNotOnList();
    string? GetSearchCriteria();
    Dictionary<string, string>? GetAdditionalQuestionsAnswers();
    bool UserHasAnsweredAdditionalQuestions();
    YesOrNo GetQualificationWasSelectedFromList();
}