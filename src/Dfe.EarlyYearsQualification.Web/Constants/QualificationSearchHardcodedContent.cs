namespace Dfe.EarlyYearsQualification.Web.Constants;

/// <summary>
///     TODO: replace all these with real contentful fields and pull from there instead.
/// </summary>
public static class QualificationSearchHardcodedContent
{
    public const string SearchWithinMultipleHeadingFormat = "Search within these {0} qualifications";

    public const string SearchWithinSingleHeading = "Search within this qualification";

    public const string EnterKeywordsMultipleFormat =
        "Enter keywords from the qualification name to search within these {0} matching qualifications";

    public const string EnterKeywordsSingle =
        "Enter keywords from the qualification name to search within this matching qualification";

    public const string SearchMatchHeadingFormat = "{0} of {1} qualifications matches \"{2}\".";

    public const string SearchNoMatchGuidanceIntroFormat =
        "Your search only checks the {0} matching qualifications shown on this page.";

    public const string SearchNoMatchTryHeading = "Try:";
    
    public static readonly string[] SearchNoMatchTryBullets =
    [
        "double-check the spelling of the qualification name",
        "use fewer words in your search",
        "use words from the qualification name, rather than the level or awarding organisation name"
    ];
}
