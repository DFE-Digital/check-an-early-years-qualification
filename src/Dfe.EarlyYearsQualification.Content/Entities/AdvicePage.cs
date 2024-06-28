using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class AdvicePage
{
    public string Heading { get; init; } = string.Empty;

    public Document? Body { get; init; }
    
    public NavigationLink? BackButton { get; init; }
}