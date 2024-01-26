using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Models;

public class DetailsPageModel
{
  public string Header { get; set; } = String.Empty;
  public CourseSummary courseSummary { get; set; } = new CourseSummary();
}
