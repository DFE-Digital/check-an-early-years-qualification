namespace Dfe.EarlyYearsQualification.Content.Entities;

public abstract class BaseFeedbackFormQuestion : IFeedbackFormQuestion
{
    public string Question { get; set; } = string.Empty;

    public bool IsTheQuestionMandatory { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;
}