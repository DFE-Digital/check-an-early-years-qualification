namespace Dfe.EarlyYearsQualification.Web.Models;

public class ErrorSummaryModel
{
    public string ErrorBannerHeading { get; init; } = string.Empty;

    public required IEnumerable<ErrorSummaryLink> ErrorSummaryLinks { get; init; }
}