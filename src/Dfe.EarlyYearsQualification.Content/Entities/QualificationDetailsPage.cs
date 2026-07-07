using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class QualificationDetailsPage
{
    public string Name { get; init; } = string.Empty;

    public DetailsPageLabels Labels { get; init; } = new DetailsPageLabels();

    public bool IsPractitionerSpecificPage { get; init; }
    
    public bool IsAutomaticallyApprovedAtLevel6 { get; init; }

    public bool IsFullAndRelevant { get; init; }

    public bool IsDegreeSpecificPage { get; init; }

    public string? Level { get; init; }

    public string? FromWhichYear { get; init; }
    
    public string? AwardedAfterWhichYear { get; init; }

    public string? ToWhichYear { get; init; }

    public string RequirementsHeading { get; init; } = string.Empty;

    public Document? RequirementsText { get; init; }
    
    public Document? UnqualifiedRatioRequirements { get; init; }

    public Document? Level2RatioRequirements { get; set; }
    
    public Document? Level3RatioRequirements { get; set; }
    
    public Document? Level6RatioRequirements { get; set; }
}