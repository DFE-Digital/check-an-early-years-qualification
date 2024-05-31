namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class CookiesBannerModel
{
    public string CookiesBannerTitle { get; init; } = string.Empty;
    public string CookiesBannerContent { get; init; } = string.Empty;
    public string AcceptButtonText { get; init; } = string.Empty;
    public string RejectButtonText { get; init; } = string.Empty;
    public string CookiesBannerLinkText { get; init; } = string.Empty;
    public string AcceptedCookiesContent { get; init; } = string.Empty;
    public string RejectedCookiesContent { get; init; } = string.Empty;
    public string HideCookieBannerButtonText { get; init; } = string.Empty;
    public bool Show { get; init; }
}