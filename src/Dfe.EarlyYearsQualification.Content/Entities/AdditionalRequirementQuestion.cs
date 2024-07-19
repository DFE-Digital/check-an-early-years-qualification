using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class AdditionalRequirementQuestion
{
    public string Question { get; set; } = string.Empty;

    public string HintText { get; set; } = string.Empty;

    public string DetailsHeading { get; set; } = string.Empty;

    public Document? DetailsContent { get; set; }

    public string ConfirmationStatement { get; set; } = string.Empty;

    public bool AnswerToBeFullAndRelevant { get; set; }
}