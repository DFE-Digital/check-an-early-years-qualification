namespace Dfe.EarlyYearsQualification.Web.Models;

public class RatioRowModel
{
    public string LevelText { get; init; } = string.Empty;

    public QualificationApprovalStatus ApprovalStatus { get; init; }

    public AdditionalInformationModel AdditionalInformation { get; init; } = new();
}