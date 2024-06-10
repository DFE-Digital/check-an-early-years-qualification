using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.Services;

public interface IContentService
{
    Task<StartPage?> GetStartPage();

    Task<List<NavigationLink>?> GetNavigationLinks();

    Task<Qualification?> GetQualificationById(string qualificationId);

    Task<DetailsPage?> GetDetailsPage();

    Task<AdvicePage?> GetAdvicePage(string entryId);

    Task<QuestionPage?> GetQuestionPage(string entryId);

    Task<AccessibilityStatementPage?> GetAccessibilityStatementPage();

    Task<CookiesPage?> GetCookiesPage();

    Task<PhaseBanner?> GetPhaseBannerContent();

    Task<List<Qualification>> GetQualifications(string? level);

    Task<CookiesBanner?> GetCookiesBannerContent();
}