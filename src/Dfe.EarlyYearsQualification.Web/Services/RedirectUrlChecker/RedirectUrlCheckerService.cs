namespace Dfe.EarlyYearsQualification.Web.Services.RedirectUrlChecker;

public class RedirectUrlCheckerService : IRedirectUrlCheckerService
{
  private readonly List<string> _validUrls =
  [
    "",
    "/",
    "/cookies",
    "/accessibility-statement",
    "/questions/where-was-the-qualification-awarded",
  ];

  private readonly string detailsUrl = "/qualifications/qualification-details";

  public string CheckUrl(string? url)
  {
    if (url == null || !_validUrls.Contains(url))
    {
      return "/cookies";
    }

    // Check details page explicitly to check provided qualification ID if any
    if (url.StartsWith(detailsUrl))
    {
      var qualificationId = url.Substring(detailsUrl.Length);

      if (qualificationId.Contains("/"))
      {
        return "/cookies";
      }
    }

    return url;
  }
}