using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class UpDownFeedback
{
    public string Question { get; init; } = string.Empty;
    public string YesButtonText { get; init; } = string.Empty;
    public string YesButtonSubText { get; init; } = string.Empty;
    public string NoButtonText { get; init; } = string.Empty;
    public string NoButtonSubText { get; init; } = string.Empty;
    public string HelpButtonText { get; init; } = string.Empty;
    public string HelpButtonLink { get; init; } = string.Empty;
    public string CancelButtonText { get; init; } = string.Empty;

    public FeedbackComponent? FeedbackComponent { get; set; }
}