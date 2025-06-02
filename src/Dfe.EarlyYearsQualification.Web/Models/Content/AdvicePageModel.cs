namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class AdvicePageModel
{
    public string Heading { get; init; } = string.Empty;

    public string BodyContent { get; init; } = string.Empty;

    public NavigationLinkModel? BackButton { get; init; }

    public FeedbackBannerModel? FeedbackBanner { get; init; }
    public UpDownFeedbackModel? UpDownFeedback { get; init; }
}