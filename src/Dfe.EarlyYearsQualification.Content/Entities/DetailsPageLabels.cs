using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class DetailsPageLabels
{
    public string MainHeader { get; init; } = string.Empty;

    public string AwardingOrgLabel { get; init; } = string.Empty;

    public string LevelLabel { get; init; } = string.Empty;

    public string DateOfCheckLabel { get; init; } = string.Empty;

    public NavigationLink? BackButton { get; init; }

    public NavigationLink? BackToConfirmAnswers { get; init; }

    public string QualificationDetailsSummaryHeader { get; init; } = string.Empty;

    public string QualificationNameLabel { get; init; } = string.Empty;

    public string QualificationNumberLabel { get; init; } = string.Empty;

    public string QualificationStartDateLabel { get; init; } = string.Empty;
    
    public string QualificationAwardedDateLabel { get; init; } = string.Empty;

    public string RatiosHeading { get; init; } = string.Empty;

    public Document? RatiosTextNotFullAndRelevant { get; init; }

    public Document? RatiosTextL3PlusNotFrBetweenSep14Aug19 { get; init; }

    public Document? RatiosTextL3Ebr { get; init; }

    public NavigationLink? CheckAnotherQualificationLink { get; init; }

    public string PrintButtonText { get; init; } = string.Empty;

    public string PrintInformationHeading { get; init; } = string.Empty;

    public Document? PrintInformationBody { get; init; }

    public FeedbackBanner? FeedbackBanner { get; init; }
    
    public string QualificationResultHeading { get; init; } = string.Empty;

    public string QualificationResultFrMessageHeading { get; init; } = string.Empty;

    public string QualificationResultFrMessageBody { get; init; } = string.Empty;

    public string QualificationResultNotFrMessageHeading { get; init; } = string.Empty;

    public string QualificationResultNotFrMessageBody { get; init; } = string.Empty;

    public string QualificationResultNotFrL3MessageHeading { get; init; } = string.Empty;

    public string QualificationResultNotFrL3MessageBody { get; init; } = string.Empty;

    public string QualificationResultNotFrL3OrL6MessageHeading { get; init; } = string.Empty;

    public string QualificationResultNotFrL3OrL6MessageBody { get; init; } = string.Empty;

    public UpDownFeedback? UpDownFeedback { get; init; }
}