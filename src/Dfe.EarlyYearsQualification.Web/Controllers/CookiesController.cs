using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("cookies")]
public class CookiesController(
    ILogger<CookiesController> logger,
    IContentService contentService,
    IHtmlTableRenderer tableRenderer,
    ISuccessBannerRenderer successBannerRenderer)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var content = await contentService.GetCookiesPage();

        if (content is null)
        {
            logger.LogError("No content for the cookies page");
            return RedirectToAction("Error", "Home");
        }

        var model = await Map(content);

        return View(model);
    }

    private async Task<CookiesPageModel> Map(CookiesPage content)
    {
        return new CookiesPageModel
               {
                   Heading = content.Heading,
                   BodyContent = await tableRenderer.ToHtml(content.Body),
                   Options = content.Options.Select(x => new OptionModel { Label = x.Label, Value = x.Value }).ToList(),
                   ButtonText = content.ButtonText,
                   SuccessBannerContent = await successBannerRenderer.ToHtml(content.SuccessBannerContent),
                   SuccessBannerHeading = content.SuccessBannerHeading,
                   ErrorText = content.ErrorText
               };
    }
}