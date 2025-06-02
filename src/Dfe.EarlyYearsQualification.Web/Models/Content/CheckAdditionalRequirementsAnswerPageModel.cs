namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class CheckAdditionalRequirementsAnswerPageModel
{
    public string PageHeading { get; init; } = string.Empty;

    public string AnswerDisclaimerText { get; init; } = string.Empty;

    public string ChangeAnswerText { get; init; } = string.Empty;

    public string ButtonText { get; init; } = string.Empty;

    public NavigationLinkModel? BackButton { get; init; }

    public Dictionary<string, string>? Answers { get; init; }

    public string ChangeQuestionHref { get; init; } = string.Empty;

    public string GetResultsHref { get; init; } = string.Empty;
}