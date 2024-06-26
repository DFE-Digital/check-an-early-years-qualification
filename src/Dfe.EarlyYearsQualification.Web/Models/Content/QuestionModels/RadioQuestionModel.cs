using System.ComponentModel.DataAnnotations;
using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

public class RadioQuestionModel : BaseQuestionModel
{
    public List<OptionModel> Options { get; set; } = [];

    [Required] public string? Option { get; init; } = string.Empty;

    public string AdditionalInformationHeader {get; set;} = string.Empty;

    public string AdditionalInformationBody {get; set;} = string.Empty;
}

public class OptionModel
{
    public string Label { get; init; } = string.Empty;

    public string Value { get; init; } = string.Empty;
}