using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class FeedbackFormQuestionTextAreaModel : BaseFeedbackFormQuestionModel
{
    public string HintText { get; set; } = string.Empty;
}