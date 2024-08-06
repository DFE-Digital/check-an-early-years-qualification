using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/advice")]
public class AdviceController(ILogger<AdviceController> logger, IContentService contentService, IHtmlRenderer renderer)
    : ServiceController
{
    [HttpGet("qualification-outside-the-united-kingdom")]
    public async Task<IActionResult> QualificationOutsideTheUnitedKingdom()
    {
        return await GetView(AdvicePages.QualificationsAchievedOutsideTheUk);
    }

    [HttpGet("level-2-qualifications-started-between-1-sept-2014-and-31-aug-2019")]
    public async Task<IActionResult> QualificationsStartedBetweenSept2014AndAug2019()
    {
        return await GetView(AdvicePages.QualificationsStartedBetweenSept2014AndAug2019);
    }

    [HttpGet("qualifications-achieved-in-northern-ireland")]
    public async Task<IActionResult> QualificationsAchievedInNorthernIreland()
    {
        return await GetView(AdvicePages.QualificationsAchievedInNorthernIreland);
    }

    [HttpGet("qualifications-achieved-in-scotland")]
    public async Task<IActionResult> QualificationsAchievedInScotland()
    {
        return await GetView(AdvicePages.QualificationsAchievedInScotland);
    }

    [HttpGet("qualifications-achieved-in-wales")]
    public async Task<IActionResult> QualificationsAchievedInWales()
    {
        return await GetView(AdvicePages.QualificationsAchievedInWales);
    }

    [HttpGet("qualification-not-on-the-list")]
    public async Task<IActionResult> QualificationNotOnTheList()
    {
        return await GetView(AdvicePages.QualificationNotOnTheList);
    }

    [HttpGet("qualification-level-7")]
    public async Task<IActionResult> QualificationLevel7()
    {
        return await GetView(AdvicePages.QualificationLevel7);
    }

    private async Task<IActionResult> GetView(string advicePageId)
    {
        var advicePage = await contentService.GetAdvicePage(advicePageId);
        if (advicePage is null)
        {
            logger.LogError("No content for the advice page");
            return RedirectToAction("Index", "Error");
        }

        var model = await Map(advicePage);

        return View("Advice", model);
    }

    private async Task<AdvicePageModel> Map(AdvicePage advicePage)
    {
        return new AdvicePageModel
               {
                   Heading = advicePage.Heading,
                   BodyContent = await renderer.ToHtml(advicePage.Body),
                   BackButton = MapToNavigationLinkModel(advicePage.BackButton)
               };
    }
}