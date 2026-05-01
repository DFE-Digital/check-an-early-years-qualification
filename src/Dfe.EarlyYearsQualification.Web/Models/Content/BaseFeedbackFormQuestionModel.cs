namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public abstract class BaseFeedbackFormQuestionModel : IFeedbackFormQuestionModel
{
    public string Question { get; init; } = string.Empty;
}