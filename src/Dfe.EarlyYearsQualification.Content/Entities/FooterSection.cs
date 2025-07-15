using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class FooterSection
{
    public string Heading { get; init; } = string.Empty;
    
    public required Document Body { get; init; }
}