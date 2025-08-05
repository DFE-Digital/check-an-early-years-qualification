namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class FeedbackFormPageModel
{
    public required string Heading { get; init; }

    public string PostHeadingContent { get; init; } = string.Empty;

    public List<IFeedbackFormQuestionModel> Questions { get; set; } = [];

    public NavigationLinkModel? BackButton { get; init; }

    public required string CtaButtonText { get; init; }

    public required string ErrorBannerHeading { get; init; }

    public string[] Answers { get; set; } = [];
    
    public string[] ConditionalInputAnswers { get; set; } = [];
}