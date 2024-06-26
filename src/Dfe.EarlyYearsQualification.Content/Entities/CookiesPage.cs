using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class CookiesPage
{
    public string Heading { get; init; } = string.Empty;

    public Document? Body { get; init; }

    public List<Option> Options { get; init; } = [];

    public string ButtonText { get; init; } = string.Empty;

    public string SuccessBannerHeading { get; init; } = string.Empty;

    public Document? SuccessBannerContent { get; init; }

    public string ErrorText { get; init; } = string.Empty;
    
    public NavigationLink? BackButton { get; init; }
}