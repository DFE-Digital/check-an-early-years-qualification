namespace Dfe.EarlyYearsQualification.Content.Entities.Help;

public class HelpEmailAddressPage
{
    public NavigationLink BackButton { get; init; } = new NavigationLink();

    public string Heading { get; init; } = string.Empty;
    
    public string PostHeadingContent { get; init; } = string.Empty;

    public string CtaButtonText { get; init; } = string.Empty;

    public string NoEmailAddressEnteredErrorMessage { get; init; } = string.Empty;

    public string InvalidEmailAddressErrorMessage { get; init; } = string.Empty;

    public string ErrorBannerHeading { get; init; } = string.Empty;
}