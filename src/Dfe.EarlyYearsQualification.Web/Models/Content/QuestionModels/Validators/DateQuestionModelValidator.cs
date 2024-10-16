using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

public class DateQuestionModelValidator(IDateTimeAdapter dateTimeAdapter) : IDateQuestionModelValidator
{
    public DateValidationResult IsValid(DateQuestionModel model, DateQuestionPage questionPage)
    {
        var resultToReturn = new DateValidationResult();
        
        if (model is { SelectedYear: null, SelectedMonth: null })
        {
            resultToReturn.MonthValid = false;
            resultToReturn.YearValid = false;
            resultToReturn.ErrorMessages.Add(questionPage.ErrorMessage);
            resultToReturn.BannerErrorMessages.Add(questionPage.ErrorBannerLinkText);

            return resultToReturn;
        }

        if (model.SelectedMonth == null)
        {
            resultToReturn.MonthValid = false;
            resultToReturn.ErrorMessages.Add(questionPage.MissingMonthErrorMessage);
            resultToReturn.BannerErrorMessages.Add(questionPage.MissingMonthBannerLinkText);
        }
        
        if (model.SelectedYear == null)
        {
            resultToReturn.YearValid = false;
            resultToReturn.ErrorMessages.Add(questionPage.MissingYearErrorMessage);
            resultToReturn.BannerErrorMessages.Add(questionPage.MissingYearBannerLinkText);
        }
        
        if (model.SelectedMonth is <= 0 or > 12)
        {
            resultToReturn.MonthValid = false;
            resultToReturn.ErrorMessages.Add(questionPage.MonthOutOfBoundsErrorMessage);
            resultToReturn.BannerErrorMessages.Add(questionPage.MonthOutOfBoundsErrorLinkText);
        }
        
        var now = dateTimeAdapter.Now();

        if (model.SelectedYear < 1900 || model.SelectedYear > now.Year)
        {
            resultToReturn.YearValid = false;
            resultToReturn.ErrorMessages.Add(questionPage.YearOutOfBoundsErrorMessage);
            resultToReturn.BannerErrorMessages.Add(questionPage.YearOutOfBoundsErrorLinkText);
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
            resultToReturn.ErrorMessages.Add(questionPage.FutureDateErrorMessage);
            resultToReturn.BannerErrorMessages.Add(questionPage.FutureDateErrorBannerLinkText);
        }

        return resultToReturn;
    }
}