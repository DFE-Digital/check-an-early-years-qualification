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

    private void SetJourneyCookie(UserJourneyModel model)
    {
        var serializedCookie = JsonSerializer.Serialize(model);
        context.HttpContext?.Response.Cookies.Append(CookieKeyNames.UserJourneyKey, serializedCookie, _options);
    }
}