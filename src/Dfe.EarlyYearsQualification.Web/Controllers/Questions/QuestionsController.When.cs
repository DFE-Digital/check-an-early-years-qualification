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

        // Set details previously entered
        var (startMonth, startYear) = questionService.GetWhenWasQualificationStarted();
        if (startMonth is not null && startYear is not null)
        {
            model.Option = questionService.WasStartedBeforeSeptember2014() ?
                radioQuestionContent.Options.OfType<Option>().First().Value :
                radioQuestionContent.Options.OfType<RadioButtonAndDateInput>().First().Value;

            var radioButtonAndDateInputModel = model.OptionsItems.OfType<RadioButtonAndDateInputModel>().FirstOrDefault();

            radioButtonAndDateInputModel.StartedQuestion.SelectedMonth = startMonth.Value;
            radioButtonAndDateInputModel.StartedQuestion.SelectedYear = startYear.Value;
        }

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

        var optionModel = model.OptionsItems.OfType<OptionModel>().First();

        var radioAndDateInputContent = radioQuestionContent.Options.Last() as RadioButtonAndDateInput;
        var radioAndDateInputModel = model.OptionsItems.OfType<RadioButtonAndDateInputModel>().First();

        // Check validation required attribute e.g an option has been selected
        if (!ModelState.IsValid)
        {
            model.HasErrors = true;

            var elementLinkId = model.OptionsItems.First(x => x.GetType() == typeof(OptionModel)) as OptionModel;

            model.ErrorSummaryModel = new ErrorSummaryModel
            {
                ErrorBannerHeading = model.ErrorBannerHeading,
                ErrorSummaryLinks =
                    [
                        new ErrorSummaryLink
                        {
                            ErrorBannerLinkText = model.ErrorBannerLinkText,
                            ElementLinkId = elementLinkId != null ? elementLinkId.Value : ""
                        }
                    ]
            };

            return View("Radio", model);
        }

        // If the option selected is 'Before September 2014' then we can set a base date and skip the date input validation
        if (model.Option == optionModel.Value)
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
        var monthValue = Request.Form["StartedQuestion.SelectedMonth"].FirstOrDefault();
        var yearValue = Request.Form["StartedQuestion.SelectedYear"].FirstOrDefault();

        // Date input validation
        if (int.TryParse(monthValue, out int parsedMonth))
        {
            radioAndDateInputModel.StartedQuestion.SelectedMonth = parsedMonth;
        }

        if (int.TryParse(yearValue, out int parsedYear))
        {
            radioAndDateInputModel.StartedQuestion.SelectedYear = parsedYear;
        }

        var dateModelValidationResult = questionService.IsValid(radioAndDateInputModel.StartedQuestion, radioAndDateInputContent.StartedQuestion);
        if (!dateModelValidationResult.MonthValid || !dateModelValidationResult.YearValid)
        {
            model.HasErrors = true;
            model.HasNestedErrors = true;

            radioAndDateInputModel.StartedQuestion = questionService.MapDateModel(radioAndDateInputModel.StartedQuestion, radioAndDateInputContent.StartedQuestion, dateModelValidationResult);

            model.ErrorSummaryModel = new ErrorSummaryModel
            {
                ErrorBannerHeading = model.ErrorBannerHeading,
                ErrorSummaryLinks = radioAndDateInputModel.StartedQuestion.ErrorSummaryLinks
            };

            return View("Radio", model);
        }

        questionService.SetWhenWasQualificationStarted(radioAndDateInputModel.StartedQuestion);
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

        var dateModelValidationResult = questionService.IsValid(model, questionPage!);
        if (!dateModelValidationResult.Valid)
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
}