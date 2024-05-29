using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class CookiesBanner
{
    public string CookiesBannerTitle { get; init; } = string.Empty;
    public Document? CookiesBannerContent { get; init; }
    public string AcceptButtonText { get; init; } = string.Empty;
    public string RejectButtonText { get; init; } = string.Empty;
    public string CookiesBannerLinkText { get; init; } = string.Empty;
    public Document? AcceptedCookiesContent { get; init; }
    public Document? RejectedCookiesContent { get; init; }
    public string HideCookieBannerButtonText { get; init; } = string.Empty;
}