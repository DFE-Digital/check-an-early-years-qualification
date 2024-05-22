using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/questions/where-was-the-qualification-awarded")]
public class QuestionsController(ILogger<QuestionsController> logger, IContentService contentService)
    : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> WhereWasTheQualificationAwarded()
    {
        return await GetView(QuestionPages.WhereWasTheQualificationAwarded, "WhereWasTheQualificationAwarded",
                             "Questions");
    }

    [HttpPost("")]
    public async Task<IActionResult> WhereWasTheQualificationAwarded(QuestionModel model)
    {
        if (!ModelState.IsValid)
        {
            var questionPage = await contentService.GetQuestionPage(QuestionPages.WhereWasTheQualificationAwarded);
            if (questionPage is not null)
            {
                model = Map(model, questionPage, "WhereWasTheQualificationAwarded", "Questions");
                model.HasErrors = true;
            }

            return View("Question", model);
        }

        return model.Option == Options.OutsideOfTheUnitedKingdom
                   ? RedirectToAction("QualificationOutsideTheUnitedKingdom", "Advice")
                   : RedirectToAction("Get", "QualificationDetails");
    }

    private async Task<IActionResult> GetView(string questionPageId, string actionName, string controllerName)
    {
        var questionPage = await contentService.GetQuestionPage(questionPageId);
        if (questionPage is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Error", "Home");
        }

        var model = Map(new QuestionModel(), questionPage, actionName, controllerName);

        return View("Question", model);
    }

    private static QuestionModel Map(QuestionModel model, QuestionPage question, string actionName,
                                     string controllerName)
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