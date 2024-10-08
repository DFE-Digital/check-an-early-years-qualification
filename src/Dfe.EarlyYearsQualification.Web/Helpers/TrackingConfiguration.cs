using Dfe.EarlyYearsQualification.Web.Services.CookiesPreferenceService;

namespace Dfe.EarlyYearsQualification.Web.Helpers;

public class TrackingConfiguration(ICookiesPreferenceService cookiesPreferenceService, IConfiguration configuration)
{
    private readonly DfeCookie _cookie = cookiesPreferenceService.GetCookie();

    public bool UseCookies
    {
        get { return _cookie.HasApproved; }
    }

    public string GtmTag
    {
        get { return configuration.GetValue<string>("GTM:Tag") ?? ""; }
    }

    public string ClarityTag
    {
        get { return configuration.GetValue<string>("Clarity:Tag") ?? ""; }
    }
}