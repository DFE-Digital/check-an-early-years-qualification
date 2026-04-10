namespace Dfe.EarlyYearsQualification.Content.Entities;

public class RadioButtonAndDateInput : IOptionItem
{
    public string Label { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public string Hint { get; set; } = string.Empty;

    public DateQuestion StartedQuestion { get; set; } = new DateQuestion();
}