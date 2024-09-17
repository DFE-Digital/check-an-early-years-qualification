using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class GovUkInsetTextModel : IContent
{
    public string? Name { get; set; }

    public Document? Content { get; init; }
}