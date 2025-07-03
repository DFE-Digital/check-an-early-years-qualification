using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class FeedbackComponent
{
    public string Header { get; init; } = string.Empty;

    public Document? Body { get; init; }
}