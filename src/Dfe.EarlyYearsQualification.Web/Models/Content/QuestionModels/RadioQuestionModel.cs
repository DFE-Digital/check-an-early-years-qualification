using System.ComponentModel.DataAnnotations;
using Dfe.EarlyYearsQualification.Web.Attributes;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

public class RadioQuestionModel : BaseQuestionModel
{
    public List<IOptionItemModel> OptionsItems { get; set; } = [];

    public bool HasErrors { get; set; }

    [Required]
    [IncludeInTelemetry]
    public string Option { get; set; } = string.Empty;
}