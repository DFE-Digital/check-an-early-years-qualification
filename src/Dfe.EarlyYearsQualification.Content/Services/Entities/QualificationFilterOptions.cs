namespace Dfe.EarlyYearsQualification.Content.Services.Entities;

public class QualificationFilterOptions
{
    public int? Level { get; init; }
    public int? StartDateMonth { get; init; }
    public int? StartDateYear { get; init; }
    public string? AwardingOrganisation { get; init; }
    public string? QualificationName { get; init; }
    public string? Nation { get; init; }
    public bool IncludeAllQualifications { get; init; }
}