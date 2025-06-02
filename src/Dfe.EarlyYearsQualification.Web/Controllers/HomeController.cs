using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers;
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
        var preCtaButtonContent = await contentParser.ToHtml(startPageContent.PreCtaButtonContent);
        var postCtaButtonContent = await contentParser.ToHtml(startPageContent.PostCtaButtonContent);
        var rightHandSideContent = await contentParser.ToHtml(startPageContent.RightHandSideContent);

        return StartPageMapper.Map(startPageContent, preCtaButtonContent, postCtaButtonContent, rightHandSideContent);
    }
}