namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class AdditionalRequirementAnswerModel
{
    public string Question { get; init; } = string.Empty;

    public string? Answer { get; set; } = string.Empty;

    public bool AnswerToBeFullAndRelevant { get; init; }

    public string? ConfirmationStatement { get; init; } = string.Empty;
}