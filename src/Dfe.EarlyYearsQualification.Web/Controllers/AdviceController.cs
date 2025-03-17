using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/advice")]
public class AdviceController(
    ILogger<AdviceController> logger,
    IContentService contentService,
    IGovUkContentParser contentParser,
    IUserJourneyCookieService userJourneyCookieService,
    INotificationService notificationService)
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
        var level = userJourneyCookieService.GetLevelOfQualification();
        var (startMonth, startYear) = userJourneyCookieService.GetWhenWasQualificationStarted();

        if (level is not null && startMonth is not null && startYear is not null)
        {
            var specificCannotFindQualificationPage =
                await contentService.GetCannotFindQualificationPage(level.Value, startMonth.Value, startYear.Value);

            if (specificCannotFindQualificationPage is not null)
            {
                var model = await Map(specificCannotFindQualificationPage);
                model.Level = level == 0 ? "Any level" : level.ToString();
                model.StartedDate = $"{startMonth}-{startYear}";

                return View("QualificationNotOnList", model);
            }
        }

        return await GetView(AdvicePages.QualificationNotOnTheList);
    }

    [HttpGet("level-7-qualifications-started-between-1-sept-2014-and-31-aug-2019")]
    [RedirectIfDateMissing]
    public async Task<IActionResult> Level7QualificationStartedBetweenSept2014AndAug2019()
    {
        return await GetView(AdvicePages.Level7QualificationStartedBetweenSept2014AndAug2019);
    }

    [HttpGet("level-7-qualification-after-aug-2019")]
    [RedirectIfDateMissing]
    public async Task<IActionResult> Level7QualificationAfterAug2019()
    {
        return await GetView(AdvicePages.Level7QualificationAfterAug2019);
    }
    
    [HttpGet("help")]
    public async Task<IActionResult> Help()
    {
        var helpPage = await contentService.GetHelpPage();
        if (helpPage is null)
        {
            logger.LogError("No content for the help page");
            return RedirectToAction("Index", "Error");
        }

        var model = await Map(helpPage);

        return View("Help", model);
    }
    
    [HttpPost("help")]
    public async Task<IActionResult> Help(HelpPageModel model)
    {
        if (ModelState.IsValid)
        {
            notificationService.SendFeedbackNotification(new FeedbackNotification
                                                         {
                                                             EmailAddress = model.EmailAddress,
                                                             Subject = model.SelectedOption,
                                                             Message = model.AdditionalInformationMessage
                                                         });
            return RedirectToAction("HelpConfirmation");
        }
        
        var helpPage = await contentService.GetHelpPage();
        if (helpPage is null)
        {
            logger.LogError("No content for the help page");
            return RedirectToAction("Index", "Error");
        }
        
        var newModel = await Map(helpPage);
        newModel.HasInvalidEmailAddressError = ModelState.Keys.Any(_ => ModelState["EmailAddress"]?.Errors.Count > 0);
        newModel.HasFurtherInformationError = ModelState.Keys.Any(_ => ModelState["AdditionalInformationMessage"]?.Errors.Count > 0);
        newModel.HasNoEnquiryOptionSelectedError = ModelState.Keys.Any(_ => ModelState["SelectedOption"]?.Errors.Count > 0);
        newModel.EmailAddress = model.EmailAddress;
        newModel.SelectedOption = model.SelectedOption;
        newModel.AdditionalInformationMessage = model.AdditionalInformationMessage;

        return View("Help", newModel);
    }
    
    [HttpGet("help/confirmation")]
    public async Task<IActionResult> HelpConfirmation()
    {
        var helpConfirmationPage = await contentService.GetHelpConfirmationPage();
        if (helpConfirmationPage is null)
        {
            logger.LogError("No content for the help confirmation page");
            return RedirectToAction("Index", "Error");
        }

        var model = await Map(helpConfirmationPage);

        return View("HelpConfirmation", model);
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
        var bodyHtml = await contentParser.ToHtml(advicePage.Body);
        var feedbackBodyHtml = await GetFeedbackBannerBodyToHtml(advicePage.FeedbackBanner, contentParser);
        var improveServiceBodyHtml = advicePage.UpDownFeedback is not null
                                         ? await contentParser.ToHtml(advicePage.UpDownFeedback.ImproveServiceContent)
                                         : null;
        return AdvicePageMapper.Map(advicePage, bodyHtml, feedbackBodyHtml, improveServiceBodyHtml);
    }

    private async Task<QualificationNotOnListPageModel> Map(CannotFindQualificationPage cannotFindQualificationPage)
    {
        var bodyHtml = await contentParser.ToHtml(cannotFindQualificationPage.Body);
        var feedbackBodyHtml =
            await GetFeedbackBannerBodyToHtml(cannotFindQualificationPage.FeedbackBanner, contentParser);
        var improveServiceBodyHtml = cannotFindQualificationPage.UpDownFeedback is not null
                                         ? await contentParser.ToHtml(cannotFindQualificationPage.UpDownFeedback.ImproveServiceContent)
                                         : null;
        return AdvicePageMapper.Map(cannotFindQualificationPage, bodyHtml, feedbackBodyHtml, improveServiceBodyHtml);
    }
    
    private async Task<HelpPageModel> Map(HelpPage helpPage)
    {
        var postHeadingContentHtml = await contentParser.ToHtml(helpPage.PostHeadingContent);
        return HelpPageMapper.Map(helpPage, postHeadingContentHtml);
    }
    
    private async Task<HelpConfirmationPageModel> Map(HelpConfirmationPage helpConfirmationPage)
    {
        var bodyHtml = await contentParser.ToHtml(helpConfirmationPage.Body);
        return new HelpConfirmationPageModel
               {
                   SuccessMessage = helpConfirmationPage.SuccessMessage,
                   BodyHeading = helpConfirmationPage.BodyHeading,
                   Body = bodyHtml
               };
    }
}