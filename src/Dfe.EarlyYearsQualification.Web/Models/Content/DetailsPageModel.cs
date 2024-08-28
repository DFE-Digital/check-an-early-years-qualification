namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class DetailsPageModel
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

    public string CheckAnotherQualificationText { get; init; } = string.Empty;
    
    public string RatiosHeading { get; init; } = string.Empty;
    
    public string RatiosText { get; init; } = string.Empty;
    
    public string RequirementsHeading { get; init; } = string.Empty;
    
    public string RequirementsText { get; init; } = string.Empty;

    public string FurtherInfoHeading { get; init; } = string.Empty;

    public string FurtherInfoText { get; init; } = string.Empty;

    public NavigationLinkModel? CheckAnotherQualificationLink { get; init; }

    public string PrintButtonText { get; init; } = string.Empty;
    
    public string QualificationDetailsSummaryHeader { get; init; } = string.Empty;

    public string QualificationNameLabel { get; init; } = string.Empty;

    public string QualificationStartDateLabel { get; init; } = string.Empty;
}