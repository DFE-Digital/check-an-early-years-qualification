using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.FeedbackForm;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/give-feedback")]
public class GiveFeedbackController(
    ILogger<GiveFeedbackController> logger,
    IContentService contentService,
    IGovUkContentParser contentParser,
    IFeedbackFormService feedbackFormService,
    IUserJourneyCookieService userJourneyCookieService,
    INotificationService notificationService)
    : ServiceController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var feedbackFormPage = await contentService.GetFeedbackFormPage();
        if (feedbackFormPage is null) RedirectToAction("Index", "Error");

        var model = FeedbackFormPageMapper.Map(feedbackFormPage!,
                                               await contentParser.ToHtml(feedbackFormPage!.PostHeadingContent));
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Post(FeedbackFormPageModel model)
    {
        // Get the page content
        var feedbackFormPage = await contentService.GetFeedbackFormPage();
        if (feedbackFormPage is null) RedirectToAction("Index", "Error");

        // Ensure that mandatory questions have been answered
        var errorSummaryModel = feedbackFormService.ValidateQuestions(feedbackFormPage!, model);

        if (errorSummaryModel.ErrorSummaryLinks.Count != 0)
        {
            var mappedModel = FeedbackFormPageMapper.Map(feedbackFormPage!,
                                                         await contentParser.ToHtml(feedbackFormPage!.PostHeadingContent));
            mappedModel.HasError = true;
            mappedModel.ErrorSummaryModel = errorSummaryModel;
            mappedModel.QuestionList = model.QuestionList; 
            return View("Get", mappedModel);
        }
        
        var message = feedbackFormService.ConvertQuestionListToString(model);
        notificationService.SendEmbeddedFeedbackFormNotification(new EmbeddedFeedbackFormNotification{ Message = message });

        return RedirectToAction("Confirmation");
    }
    
    [HttpGet("confirmation")]
    public async Task<IActionResult> Confirmation()
    {
        var feedbackFormConfirmationPage = await contentService.GetFeedbackFormConfirmationPage();
        if (feedbackFormConfirmationPage is null) RedirectToAction("Index", "Error");

        var model = FeedbackFormConfirmationPageMapper.Map(feedbackFormConfirmationPage!,
                                               await contentParser.ToHtml(feedbackFormConfirmationPage!.Body),
                                               await contentParser.ToHtml(feedbackFormConfirmationPage.OptionalEmailBody));
        
        // TODO: need to get this value to be set based on whether or not they submitted an email?
        model.ShowOptionalSection = true;
        return View(model);
    }
}