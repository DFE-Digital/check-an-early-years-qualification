namespace Dfe.EarlyYearsQualification.Content.Entities;

public class Option : IOptionItem
{
    public string Label { get; init; } = string.Empty;

    public string Value { get; init; } = string.Empty;

    public string Hint { get; init; } = string.Empty;
}