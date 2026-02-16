using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/advice")]
public class AdviceController(
    ILogger<AdviceController> logger,
    IContentService contentService,
    IUserJourneyCookieService userJourneyCookieService,
    IStaticPageMapper staticPageMapper)
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
        return await GetView(StaticPages.QualificationsAchievedOutsideTheUk);
    }

    [HttpGet("level-2-qualifications-started-between-1-sept-2014-and-31-aug-2019")]
    [RedirectIfDateMissing]
    public async Task<IActionResult> QualificationsStartedBetweenSept2014AndAug2019()
    {
        return await GetView(StaticPages.QualificationsStartedBetweenSept2014AndAug2019);
    }

    [HttpGet("qualifications-achieved-in-northern-ireland")]
    public async Task<IActionResult> QualificationsAchievedInNorthernIreland()
    {
        return await GetView(StaticPages.QualificationsAchievedInNorthernIreland);
    }

    [HttpGet("qualifications-achieved-in-scotland")]
    public async Task<IActionResult> QualificationsAchievedInScotland()
    {
        return await GetView(StaticPages.QualificationsAchievedInScotland);
    }

    [HttpGet("qualifications-achieved-in-wales")]
    public async Task<IActionResult> QualificationsAchievedInWales()
    {
        return await GetView(StaticPages.QualificationsAchievedInWales);
    }

    [HttpGet("qualification-not-on-the-list")]
    [RedirectIfDateMissing]
    public async Task<IActionResult> QualificationNotOnTheList()
    {
        var level = userJourneyCookieService.GetLevelOfQualification();
        var (startMonth, startYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
        var isUserCheckingTheirOwnQualification = userJourneyCookieService.GetIsUserCheckingTheirOwnQualification();

        if (level is not null && startMonth is not null && startYear is not null)
        {
            var specificCannotFindQualificationPage =
                await contentService.GetCannotFindQualificationPage(level.Value, startMonth.Value, startYear.Value,
                                                                    isUserCheckingTheirOwnQualification == "yes");

            if (specificCannotFindQualificationPage is not null)
            {
                var model = await staticPageMapper.Map(specificCannotFindQualificationPage);
                model.Level = level == 0 ? "Any level" : level.ToString();
                model.StartedDate = $"{startMonth}-{startYear}";

                return View("../Static/QualificationNotOnList", model);
            }
        }

        return await GetView(StaticPages.QualificationNotOnTheList);
    }

    [HttpGet("level-7-qualifications-started-between-1-sept-2014-and-31-aug-2019")]
    [RedirectIfDateMissing]
    public async Task<IActionResult> Level7QualificationStartedBetweenSept2014AndAug2019()
    {
        return await GetView(StaticPages.Level7QualificationStartedBetweenSept2014AndAug2019);
    }

    [HttpGet("level-7-qualification-after-aug-2019")]
    [RedirectIfDateMissing]
    public async Task<IActionResult> Level7QualificationAfterAug2019()
    {
        return await GetView(StaticPages.Level7QualificationAfterAug2019);
    }

    [HttpGet("help")]
    public IActionResult Help()
    {
        return RedirectToAction("GetHelp", "Help");
    }

    private async Task<IActionResult> GetView(string staticPageId)
    {
        var staticPage = await contentService.GetStaticPage(staticPageId);
        if (staticPage is null)
        {
            logger.LogError("No content for the advice page");
            return RedirectToAction("Index", "Error");
        }

        var model = await staticPageMapper.Map(staticPage);

        return View("../Static/Static", model);
    }
}