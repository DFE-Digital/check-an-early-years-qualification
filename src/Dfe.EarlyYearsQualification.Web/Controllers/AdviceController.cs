using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/advice")]
public class AdviceController(ILogger<AdviceController> logger, IContentService contentService, IHtmlRenderer renderer)
    : Controller
{
    [HttpGet("qualification-outside-the-united-kingdom")]
    public async Task<IActionResult> QualificationOutsideTheUnitedKingdom()
    {
        return await GetView(AdvicePages.QualificationsAchievedOutsideTheUk);
    }

    private async Task<IActionResult> GetView(string advicePageId)
    {
        var advicePage = await contentService.GetAdvicePage(advicePageId);
        if (advicePage is null)
        {
          logger.LogError("No content for the advice page");
            return RedirectToAction("Error", "Home");
        }

        var model = await Map(advicePage);

        return View("Advice", model);
    }

    private async Task<AdvicePageModel> Map(AdvicePage advicePage)
    {
        return new AdvicePageModel
               {
                   Heading = advicePage.Heading,
                   BodyContent = await renderer.ToHtml(advicePage.Body)
               };
    }
}