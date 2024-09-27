using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/advice")]
public class AdviceController(
    ILogger<AdviceController> logger,
    IContentService contentService,
    IGovUkContentParser contentParser,
    IUserJourneyCookieService userJourneyCookieService)
    : ServiceController
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // user has landed on an Advice page, so is not going to select a qualification from the list,
        // partly determines where the back button should go when displaying qualification details
        userJourneyCookieService.SetUserSelectedQualificationFromList(YesOrNo.No);
        userJourneyCookieService.ClearAdditionalQuestionsAnswers();

        base.OnActionExecuting(context);
    }

    [HttpGet("qualification-outside-the-united-kingdom")]
    public async Task<IActionResult> QualificationOutsideTheUnitedKingdom()
    {
        return await GetView(AdvicePages.QualificationsAchievedOutsideTheUk);
    }

    [HttpGet("level-2-qualifications-started-between-1-sept-2014-and-31-aug-2019")]
    [RedirectIfDateMissing]
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

    [HttpGet("level-6-qualification-pre-2014")]
    [RedirectIfDateMissing]
    public async Task<IActionResult> Level6QualificationPre2014()
    {
        return await GetView(AdvicePages.Level6QualificationPre2014);
    }

    [HttpGet("level-6-qualification-post-2014")]
    [RedirectIfDateMissing]
    public async Task<IActionResult> Level6QualificationPost2014()
    {
        return await GetView(AdvicePages.Level6QualificationPost2014);
    }

    [HttpGet("qualification-level-7")]
    [RedirectIfDateMissing]
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
                   BodyContent = await contentParser.ToHtml(advicePage.Body),
                   BackButton = MapToNavigationLinkModel(advicePage.BackButton),
                   FeedbackBanner = await MapToFeedbackBannerModel(advicePage.FeedbackBanner, contentParser)
               };
    }
}