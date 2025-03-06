using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class UpDownFeedback
{
    public string Question { get; init; } = string.Empty;
    public string YesButtonText { get; init; } = string.Empty;
    public string YesButtonSubText { get; init; } = string.Empty;
    public string NoButtonText { get; init; } = string.Empty;
    public string NoButtonSubText { get; init; } = string.Empty;
    public string RaPButtonText { get; init; } = string.Empty;
    public string CancelButtonText { get; init; } = string.Empty;
    public Document? ImproveServiceContent { get; init; }
    public string UsefulResponse { get; init; } = string.Empty;
}