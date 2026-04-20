using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Dfe.EarlyYearsQualification.Web.Services.WebView;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("early-years-qualification-list")]
public class QualificationListController(
    ILogger<QualificationListController> logger,
    IWebViewService webViewService) : ServiceController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var content = await webViewService.GetWebViewPage();

        if (content is null)
        {
            logger.LogError("Web view page content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var model = await webViewService.MapWebViewPageContentToViewModelAsync(content);

        return View(model);
    }

    [HttpGet("/clear-filters")]
    public IActionResult ClearFilters()
    {
        webViewService.SetWebViewFilters(new WebViewFilters());

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("ApplyFilter")]
    public IActionResult ApplyFilter(EarlyYearsQualificationListModel model)
    {
        webViewService.ApplyFilters(model);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("RemoveFilter")]
    public IActionResult RemoveFilter(string removeFilter)
    {
        webViewService.RemoveFilter(removeFilter);

        return RedirectToAction(nameof(Index));
    }
}