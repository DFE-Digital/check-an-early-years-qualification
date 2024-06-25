using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.ViewComponents;

public class FooterLinksViewComponent(IContentService contentService, ILogger<FooterLinksViewComponent> logger)
    : ViewComponent
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
            return await contentService.GetNavigationLinks();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving navigation links for footer");

            return await Task.FromResult(Array.Empty<NavigationLink>().AsEnumerable());
        }
    }
}