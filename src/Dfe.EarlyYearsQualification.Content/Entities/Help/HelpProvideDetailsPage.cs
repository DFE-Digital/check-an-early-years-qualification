namespace Dfe.EarlyYearsQualification.Content.Entities.Help;

public class HelpProvideDetailsPage
{
    public NavigationLink BackButtonToGetHelpPage { get; init; } = new NavigationLink();

    public NavigationLink BackButtonToQualificationDetailsPage { get; init; } = new NavigationLink();

    public string Heading { get; init; } = string.Empty;
    
    public string PostHeadingContent { get; init; } = string.Empty;

    public string CtaButtonText { get; init; } = string.Empty;

    public string AdditionalInformationWarningText { get; init; } = string.Empty;

    public string AdditionalInformationErrorMessage { get; init; } = string.Empty;

    public string ErrorBannerHeading { get; init; } = string.Empty;
}