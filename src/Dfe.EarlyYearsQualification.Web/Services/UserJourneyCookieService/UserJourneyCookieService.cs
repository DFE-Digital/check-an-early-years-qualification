using System.Globalization;
using System.Text.Json;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Models;

namespace Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

public class UserJourneyCookieService(IHttpContextAccessor context, ILogger<UserJourneyCookieService> logger)
    : IUserJourneyCookieService
{
    private readonly CookieOptions _options = new()
                                              {
                                                  Secure = true,
                                                  HttpOnly = true,
                                                  Expires = new DateTimeOffset(DateTime.Now.AddYears(1))
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

    public void SetQualificationNameSearchCriteria(string searchCriteria)
    {
        var model = GetUserJourneyModelFromCookie();

        model.SearchCriteria = searchCriteria;

        SetJourneyCookie(model);
    }

    public UserJourneyModel GetUserJourneyModelFromCookie()
    {
        var cookie = context.HttpContext?.Request.Cookies[CookieKeyNames.UserJourneyKey];
        if (cookie is null)
        {
            ResetUserJourneyCookie();
            return new UserJourneyModel();
        }

        try
        {
            var toReturn = JsonSerializer.Deserialize<UserJourneyModel>(cookie);
            return toReturn!;
        }
        catch
        {
            logger.LogWarning("Failed to deserialize cookie");
            ResetUserJourneyCookie();
            return new UserJourneyModel();
        }
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

    private void SetJourneyCookie(UserJourneyModel model)
    {
        var serializedCookie = JsonSerializer.Serialize(model);
        context.HttpContext?.Response.Cookies.Append(CookieKeyNames.UserJourneyKey, serializedCookie, _options);
    }
}