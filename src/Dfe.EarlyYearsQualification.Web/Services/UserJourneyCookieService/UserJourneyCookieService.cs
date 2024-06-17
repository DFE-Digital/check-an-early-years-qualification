using System.Text.Json;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Models;

namespace Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

public class UserJourneyCookieService(IHttpContextAccessor context) : IUserJourneyCookieService
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

    public UserJourneyModel GetUserJourneyModelFromCookie()
    {
        var cookie = context.HttpContext?.Request.Cookies[CookieKeyNames.UserJourneyKey];
        if (cookie is null)
        {
            var model = new UserJourneyModel();
            SetJourneyCookie(model);
            return model;
        }

        try
        {
            var journeyCookie = JsonSerializer.Deserialize<UserJourneyModel>(cookie);
            return journeyCookie ?? new UserJourneyModel();
        }
        catch
        {
            return new UserJourneyModel();
        }
    }

    public void ResetUserJourneyCookie()
    {
        SetJourneyCookie(new UserJourneyModel());
    }

    private void SetJourneyCookie(UserJourneyModel model)
    {
        try
        {
            var serializedCookie = JsonSerializer.Serialize(model);
            context.HttpContext?.Response.Cookies.Append(CookieKeyNames.UserJourneyKey, serializedCookie, _options);
        }
        catch (Exception e)
        {
            // TODO: log when we fail to serialise the UserJourneyModel?
        }
    }
}