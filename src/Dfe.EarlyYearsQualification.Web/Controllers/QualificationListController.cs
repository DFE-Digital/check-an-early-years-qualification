using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Dfe.EarlyYearsQualification.Web.Services.WebView;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("early-years-qualification-list")]
public class QualificationListController(
    ILogger<QualificationListController> logger,
    IWebViewService webViewService,
    IQualificationDownloadService qualificationDownloadService) : ServiceController
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

    [HttpGet("/download")]
    public async Task<IActionResult> Download()
    {
        var fileContent = await qualificationDownloadService.GetEyqlDownloadAsByteArray();
        if (fileContent.Length != 0) 
            return File(fileContent, "text/csv", "Early-Years-Qualifications-List.csv");
        
        logger.LogError("Null or empty EYQL content returned");
        return RedirectToAction("Index", "Error");
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