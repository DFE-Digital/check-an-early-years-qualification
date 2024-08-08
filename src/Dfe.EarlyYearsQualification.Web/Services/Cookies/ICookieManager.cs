namespace Dfe.EarlyYearsQualification.Web.Services.Cookies;

public interface ICookieManager
{
    IDictionary<string, string>? ReadInboundCookies();

    void SetOutboundCookie(string key, string value, CookieOptions options);
}