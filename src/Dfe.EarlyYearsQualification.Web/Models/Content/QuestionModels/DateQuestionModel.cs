using System.ComponentModel.DataAnnotations;
using Dfe.EarlyYearsQualification.Web.Attributes;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

public class DateQuestionModel : BaseDateQuestionModel
{
    [Required]
    [IncludeInTelemetry]
    public int? SelectedMonth { get; set; }

    [Required]
    [IncludeInTelemetry]
    public int? SelectedYear { get; set; }
}