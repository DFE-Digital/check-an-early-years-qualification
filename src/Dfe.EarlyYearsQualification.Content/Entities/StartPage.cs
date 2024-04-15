using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class StartPage
{
    public string Header { get; init; } = string.Empty;

    public Document? PreCtaButtonContent { get; init; }

    public string PreCtaButtonContentHtml { get; set; } = string.Empty;

    public string CtaButtonText { get; init; } = string.Empty;

    public Document? PostCtaButtonContent { get; init; }

    public string PostCtaButtonContentHtml { get; set; } = string.Empty;

    public string RightHandSideContentHeader { get; init; } = string.Empty;

    public Document? RightHandSideContent { get; init; }

    public string RightHandSideContentHtml { get; set; } = string.Empty;
}
