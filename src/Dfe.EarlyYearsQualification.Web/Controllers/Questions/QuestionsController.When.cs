using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Questions;

public partial class QuestionsController
{
    [HttpGet("when-was-the-qualification-started-and-awarded")]
    public async Task<IActionResult> WhenWasTheQualificationStarted()
    {
        var questionPage =
            await questionService.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationStartedAndAwarded);
        if (questionPage is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Index", "Error");
        }

        var model = questionService.MapDatesModel(new DatesQuestionModel(), questionPage,
                                  nameof(this.WhenWasTheQualificationStarted),
                                  Questions,
                                  null
                                 );

        return View("Dates", model);
    }

    [HttpPost("when-was-the-qualification-started-and-awarded")]
#pragma warning disable S6967
    public async Task<IActionResult> WhenWasTheQualificationStarted([FromForm] DatesQuestionModel model)
#pragma warning restore S6967
    {
        var questionPage =
            await questionService.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationStartedAndAwarded);
        var dateModelValidationResult = questionService.IsValid(model, questionPage!);
        if (!dateModelValidationResult.Valid)
        {
            model.HasErrors = true;

            if (questionPage is not null)
            {
                model = questionService.MapDatesModel(model, questionPage, nameof(this.WhenWasTheQualificationStarted), Questions,
                                      dateModelValidationResult);
            }

            return View("Dates", model);
        }

        questionService.SetWhenWasQualificationStarted(model.StartedQuestion!);
        questionService.SetWhenWasQualificationAwarded(model.AwardedQuestion!);

        return RedirectToAction(nameof(this.WhatLevelIsTheQualification));
    }

    
}