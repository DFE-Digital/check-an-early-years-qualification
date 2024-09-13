namespace Dfe.EarlyYearsQualification.Web.Services.CookiesPreferenceService;

public interface ICookiesPreferenceService
{
    DfeCookie GetCookie();

    void SetPreference(bool userPreference);

    void SetVisibility(bool visibility);

    void RejectCookies();
}