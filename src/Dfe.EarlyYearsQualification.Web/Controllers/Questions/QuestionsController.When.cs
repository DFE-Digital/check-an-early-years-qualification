using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Questions;

public partial class QuestionsController
{
    [HttpGet("when-was-the-qualification-started-and-awarded")]
    public async Task<IActionResult> WhenWasTheQualificationStarted()
    {
        var questionPage =
            await contentService.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationStartedAndAwarded);
        if (questionPage is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Index", "Error");
        }

        var model = MapDatesModel(new DatesQuestionModel(), questionPage,
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
            await contentService.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationStartedAndAwarded);
        var dateModelValidationResult = questionModelValidator.IsValid(model, questionPage!);
        if (!dateModelValidationResult.Valid)
        {
            model.HasErrors = true;

            if (questionPage is not null)
            {
                model = MapDatesModel(model, questionPage, nameof(this.WhenWasTheQualificationStarted), Questions,
                                      dateModelValidationResult);
            }

            return View("Dates", model);
        }

        userJourneyCookieService.SetWhenWasQualificationStarted(model.StartedQuestion!.SelectedMonth.ToString() + '/' +
                                                                model.StartedQuestion.SelectedYear);
        userJourneyCookieService.SetWhenWasQualificationAwarded(model.AwardedQuestion!.SelectedMonth.ToString() + '/' +
                                                                model.AwardedQuestion.SelectedYear);

        return RedirectToAction(nameof(this.WhatLevelIsTheQualification));
    }

    private DateQuestionModel? MapDateModel(DateQuestionModel model, DateQuestion? question,
                                            DateValidationResult? validationResult,
                                            int? selectedMonth,
                                            int? selectedYear)
    {
        if (question is null) return null;
        var bannerErrors = validationResult is { BannerErrorMessages.Count: > 0 } ? validationResult.BannerErrorMessages : null;

        var errorMessageText = validationResult is { ErrorMessages.Count: > 0 }
                                   ? string.Join("<br />", validationResult.ErrorMessages)
                                   : null;

        var errorBannerMessages = new List<BannerError>();
        if (bannerErrors is null)
        {
            errorBannerMessages.Add(new BannerError(question.ErrorMessage, FieldId.Month));
        }
        else
        {
            foreach (var bannerError in bannerErrors)
            {
                errorBannerMessages.Add(new BannerError(placeholderUpdater.Replace(bannerError.Message), bannerError.FieldId));
            }
        }

        var errorMessage = placeholderUpdater.Replace(errorMessageText ?? question.ErrorMessage);

        return DateQuestionMapper.Map(model, question, errorBannerMessages, errorMessage, validationResult,
                                      selectedMonth, selectedYear);
    }

    private DatesQuestionModel MapDatesModel(DatesQuestionModel model, DatesQuestionPage question, string actionName,
                                             string controllerName, DatesValidationResult? validationResult)
    {
        var (startMonth, startYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
        var startedModel = MapDateModel(new DateQuestionModel(), question.StartedQuestion,
                                        validationResult?.StartedValidationResult, startMonth, startYear);

        var (awardedMonth, awardedYear) = userJourneyCookieService.GetWhenWasQualificationAwarded();
        var awardedModel = MapDateModel(new DateQuestionModel(), question.AwardedQuestion,
                                        validationResult?.AwardedValidationResult, awardedMonth, awardedYear);

        return DatesQuestionMapper.Map(model, question, actionName, controllerName, startedModel, awardedModel);
    }
}