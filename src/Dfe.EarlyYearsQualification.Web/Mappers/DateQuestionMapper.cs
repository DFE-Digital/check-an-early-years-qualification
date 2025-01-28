using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class DateQuestionMapper
{
    public static DateQuestionModel Map(DateQuestionModel model, DateQuestionPage question,
                                        string actionName,
                                        string controllerName,
                                        string errorBannerLinkText,
                                        string errorMessage,
                                        string additionalInformationBody,
                                        string postHeaderContent,
                                        DateValidationResult? validationResult,
                                        int? selectedMonth,
                                        int? selectedYear)
    {
        model.Question = question.Question;
        model.CtaButtonText = question.CtaButtonText;
        model.ActionName = actionName;
        model.ControllerName = controllerName;
        model.QuestionHintHeader = question.QuestionHintHeader;
        model.QuestionHint = question.QuestionHint;
        model.MonthLabel = question.MonthLabel;
        model.YearLabel = question.YearLabel;
        model.BackButton = NavigationLinkMapper.Map(question.BackButton);
        model.ErrorBannerHeading = question.ErrorBannerHeading;
        model.ErrorBannerLinkText = errorBannerLinkText;
        model.ErrorMessage = errorMessage;
        model.AdditionalInformationHeader = question.AdditionalInformationHeader;
        model.AdditionalInformationBody = additionalInformationBody;
        model.PostHeaderContent = postHeaderContent;
        model.MonthError = !validationResult?.MonthValid ?? false;
        model.YearError = !validationResult?.YearValid ?? false;

        // ReSharper disable once InvertIf
        if (selectedMonth.HasValue && selectedYear.HasValue)
        {
            model.SelectedMonth = selectedMonth.Value;
            model.SelectedYear = selectedYear.Value;
        }

        return model;
    }
}