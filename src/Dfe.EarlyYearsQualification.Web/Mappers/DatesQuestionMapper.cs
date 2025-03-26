using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class DatesQuestionMapper
{
    public static DatesQuestionModel Map(DatesQuestionModel model, DatesQuestionPage question,
                                         string actionName,
                                         string controllerName, DateQuestionModel? startedQuestion,
                                         DateQuestionModel? awardedQuestion)
    {
        model.Question = question.Question;
        model.CtaButtonText = question.CtaButtonText;
        model.ActionName = actionName;
        model.ControllerName = controllerName;
        model.BackButton = NavigationLinkMapper.Map(question.BackButton);

        if (startedQuestion != null)
        {
            model.StartedQuestion = startedQuestion;
            model.StartedQuestion.Prefix = "started";
            model.StartedQuestion.QuestionId = "date-started";
            model.StartedQuestion.MonthId = "StartedQuestion.SelectedMonth";
            model.StartedQuestion.YearId = "StartedQuestion.SelectedYear";

            foreach (var errorSummaryLink in model.StartedQuestion.ErrorSummaryLinks)
            {
                if (errorSummaryLink.ElementLinkId == FieldId.Month.ToString())
                {
                    errorSummaryLink.ElementLinkId = model.StartedQuestion.MonthId;
                }

                else if (errorSummaryLink.ElementLinkId == FieldId.Year.ToString())
                {
                    errorSummaryLink.ElementLinkId = model.StartedQuestion.YearId;
                }
            }
        }

        if (awardedQuestion != null)
        {
            model.AwardedQuestion = awardedQuestion;
            model.AwardedQuestion.Prefix = "awarded";
            model.AwardedQuestion.QuestionId = "date-awarded";
            model.AwardedQuestion.MonthId = "AwardedQuestion.SelectedMonth";
            model.AwardedQuestion.YearId = "AwardedQuestion.SelectedYear";
            foreach (var errorSummaryLink in model.AwardedQuestion.ErrorSummaryLinks)
            {
                if (errorSummaryLink.ElementLinkId == FieldId.Month.ToString())
                {
                    errorSummaryLink.ElementLinkId = model.AwardedQuestion.MonthId;
                }

                else if (errorSummaryLink.ElementLinkId == FieldId.Year.ToString())
                {
                    errorSummaryLink.ElementLinkId = model.AwardedQuestion.YearId;
                }
            }
        }

        var errorLinks = new List<ErrorSummaryLink>();

        if (model.StartedQuestion is not null &&
            (model.StartedQuestion.MonthError || model.StartedQuestion.YearError) &&
            model.StartedQuestion.ErrorSummaryLinks is not null)
        {
            errorLinks.AddRange(model.StartedQuestion.ErrorSummaryLinks);
        }

        if (model.AwardedQuestion is not null &&
            (model.AwardedQuestion.MonthError || model.AwardedQuestion.YearError) &&
            model.AwardedQuestion.ErrorSummaryLinks is not null)
        {
            errorLinks.AddRange(model.AwardedQuestion.ErrorSummaryLinks);
        }

        model.Errors = new ErrorSummaryModel
                       {
                           ErrorBannerHeading = question.ErrorBannerHeading,
                           ErrorSummaryLinks = errorLinks
                       };

        return model;
    }
}