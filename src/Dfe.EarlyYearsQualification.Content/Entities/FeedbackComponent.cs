using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class FeedbackComponent
{
    public string Header { get; set; } = string.Empty;

    public Document? Body { get; set; }
}