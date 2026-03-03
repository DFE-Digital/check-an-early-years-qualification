using Dfe.EarlyYearsQualification.Web.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

public class RadioQuestionModel : BaseQuestionModel
{
    public List<IOptionItemModel> OptionsItems { get; set; } = [];

    public bool HasErrors { get; set; }

    public bool HasNestedErrors { get; set; }

    public string PostHeadingContent { get; set; } = string.Empty;

    public string? WarningText { get; set; } = string.Empty;

    public string? PostRadioButtonContent { get; set; } = string.Empty;

    public ErrorSummaryModel? ErrorSummaryModel { get; set; }

    [Required]
    [IncludeInTelemetry]
    public string Option { get; set; } = string.Empty;
}