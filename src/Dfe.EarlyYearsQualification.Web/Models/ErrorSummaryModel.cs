namespace Dfe.EarlyYearsQualification.Web.Models;

public class ErrorSummaryModel
{
    public string ErrorBannerHeading { get; init; } = string.Empty;

    public required List<ErrorSummaryLink> ErrorSummaryLinks { get; set; }
}