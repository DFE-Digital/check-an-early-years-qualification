namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class GiveFeedbackPageModel
{
    public required string Heading { get; init; }

    public string PostHeadingContent { get; init; } = string.Empty;

    public List<IFeedbackFormQuestionModel> Questions { get; set; } = [];

    public NavigationLinkModel? BackButton { get; init; }

    public required string CtaButtonText { get; init; }

    public required string ErrorBannerHeading { get; init; }

    public string[] RadioAnswers { get; set; } = [];
}