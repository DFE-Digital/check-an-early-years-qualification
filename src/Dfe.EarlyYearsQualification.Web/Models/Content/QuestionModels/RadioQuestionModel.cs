using System.ComponentModel.DataAnnotations;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

public class RadioQuestionModel : BaseQuestionModel
{
    public List<OptionModel> Options { get; set; } = [];

    [Required] public string? Option { get; init; } = string.Empty;

    public string AdditionalInformationHeader { get; set; } = string.Empty;

    public string AdditionalInformationBody { get; set; } = string.Empty;
}