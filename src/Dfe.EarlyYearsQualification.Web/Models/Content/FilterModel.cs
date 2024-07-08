namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class FilterModel
{
    public string Country { get; init; } = string.Empty;

    public string StartDate { get; set; } = string.Empty;

    public string Level { get; set; } = "Any level";

    public string AwardingOrganisation { get; set; } = "Any organisation";
}