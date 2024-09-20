using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.ViewComponents;

public class FooterLinksViewComponent(IContentService contentService, ILogger<FooterLinksViewComponent> logger)
    : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var navigationLinks = await GetNavigationLinksAsync();
        var navigationLinkModels = MapToNavigationLinkModels(navigationLinks);

        return View(navigationLinkModels);
    }

    private async Task<IEnumerable<NavigationLink>> GetNavigationLinksAsync()
    {
        try
        {
            return await contentService.GetNavigationLinks();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving navigation links for footer");

            return await Task.FromResult(Array.Empty<NavigationLink>().AsEnumerable());
        }
    }

    private static IEnumerable<NavigationLinkModel> MapToNavigationLinkModels(
        IEnumerable<NavigationLink> navigationLinks)
    {
        return
            navigationLinks.Select(navigationLink => new NavigationLinkModel
                                                     {
                                                         DisplayText = navigationLink.DisplayText,
                                                         OpenInNewTab = navigationLink.OpenInNewTab,
                                                         Href = navigationLink.Href
                                                     });
    }
}