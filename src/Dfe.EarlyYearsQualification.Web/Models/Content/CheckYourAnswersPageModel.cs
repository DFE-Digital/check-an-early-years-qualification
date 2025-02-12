namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class CheckYourAnswersPageModel
{
    public string PageHeading { get; init; } = string.Empty;
    
    public string ChangeAnswerText { get; init; } = string.Empty;

    public string CtaButtonText { get; init; } = string.Empty;

    public List<QuestionAnswerModel> QuestionAnswerModels { get; init; } = [];
    
    public NavigationLinkModel? BackButton { get; init; }
}

public class QuestionAnswerModel
{
    public string Question { get; init; } = string.Empty;

    public string[] Answer { get; init; } = [];

    public string ChangeAnswerHref { get; init; } = string.Empty;
}