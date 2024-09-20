using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.ViewComponents;

public class PhaseBannerViewComponent(
    IContentService contentService,
    ILogger<PhaseBannerViewComponent> logger,
    IGovUkContentParser contentParser)
    : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var content = await contentService.GetPhaseBannerContent();

        if (content is null)
        {
            logger.LogError("No content for the phase banner");
            return View();
        }

        var model = await Map(content);

        return View(model);
    }

    private async Task<PhaseBannerModel> Map(PhaseBanner content)
    {
        return new PhaseBannerModel
               {
                   PhaseName = content.PhaseName,
                   Show = content.Show,
                   Content = await contentParser.ToHtml(content.Content)
               };
    }
}