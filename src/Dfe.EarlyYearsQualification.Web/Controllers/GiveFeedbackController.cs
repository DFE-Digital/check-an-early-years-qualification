using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.FeedbackForm;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/give-feedback")]
public class GiveFeedbackController(
    IContentService contentService,
    IFeedbackFormService feedbackFormService,
    IUserJourneyCookieService userJourneyCookieService,
    INotificationService notificationService,
    IFeedbackFormPageMapper feedbackFormPageMapper,
    IFeedbackFormConfirmationPageMapper feedbackFormConfirmationPageMapper)
    : ServiceController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var feedbackFormPage = await contentService.GetFeedbackFormPage();
        if (feedbackFormPage == null) return RedirectToAction("Index", "Error");

        var model = await feedbackFormPageMapper.Map(feedbackFormPage);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Post(FeedbackFormPageModel model)
    {
        if (model.QuestionList.Any(x => x.Answer is not null))
        {
            var message = feedbackFormService.ConvertQuestionListToString(model, "/give-feedback");

            notificationService.SendEmbeddedFeedbackFormNotification(new EmbeddedFeedbackFormNotification{ Message = message });
        }

        return RedirectToAction(nameof(Confirmation));
    }

    [HttpGet("confirmation")]
    public async Task<IActionResult> Confirmation()
    {
        var feedbackFormConfirmationPage = await contentService.GetFeedbackFormConfirmationPage();
        if (feedbackFormConfirmationPage == null) return RedirectToAction("Index", "Error");

        var model = await feedbackFormConfirmationPageMapper.Map(feedbackFormConfirmationPage);
        
        model.ShowOptionalSection = userJourneyCookieService.GetHasSubmittedEmailAddressInFeedbackFormQuestion();
        return View(model);
    }
}