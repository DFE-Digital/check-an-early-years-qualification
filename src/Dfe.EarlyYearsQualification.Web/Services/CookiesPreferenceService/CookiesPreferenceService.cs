using System.Text.Json;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Services.Cookies;

namespace Dfe.EarlyYearsQualification.Web.Services.CookiesPreferenceService;

public class CookiesPreferenceService(ICookieManager cookieManager) : ICookiesPreferenceService
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
        var cookies = cookieManager.ReadInboundCookies();
        if (cookies is null)
        {
            return new DfeCookie();
        }

        var cookieFound = cookies.TryGetValue(CookieKeyNames.CookiesPreferenceKey, out var cookie);
        if (!cookieFound)
        {
            return new DfeCookie();
        }

        try
        {
            var dfeCookie = JsonSerializer.Deserialize<DfeCookie>(cookie!);
            return dfeCookie ?? new DfeCookie();
        }
        catch
        {
            return new DfeCookie();
        }
    }

    private void DeleteCookie()
    {
        cookieManager.DeleteOutboundCookie(CookieKeyNames.CookiesPreferenceKey);
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

        cookieManager.SetOutboundCookie(key, serializedCookie, cookieOptions);
    }
}