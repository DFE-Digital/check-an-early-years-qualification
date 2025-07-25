using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Services.Environments;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.ViewComponents;

public class FooterViewComponent(
    ILogger<FooterViewComponent> logger,
    IContentService contentService,
    IGovUkContentParser contentParser,
    IEnvironmentService environmentService)
    : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var footer = await GetFooterAsync();
        var leftHandSideContent = footer.LeftHandSideFooterSection is not null
                                      ? await contentParser.ToHtml(footer.LeftHandSideFooterSection.Body)
                                      : null;
        var rightHandSideContent = footer.RightHandSideFooterSection is not null
                                      ? await contentParser.ToHtml(footer.RightHandSideFooterSection.Body)
                                      : null;

        var footerModel = FooterMapper.Map(footer, leftHandSideContent, rightHandSideContent);

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