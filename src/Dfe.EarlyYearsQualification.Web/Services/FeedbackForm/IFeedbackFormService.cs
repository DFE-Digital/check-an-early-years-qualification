using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Services.FeedbackForm;

public interface IFeedbackFormService
{
    string ConvertQuestionListToString(FeedbackFormPageModel model);
}