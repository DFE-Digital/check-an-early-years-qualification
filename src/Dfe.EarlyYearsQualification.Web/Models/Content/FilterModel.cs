namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class FilterModel
{
    public string Country { get; init; } = string.Empty;

    public string StartDate { get; set; } = string.Empty;

    public string AwardedDate { get; set; } = string.Empty;

    public string Level { get; set; } = string.Empty;

    public string AwardingOrganisation { get; set; } = string.Empty;
}