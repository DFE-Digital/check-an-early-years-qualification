using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Services.FeedbackForm;

public interface IFeedbackFormService
{
    ErrorSummaryModel ValidateQuestions(FeedbackFormPage feedbackFormPage, FeedbackFormPageModel model);
    
    string ConvertQuestionListToString(FeedbackFormPageModel model);
    void SetDefaultAnswers(FeedbackFormPage feedbackFormPage, FeedbackFormPageModel model);
}