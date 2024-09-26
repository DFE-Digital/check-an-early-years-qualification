using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

public class DateQuestionModelValidator(IDateTimeAdapter dateTimeAdapter) : IDateQuestionModelValidator
{
    public ValidationResult IsValid(DateQuestionModel model, DateQuestionPage questionPage)
    {
        model.MonthError = false;
        model.YearError = false;
        
        if (model is { SelectedYear: null, SelectedMonth: null })
        {
            model.MonthError = true;
            model.YearError = true;
            
            return new ValidationResult
                   {
                       IsValid = false, 
                       ErrorMessage = questionPage.ErrorMessage, 
                       BannerErrorMessage = questionPage.ErrorBannerLinkText
                   };
        }

        if (model.SelectedMonth == null)
        {
            model.MonthError = true;
            
            return new ValidationResult
                   {
                       IsValid = false, 
                       ErrorMessage = questionPage.MissingMonthErrorMessage,
                       BannerErrorMessage = questionPage.MissingMonthBannerLinkText
                   };
        }
        
        if (model.SelectedYear == null)
        {
            model.YearError = true;
            
            return new ValidationResult
                   {
                       IsValid = false, 
                       ErrorMessage = questionPage.MissingYearErrorMessage,
                       BannerErrorMessage = questionPage.MissingYearBannerLinkText
                   };
        }
        
        if (model.SelectedMonth is <= 0 or > 12)
        {
            model.MonthError = true;
            
            return new ValidationResult
                   {
                       IsValid = false, 
                       ErrorMessage = questionPage.IncorrectMonthFormatErrorMessage,
                       BannerErrorMessage = questionPage.IncorrectMonthFormatErrorBannerLinkText
                   };
        }
        
        var now = dateTimeAdapter.Now();

        if (model.SelectedYear < 1900 || model.SelectedYear > now.Year)
        {
            model.YearError = true;
            
            return new ValidationResult
                   {
                       IsValid = false, 
                       ErrorMessage = questionPage.IncorrectYearFormatErrorMessage,
                       BannerErrorMessage = questionPage.IncorrectYearFormatErrorBannerLinkText
                   };
        }
        
        var selectedDate = new DateOnly(model.SelectedYear!.Value, model.SelectedMonth!.Value, 1);

        var isValid = selectedDate <= DateOnly.FromDateTime(now);
        
        model.MonthError = !isValid;
        model.YearError = !isValid;
        
        return new ValidationResult
               {
                   IsValid = isValid,
                   ErrorMessage = questionPage.FutureDateErrorMessage,
                   BannerErrorMessage = questionPage.FutureDateErrorBannerLinkText
               };
    }
}