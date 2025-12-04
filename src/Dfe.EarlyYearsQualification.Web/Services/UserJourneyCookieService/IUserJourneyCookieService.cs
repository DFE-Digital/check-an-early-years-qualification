namespace Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

public interface IUserJourneyCookieService
{
    void SetWhereWasQualificationAwarded(string location);
    void SetWhenWasQualificationStarted(string date);
    void SetWhenWasQualificationAwarded(string date);
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
    (int? startMonth, int? startYear) GetWhenWasQualificationAwarded();
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
    bool WasAwardedBeforeJune2016();
    bool WasAwardedInJune2016();
    bool WasAwardedAfterJune2016();
    bool WasAwardedBeforeSeptember2014();
    bool WasAwardedOnOrAfterSeptember2014();
    bool WasAwardedBetweenSeptember2014AndMay2016();
    void SetHasSubmittedEmailAddressInFeedbackFormQuestion(bool hasSubmittedEmailAddress);
    bool GetHasSubmittedEmailAddressInFeedbackFormQuestion();
    void SetHasUserGotEverythingTheyNeededToday(string hasGotEverythingTheyNeededToday);
    string GetHasUserGotEverythingTheyNeededToday();
    HelpFormEnquiry GetHelpFormEnquiry();
    void SetHelpFormEnquiry(HelpFormEnquiry formEnquiry);
    void SetIsUserCheckingTheirOwnQualification(string value);
    string GetIsUserCheckingTheirOwnQualification();
    
    ListFilters GetListFilters();
    
    void SetListFilters(ListFilters listFilters);
}