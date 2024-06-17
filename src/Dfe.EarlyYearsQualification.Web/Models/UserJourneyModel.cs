namespace Dfe.EarlyYearsQualification.Web.Models;

public class UserJourneyModel
{
  public string WhereWasQualAwarded { get; set; } = string.Empty;
  public DateTime? WhenWasQualAwarded { get; set; }
  public int? LevelOfQual { get; set; }
}