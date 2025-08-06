namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class FeedbackFormPageModel
{
    public required string Heading { get; init; }

    public string PostHeadingContent { get; init; } = string.Empty;

    public List<IFeedbackFormQuestionModel> Questions { get; init; } = [];

    public NavigationLinkModel? BackButton { get; init; }

    public ErrorSummaryModel? ErrorSummaryModel { get; set; }

    public required string CtaButtonText { get; init; }

    public required string ErrorBannerHeading { get; init; }

    public List<FeedbackFormQuestionListModel> QuestionList { get; set; } = [];

    public bool HasError { get; set; }
    
}