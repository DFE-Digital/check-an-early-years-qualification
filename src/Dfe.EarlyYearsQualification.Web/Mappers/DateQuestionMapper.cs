using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class DateQuestionMapper
{
    public static DateQuestionModel Map(DateQuestionModel model, DateQuestion question,
                                        string errorBannerLinkText,
                                        string errorMessage,
                                        DateValidationResult? validationResult,
                                        int? selectedMonth,
                                        int? selectedYear)
    {
        model.QuestionHeader = question.QuestionHintHeader;
        model.QuestionHint = question.QuestionHint;
        model.MonthLabel = question.MonthLabel;
        model.YearLabel = question.YearLabel;
    
        model.MonthError = !validationResult?.MonthValid ?? false;
        model.YearError = !validationResult?.YearValid ?? false;
        model.ErrorMessage = errorMessage;
        model.ErrorSummaryLink = new ErrorSummaryLink
                                 {
                                     ErrorBannerLinkText = errorBannerLinkText
                                 };

        if (selectedMonth.HasValue && selectedYear.HasValue)
        {
            model.SelectedMonth = selectedMonth.Value;
            model.SelectedYear = selectedYear.Value;
        }

        return model;
    }
}