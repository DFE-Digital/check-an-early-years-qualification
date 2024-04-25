using Microsoft.AspNetCore.Mvc;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Web.Constants;

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
        return await GetView(QuestionPages.WhereWasTheQualificationAwarded, "WhereWasTheQualificationAwarded", "Questions");
    }

    [HttpPost("where-was-the-qualification-awarded")]
    public async Task<IActionResult> WhereWasTheQualificationAwarded(QuestionModel model)
    {
        if (!ModelState.IsValid)
        {
            var questionPage = await _contentService.GetQuestionPage(QuestionPages.WhereWasTheQualificationAwarded);
            if (questionPage is not null)
            {
                model = Map(model, questionPage, "WhereWasTheQualificationAwarded", "Questions");
                model.HasErrors = true;
            }
            
            return View("Question", model);
        }

        if (model.Option == Options.OutsideOfTheUnitedKingdom)
        {
            return RedirectToAction("QualificationOutsideTheUnitedKingdom", "Advice");
        }
        return RedirectToAction("Get", "QualificationDetails");
    }

    private async Task<IActionResult> GetView(string questionPageId, string actionName, string controllerName)
    {
        var questionPage = await _contentService.GetQuestionPage(questionPageId);
        if (questionPage is null)
        {
            return RedirectToAction("Error", "Home");
        }

        var model = Map(new QuestionModel(), questionPage, actionName, controllerName);

        return View("Question", model);
    }

    private static QuestionModel Map(QuestionModel model, QuestionPage question, string actionName, string controllerName)
    {
        model.Question = question.Question;
        model.Options = question.Options.Select(x => new OptionModel { Label = x.Label, Value = x.Value }).ToList();
        model.CtaButtonText = question.CtaButtonText;
        model.ActionName = actionName;
        model.ControllerName = controllerName;
        model.ErrorMessage = question.ErrorMessage;
        return model;
    }
}
