namespace Dfe.EarlyYearsQualification.Web.Models.Error;

public class ErrorViewModel
{
    public string? RequestId { get; init; }

    public bool ShowRequestId => !string.IsNullOrWhiteSpace(RequestId);
}