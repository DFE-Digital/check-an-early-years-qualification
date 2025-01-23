using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class OpenGraphData
{
    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string Domain { get; init; } = string.Empty;

    public Asset? Image { get; init; }
}