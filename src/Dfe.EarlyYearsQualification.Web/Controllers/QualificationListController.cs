using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.Environments;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Dfe.EarlyYearsQualification.Web.Services.WebView;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("early-years-qualification-list")]
public class QualificationListController(
    ILogger<QualificationListController> logger,
    IWebViewService webViewService,
    IQualificationDownloadService qualificationDownloadService,
    IEnvironmentService environmentService) : ServiceController
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

        var model = await webViewService.MapWebViewPageContentToViewModelAsync(content, environmentService.IsProduction());

        return View(model);
    }

    [HttpGet("download")]
    public async Task<IActionResult> Download()
    {
        var environment = environmentService.GetEnvironment();

        var (fileContents, fileName) = await qualificationDownloadService.GetEyqlDownload(environment);
        if (fileContents.Length != 0) 
            return File(fileContents, "text/csv", fileName);
        
        logger.LogError("Null or empty EYQL content returned");
        return RedirectToAction("Index", "Error");
    }
    
    [HttpGet("download/internal")]
    public async Task<IActionResult> InternalDownload()
    {
        if (environmentService.IsProduction())
        {
            return NotFound();
        }

        var fileContents = await qualificationDownloadService.GetEyqlDataForInternalDownload();
        if (fileContents is not null && fileContents.Length != 0) 
            return File(fileContents, "text/csv", $"published_qualifications_{DateTime.Now.ToShortDateString()}.csv");
        
        logger.LogError("Null or empty EYQL content returned");
        return RedirectToAction("Index", "Error");
    }

    [HttpGet("clear-filters")]
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