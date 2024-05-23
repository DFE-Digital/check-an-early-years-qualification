using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.ViewComponents;

public class FooterLinksViewComponent(IContentService contentService, ILogger<FooterLinksViewComponent> logger) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var navigationLinks = await GetNavigationLinksAsync();

        return View(navigationLinks);
    }

    private async Task<IEnumerable<NavigationLink>> GetNavigationLinksAsync()
    {
        try
        {
            var navigationLinks = await contentService.GetNavigationLinks();
            if (navigationLinks is null || navigationLinks.Count == 0)
            {
                return await Task.FromResult(Array.Empty<NavigationLink>().AsEnumerable());
            }

            return navigationLinks;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving navigation links for footer");

            return await Task.FromResult(Array.Empty<NavigationLink>().AsEnumerable());
        }
    }
}