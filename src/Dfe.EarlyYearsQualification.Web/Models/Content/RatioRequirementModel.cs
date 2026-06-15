namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class RatioRequirementModel
{
    public QualificationApprovalStatus ApprovedForLevel2 { get; set; }

    public QualificationApprovalStatus ApprovedForLevel3 { get; set; }

    public QualificationApprovalStatus ApprovedForLevel6 { get; set; }
    
    public QualificationApprovalStatus ApprovedForUnqualified { get; set; }
    
    public bool OverrideToBeNotFullAndRelevant { get; set; }

    public bool IsNotFullAndRelevant
    {
        get
        {
            if (OverrideToBeNotFullAndRelevant) return true;
            
            return ApprovedForLevel2 != QualificationApprovalStatus.Approved
                   && ApprovedForLevel3 != QualificationApprovalStatus.Approved
                   && ApprovedForLevel6 != QualificationApprovalStatus.Approved;
        }
    }

    public string RequirementsForLevel2 { get; set; } = string.Empty;

    public string RequirementsForLevel3 { get; set; } = string.Empty;

    public string RequirementsForLevel6 { get; set; } = string.Empty;

    // public bool ShowRequirementsForLevel2ByDefault { get; set; }
    //
    // public bool ShowRequirementsForLevel3ByDefault { get; set; }
    //
    // public bool ShowRequirementsForLevel6ByDefault { get; set; }

    public string RequirementsForUnqualified { get; set; } = string.Empty;
}