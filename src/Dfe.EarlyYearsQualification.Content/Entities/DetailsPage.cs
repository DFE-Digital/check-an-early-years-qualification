using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class DetailsPage
{
    public string MainHeader { get; init; } = string.Empty;

    public string AwardingOrgLabel { get; init; } = string.Empty;

    public string LevelLabel { get; init; } = string.Empty;

    public string QualificationNumberLabel { get; init; } = string.Empty;

    public string DateAddedLabel { get; init; } = string.Empty;

    public string DateOfCheckLabel { get; init; } = string.Empty;

    public string BookmarkHeading { get; init; } = string.Empty;

    public string BookmarkText { get; init; } = string.Empty;

    public string CheckAnotherQualificationHeading { get; init; } = string.Empty;

    public Document? CheckAnotherQualificationText { get; init; }

    public string FurtherInfoHeading { get; init; } = string.Empty;

    public Document? FurtherInfoText { get; init; }
    
    public NavigationLink? BackButton { get; init; }
}