using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class RadioQuestionPage
{
    public string Question { get; init; } = string.Empty;

    public List<Option> Options { get; init; } = [];

    public string CtaButtonText { get; init; } = string.Empty;

    public string ErrorMessage { get; init; } = string.Empty;

    public string AdditionalInformationHeader { get; init; } = string.Empty;

    public Document? AdditionalInformationBody { get; init; }
    
    public NavigationLink? BackButton { get; init; }
}