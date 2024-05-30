using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/questions")]
public class QuestionsController(ILogger<QuestionsController> logger, IContentService contentService, IHtmlRenderer renderer)
    : Controller
{
    private const string Questions = "Questions";

    [HttpGet("where-was-the-qualification-awarded")]
    public async Task<IActionResult> WhereWasTheQualificationAwarded()
    {
        return await GetView(QuestionPages.WhereWasTheQualificationAwarded, nameof(this.WhereWasTheQualificationAwarded),
                             Questions);
    }

    [HttpPost("where-was-the-qualification-awarded")]
    public async Task<IActionResult> WhereWasTheQualificationAwarded(QuestionModel model)
    {
        if (!ModelState.IsValid)
        {
            var questionPage = await contentService.GetQuestionPage(QuestionPages.WhereWasTheQualificationAwarded);
            if (questionPage is not null)
            {
                model = await Map(model, questionPage, nameof(this.WhereWasTheQualificationAwarded), Questions);
                model.HasErrors = true;
            }

            return View("Question", model);
        }

        return model.Option == Options.OutsideOfTheUnitedKingdom
                   ? RedirectToAction("QualificationOutsideTheUnitedKingdom", "Advice")
                   : RedirectToAction(nameof(this.WhenWasTheQualificationStarted));
    }

    [HttpGet("when-was-the-qualification-started")]
    public async Task<IActionResult> WhenWasTheQualificationStarted()
    {
        // TODO: This is just a temporary page until the design is finalised through UR
        var questionPage = new QuestionPage()
                           {
                                CtaButtonText = "Continue",
                                Question = "When was the qualification started?",
                                Options = new List<Option>()
                           };
        var model = await Map(new QuestionModel(), questionPage, nameof(this.WhenWasTheQualificationStarted), Questions);
        return View("Question", model);
    }

    [HttpPost("when-was-the-qualification-started")]
    public IActionResult WhenWasTheQualificationStarted(QuestionModel model)
    {
        // TODO: This is just a temporary page until the design is finalised through UR
        // For now just redirect to the next page. Model validation will be done at a later date
        return RedirectToAction(nameof(this.WhatLevelIsTheQualification));
    }

    [HttpGet("what-level-is-the-qualification")]
    public async Task<IActionResult> WhatLevelIsTheQualification()
    {
        return await GetView(QuestionPages.WhatLevelIsTheQualification, nameof(this.WhatLevelIsTheQualification),
                             Questions);
    }

    [HttpPost("what-level-is-the-qualification")]
    public async Task<IActionResult> WhatLevelIsTheQualification(QuestionModel model)
    {
        if (!ModelState.IsValid)
        {
            var questionPage = await contentService.GetQuestionPage(QuestionPages.WhatLevelIsTheQualification);
            if (questionPage is not null)
            {
                model = await Map(model, questionPage, nameof(this.WhatLevelIsTheQualification), Questions);
                model.HasErrors = true;
            }

            return View("Question", model);
        }

        return RedirectToAction("Get", "QualificationDetails");
    }

    private async Task<IActionResult> GetView(string questionPageId, string actionName, string controllerName)
    {
        var questionPage = await contentService.GetQuestionPage(questionPageId);
        if (questionPage is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Error", "Home");
        }

        var model = await Map(new QuestionModel(), questionPage, actionName, controllerName);

        return View("Question", model);
    }

    private async Task<QuestionModel> Map(QuestionModel model, QuestionPage question, string actionName,
                                     string controllerName)
    {
        model.Question = question.Question;
        model.Options = question.Options.Select(x => new OptionModel { Label = x.Label, Value = x.Value }).ToList();
        model.CtaButtonText = question.CtaButtonText;
        model.ActionName = actionName;
        model.ControllerName = controllerName;
        model.ErrorMessage = question.ErrorMessage;
        model.AdditionalInformationHeader = question.AdditionalInformationHeader;
        model.AdditionalInformationBody = await renderer.ToHtml(question.AdditionalInformationBody);
        return model;
    }
}