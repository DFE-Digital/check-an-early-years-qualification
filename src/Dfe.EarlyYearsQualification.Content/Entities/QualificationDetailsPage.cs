using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class QualificationDetailsPage
{
    public string Name { get; init; } = string.Empty;

    public DetailsPageLabels Labels { get; init; } = new DetailsPageLabels();

    public bool IsPractitionerSpecificPage { get; init; }

    public bool IsFullAndRelevant { get; init; }

    public string? Level { get; init; }

    public string? FromWhichYear { get; init; }

    public string? ToWhichYear { get; init; }

    public string RequirementsHeading { get; init; } = string.Empty;

    public Document? RequirementsText { get; init; }
}