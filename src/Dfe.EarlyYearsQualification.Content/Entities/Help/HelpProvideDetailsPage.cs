namespace Dfe.EarlyYearsQualification.Content.Entities.Help;

public class HelpProvideDetailsPage
{
    public NavigationLink BackButton { get; init; } = new NavigationLink();

    public string Heading { get; init; } = string.Empty;
    
    public string PostHeadingContent { get; init; } = string.Empty;

    public string CtaButtonText { get; init; } = string.Empty;

    public string AdditionalInformationWarningText { get; init; } = string.Empty;

    public string AdditionalInformationErrorMessage { get; init; } = string.Empty;

    public string ErrorBannerHeading { get; init; } = string.Empty;

    public string StaticCharactersRemainingMessage { get; init; } = string.Empty;

    public string DynamicCharactersRemainingMessage { get; init; } = string.Empty;

    public string SingularCharacterRemainingMessage { get; init; } = string.Empty;

    public string DynamicTooManyCharactersEnteredMessage { get; init; } = string.Empty;

    public string SingularTooManyCharactersEnteredMessage { get; init; } = string.Empty;

    public string TooManyCharactersEnteredErrorMessage { get; init; } = string.Empty;
}