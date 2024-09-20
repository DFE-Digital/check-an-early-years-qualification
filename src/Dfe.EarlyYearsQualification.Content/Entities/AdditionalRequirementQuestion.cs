using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class AdditionalRequirementQuestion
{
    public string Question { get; init; } = string.Empty;

    public string HintText { get; init; } = string.Empty;

    public string DetailsHeading { get; init; } = string.Empty;

    public Document? DetailsContent { get; init; }

    public string ConfirmationStatement { get; init; } = string.Empty;

    public bool AnswerToBeFullAndRelevant { get; init; }

    public List<Option> Answers { get; init; } = [];
}