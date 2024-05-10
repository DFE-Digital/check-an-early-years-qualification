using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class PhaseBanner
{
  public string PhaseName { get; init; } = string.Empty;

  public Document? Content { get; set; }

  public string ContentHtml {get; set; } = string.Empty;

  public bool Show { get; init; }
}