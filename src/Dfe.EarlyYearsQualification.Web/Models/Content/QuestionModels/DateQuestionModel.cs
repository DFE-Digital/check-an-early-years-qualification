namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class DateQuestionModel : BaseQuestionModel
{
  public string QustionHint { get; set; } = string.Empty;

  public string MonthLabel { get; set; } = string.Empty;

  public string YearLabel { get; set; } = string.Empty;
}