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

    public NavigationLink? BackToConfirmAnswers { get; init; }

    public NavigationLink? BackToLevelSixAdviceBefore2014 { get; init; }

    public NavigationLink? BackToLevelSixAdvice { get; init; }

    public string RatiosHeading { get; init; } = string.Empty;

    public Document? RatiosText { get; init; }

    public string RequirementsHeading { get; init; } = string.Empty;

    public Document? RequirementsText { get; init; }

    public NavigationLink? CheckAnotherQualificationLink { get; init; }

    public string PrintButtonText { get; init; } = string.Empty;

    public string QualificationDetailsSummaryHeader { get; init; } = string.Empty;

    public string QualificationNameLabel { get; init; } = string.Empty;

    public string QualificationStartDateLabel { get; init; } = string.Empty;
    public string QualificationAwardedDateLabel { get; init; } = string.Empty;

    public Document? RatiosTextNotFullAndRelevant { get; init; }

    public Document? RatiosTextL3PlusNotFrBetweenSep14Aug19 { get; init; }

    public FeedbackBanner? FeedbackBanner { get; init; }

    public string QualificationResultHeading { get; init; } = string.Empty;

    public string QualificationResultFrMessageHeading { get; init; } = string.Empty;

    public string QualificationResultFrMessageBody { get; init; } = string.Empty;

    public string QualificationResultNotFrMessageHeading { get; init; } = string.Empty;

    public string QualificationResultNotFrMessageBody { get; init; } = string.Empty;

    public string QualificationResultNotFrL3MessageHeading { get; init; } = string.Empty;

    public string QualificationResultNotFrL3MessageBody { get; init; } = string.Empty;
}