using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class SearchResultPageModel
{
  public SearchResultPageModel(string header, List<Qualification> searchResults)
  {
    Header = header;
    SearchResults = searchResults;
  }

  public string Header { get; set; }

  public List<Qualification> SearchResults { get; set; }
}