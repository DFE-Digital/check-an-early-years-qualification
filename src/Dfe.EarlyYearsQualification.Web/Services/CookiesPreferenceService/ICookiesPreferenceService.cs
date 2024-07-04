namespace Dfe.EarlyYearsQualification.Web.Services.CookiesPreferenceService;

public interface ICookiesPreferenceService
{
    public DfeCookie GetCookie();

    public void SetPreference(bool userPreference);

    public void SetVisibility(bool visibility);

    public void RejectCookies();
}