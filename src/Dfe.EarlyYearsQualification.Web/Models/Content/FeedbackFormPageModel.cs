namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class FeedbackFormPageModel
{
    public required string Heading { get; init; }

    public string PostHeadingContent { get; init; } = string.Empty;

    public List<IFeedbackFormQuestionModel> Questions { get; init; } = [];

    public NavigationLinkModel? BackButton { get; init; }

    public required string CtaButtonText { get; init; }

    public List<FeedbackFormQuestionListModel> QuestionList { get; set; } = [];

    public string SubmittedFrom { get; init; } = string.Empty;
}