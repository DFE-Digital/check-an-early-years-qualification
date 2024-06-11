using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

public class HomeController(
    ILogger<HomeController> logger,
    IContentService contentService,
    IHtmlRenderer htmlRenderer,
    ISideContentRenderer sideContentRenderer)
    : ServiceController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var startPageContent = await contentService.GetStartPage();
        if (startPageContent is null)
        {
            logger.LogCritical("Start page content not found");
            return RedirectToAction("Index", "Error");
        }

        var model = await Map(startPageContent);
        return View(model);
    }

    private async Task<StartPageModel> Map(StartPage startPageContent)
    {
        return new StartPageModel
               {
                   Header = startPageContent.Header,
                   PreCtaButtonContent = await htmlRenderer.ToHtml(startPageContent.PreCtaButtonContent),
                   CtaButtonText = startPageContent.CtaButtonText,
                   PostCtaButtonContent = await htmlRenderer.ToHtml(startPageContent.PostCtaButtonContent),
                   RightHandSideContentHeader = startPageContent.RightHandSideContentHeader,
                   RightHandSideContent = await sideContentRenderer.ToHtml(startPageContent.RightHandSideContent)
               };
    }
}