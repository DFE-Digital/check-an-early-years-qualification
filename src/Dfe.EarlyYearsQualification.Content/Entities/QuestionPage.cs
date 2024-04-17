namespace Dfe.EarlyYearsQualification.Content.Entities;

public class QuestionPage
{
  public string Question { get; init; } = string.Empty;

  public List<Option> Options { get; init; } = new List<Option>();

  public string CtaButtonText { get; init; } = string.Empty;
}