using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class StartPage
{
    public string Header { get; init; } = string.Empty;

    public Document? PreCtaButtonContent { get; init; }

    public string CtaButtonText { get; init; } = string.Empty;

    public Document? PostCtaButtonContent { get; init; }

    public string RightHandSideContentHeader { get; init; } = string.Empty;

    public Document? RightHandSideContent { get; init; }
}