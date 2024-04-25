using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class AccessibilityStatementPage
{
  public string Heading { get; init; } = string.Empty;

  public Document? Body { get; set; }

  public string BodyHtml { get; set; } = string.Empty;
}