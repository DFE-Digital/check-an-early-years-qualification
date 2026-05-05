using Dfe.EarlyYearsQualification.Web.Constants;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class FeedbackFormPageModel
{
    public required string? Heading { get; init; }

    public string PostHeadingContent { get; init; } = string.Empty;

    public List<IFeedbackFormQuestionModel> Questions { get; init; } = [];

    public NavigationLinkModel? BackButton { get; init; }

    public required string CtaButtonText { get; init; }

    public List<FeedbackFormQuestionListModel> QuestionList { get; set; } = [];

    public string PageSubmittedOn { get; init; } = string.Empty;

    public bool IsFeedbackPageEmbeded => PageSubmittedOn == PageNames.HelpConfirmation;

    public string HeaderClass => IsFeedbackPageEmbeded ? "govuk-heading-m" : "govuk-heading-l";

    public string QuestionClass => IsFeedbackPageEmbeded ? "govuk-heading-s" : "govuk-heading-m";
}