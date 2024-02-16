using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.Services;

public interface IContentService
{
    Task<LandingPage> GetLandingPage();

    Task<List<NavigationLink>> GetNavigationLinks();
}
