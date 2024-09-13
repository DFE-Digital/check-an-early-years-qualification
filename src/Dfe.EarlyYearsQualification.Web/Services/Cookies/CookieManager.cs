namespace Dfe.EarlyYearsQualification.Web.Services.Cookies;

public class CookieManager(IHttpContextAccessor context) : ICookieManager
{
    public void SetOutboundCookie(string key, string value, CookieOptions options)
    {
        var httpContext = context.HttpContext;

        if (httpContext is null)
        {
            throw new NullReferenceException("HTTP context cannot be null");
        }

        httpContext.Response.Cookies.Delete(key, options);
        httpContext.Response.Cookies.Append(key, value, options);
    }

    public IDictionary<string, string>? ReadInboundCookies()
    {
        var cookies = context.HttpContext?.Request.Cookies;

        return cookies?.ToDictionary();
    }

    public void DeleteOutboundCookie(string key)
    {
        var httpContext = context.HttpContext;

        if (httpContext is null)
        {
            // no context, no outbound cookies!
            return;
        }

        httpContext.Response.Cookies.Delete(key);
    }
}