using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class ConfirmQualificationPage
{
    public string Heading { get; init; } = string.Empty;
    public Document? PostHeadingContent { get; init; }
    public string QualificationLabel { get; init; } = string.Empty;
    public string LevelLabel { get; init; } = string.Empty;
    public string AwardingOrganisationLabel { get; init; } = string.Empty;
    public string DateAddedLabel { get; init; } = string.Empty;
    public Document? VariousAwardingOrganisationsExplanation { get; init; }
    public string RadioHeading { get; init; } = string.Empty;
    public List<Option> Options { get; init; } = [];
    public string ErrorBannerHeading { get; init; } = string.Empty;
    public string ErrorBannerLink { get; init; } = string.Empty;
    public string ErrorText { get; init; } = string.Empty;
    public string ButtonText { get; init; } = string.Empty;
    public NavigationLink? BackButton { get; init; }
}