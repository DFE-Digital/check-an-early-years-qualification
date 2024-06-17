namespace Dfe.EarlyYearsQualification.Web.Models;

public class ErrorViewModel
{
    public string? RequestId { get; init; }

    public bool ShowRequestId
    {
        get { return !string.IsNullOrWhiteSpace(RequestId); }
    }
}