using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class DateQuestionPage
{
    public string Question { get; init; } = string.Empty;

    public string CtaButtonText { get; init; } = string.Empty;
    
    public string QuestionHint { get; init; } = string.Empty;

    public string MonthLabel { get; init; } = string.Empty;

    public string YearLabel { get; init; } = string.Empty;

    public NavigationLink? BackButton { get; init; }
    
    public string AdditionalInformationHeader { get; init; } = string.Empty;

    public Document? AdditionalInformationBody { get; init; }
    
    public string ErrorBannerHeading { get; init; } = string.Empty;

    public string ErrorBannerLinkText { get; init; } = string.Empty;
    
    public string ErrorMessage { get; init; } = string.Empty;
    
    public string FutureDateErrorBannerLinkText { get; init; } = string.Empty;
    
    public string FutureDateErrorMessage { get; init; } = string.Empty;

    public string MissingMonthBannerLinkText { get; init; } = string.Empty;

    public string MissingMonthErrorMessage { get; init; } = string.Empty;

    public string MissingYearBannerLinkText { get; init; } = string.Empty;

    public string MissingYearErrorMessage { get; init; } = string.Empty;
    
    public string MonthOutOfBoundsErrorMessage { get; init; } = string.Empty;
    
    public string MonthOutOfBoundsErrorLinkText { get; init; } = string.Empty;
    
    public string YearOutOfBoundsErrorMessage { get; init; } = string.Empty;
    
    public string YearOutOfBoundsErrorLinkText { get; init; } = string.Empty;
}