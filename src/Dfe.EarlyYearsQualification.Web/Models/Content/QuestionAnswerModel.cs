namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QuestionAnswerModel
{
    public string Question { get; init; } = string.Empty;

    public string[] Answer { get; init; } = [];

    public string ChangeAnswerHref { get; init; } = string.Empty;
}