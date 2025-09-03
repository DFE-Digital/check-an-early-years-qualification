using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

public class HomeController(
    ILogger<HomeController> logger,
    IContentService contentService,
    IUserJourneyCookieService userJourneyCookieService,
    IStartPageMapper startPageMapper)
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

        var model = await startPageMapper.Map(startPageContent);

        userJourneyCookieService.ResetUserJourneyCookie();

        return View(model);
    }
}