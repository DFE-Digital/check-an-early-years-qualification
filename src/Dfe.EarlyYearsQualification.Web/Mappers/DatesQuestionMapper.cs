using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class DatesQuestionMapper
{
    public static DatesQuestionModel Map(DatesQuestionModel model, DatesQuestionPage question,
                                         string actionName,
                                         string controllerName, DateQuestionModel startedQuestion, DateQuestionModel awardedQuestion)
    {
        model.Question = question.Question;
        model.CtaButtonText = question.CtaButtonText;
        model.ActionName = actionName;
        model.ControllerName = controllerName;
        model.BackButton = NavigationLinkMapper.Map(question.BackButton);

        model.StartedQuestion = startedQuestion;
        model.AwardedQuestion = awardedQuestion;
        model.StartedQuestion.QuestionId = "date-started";
        model.StartedQuestion.MonthId = "date-started-month";
        model.StartedQuestion.YearId = "date-started-year";
        model.StartedQuestion.ErrorSummaryLink = new ErrorSummaryLink
                                                 {
                                                     ElementLinkId = model.StartedQuestion.MonthError ? model.StartedQuestion.MonthId : model.StartedQuestion.YearId
                                                 };
        model.AwardedQuestion.QuestionId = "date-awarded";
        model.AwardedQuestion.MonthId = "date-awarded-month";
        model.AwardedQuestion.YearId = "date-awarded-year";
        model.AwardedQuestion.ErrorSummaryLink = new ErrorSummaryLink
                                                 {
                                                     ElementLinkId = model.AwardedQuestion.MonthError ? model.AwardedQuestion.MonthId : model.AwardedQuestion.YearId
                                                 };
        var errorLinks = new List<ErrorSummaryLink>();

        if (model.StartedQuestion.MonthError || model.StartedQuestion.YearError)
        {
            errorLinks.Add(model.StartedQuestion.ErrorSummaryLink);
        }

        if (model.AwardedQuestion.MonthError || model.AwardedQuestion.YearError)
        {
            errorLinks.Add(model.AwardedQuestion.ErrorSummaryLink);
        }

        model.Errors = new ErrorSummaryModel
                       {
                           ErrorBannerHeading = question.ErrorBannerHeading,
                           ErrorSummaryLinks = errorLinks
                       };

        return model;
    }
}