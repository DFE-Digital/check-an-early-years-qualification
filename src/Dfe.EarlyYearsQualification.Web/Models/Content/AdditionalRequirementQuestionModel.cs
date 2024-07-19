namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class AdditionalRequirementQuestionModel
{
    public string Question { get; set; } = string.Empty;

    public string HintText { get; set; } = string.Empty;

    public string DetailsHeading { get; set; } = string.Empty;

    public string DetailsContent { get; set; } = string.Empty;

    public string ConfirmationStatement { get; set; } = string.Empty;

    public bool AnswerToBeFullAndRelevant { get; set; }
}