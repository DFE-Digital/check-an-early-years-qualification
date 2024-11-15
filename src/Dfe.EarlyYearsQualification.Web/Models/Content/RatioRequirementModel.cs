namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class RatioRequirementModel
{
    public bool ApprovedForLevel2 { get; set; }

    public bool ApprovedForLevel3 { get; set; }

    public bool ApprovedForLevel6 { get; set; }

    public bool ApprovedForUnqualified { get; set; }

    public bool IsNotFullAndRelevant
    {
        get { return ApprovedForUnqualified && !ApprovedForLevel2 & !ApprovedForLevel3 && !ApprovedForLevel6; }
    }

    public string RequirementsHeadingForLevel2 { get; set; } = string.Empty;

    public string RequirementsForLevel2 { get; set; } = string.Empty;

    public string RequirementsHeadingForLevel3 { get; set; } = string.Empty;

    public string RequirementsForLevel3 { get; set; } = string.Empty;

    public string RequirementsHeadingForLevel6 { get; set; } = string.Empty;

    public string RequirementsForLevel6 { get; set; } = string.Empty;

    public bool ShowRequirementsForLevel6ByDefault { get; set; }

    public string RequirementsHeadingForUnqualified { get; set; } = string.Empty;

    public string RequirementsForUnqualified { get; set; } = string.Empty;
}