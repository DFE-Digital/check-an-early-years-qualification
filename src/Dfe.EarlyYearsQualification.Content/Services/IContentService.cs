using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.Services;

public interface IContentService
{
    Task<StartPage?> GetStartPage();

    Task<List<NavigationLink>> GetNavigationLinks();

    Task<Qualification?> GetQualificationById(string qualificationId);

    Task<DetailsPage?> GetDetailsPage();

    Task<AdvicePage?> GetAdvicePage(string entryId);

    Task<RadioQuestionPage?> GetRadioQuestionPage(string entryId);

    Task<AccessibilityStatementPage?> GetAccessibilityStatementPage();

    Task<CookiesPage?> GetCookiesPage();

    Task<PhaseBanner?> GetPhaseBannerContent();

    Task<CookiesBanner?> GetCookiesBannerContent();

    Task<DateQuestionPage?> GetDateQuestionPage(string entryId);

    Task<DropdownQuestionPage?> GetDropdownQuestionPage(string entryId);

    Task<List<Qualification>> GetQualifications();

    Task<ConfirmQualificationPage?> GetConfirmQualificationPage();
}