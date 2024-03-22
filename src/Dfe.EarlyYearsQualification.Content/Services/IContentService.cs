using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.Services;

public interface IContentService
{
    Task<StartPage?> GetStartPage();

    Task<List<NavigationLink>?> GetNavigationLinks();

    Task<Qualification?> GetQualificationById(string qualificationId);

    Task<SearchResultPage> GetSearchResultPage();

    Task<List<Qualification>> SearchQualifications(string qualificationName);
}
