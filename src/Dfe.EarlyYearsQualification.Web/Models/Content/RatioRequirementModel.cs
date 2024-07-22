namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class RatioRequirementModel
{
    public string RatioRequirementName { get; set; } = string.Empty;

    public bool ApprovedForLevel2 { get; set; }
    
    public bool ApprovedForLevel3 { get; set; }
    
    public bool ApprovedForLevel6 { get; set; }
    
    public bool ApprovedForUnqualified { get; set; }
    
    public string RequirementsForLevel2 { get; set; } = string.Empty;
    
    public string RequirementsForLevel3 { get; set; } = string.Empty;
    
    public string RequirementsForLevel6 { get; set; } = string.Empty;
    
    public string RequirementsForUnqualified { get; set; } = string.Empty;
}