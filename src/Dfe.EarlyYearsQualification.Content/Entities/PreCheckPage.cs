using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class PreCheckPage
{
    public string Header { get; init; } = string.Empty;

    public NavigationLink? BackButton { get; init; }
    
    public Document? PostHeaderContent { get; init; }

    public string Question { get; init; } = string.Empty;
    
    public List<IOptionItem> Options { get; init; } = [];

    public string InformationMessage { get; init; } = string.Empty;

    public string CtaButtonText { get; init; } = string.Empty;
    
    public string ErrorBannerHeading { get; init; } = string.Empty;

    public string ErrorMessage { get; init; } = string.Empty;
}