using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class AdditionalRequirementQuestionModel
{
    public string Question { get; init; } = string.Empty;

    public string HintText { get; init; } = string.Empty;

    public string DetailsHeading { get; init; } = string.Empty;

    public string DetailsContent { get; init; } = string.Empty;

    public List<OptionModel> Options { get; init; } = [];

    public bool HasError { get; set; }
}