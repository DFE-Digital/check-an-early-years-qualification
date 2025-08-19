namespace Dfe.EarlyYearsQualification.Content.Entities;

public class FeedbackFormQuestionRadio : BaseFeedbackFormQuestion
{
    public List<IOptionItem> Options { get; init; } = [];
}