namespace Dfe.EarlyYearsQualification.Web.Models;

public class RatioRowModel
{
    public string LevelText { get; init; } = string.Empty;

    public bool IsApproved { get; init; }

    public AdditionalInformationModel AdditionalInformation { get; init; } = new();
}