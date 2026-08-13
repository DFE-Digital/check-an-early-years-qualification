namespace Dfe.EarlyYearsQualification.Content.Services.Entities;

public class QualificationFilterOptions
{
    public int? Level { get; set; }
    public int? StartDateMonth { get; set; }
    public int? StartDateYear { get; set; }
    public string? AwardingOrganisation { get; set; }
    public string? QualificationName { get; set; }
    public string? Nation { get; set; }
    public bool IncludeAllQualifications { get; set; }
}