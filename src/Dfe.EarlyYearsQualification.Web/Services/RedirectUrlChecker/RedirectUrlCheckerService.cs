namespace Dfe.EarlyYearsQualification.Web.Services.RedirectUrlChecker;

public class RedirectUrlCheckerService : IRedirectUrlCheckerService
{
    private const string CookiesUrl = "/cookies";

    private const string DetailsUrl = "/qualifications/qualification-details/";

    private readonly List<string> _validUrls =
    [
        "",
        "/",
        "/cookies",
        "/accessibility-statement",
        "/questions/where-was-the-qualification-awarded"
    ];

    public string CheckUrl(string? url)
    {
        if (url == null)
        {
            return CookiesUrl;
        }

        // Check details page explicitly to check provided qualification ID if any
        if (url.StartsWith(DetailsUrl))
        {
            var qualificationId = url[DetailsUrl.Length..];

            return qualificationId.Contains('/') ? CookiesUrl : url;
        }

        return _validUrls.Contains(url) ? url : CookiesUrl;
    }
}