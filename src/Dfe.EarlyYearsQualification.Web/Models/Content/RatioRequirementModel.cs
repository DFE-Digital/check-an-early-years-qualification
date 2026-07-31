namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class RatioRequirementModel
{
    public QualificationApprovalStatus ApprovedForLevel2 { get; set; }

    public QualificationApprovalStatus ApprovedForLevel3 { get; set; }

    public QualificationApprovalStatus ApprovedForLevel6 { get; set; }
    
    public QualificationApprovalStatus ApprovedForUnqualified { get; set; }

    public bool IsFullAndRelevant { get; init; }

    public string RequirementsForLevel2 { get; set; } = string.Empty;

    public string RequirementsForLevel3 { get; set; } = string.Empty;

    public string RequirementsForLevel6 { get; set; } = string.Empty;

    public string RequirementsForUnqualified { get; set; } = string.Empty;
}