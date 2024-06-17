using System.Text.Json;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Models;

namespace Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

public class UserJourneyCookieService(IHttpContextAccessor context) : IUserJourneyCookieService
{
    private readonly CookieOptions _options = new CookieOptions
    {
        Secure = true,
        HttpOnly = true,
        Expires = new DateTimeOffset(DateTime.Now.AddYears(1))
    };

    public void SetWhereWasQualificationAwarded(string location)
    {
        var model = GetUserJourneyCookie();

        model.WhereWasQualAwarded = location;

        SetJourneyCookie(model);
    }

    public void SetWhenWasQualificationAwarded(DateTime date)
    {
        var model = GetUserJourneyCookie();

        model.WhenWasQualAwarded = date;

        SetJourneyCookie(model);
    }

    public void SetLevelOfQualification(int? level)
    {
        var model = GetUserJourneyCookie();

        model.LevelOfQual = level;

        SetJourneyCookie(model);
    }

    public UserJourneyModel GetUserJourneyCookie()
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