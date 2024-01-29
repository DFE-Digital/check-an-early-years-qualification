using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Models;

public class DetailsPageModel
{
  public string Header { get; set; } = String.Empty;
  public CourseSummary CourseSummary { get; set; } = new CourseSummary();
}
