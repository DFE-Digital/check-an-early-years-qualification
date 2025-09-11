using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Services.Environments;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.ViewComponents;

public class FooterViewComponent(
    ILogger<FooterViewComponent> logger,
    IContentService contentService,
    IEnvironmentService environmentService,
    IFooterMapper footerMapper)
    : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var footer = await GetFooterAsync();
        
        var footerModel = await footerMapper.Map(footer);

        return View(footerModel);
    }

    private async Task<Footer> GetFooterAsync()
    {
        var defaultFooter = new Footer { NavigationLinks = [] };
        try
        {
            var footer = await contentService.GetFooter();
            if (footer is not null)
            {
                footer.NavigationLinks = footer.NavigationLinks.Concat(OptionsLinks()).ToList();
            }
            return footer ?? defaultFooter;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving navigation links for footer");

            return defaultFooter;
        }
    }

    private IEnumerable<NavigationLink> OptionsLinks()
    {
        if (!environmentService.IsProduction())
        {
            logger.LogInformation("Showing Options link in Footer");
            yield return new NavigationLink
                         {
                             DisplayText = "Options",
                             Href = "/options",
                             OpenInNewTab = false
                         };
        }
    }
}