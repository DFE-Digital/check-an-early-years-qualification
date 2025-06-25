using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class DatesQuestionMapper
{
    public static DatesQuestionModel Map(DatesQuestionModel model, DatesQuestionPage question,
                                         string actionName,
                                         string controllerName, DateQuestionModel startedQuestion,
                                         DateQuestionModel awardedQuestion)
    {
        var errorLinks = new List<ErrorSummaryLink>();
        model.Question = question.Question;
        model.CtaButtonText = question.CtaButtonText;
        model.ActionName = actionName;
        model.ControllerName = controllerName;
        model.BackButton = NavigationLinkMapper.Map(question.BackButton);

        var (startedQuestionMapped, startedQuestionErrors) = MapDate(startedQuestion, "started", nameof(model.StartedQuestion));
        model.StartedQuestion = startedQuestionMapped;
        errorLinks.AddRange(startedQuestionErrors);

        var (awardedQuestionMapped, awardedQuestionErrors) = MapDate(awardedQuestion, "awarded", nameof(model.AwardedQuestion));
        model.AwardedQuestion = awardedQuestionMapped;
        errorLinks.AddRange(awardedQuestionErrors);

        model.Errors = new ErrorSummaryModel
                       {
                           ErrorBannerHeading = question.ErrorBannerHeading,
                           ErrorSummaryLinks = errorLinks
                       };

        return model;
    }

    private static (DateQuestionModel, List<ErrorSummaryLink>) MapDate(DateQuestionModel dateQuestion, string prefix, string fieldName)
    {
        var errorLinks = new List<ErrorSummaryLink>();
        dateQuestion.Prefix = prefix;
        dateQuestion.QuestionId = $"date-{prefix}";
        dateQuestion.MonthId = $"{fieldName}.SelectedMonth";
        dateQuestion.YearId = $"{fieldName}.SelectedYear";

        foreach (var errorSummaryLink in dateQuestion.ErrorSummaryLinks)
        {
            errorSummaryLink.ElementLinkId = errorSummaryLink.ElementLinkId switch
                                             {
                                                 nameof(FieldId.Month) => dateQuestion.MonthId,
                                                 nameof(FieldId.Year) => dateQuestion.YearId,
                                                 _ => errorSummaryLink.ElementLinkId
                                             };
        }

        if (dateQuestion is not null &&
            (dateQuestion.MonthError || dateQuestion.YearError) &&
            dateQuestion.ErrorSummaryLinks is not null)
        {
            errorLinks.AddRange(dateQuestion.ErrorSummaryLinks);
        }

        return (dateQuestion!, errorLinks);
    }
}