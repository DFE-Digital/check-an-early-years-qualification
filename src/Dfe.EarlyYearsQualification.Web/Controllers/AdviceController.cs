using Microsoft.AspNetCore.Mvc;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Constants;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/advice")]
public class AdviceController : Controller
{
    private readonly ILogger<AdviceController> _logger;
    private readonly IContentService _contentService;

    public AdviceController(ILogger<AdviceController> logger, IContentService contentService)
    {
        _logger = logger;
        _contentService = contentService;
    }

    [HttpGet("qualification-outside-the-united-kingdom")]
    public async Task<IActionResult> QualificationOutsideTheUnitedKingdom()
    {
        return await GetView(AdvicePages.QualificationsAchievedOutsideTheUk);
    }

    private async Task<IActionResult> GetView(string advicePageId)
    {
        var advicePage = await _contentService.GetAdvicePage(advicePageId);
        if (advicePage is null)
        {
            return RedirectToAction("Error", "Home");
        }

        var model = Map(advicePage);

        return View("Advice", model);
    }

    private static AdvicePageModel Map(AdvicePage advicePage)
    {
        return new AdvicePageModel
        {
            Heading = advicePage.Heading,
            BodyContent = advicePage.BodyHtml
        };
    }
}
