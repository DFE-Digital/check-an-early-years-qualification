using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Models.Result;

public class ResultPageModel
{
  public string Header { get; set; } = string.Empty;

  public List<CourseSummary> SearchResults { get; set; } = new List<CourseSummary>();
}
