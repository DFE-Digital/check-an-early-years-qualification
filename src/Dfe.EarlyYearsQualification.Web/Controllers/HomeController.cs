using System.Diagnostics;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

public class HomeController : Controller
{
    private readonly IContentService _contentService;
    private readonly IHtmlRenderer _htmlRenderer;
    private readonly ILogger<HomeController> _logger;
    private readonly ISideContentRenderer _sideContentRenderer;

    public HomeController(
        ILogger<HomeController> logger,
        IContentService contentService,
        IHtmlRenderer htmlRenderer,
        ISideContentRenderer sideContentRenderer)
    {
        _logger = logger;
        _contentService = contentService;
        _htmlRenderer = htmlRenderer;
        _sideContentRenderer = sideContentRenderer;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var startPageContent = await _contentService.GetStartPage();
        if (startPageContent is null)
        {
            _logger.LogCritical("Start page content not found");
            return RedirectToAction("Error");
        }

        var model = await Map(startPageContent);
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private async Task<StartPageModel> Map(StartPage startPageContent)
    {
        return new StartPageModel
               {
                   Header = startPageContent.Header,
                   PreCtaButtonContent = await _htmlRenderer.ToHtml(startPageContent.PreCtaButtonContent),
                   CtaButtonText = startPageContent.CtaButtonText,
                   PostCtaButtonContent = await _htmlRenderer.ToHtml(startPageContent.PostCtaButtonContent),
                   RightHandSideContentHeader = startPageContent.RightHandSideContentHeader,
                   RightHandSideContent = await _sideContentRenderer.ToHtml(startPageContent.RightHandSideContent)
               };
    }
}