namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class DetailsPageModel
{
    public string MainHeader { get; init; } = string.Empty;

    public string AwardingOrgLabel { get; init; } = string.Empty;

    public string LevelLabel { get; init; } = string.Empty;

    public string DateOfCheckLabel { get; init; } = string.Empty;

    public string RatiosHeading { get; init; } = string.Empty;

    public string RatiosText { get; set; } = string.Empty;
    public string RatiosAdditionalInfoText { get; set; } = string.Empty;

    public string RequirementsHeading { get; init; } = string.Empty;

    public string RequirementsText { get; init; } = string.Empty;

    public NavigationLinkModel? CheckAnotherQualificationLink { get; init; }

    public string PrintButtonText { get; init; } = string.Empty;

    public string QualificationDetailsSummaryHeader { get; init; } = string.Empty;

    public string QualificationNameLabel { get; init; } = string.Empty;

    public string QualificationStartDateLabel { get; init; } = string.Empty;
    public string QualificationAwardedDateLabel { get; init; } = string.Empty;

    public FeedbackBannerModel? FeedbackBanner { get; init; }

    public string QualificationResultHeading { get; set; } = string.Empty;

    public string QualificationResultMessageHeading { get; set; } = string.Empty;

    public string QualificationResultMessageBody { get; set; } = string.Empty;
}