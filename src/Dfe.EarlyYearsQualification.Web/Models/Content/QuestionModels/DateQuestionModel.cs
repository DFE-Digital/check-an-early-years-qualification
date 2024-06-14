using System.ComponentModel.DataAnnotations;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class DateQuestionModel : BaseQuestionModel
{
  public string QuestionHint { get; set; } = string.Empty;

  public string MonthLabel { get; set; } = string.Empty;

  public string YearLabel { get; set; } = string.Empty;

  [Required] public int SelectedMonth { get; set; }

  [Required] public int SelectedYear { get; set; }

  public bool IsModelValid()
  {
    if (SelectedMonth < 1 || SelectedMonth > 12)
    {
      return false;
    }

    if (SelectedYear <= 1900 || SelectedYear > DateTime.UtcNow.Year)
    {
      return false;
    }

    return true;
  }
}