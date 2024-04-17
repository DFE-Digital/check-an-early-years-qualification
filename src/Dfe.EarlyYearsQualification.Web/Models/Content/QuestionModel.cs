namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QuestionModel
{
    public string Question { get; init; } = string.Empty;

    public List<OptionModel> Options { get; init; } = new List<OptionModel>();

    public string CtaButtonText { get; init; } = string.Empty;

    public string ActionName { get; init; } = string.Empty;

    public string ControllerName { get; init; } = string.Empty;
}

public class OptionModel
{
    public string Label { get; init; } = string.Empty;

    public string Value { get; init; } = string.Empty;
}