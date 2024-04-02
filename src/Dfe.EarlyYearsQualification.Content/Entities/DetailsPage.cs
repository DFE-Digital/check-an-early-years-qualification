using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class DetailsPage
{
  public string MainHeader { get; set; } = string.Empty;

  public string AwardingOrgLabel { get; set; } = string.Empty;

  public string LevelLabel { get; set; } = string.Empty;

  public string QualificationNumberLabel { get; set; } = string.Empty;

  public string DateAddedLabel { get; set; } = string.Empty;

  public string DateOfCheckLabel { get; set; } = string.Empty;

  public string BookmarkHeading { get; set; } = string.Empty;

  public string BookmarkText { get; set; } = string.Empty;

  public string CheckAnotherQualificationHeading { get; set; } = string.Empty;

  public Document? CheckAnotherQualificationText { get; set; }

  public string CheckAnotherQualificationTextHtml { get; set; } = string.Empty;

  public string FutherInfoHeading { get; set; } = string.Empty;

  public Document? FurtherInfoText { get; set; }

  public string FurtherInfoTextHtml { get; set; } = string.Empty;
}
