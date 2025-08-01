namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public abstract class BaseFeedbackFormQuestionModel : IFeedbackFormQuestionModel
{
    public string Question { get; set; } = string.Empty;

    public bool IsTheQuestionMandatory { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;
}