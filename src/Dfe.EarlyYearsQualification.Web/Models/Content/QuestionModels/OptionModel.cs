namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

public class OptionModel : IOptionItemModel
{
    public string Label { get; init; } = string.Empty;

    public string Value { get; init; } = string.Empty;

    public string Hint { get; init; } = string.Empty;
}