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

    public string AwardedDateIsAfterStartedDateErrorText { get; init; } = string.Empty;

    public DateQuestion StartDateQuestion { get; init; } = new();

    public DateQuestion AwardedDateQuestion { get; init; } = new();
}