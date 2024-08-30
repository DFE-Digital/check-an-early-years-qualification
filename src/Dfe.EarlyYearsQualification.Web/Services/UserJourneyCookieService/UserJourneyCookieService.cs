using System.Globalization;
using System.Text.Json;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Models;
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

    public void SetWhereWasQualificationAwarded(string location)
    {
        var model = GetUserJourneyModelFromCookie();

        model.WhereWasQualificationAwarded = location;

        SetJourneyCookie(model);
    }

    public void SetWhenWasQualificationStarted(string date)
    {
        var model = GetUserJourneyModelFromCookie();

        model.WhenWasQualificationStarted = date;

        SetJourneyCookie(model);
    }

    public void SetLevelOfQualification(string level)
    {
        var model = GetUserJourneyModelFromCookie();

        model.LevelOfQualification = level;

        SetJourneyCookie(model);
    }

    public void SetAwardingOrganisation(string awardingOrganisation)
    {
        var model = GetUserJourneyModelFromCookie();

        model.WhatIsTheAwardingOrganisation = awardingOrganisation;

        SetJourneyCookie(model);
    }

    public void SetUserSelectedQualificationFromList(YesOrNo yesOrNo)
    {
        var model = GetUserJourneyModelFromCookie();

        model.QualificationWasSelectedFromList = yesOrNo;

        SetJourneyCookie(model);
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
        var model = GetUserJourneyModelFromCookie();

        model.SearchCriteria = searchCriteria;

        SetJourneyCookie(model);
    }

    public UserJourneyModel GetUserJourneyModelFromCookie()
    {
        var cookies = cookieManager.ReadInboundCookies();

        if (cookies?.TryGetValue(CookieKeyNames.UserJourneyKey, out var cookie) != true)
        {
            ResetUserJourneyCookie();
            return new UserJourneyModel();
        }

        UserJourneyModel? userJourneyModel;
        try
        {
            userJourneyModel = JsonSerializer.Deserialize<UserJourneyModel>(cookie!);
        }
        catch
        {
            logger.LogWarning("Failed to deserialize cookie");
            ResetUserJourneyCookie();
            return new UserJourneyModel();
        }

        return userJourneyModel ?? new UserJourneyModel();
    }

    public void SetUserJourneyModelCookie(UserJourneyModel model)
    {
        SetJourneyCookie(model);
    }

    public void ResetUserJourneyCookie()
    {
        SetJourneyCookie(new UserJourneyModel());
    }

    public string? GetWhereWasQualificationAwarded()
    {
        var cookie = GetUserJourneyModelFromCookie();
        string? awardingCountry = null;
        if (!string.IsNullOrEmpty(cookie.WhereWasQualificationAwarded))
        {
            awardingCountry = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cookie.WhereWasQualificationAwarded);
        }

        return awardingCountry;
    }

    public (int? startMonth, int? startYear) GetWhenWasQualificationStarted()
    {
        var cookie = GetUserJourneyModelFromCookie();

        int? startDateMonth = null;
        int? startDateYear = null;
        var qualificationAwardedDateSplit = cookie.WhenWasQualificationStarted.Split('/');

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
        var cookie = GetUserJourneyModelFromCookie();
        int? level = null;
        if (int.TryParse(cookie.LevelOfQualification, out var parsedLevel))
        {
            level = parsedLevel;
        }

        return level;
    }

    public string? GetAwardingOrganisation()
    {
        var cookie = GetUserJourneyModelFromCookie();
        string? awardingOrganisation = null;
        if (!string.IsNullOrEmpty(cookie.WhatIsTheAwardingOrganisation))
        {
            awardingOrganisation = cookie.WhatIsTheAwardingOrganisation;
        }

        return awardingOrganisation;
    }

    public string? GetSearchCriteria()
    {
        var cookie = GetUserJourneyModelFromCookie();
        string? searchCriteria = null;
        if (!string.IsNullOrEmpty(cookie.SearchCriteria))
        {
            searchCriteria = cookie.SearchCriteria;
        }

        return searchCriteria;
    }

    public Dictionary<string, string> GetAdditionalQuestionsAnswers()
    {
        var cookie = GetUserJourneyModelFromCookie();
        return cookie.AdditionalQuestionsAnswers;
    }

    public bool UserHasAnsweredAdditionalQuestions()
    {
        var cookie = GetUserJourneyModelFromCookie();
        return cookie.AdditionalQuestionsAnswers.Count > 0;
    }

    public YesOrNo GetQualificationWasSelectedFromList()
    {
        var model = GetUserJourneyModelFromCookie();

        return model.QualificationWasSelectedFromList;
    }

    private void SetAdditionalQuestionsAnswersInternal(
        IEnumerable<KeyValuePair<string, string>> additionalQuestionsAnswers)
    {
        var model = GetUserJourneyModelFromCookie();

        model.AdditionalQuestionsAnswers.Clear();

        foreach (var answer in additionalQuestionsAnswers)
        {
            model.AdditionalQuestionsAnswers[answer.Key] = answer.Value;
        }

        SetJourneyCookie(model);
    }

    private void SetJourneyCookie(UserJourneyModel model)
    {
        var serializedCookie = JsonSerializer.Serialize(model);
        cookieManager.SetOutboundCookie(CookieKeyNames.UserJourneyKey, serializedCookie, _cookieOptions);
    }
}