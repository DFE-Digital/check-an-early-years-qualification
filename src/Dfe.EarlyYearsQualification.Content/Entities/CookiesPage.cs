using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class CookiesPage
{
  public string Heading { get; init; } = string.Empty;

  public Document? Body { get; set; }

  public string BodyHtml { get; set; } = string.Empty;

  public List<Option> Options { get; init; } = [];

  public string ButtonText { get; init; } = string.Empty;

  public string SuccessBannerHeading { get; init; } = string.Empty;

  public Document? SuccessBannerContent { get; set; }

  public string SuccessBannerContentHtml { get; set; } = string.Empty;

  public string ErrorText { get; init; } = string.Empty;
}