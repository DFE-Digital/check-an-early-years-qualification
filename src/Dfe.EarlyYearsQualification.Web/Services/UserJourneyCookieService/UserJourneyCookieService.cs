using System.Globalization;
using System.Text.Json;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Services.Cookies;

namespace Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

public class UserJourneyCookieService(ICookieManager cookieManager, ILogger<UserJourneyCookieService> logger)
    : IUserJourneyCookieService
{
    private readonly CookieOptions _options = new()
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

    public void SetWhenWasQualificationAwarded(string date)
    {
        var model = GetUserJourneyModelFromCookie();

        model.WhenWasQualificationAwarded = date;

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

    /// <summary>
    ///     Replaces all the existing question answers in the user journey with <paramref name="additionalQuestionsAnswers" />
    /// </summary>
    /// <param name="additionalQuestionsAnswers"></param>
    public void SetAdditionalQuestionsAnswers(Dictionary<string, string> additionalQuestionsAnswers)
    {
        var model = GetUserJourneyModelFromCookie();

        model.AdditionalQuestionsAnswers.Clear();

        foreach (var answer in additionalQuestionsAnswers)
        {
            model.AdditionalQuestionsAnswers[answer.Key] = answer.Value;
        }

        SetJourneyCookie(model);
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

    public (int? startMonth, int? startYear) GetWhenWasQualificationAwarded()
    {
        var cookie = GetUserJourneyModelFromCookie();

        int? startDateMonth = null;
        int? startDateYear = null;
        var qualificationAwardedDateSplit = cookie.WhenWasQualificationAwarded.Split('/');

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

    private void SetJourneyCookie(UserJourneyModel model)
    {
        var serializedCookie = JsonSerializer.Serialize(model);
        cookieManager.SetOutboundCookie(CookieKeyNames.UserJourneyKey, serializedCookie, _options);
    }
}