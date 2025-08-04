namespace Dfe.EarlyYearsQualification.Content.Entities;

public class FeedbackFormQuestionRadioAndInput : BaseFeedbackFormQuestion
{
    public List<IOptionItem> Options { get; set; } = [];

    public string InputHeading { get; set; } = string.Empty;

    public string InputHeadingHintText { get; set; } = string.Empty;

    public string ErrorMessageForInput { get; set; } = string.Empty;
}