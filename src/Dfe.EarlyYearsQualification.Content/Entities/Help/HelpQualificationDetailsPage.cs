namespace Dfe.EarlyYearsQualification.Content.Entities.Help;

public class HelpQualificationDetailsPage
{
    public NavigationLink BackButton { get; init; } = new NavigationLink();

    public string Heading { get; init; } = string.Empty;
    
    public string PostHeadingContent { get; init; } = string.Empty;

    public string CtaButtonText { get; init; } = string.Empty;

    public string QualificationNameHeading { get; init; } = string.Empty;

    public string QualificationNameErrorMessage { get; init; } = string.Empty;

    public string AwardingOrganisationHeading { get; init; } = string.Empty;

    public string AwardingOrganisationErrorMessage { get; init; } = string.Empty;

    public string ErrorBannerHeading { get; init; } = string.Empty;

    
    
    //todo add to contentful
    public string AwardedDateIsAfterStartedDateErrorText { get; init; } = "Awarded date is after";
    public DateQuestion? StartedQuestion { get; init; } = new()
    {
        ErrorBannerLinkText = "Enter the month and year that the qualification was started",
        ErrorMessage = "Enter the month and year that the qualification was started",
        FutureDateErrorBannerLinkText = "The date the qualification was started must be in the past",
        FutureDateErrorMessage = "The date the qualification was started must be in the past",
        MissingMonthBannerLinkText = "Enter the month that the qualification was started",
        MissingMonthErrorMessage = "Enter the month that the qualification was started",
        MissingYearBannerLinkText = "Enter the year that the qualification was started",
        MissingYearErrorMessage = "Enter the year that the qualification was started",
        MonthLabel = "Month",
        MonthOutOfBoundsErrorLinkText = "The month the qualification was started must be between 1 and 12",
        MonthOutOfBoundsErrorMessage = "The month the qualification was started must be between 1 and 12",
        QuestionHeader = "Start date",
        QuestionHint = "Enter the start date so we can check if the qualification is approved as full and relevant. For example 9 2013.",
        YearLabel = "Year",
        YearOutOfBoundsErrorLinkText = "The year the qualification was started must be between 1900 and $[actual-year]$",
        YearOutOfBoundsErrorMessage = "The year the qualification was started must be between 1900 and $[actual-year]$",
    };

    public DateQuestion? AwardedQuestion { get; init; } = new()
    {
        ErrorBannerLinkText = "Enter the month and year that the qualification was awarded",
        ErrorMessage = "Enter the month and year that the qualification was awarded",
        FutureDateErrorBannerLinkText = "The date the qualification was awarded must be in the past",
        FutureDateErrorMessage = "The date the qualification was awarded must be in the past",
        MissingMonthBannerLinkText = "Enter the month that the qualification was awarded",
        MissingMonthErrorMessage = "Enter the month that the qualification was awarded",
        MissingYearBannerLinkText = "Enter the year that the qualification was awarded",
        MissingYearErrorMessage = "Enter the year that the qualification was awarded",
        MonthLabel = "Month",
        MonthOutOfBoundsErrorLinkText = "The month the qualification was awarded must be between 1 and 12",
        MonthOutOfBoundsErrorMessage = "The month the qualification was awarded must be between 1 and 12",
        QuestionHeader = "Award date",
        QuestionHint = "Enter the date the qualification was awarded so we can tell you if other requirements apply. For example 6 2015.",
        YearLabel = "Year",
        YearOutOfBoundsErrorLinkText = "The year the qualification was awarded must be between 1900 and $[actual-year]$",
        YearOutOfBoundsErrorMessage = "The year the qualification was awarded must be between 1900 and $[actual-year]$",
    };
}