namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class UpDownFeedbackModel
{
    public string Question { get; init; } = string.Empty;
    public string YesButtonText { get; init; } = string.Empty;
    public string YesButtonSubText { get; init; } = string.Empty;
    public string NoButtonText { get; init; } = string.Empty;
    public string NoButtonSubText { get; init; } = string.Empty;
    public string HelpButtonText { get; init; } = string.Empty;
    public string HelpButtonLink { get; init; } = string.Empty;
    public string CancelButtonText { get; init; } = string.Empty;
    public string FeedbackHeader { get; set; } = string.Empty;
    public string FeedbackBody { get; set; } = string.Empty;
}