using System.ComponentModel.DataAnnotations;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

[CustomValidation(typeof(DateQuestionModelValidator), nameof(DateQuestionModelValidator.IsValid))]
public class DateQuestionModel : BaseQuestionModel
{
    public string QuestionHint { get; set; } = string.Empty;

    public string MonthLabel { get; set; } = string.Empty;

    public string YearLabel { get; set; } = string.Empty;

    [Required] public int SelectedMonth { get; init; }

    [Required] public int SelectedYear { get; init; }
}