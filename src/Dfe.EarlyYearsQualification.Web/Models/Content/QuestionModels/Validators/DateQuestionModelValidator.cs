using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

public class DateQuestionModelValidator(IDateTimeAdapter dateTimeAdapter) : IDateQuestionModelValidator
{
    public DateValidationResult IsValid(DateQuestionModel model, DateQuestion question)
    {
        var resultToReturn = new DateValidationResult();

        if (model is { SelectedYear: null, SelectedMonth: null })
        {
            resultToReturn.MonthValid = false;
            resultToReturn.YearValid = false;
            resultToReturn.ErrorMessages.Add(question.ErrorMessage);
            resultToReturn.BannerErrorMessages.Add(question.ErrorBannerLinkText);

            return resultToReturn;
        }

        if (model.SelectedMonth == null)
        {
            resultToReturn.MonthValid = false;
            resultToReturn.ErrorMessages.Add(question.MissingMonthErrorMessage);
            resultToReturn.BannerErrorMessages.Add(question.MissingMonthBannerLinkText);
        }

        if (model.SelectedMonth is <= 0 or > 12)
        {
            resultToReturn.MonthValid = false;
            resultToReturn.ErrorMessages.Add(question.MonthOutOfBoundsErrorMessage);
            resultToReturn.BannerErrorMessages.Add(question.MonthOutOfBoundsErrorLinkText);
        }

        if (model.SelectedYear == null)
        {
            resultToReturn.YearValid = false;
            resultToReturn.ErrorMessages.Add(question.MissingYearErrorMessage);
            resultToReturn.BannerErrorMessages.Add(question.MissingYearBannerLinkText);
        }

        var now = dateTimeAdapter.Now();

        if (model.SelectedYear < 1900 || model.SelectedYear > now.Year)
        {
            resultToReturn.YearValid = false;
            resultToReturn.ErrorMessages.Add(question.YearOutOfBoundsErrorMessage);
            resultToReturn.BannerErrorMessages.Add(question.YearOutOfBoundsErrorLinkText);
        }

        if (resultToReturn.ErrorMessages.Count != 0)
        {
            return resultToReturn;
        }

        var selectedDate = new DateOnly(model.SelectedYear!.Value, model.SelectedMonth!.Value, 1);

        if (selectedDate > DateOnly.FromDateTime(now))
        {
            resultToReturn.MonthValid = false;
            resultToReturn.YearValid = false;
            resultToReturn.ErrorMessages.Add(question.FutureDateErrorMessage);
            resultToReturn.BannerErrorMessages.Add(question.FutureDateErrorBannerLinkText);
        }

        return resultToReturn;
    }

    public DatesValidationResult IsValid(DatesQuestionModel model, DatesQuestionPage questionPage)
    {
        var startedQuestion = model.StartedQuestion;
        var awardedQuestion = model.AwardedQuestion;
        if (startedQuestion is null || awardedQuestion is null) throw new NullReferenceException("Started question or awarded question is null");
        var startedValidationResult = IsValid(startedQuestion, questionPage.StartedQuestion!);
        var awardedValidationResult = IsValid(awardedQuestion, questionPage.AwardedQuestion!);

        if (awardedValidationResult.YearValid && DisplayAwardedDateBeforeStartDateError(startedQuestion, awardedQuestion))
        {
            awardedValidationResult.MonthValid = false;
            awardedValidationResult.YearValid = false;
            awardedValidationResult.ErrorMessages.Add(questionPage.AwardedDateIsAfterStartedDateErrorText);
            awardedValidationResult.BannerErrorMessages.Add(questionPage.AwardedDateIsAfterStartedDateErrorText);
        }

        return new DatesValidationResult
               {
                   StartedValidationResult = startedValidationResult,
                   AwardedValidationResult = awardedValidationResult
               };
    }

    public bool DisplayAwardedDateBeforeStartDateError(DateQuestionModel startedQuestion, DateQuestionModel awardedQuestion)
    {
        var startDateMonth = startedQuestion.SelectedMonth;
        var startDateYear = startedQuestion.SelectedYear;
        var awardedDateMonth = awardedQuestion.SelectedMonth;
        var awardedDateYear = awardedQuestion.SelectedYear;

        if (startDateYear is null || awardedDateYear is null) return false;
        if (awardedDateYear.Value < startDateYear.Value) return true;
        if (startDateMonth is null || awardedDateMonth is null) return false;

        var startedDate = new DateOnly(startDateYear.Value, startDateMonth.Value, 1);
        var awardedDate = new DateOnly(awardedDateYear.Value, awardedDateMonth.Value, 1);

        return startedDate >= awardedDate;
    }
}