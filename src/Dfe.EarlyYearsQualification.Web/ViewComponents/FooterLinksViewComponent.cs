using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.Environments;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.ViewComponents;

public class FooterLinksViewComponent(
    ILogger<FooterLinksViewComponent> logger,
    IContentService contentService,
    IEnvironmentService environmentService)
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
            var links = await contentService.GetNavigationLinks();

            return links.Concat(OptionsLinks());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving navigation links for footer");

            return await Task.FromResult(Array.Empty<NavigationLink>().AsEnumerable());
        }
    }

    private IEnumerable<NavigationLink> OptionsLinks()
    {
        if (!environmentService.IsProduction())
        {
            yield return new NavigationLink
                         {
                             DisplayText = "Options",
                             Href = "/options",
                             OpenInNewTab = false
                         };
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