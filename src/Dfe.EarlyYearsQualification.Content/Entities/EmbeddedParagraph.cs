using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class EmbeddedParagraph : IContent
{
    public SystemProperties Sys { get; set; } = new();

    public string Name { get; set; } = string.Empty;

    public Document? Content { get; set; }
}