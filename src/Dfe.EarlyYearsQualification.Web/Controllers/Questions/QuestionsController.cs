using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Services.Help;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Questions;

[Route("/questions")]
public partial class QuestionsController(
    ILogger<QuestionsController> logger,
    IQuestionService questionService)
    : ServiceController
{
    private const string Questions = "Questions";

    [HttpGet("start-new")]
    [ResponseCache(NoStore = true)]
    public IActionResult StartNew()
    {
        questionService.ResetUserJourneyCookie();
        return RedirectToAction(nameof(this.AreYouCheckingYourOwnQualification));
    }

    private async Task<IActionResult> GetRadioView(string questionPageId, string actionName, string controllerName,
                                                   string? selectedAnswer)
    {
        var questionPage = await questionService.GetRadioQuestionPageContent(questionPageId);

        if (questionPage is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Index", "Error");
        }

        var model = await questionService.Map(new RadioQuestionModel(), questionPage, actionName, controllerName, selectedAnswer);

        return View("Radio", model);
    }
}