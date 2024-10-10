using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.ViewComponents;

public class CookiesBannerViewComponent(
    ILogger<CookiesBannerViewComponent> logger,
    IContentService contentService,
    IGovUkContentParser contentParser)
    : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var content = await contentService.GetCookiesBannerContent();

        if (content is null)
        {
            logger.LogError("No content for the cookies banner");
            return View(new CookiesBannerModel
                        {
                            Show = false
                        });
        }

        var model = await Map(content);

        return View(model);
    }

    private async Task<CookiesBannerModel> Map(CookiesBanner content)
    {
        return new CookiesBannerModel
               {
                   AcceptButtonText = content.AcceptButtonText,
                   AcceptedCookiesContent = await contentParser.ToHtml(content.AcceptedCookiesContent),
                   CookiesBannerContent = await contentParser.ToHtml(content.CookiesBannerContent),
                   CookiesBannerTitle = content.CookiesBannerTitle,
                   CookiesBannerLinkText = content.CookiesBannerLinkText,
                   HideCookieBannerButtonText = content.HideCookieBannerButtonText,
                   RejectedCookiesContent = await contentParser.ToHtml(content.RejectedCookiesContent),
                   RejectButtonText = content.RejectButtonText,
                   Show = true
               };
    }
}