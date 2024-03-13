using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class StartPage
{
    public string Header { get; set; } = string.Empty;

    public Document? PreCtaButtonContent { get; set; }

    public string PreCtaButtonContentHtml { get; set; } = string.Empty;

    public string CtaButtonText { get; set; } = string.Empty;

    public Document? PostCtaButtonContent { get; set; }

    public string PostCtaButtonContentHtml { get; set; } = string.Empty;

    public string RightHandSideContentHeader { get; set; } = string.Empty;

    public Document? RightHandSideContent { get; set; }

    public string RightHandSideContentHtml { get; set; } = string.Empty;
}
