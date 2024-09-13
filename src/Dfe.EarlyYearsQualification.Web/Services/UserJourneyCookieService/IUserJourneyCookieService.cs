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
    public int? GetLevelOfQualification();
    public string? GetAwardingOrganisation();
    public bool GetAwardingOrganisationIsNotOnList();
    public string? GetSearchCriteria();
    public Dictionary<string, string>? GetAdditionalQuestionsAnswers();
    public bool UserHasAnsweredAdditionalQuestions();
    public YesOrNo GetQualificationWasSelectedFromList();
}