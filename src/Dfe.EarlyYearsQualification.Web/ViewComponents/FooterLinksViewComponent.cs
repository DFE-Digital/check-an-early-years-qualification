using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.ViewComponents;

public class FooterLinksViewComponent : ViewComponent
{
    private readonly IContentService _contentService;
    private readonly ILogger<FooterLinksViewComponent> _logger;

    public FooterLinksViewComponent(IContentService contentService, ILogger<FooterLinksViewComponent> logger)
    {
        _contentService = contentService;
        _logger = logger;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var navigationLinks = await GetNavigationLinksAsync();

        return View(navigationLinks);
    }

    private async Task<IEnumerable<NavigationLink>> GetNavigationLinksAsync()
    {
        try
        {
            var navigationLinks = await _contentService.GetNavigationLinks();

            return navigationLinks;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error retrieving navigation links for footer");

            return await Task.FromResult(Array.Empty<NavigationLink>().AsEnumerable());
        }
    }
}