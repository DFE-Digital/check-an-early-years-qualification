namespace Dfe.EarlyYearsQualification.Content.Entities;

public class DateQuestion
{
    public string QuestionHeader { get; init; } = string.Empty;
    public string QuestionHint { get; init; } = string.Empty;

    public string MonthLabel { get; init; } = string.Empty;

    public string YearLabel { get; init; } = string.Empty;
    
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