namespace Dfe.EarlyYearsQualification.Web.Models;

public class RatioRowModel
{
    public int Level { get; init; }
    
    public string LevelText { get; init; } = string.Empty;

    public string RatioId
    {
        get { return string.Concat(LevelText.Where(c => !char.IsWhiteSpace(c))); }
    }

    public QualificationApprovalStatus ApprovalStatus { get; init; }

    public AdditionalInformationModel AdditionalInformation { get; init; } = new();
}