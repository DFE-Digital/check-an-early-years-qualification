using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

public class DateQuestionModelValidator(IDateTimeAdapter dateTimeAdapter) : IDateQuestionModelValidator
{
    public ValidationResult IsValid(DateQuestionModel model, DateQuestionPage questionPage)
    {
        if (model is { SelectedYear: 0, SelectedMonth: 0 })
        {
            return new ValidationResult
                   {
                       IsValid = false, 
                       ErrorMessage = questionPage.ErrorMessage, 
                       BannerErrorMessage = questionPage.ErrorBannerLinkText
                   };
        }
        
        if (model.SelectedYear < 1900
            || model.SelectedMonth < 1
            || model.SelectedMonth > 12)
        {
            return new ValidationResult
                   {
                       IsValid = false, 
                       ErrorMessage = questionPage.IncorrectFormatErrorMessage, 
                       BannerErrorMessage = questionPage.IncorrectFormatErrorBannerLinkText
                   };
        }

        var selectedDate = new DateOnly(model.SelectedYear, model.SelectedMonth, 1);

        var now = dateTimeAdapter.Now();

        return new ValidationResult
               {
                   IsValid = selectedDate <= DateOnly.FromDateTime(now),
                   ErrorMessage = questionPage.FutureDateErrorMessage,
                   BannerErrorMessage = questionPage.FutureDateErrorBannerLinkText
               };
    }
}