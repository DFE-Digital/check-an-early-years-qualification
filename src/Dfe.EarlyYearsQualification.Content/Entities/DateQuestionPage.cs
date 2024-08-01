using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class DateQuestionPage
{
    public string Question { get; init; } = string.Empty;

    public string CtaButtonText { get; init; } = string.Empty;

    public string ErrorMessage { get; init; } = string.Empty;

    public string QuestionHint { get; init; } = string.Empty;

    public string MonthLabel { get; init; } = string.Empty;

    public string YearLabel { get; init; } = string.Empty;

    public NavigationLink? BackButton { get; init; }
    
    public string ErrorBannerHeading { get; init; } = string.Empty;

    public string ErrorBannerLinkText { get; init; } = string.Empty;
    
    public string AdditionalInformationHeader { get; init; } = string.Empty;

    public Document? AdditionalInformationBody { get; init; }
}