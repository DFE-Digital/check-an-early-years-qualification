using System.Text.Json;
using Dfe.EarlyYearsQualification.Web.Constants;

namespace Dfe.EarlyYearsQualification.Web.Services.CookiesPreferenceService;

public class CookiesPreferenceService(IHttpContextAccessor context) : ICookiesPreferenceService
{
    public void SetVisibility(bool visibility)
    {
        var currentCookie = GetCookie();
        DeleteCookie();
        CreateCookie(CookieKeyNames.CookiesPreferenceKey, currentCookie.HasApproved, visibility,
                     currentCookie.IsRejected);
    }

    public void RejectCookies()
    {
        DeleteCookie();
        CreateCookie(CookieKeyNames.CookiesPreferenceKey, false, true, true);
    }

    public void SetPreference(bool userPreference)
    {
        CreateCookie(CookieKeyNames.CookiesPreferenceKey, userPreference);
    }

    public DfeCookie GetCookie()
    {
        var cookie = context.HttpContext?.Request.Cookies[CookieKeyNames.CookiesPreferenceKey];
        if (cookie is null)
        {
            return new DfeCookie();
        }

        try
        {
            var dfeCookie = JsonSerializer.Deserialize<DfeCookie>(cookie);
            return dfeCookie ?? new DfeCookie();
        }
        catch
        {
            return new DfeCookie();
        }
    }

    private void DeleteCookie()
    {
        context.HttpContext?.Response.Cookies.Delete(CookieKeyNames.CookiesPreferenceKey);
    }

    private void CreateCookie(string key, bool value, bool visibility = true, bool rejected = false)
    {
        var cookieOptions = new CookieOptions
                            {
                                Secure = true,
                                HttpOnly = true,
                                Expires = new DateTimeOffset(DateTime.Now.AddYears(1))
                            };

        var cookie = new DfeCookie { IsVisible = visibility, HasApproved = value, IsRejected = rejected };
        var serializedCookie = JsonSerializer.Serialize(cookie);
        context.HttpContext?.Response.Cookies.Append(key, serializedCookie, cookieOptions);
    }
}