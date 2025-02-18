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
        return new DatesValidationResult
               {
                   StartedValidationResult = IsValid(model.StartedQuestion!, questionPage.StartedQuestion!),
                   AwardedValidationResult = IsValid(model.AwardedQuestion!, questionPage.AwardedQuestion!)
               };
    }
}