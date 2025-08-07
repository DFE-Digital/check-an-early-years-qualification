using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Services.FeedbackForm;

public interface IFeedbackFormService
{
    ErrorSummaryModel ValidateQuestions(FeedbackFormPage feedbackFormPage, FeedbackFormPageModel model);
}