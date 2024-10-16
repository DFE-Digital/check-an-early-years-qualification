using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

public class HomeController(
    ILogger<HomeController> logger,
    IContentService contentService,
    IGovUkContentParser contentParser,
    IUserJourneyCookieService userJourneyCookieService)
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

        userJourneyCookieService.ResetUserJourneyCookie();

        return View(model);
    }

    private async Task<StartPageModel> Map(StartPage startPageContent)
    {
        return new StartPageModel
               {
                   Header = startPageContent.Header,
                   PreCtaButtonContent = await contentParser.ToHtml(startPageContent.PreCtaButtonContent),
                   CtaButtonText = startPageContent.CtaButtonText,
                   PostCtaButtonContent = await contentParser.ToHtml(startPageContent.PostCtaButtonContent),
                   RightHandSideContentHeader = startPageContent.RightHandSideContentHeader,
                   RightHandSideContent = await contentParser.ToHtml(startPageContent.RightHandSideContent)
               };
    }
}