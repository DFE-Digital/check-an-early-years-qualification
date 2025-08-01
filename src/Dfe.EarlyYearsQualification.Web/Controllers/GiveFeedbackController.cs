using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/give-feedback")]
public class GiveFeedbackController(
    ILogger<AdviceController> logger,
    IContentService contentService,
    IGovUkContentParser contentParser,
    IUserJourneyCookieService userJourneyCookieService,
    INotificationService notificationService)
    : ServiceController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var feedbackFormPage = await contentService.GetFeedbackFormPage();
        if (feedbackFormPage is null) RedirectToAction("Index", "Error");

        var model = FeedbackFormPageMapper.Map(feedbackFormPage!, await contentParser.ToHtml(feedbackFormPage!.PostHeadingContent));
        model.RadioAnswers = new string[model.Questions.Count(x => x.GetType() == typeof(FeedbackFormQuestionRadioModel))];
        return View(model);
    }
}