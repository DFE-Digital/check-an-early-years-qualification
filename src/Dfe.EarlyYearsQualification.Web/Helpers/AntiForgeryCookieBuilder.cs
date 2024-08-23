using System.Diagnostics.CodeAnalysis;

namespace Dfe.EarlyYearsQualification.Web.Helpers;

public class AntiForgeryCookieBuilder : CookieBuilder
{
    public override CookieOptions Build(HttpContext context, DateTimeOffset expiresFrom)
    {
        var cookieOptions = base.Build(context, expiresFrom);
        cookieOptions.Secure = true;
        return cookieOptions;
    }
}