using System.Globalization;
using System.Text.Json;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Services.Cookies;

namespace Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

public class UserJourneyCookieService(ILogger<UserJourneyCookieService> logger, ICookieManager cookieManager)
    : IUserJourneyCookieService
{
    private readonly CookieOptions _cookieOptions = new()
                                                    {
                                                        Secure = true,
                                                        HttpOnly = true,
                                                        Expires = new DateTimeOffset(DateTime.Now.AddMinutes(30))
                                                    };

    private readonly object _lockObject = new();
    private volatile UserJourneyModel? _model;

    public void SetWhereWasQualificationAwarded(string location)
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            _model!.WhereWasQualificationAwarded = location;

            SetJourneyCookie();
        }
    }

    public void SetWhenWasQualificationStarted(string date)
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            _model!.WhenWasQualificationStarted = date;

            SetJourneyCookie();
        }
    }

    public void SetWhenWasQualificationAwarded(string date)
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            _model!.WhenWasQualificationAwarded = date;

            SetJourneyCookie();
        }
    }

    public void SetLevelOfQualification(string level)
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            _model!.LevelOfQualification = level;

            SetJourneyCookie();
        }
    }


    public void SetAwardingOrganisation(string awardingOrganisation)
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            _model!.WhatIsTheAwardingOrganisation = awardingOrganisation;

            SetJourneyCookie();
        }
    }

    public void SetAwardingOrganisationNotOnList(bool isOnList)
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            _model!.SelectedAwardingOrganisationNotOnTheList = isOnList;

            SetJourneyCookie();
        }
    }

    public void SetUserSelectedQualificationFromList(YesOrNo yesOrNo)
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            _model!.QualificationWasSelectedFromList = yesOrNo;

            SetJourneyCookie();
        }
    }

    /// <summary>
    ///     Replaces all the existing question answers in the user journey with <paramref name="additionalQuestionsAnswers" />
    /// </summary>
    /// <param name="additionalQuestionsAnswers"></param>
    public void SetAdditionalQuestionsAnswers(IDictionary<string, string> additionalQuestionsAnswers)
    {
        SetAdditionalQuestionsAnswersInternal(additionalQuestionsAnswers);
    }

    /// <summary>
    ///     Removes existing question answers in the user journey
    /// </summary>
    public void ClearAdditionalQuestionsAnswers()
    {
        SetAdditionalQuestionsAnswersInternal([]);
    }

    public void SetQualificationNameSearchCriteria(string searchCriteria)
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            _model!.SearchCriteria = searchCriteria;

            SetJourneyCookie();
        }
    }

    public void ResetUserJourneyCookie()
    {
        lock (_lockObject)
        {
            _model = new UserJourneyModel();

            SetJourneyCookie();
        }
    }

    public string? GetWhereWasQualificationAwarded()
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            string? awardingCountry = null;
            if (!string.IsNullOrEmpty(_model!.WhereWasQualificationAwarded))
            {
                awardingCountry = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_model.WhereWasQualificationAwarded);
            }

            return awardingCountry;
        }
    }

    public (int? startMonth, int? startYear) GetWhenWasQualificationStarted()
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            return StringDateHelper.SplitDate(_model!.WhenWasQualificationStarted);
        }
    }

    public (int? startMonth, int? startYear) GetWhenWasQualificationAwarded()
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            return StringDateHelper.SplitDate(_model!.WhenWasQualificationAwarded);
        }
    }

    public bool WasStartedBetweenSeptember2014AndAugust2019()
    {
        var (startDateMonth, startDateYear) = GetWhenWasQualificationStarted();

        if (startDateMonth is null || startDateYear is null)
        {
            throw new
                InvalidOperationException("Unable to determine whether qualification was started between 09-2014 and 08-2019");
        }

        var date = new DateOnly(startDateYear.Value, startDateMonth.Value, 1);
        return date >= new DateOnly(2014, 09, 01) && date <= new DateOnly(2019, 08, 31);
    }

    public bool WasStartedBeforeSeptember2014()
    {
        var (startDateMonth, startDateYear) = GetWhenWasQualificationStarted();

        if (startDateMonth is null || startDateYear is null)
        {
            throw new
                InvalidOperationException("Unable to determine whether qualification was started before 09-2014");
        }

        var date = new DateOnly(startDateYear.Value, startDateMonth.Value, 1);
        return date < new DateOnly(2014, 9, 1);
    }

    public bool WasStartedOnOrAfterSeptember2019()
    {
        var (startDateMonth, startDateYear) = GetWhenWasQualificationStarted();

        if (startDateMonth is null || startDateYear is null)
        {
            throw new
                InvalidOperationException("Unable to determine whether qualification was started on or after 09-2019");
        }

        var date = new DateOnly(startDateYear.Value, startDateMonth.Value, 1);
        return date >= new DateOnly(2019, 9, 1);
    }

    public int? GetLevelOfQualification()
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            int? level = null;
            if (int.TryParse(_model!.LevelOfQualification, out var parsedLevel))
            {
                level = parsedLevel;
            }

            return level;
        }
    }

    public string? GetAwardingOrganisation()
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            string? awardingOrganisation = null;
            if (!string.IsNullOrEmpty(_model!.WhatIsTheAwardingOrganisation))
            {
                awardingOrganisation = _model.WhatIsTheAwardingOrganisation;
            }

            return awardingOrganisation;
        }
    }

    public bool GetAwardingOrganisationIsNotOnList()
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            return _model!.SelectedAwardingOrganisationNotOnTheList;
        }
    }

    public string? GetSearchCriteria()
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            string? searchCriteria = null;
            if (!string.IsNullOrEmpty(_model!.SearchCriteria))
            {
                searchCriteria = _model.SearchCriteria;
            }

            return searchCriteria;
        }
    }

    public Dictionary<string, string> GetAdditionalQuestionsAnswers()
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            return _model!.AdditionalQuestionsAnswers;
        }
    }

    public bool UserHasAnsweredAdditionalQuestions()
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            return _model!.AdditionalQuestionsAnswers.Count > 0;
        }
    }

    public YesOrNo GetQualificationWasSelectedFromList()
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            return _model!.QualificationWasSelectedFromList;
        }
    }

    public bool WasAwardedBeforeJune2016()
    {
        var (awardedDateMonth, awardedDateYear) = GetWhenWasQualificationAwarded();

        if (awardedDateMonth is null || awardedDateYear is null)
        {
            throw new
                InvalidOperationException("Unable to determine whether qualification was awarded before 06-2016");
        }

        var date = new DateOnly(awardedDateYear.Value, awardedDateMonth.Value, 1);
        return date < new DateOnly(2016, 6, 1);
    }

    public bool WasAwardedInJune2016()
    {
        var (awardedDateMonth, awardedDateYear) = GetWhenWasQualificationAwarded();

        if (awardedDateMonth is null || awardedDateYear is null)
        {
            throw new
                InvalidOperationException("Unable to determine whether qualification was awarded in 06-2016");
        }

        return awardedDateMonth == 6 && awardedDateYear == 2016;
    }

    public bool WasAwardedAfterJune2016()
    {
        var (awardedDateMonth, awardedDateYear) = GetWhenWasQualificationAwarded();

        if (awardedDateMonth is null || awardedDateYear is null)
        {
            throw new
                InvalidOperationException("Unable to determine whether qualification was awarded after 06-2016");
        }

        var date = new DateOnly(awardedDateYear.Value, awardedDateMonth.Value, 1);
        return date > new DateOnly(2016, 6, 1);
    }

    public bool WasAwardedBeforeSeptember2014()
    {
        var (awardedDateMonth, awardedDateYear) = GetWhenWasQualificationAwarded();

        if (awardedDateMonth is null || awardedDateYear is null)
        {
            throw new
                InvalidOperationException("Unable to determine whether qualification was awarded before 09-2014");
        }

        var date = new DateOnly(awardedDateYear.Value, awardedDateMonth.Value, 1);
        return date < new DateOnly(2014, 9, 1);
    }

    public bool WasAwardedOnOrAfterSeptember2014()
    {
        var (awardedDateMonth, awardedDateYear) = GetWhenWasQualificationAwarded();

        if (awardedDateMonth is null || awardedDateYear is null)
        {
            throw new
                InvalidOperationException("Unable to determine whether qualification was awarded on or after 09-2014");
        }

        var date = new DateOnly(awardedDateYear.Value, awardedDateMonth.Value, 1);
        return date >= new DateOnly(2014, 09, 01);
    }

    public bool WasAwardedBetweenSeptember2014AndMay2016()
    {
        var (awardDateMonth, awardDateYear) = GetWhenWasQualificationAwarded();

        if (awardDateMonth is null || awardDateYear is null)
        {
            throw new
                InvalidOperationException("Unable to determine whether qualification was awarded between 09-2014 and 05-2016");
        }

        var date = new DateOnly(awardDateYear.Value, awardDateMonth.Value, 1);
        return date >= new DateOnly(2014, 09, 01) && date <= new DateOnly(2016, 05, 31);
    }

    public void SetHasSubmittedEmailAddressInFeedbackFormQuestion(bool hasSubmittedEmailAddress)
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            _model!.HasSubmittedEmailAddressInFeedbackFormQuestion = hasSubmittedEmailAddress;

            SetJourneyCookie();
        }
    }

    public bool GetHasSubmittedEmailAddressInFeedbackFormQuestion()
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            return _model!.HasSubmittedEmailAddressInFeedbackFormQuestion;
        }
    }

    public void SetHasUserGotEverythingTheyNeededToday(string hasGotEverythingTheyNeededToday)
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            _model!.HasUserGotEverythingTheyNeededToday = hasGotEverythingTheyNeededToday;

            SetJourneyCookie();
        }
    }

    public string GetHasUserGotEverythingTheyNeededToday()
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            return _model!.HasUserGotEverythingTheyNeededToday;
        }
    }

    public HelpFormEnquiry GetHelpFormEnquiry()
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            return _model!.HelpFormEnquiry;
        }
    }

    public void SetHelpFormEnquiry(HelpFormEnquiry formEnquiry)
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            _model!.HelpFormEnquiry = formEnquiry;

            SetJourneyCookie();
        }
    }

    public void SetIsUserCheckingTheirOwnQualification(string value)
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            _model!.IsUserCheckingTheirOwnQualification = value;

            SetJourneyCookie();
        }
    }

    public string GetIsUserCheckingTheirOwnQualification()
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            return _model!.IsUserCheckingTheirOwnQualification;
        }
    }

    private void EnsureModelLoaded()
    {
        if (_model != null)
        {
            return;
        }

        lock (_lockObject)
        {
            if (_model != null)
            {
                return;
            }

            var cookies = cookieManager.ReadInboundCookies();

            UserJourneyModel? userJourneyModel = null;

            if (cookies?.TryGetValue(CookieKeyNames.UserJourneyKey, out var cookie) == true)
            {
                try
                {
                    userJourneyModel = JsonSerializer.Deserialize<UserJourneyModel>(cookie);
                }
                catch
                {
                    logger.LogWarning("Failed to deserialize cookie");
                }
            }

            _model = userJourneyModel ?? new UserJourneyModel();

            SetJourneyCookie();
        }
    }

    private void SetAdditionalQuestionsAnswersInternal(
        IEnumerable<KeyValuePair<string, string>> additionalQuestionsAnswers)
    {
        lock (_lockObject)
        {
            EnsureModelLoaded();

            _model!.AdditionalQuestionsAnswers.Clear();

            foreach (var answer in additionalQuestionsAnswers)
            {
                _model.AdditionalQuestionsAnswers[answer.Key] = answer.Value;
            }

            SetJourneyCookie();
        }
    }

    private void SetJourneyCookie()
    {
        EnsureModelLoaded();

        var serializedCookie = JsonSerializer.Serialize(_model);
        cookieManager.SetOutboundCookie(CookieKeyNames.UserJourneyKey, serializedCookie, _cookieOptions);
    }

    /// <summary>
    ///     Model used to serialise and deserialise the cookie.
    /// </summary>
    /// <remarks>
    ///     Do not expose an instance of this model in the service's interface. It is made
    ///     a public type in order to simplify testing that the cookie's value is
    ///     set correctly by the service's methods.
    /// </remarks>
    public class UserJourneyModel
    {
        public string SelectedQualificationName { get; set; } = string.Empty;

        public string WhereWasQualificationAwarded { get; set; } = string.Empty;

        public string WhenWasQualificationStarted { get; set; } = string.Empty;

        public string WhenWasQualificationAwarded { get; set; } = string.Empty;

        public string LevelOfQualification { get; set; } = string.Empty;

        public string WhatIsTheAwardingOrganisation { get; set; } = string.Empty;

        public bool SelectedAwardingOrganisationNotOnTheList { get; set; }

        public string SearchCriteria { get; set; } = string.Empty;

        public Dictionary<string, string> AdditionalQuestionsAnswers { get; init; } = new();

        public YesOrNo QualificationWasSelectedFromList { get; set; }

        public bool HasSubmittedEmailAddressInFeedbackFormQuestion { get; set; }

        public string HasUserGotEverythingTheyNeededToday { get; set; } = string.Empty;

        public HelpFormEnquiry HelpFormEnquiry { get; set; } = new();
        
        public string IsUserCheckingTheirOwnQualification { get; set; } = string.Empty;
    }
}