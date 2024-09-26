using System.ComponentModel.DataAnnotations;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

public class DateQuestionModel : BaseQuestionModel
{
    public string QuestionHint { get; set; } = string.Empty;

    public string MonthLabel { get; set; } = string.Empty;

    public string YearLabel { get; set; } = string.Empty;
    
    public bool MonthError { get; set; }
    
    public bool YearError { get; set; }

    [Required] public int? SelectedMonth { get; set; }

    [Required] public int? SelectedYear { get; set; }
}