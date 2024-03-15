using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IContentService _contentService;

    public HomeController(ILogger<HomeController> logger, IContentService contentService)
    {
        _logger = logger;
        _contentService = contentService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var startPageContent = await _contentService.GetStartPage();
        if (startPageContent is null) return RedirectToAction("Error");
        var model = new StartPageModel() 
        { 
            Header = startPageContent.Header, 
            PreCtaButtonContent = startPageContent.PreCtaButtonContentHtml, 
            CtaButtonText = startPageContent.CtaButtonText,
            PostCtaButtonContent = startPageContent.PostCtaButtonContentHtml,
            RightHandSideContentHeader = startPageContent.RightHandSideContentHeader,
            RightHandSideContent = startPageContent.RightHandSideContentHtml
        };
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
