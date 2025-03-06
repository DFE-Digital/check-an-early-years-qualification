using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class UpDownFeedback
{
    public string Question { get; set; }
    public string YesButtonText { get; set; }
    public string YesButtonSubText { get; set; }
    public string NoButtonText { get; set; }
    public string NoButtonSubText { get; set; }
    public string RaPButtonText { get; set; }
    public string CancelButtonText { get; set; }
    public Document? ImproveServiceContent { get; set; }
    public string UsefulResponse { get; set; }
}