using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class LandingPage
{
    public string Header { get; set; } = string.Empty;

    public Document? ServiceIntroduction { get; set; }

    public string ServiceIntroductionHtml { get; set; } = string.Empty;

    public string StartButtonText { get; set; } = string.Empty;
}
