using Microsoft.AspNetCore.Mvc;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Constants;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/questions")]
public class QuestionsController : Controller
{
    private readonly ILogger<QuestionsController> _logger;
    private readonly IContentService _contentService;

    public QuestionsController(ILogger<QuestionsController> logger, IContentService contentService)
    {
        _logger = logger;
        _contentService = contentService;
    }

    [HttpGet("where-was-the-qualification-awarded")]
    public async Task<IActionResult> WhereWasTheQualificationAwarded()
    {
        var questionPage = await _contentService.GetQuestionPage(QuestionPages.WhereWasTheQualificationAwarded);
        if (questionPage is null)
        {
            return RedirectToAction("Error", "Home");
        }

        var model = Map(questionPage, "WhereWasTheQualificationAwarded", "Questions");

        return View("Question", model);
    }

    [ValidateAntiForgeryToken]
    [HttpPost("where-was-the-qualification-awarded")]
    public IActionResult WhereWasTheQualificationAwarded(string option)
    {
        if (option == "outside-uk")
        {
            return RedirectToAction("QualificationOutsideTheUnitedKingdom", "Advice");
        }
        return RedirectToAction("Get", "QualificationDetails");
    }

    private QuestionModel Map(QuestionPage question, string actionName, string controllerName)
    {
        return new QuestionModel
        {
            Question = question.Question,
            Options = question.Options.Select(x => new OptionModel { Label = x.Label, Value = x.Value }).ToList(),
            CtaButtonText = question.CtaButtonText,
            ActionName = actionName,
            ControllerName = controllerName,
        };
    }
}
