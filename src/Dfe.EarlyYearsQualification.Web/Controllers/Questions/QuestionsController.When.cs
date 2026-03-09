using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Questions;

public partial class QuestionsController
{
    [HttpGet("when-was-the-qualification-started")]
    public async Task<IActionResult> WhenWasTheQualificationStarted()
    {
        var radioQuestionContent = await questionService.GetRadioQuestionPageContent(QuestionPages.WhenWasTheQualificationStarted);

        if (radioQuestionContent is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Index", "Error");
        }

        var model = new RadioQuestionModel();

        model = await questionService.Map(model, radioQuestionContent, nameof(this.WhenWasTheQualificationStarted), Questions, model.Option);

        questionService.SetPreviouslyEnteredDetails(model, radioQuestionContent);

        return View("Radio", model);
    }

    [HttpPost("when-was-the-qualification-started")]
    public async Task<IActionResult> WhenWasTheQualificationStarted([FromForm] RadioQuestionModel model)
    {
        var radioQuestionContent = await questionService.GetRadioQuestionPageContent(QuestionPages.WhenWasTheQualificationStarted);

        if (radioQuestionContent is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Index", "Error");
        }
        
        model = await questionService.Map(model, radioQuestionContent, nameof(WhenWasTheQualificationStarted), Questions, model.Option);

        var firstOption = model.OptionsItems.OfType<OptionModel>().First();

        var radioAndDateInputContent = radioQuestionContent.Options.Last() as RadioButtonAndDateInput;
        var radioAndDateInputModel = model.OptionsItems.OfType<RadioButtonAndDateInputModel>().First();

        if (radioAndDateInputModel.Question is null || radioAndDateInputContent is null)
        {
            logger.LogError("RadioAndDateInputModel StartedQuestion is null");
            return RedirectToAction("Index", "Error");
        }
        
        // Check validation required attribute e.g. an option has been selected
        if (!ModelState.IsValid)
        {
            model.HasErrors = true;
            model.ErrorSummaryModel = new ErrorSummaryModel
            {
                ErrorBannerHeading = model.ErrorBannerHeading,
                ErrorSummaryLinks =
                    [
                        new ErrorSummaryLink
                        {
                            ErrorBannerLinkText = model.ErrorBannerLinkText,
                            ElementLinkId = firstOption.Value
                        }
                    ]
            };

            return View("Radio", model);
        }

        // If the option selected is 'Before September 2014' then we can set a base date and skip the date input validation
        if (model.Option == firstOption.Value)
        {
            // set a base date
            questionService.SetWhenWasQualificationStarted(
                new DateQuestionModel()
                {
                    SelectedMonth = 1,
                    SelectedYear = 1900
                }
            );

            return RedirectToAction(nameof(WhenWasTheQualificationAwarded));
        }

        // Get values from the nested date input
        radioAndDateInputModel.Question.SelectedMonth = ParseValueFromForm("Month");
        radioAndDateInputModel.Question.SelectedYear = ParseValueFromForm("Year");

        // Validate date input
        var dateModelValidationResult = questionService.StartDateIsValid(radioAndDateInputModel.Question, radioAndDateInputContent.StartedQuestion);

        if (!dateModelValidationResult.MonthValid || !dateModelValidationResult.YearValid)
        {
            model.HasErrors = true;
            model.HasNestedErrors = true;

            radioAndDateInputModel.Question = questionService.MapDateModel(radioAndDateInputModel.Question, radioAndDateInputContent.StartedQuestion, dateModelValidationResult);

            model.ErrorSummaryModel = new ErrorSummaryModel
                                      {
                                          ErrorBannerHeading = model.ErrorBannerHeading,
                                          ErrorSummaryLinks = radioAndDateInputModel.Question.ErrorSummaryLinks
                                      };
        
            return View("Radio", model);
        }

        questionService.SetWhenWasQualificationStarted(radioAndDateInputModel.Question);
        return RedirectToAction(nameof(WhenWasTheQualificationAwarded));
    }

    [HttpGet("when-was-the-qualification-awarded")]
    public async Task<IActionResult> WhenWasTheQualificationAwarded()
    {
        var questionPage = await questionService.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationAwarded);
        
        if (questionPage is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Index", "Error");
        }

        var model = questionService.MapDatesModel(new DatesQuestionModel(), questionPage,
                                  nameof(WhenWasTheQualificationAwarded),
                                  Questions,
                                  null
                                 );

        return View("Dates", model);
    }

    [HttpPost("when-was-the-qualification-awarded")]
    public async Task<IActionResult> WhenWasTheQualificationAwarded([FromForm] DatesQuestionModel model)
    {
        var questionPage = await questionService.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationAwarded);

        if (questionPage is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Index", "Error");
        }

        var (startMonth, startYear) = questionService.GetWhenWasQualificationStarted();

        model.StartedQuestion = new DateQuestionModel
        {
            SelectedMonth = startMonth,
            SelectedYear = startYear
        };

        var dateModelValidationResult = questionService.IsValid(model, questionPage!);
        if (!dateModelValidationResult.AwardedValidationResult!.MonthValid || !dateModelValidationResult.AwardedValidationResult.YearValid)
        {
            model.HasErrors = true;

            model = questionService.MapDatesModel(model, questionPage, nameof(WhenWasTheQualificationAwarded), Questions,
                                    dateModelValidationResult);

            return View("Dates", model);
        }

        questionService.SetWhenWasQualificationStarted(model.StartedQuestion!);
        questionService.SetWhenWasQualificationAwarded(model.AwardedQuestion!);

        return RedirectToAction(nameof(WhatLevelIsTheQualification));
    }
    
    private int? ParseValueFromForm(string key)
    {
        var submittedValue = Request.Form[key].FirstOrDefault();
        
        if (int.TryParse(submittedValue, out var parsedValue))
        {
            return parsedValue;
        }

        return null;
    }
}