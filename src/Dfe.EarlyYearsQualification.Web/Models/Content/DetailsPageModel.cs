namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class DetailsPageModel
{
  public string MainHeader { get; init; } = string.Empty;

  public string AwardingOrgLabel { get; init; } = string.Empty;

  public string LevelLabel { get; init; } = string.Empty;

  public string QualificationNumberLabel { get; init; } = string.Empty;

  public string DateAddedLabel { get; init; } = string.Empty;

  public string DateOfCheckLabel { get; init; } = string.Empty;

  public string BookmarkHeading { get; init; } = string.Empty;

  public string BookmarkText { get; init; } = string.Empty;

  public string CheckAnotherQualificationHeading { get; init; } = string.Empty;

  public string CheckAnotherQualificationText { get; set; } = string.Empty;

  public string FurtherInfoHeading { get; init; } = string.Empty;

  public string FurtherInfoText { get; set; } = string.Empty;
}