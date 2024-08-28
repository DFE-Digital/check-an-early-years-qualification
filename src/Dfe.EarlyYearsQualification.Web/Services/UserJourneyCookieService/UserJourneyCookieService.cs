using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Services.Cookies;

namespace Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

public class UserJourneyCookieService(ICookieManager cookieManager, ILogger<UserJourneyCookieService> logger)
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
        EnsureModelLoaded();

        Debug.Assert(_model != null);

        _model.WhereWasQualificationAwarded = location;

        SetJourneyCookie();
    }

    public void SetWhenWasQualificationStarted(string date)
    {
        EnsureModelLoaded();

        Debug.Assert(_model != null);

        _model.WhenWasQualificationStarted = date;

        SetJourneyCookie();
    }

    public void SetLevelOfQualification(string level)
    {
        EnsureModelLoaded();

        Debug.Assert(_model != null);

        _model.LevelOfQualification = level;

        SetJourneyCookie();
    }

    public void SetAwardingOrganisation(string awardingOrganisation)
    {
        EnsureModelLoaded();

        Debug.Assert(_model != null);

        _model.WhatIsTheAwardingOrganisation = awardingOrganisation;

        SetJourneyCookie();
    }

    public void SetUserSelectedQualificationFromList(YesOrNo yesOrNo)
    {
        EnsureModelLoaded();

        Debug.Assert(_model != null);

        _model.QualificationWasSelectedFromList = yesOrNo;

        SetJourneyCookie();
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
        EnsureModelLoaded();

        Debug.Assert(_model != null);

        _model.SearchCriteria = searchCriteria;

        SetJourneyCookie();
    }

    public void ResetUserJourneyCookie()
    {
        lock (_lockObject)
        {
            _model = new UserJourneyModel();
        }

        SetJourneyCookie();
    }

    public string? GetWhereWasQualificationAwarded()
    {
        EnsureModelLoaded();

        Debug.Assert(_model != null);

        string? awardingCountry = null;
        if (!string.IsNullOrEmpty(_model.WhereWasQualificationAwarded))
        {
            awardingCountry = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_model.WhereWasQualificationAwarded);
        }

        return awardingCountry;
    }

    public (int? startMonth, int? startYear) GetWhenWasQualificationStarted()
    {
        EnsureModelLoaded();

        Debug.Assert(_model != null);

        int? startDateMonth = null;
        int? startDateYear = null;
        var qualificationAwardedDateSplit = _model.WhenWasQualificationStarted.Split('/');

        // ReSharper disable once InvertIf
        if (qualificationAwardedDateSplit.Length == 2
            && int.TryParse(qualificationAwardedDateSplit[0], out var parsedStartMonth)
            && int.TryParse(qualificationAwardedDateSplit[1], out var parsedStartYear))
        {
            startDateMonth = parsedStartMonth;
            startDateYear = parsedStartYear;
        }

        return (startDateMonth, startDateYear);
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

    public int? GetLevelOfQualification()
    {
        EnsureModelLoaded();

        Debug.Assert(_model != null);

        int? level = null;
        if (int.TryParse(_model.LevelOfQualification, out var parsedLevel))
        {
            level = parsedLevel;
        }

        return level;
    }

    public string? GetAwardingOrganisation()
    {
        EnsureModelLoaded();

        Debug.Assert(_model != null);

        string? awardingOrganisation = null;
        if (!string.IsNullOrEmpty(_model.WhatIsTheAwardingOrganisation))
        {
            awardingOrganisation = _model.WhatIsTheAwardingOrganisation;
        }

        return awardingOrganisation;
    }

    public string? GetSearchCriteria()
    {
        EnsureModelLoaded();

        Debug.Assert(_model != null);

        string? searchCriteria = null;
        if (!string.IsNullOrEmpty(_model.SearchCriteria))
        {
            searchCriteria = _model.SearchCriteria;
        }

        return searchCriteria;
    }

    public Dictionary<string, string> GetAdditionalQuestionsAnswers()
    {
        EnsureModelLoaded();

        Debug.Assert(_model != null);

        return _model.AdditionalQuestionsAnswers;
    }

    public bool UserHasAnsweredAdditionalQuestions()
    {
        EnsureModelLoaded();

        Debug.Assert(_model != null);

        return _model.AdditionalQuestionsAnswers.Count > 0;
    }

    public YesOrNo GetQualificationWasSelectedFromList()
    {
        EnsureModelLoaded();

        Debug.Assert(_model != null);

        return _model.QualificationWasSelectedFromList;
    }

    private void EnsureModelLoaded()
    {
        if (_model == null)
        {
            lock (_lockObject)
            {
                if (_model == null)
                {
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
                            userJourneyModel = new UserJourneyModel();
                        }
                    }

                    _model = userJourneyModel ?? new UserJourneyModel();
                }

                SetJourneyCookie();
            }
        }
    }

    private void SetAdditionalQuestionsAnswersInternal(
        IEnumerable<KeyValuePair<string, string>> additionalQuestionsAnswers)
    {
        EnsureModelLoaded();

        Debug.Assert(_model != null);

        _model.AdditionalQuestionsAnswers.Clear();

        foreach (var answer in additionalQuestionsAnswers)
        {
            _model.AdditionalQuestionsAnswers[answer.Key] = answer.Value;
        }

        SetJourneyCookie();
    }

    private void SetJourneyCookie()
    {
        EnsureModelLoaded();

        Debug.Assert(_model != null);

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
        public string WhereWasQualificationAwarded { get; set; } = string.Empty;
        public string WhenWasQualificationStarted { get; set; } = string.Empty;
        public string LevelOfQualification { get; set; } = string.Empty;
        public string WhatIsTheAwardingOrganisation { get; set; } = string.Empty;

        public string SearchCriteria { get; set; } = string.Empty;

        public Dictionary<string, string> AdditionalQuestionsAnswers { get; init; } = new();
        public YesOrNo QualificationWasSelectedFromList { get; set; }
    }
}