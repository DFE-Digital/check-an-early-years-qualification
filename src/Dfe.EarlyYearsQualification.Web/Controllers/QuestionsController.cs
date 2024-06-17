using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Services.SessionService;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/questions")]
public class QuestionsController(
    ILogger<QuestionsController> logger,
    IContentService contentService,
    IHtmlRenderer renderer)
    : ServiceController
{
    private const string Questions = "Questions";

    [HttpGet("where-was-the-qualification-awarded")]
    public async Task<IActionResult> WhereWasTheQualificationAwarded()
    {
        var currentSession = HttpContext.Session.GetSessionModel();

        return await GetRadioView(QuestionPages.WhereWasTheQualificationAwarded,
                             nameof(this.WhereWasTheQualificationAwarded),
                             Questions);
    }

    [HttpPost("where-was-the-qualification-awarded")]
    public async Task<IActionResult> WhereWasTheQualificationAwarded(RadioQuestionModel model)
    {
        if (!ModelState.IsValid)
        {
            var questionPage = await contentService.GetRadioQuestionPage(QuestionPages.WhereWasTheQualificationAwarded);
            if (questionPage is not null)
            {
                model = await MapRadioModel(model, questionPage, nameof(this.WhereWasTheQualificationAwarded), Questions);
                model.HasErrors = true;
            }

            return View("Radio", model);
        }

        var currentSession = HttpContext.Session.GetSessionModel();
        currentSession.WhereWasQualAwarded = model.Option!;
        HttpContext.Session.SetSessionModel(currentSession);

        return model.Option == Options.OutsideOfTheUnitedKingdom
                   ? RedirectToAction("QualificationOutsideTheUnitedKingdom", "Advice")
                   : RedirectToAction(nameof(this.WhenWasTheQualificationStarted));
    }

    [HttpGet("when-was-the-qualification-started")]
    public async Task<IActionResult> WhenWasTheQualificationStarted()
    {
        // This is just a temporary page until the design is finalised through UR
        var questionPage = await contentService.GetDateQuestionPage(QuestionPages.WhenWasTheQualificationStarted);
        if (questionPage is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Index", "Error");
        }

        var model = MapDateModel(new DateQuestionModel(), questionPage, nameof(this.WhenWasTheQualificationStarted),
                              Questions);
        return View("Date", model);
    }

    [HttpPost("when-was-the-qualification-started")]
    public async Task<IActionResult> WhenWasTheQualificationStarted(DateQuestionModel model)
    {
        if (!ModelState.IsValid || !model.IsModelValid())
        {
            var questionPage = await contentService.GetDateQuestionPage(QuestionPages.WhenWasTheQualificationStarted);
            if (questionPage is not null)
            {
                model = MapDateModel(model, questionPage, nameof(this.WhenWasTheQualificationStarted), Questions);
                model.HasErrors = true;
            }

            return View("Date", model);
        }

        return RedirectToAction(nameof(this.WhatLevelIsTheQualification));
    }

    [HttpGet("what-level-is-the-qualification")]
    public async Task<IActionResult> WhatLevelIsTheQualification()
    {
        var currentSession = HttpContext.Session.GetSessionModel();

        return await GetRadioView(QuestionPages.WhatLevelIsTheQualification, nameof(this.WhatLevelIsTheQualification),
                             Questions);
    }

    [HttpPost("what-level-is-the-qualification")]
    public async Task<IActionResult> WhatLevelIsTheQualification(RadioQuestionModel model)
    {
        if (!ModelState.IsValid)
        {
            var questionPage = await contentService.GetRadioQuestionPage(QuestionPages.WhatLevelIsTheQualification);
            if (questionPage is not null)
            {
                model = await MapRadioModel(model, questionPage, nameof(this.WhatLevelIsTheQualification), Questions);
                model.HasErrors = true;
            }

            return View("Radio", model);
        }

        var currentSession = HttpContext.Session.GetSessionModel();
        currentSession.LevelOfQual= model.Option!;
        HttpContext.Session.SetSessionModel(currentSession);

        return RedirectToAction("Get", "QualificationDetails");
    }

    private async Task<IActionResult> GetRadioView(string questionPageId, string actionName, string controllerName)
    {
        var questionPage = await contentService.GetRadioQuestionPage(questionPageId);
        if (questionPage is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Index", "Error");
        }

        var model = await MapRadioModel(new RadioQuestionModel(), questionPage, actionName, controllerName);

        return View("Radio", model);
    }

    private async Task<RadioQuestionModel> MapRadioModel(RadioQuestionModel model, RadioQuestionPage question, string actionName,
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

    private static DateQuestionModel MapDateModel(DateQuestionModel model, DateQuestionPage question, string actionName,
                                          string controllerName)
    {
        model.Question = question.Question;
        model.CtaButtonText = question.CtaButtonText;
        model.ActionName = actionName;
        model.ControllerName = controllerName;
        model.ErrorMessage = question.ErrorMessage;
        model.QuestionHint = question.QuestionHint;
        model.MonthLabel = question.MonthLabel;
        model.YearLabel = question.YearLabel;
        return model;
    }
}