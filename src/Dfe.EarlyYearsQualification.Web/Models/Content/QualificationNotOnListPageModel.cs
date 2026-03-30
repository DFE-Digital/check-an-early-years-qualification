namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationNotOnListPageModel : StaticPageModel
{
    public string? Level { get; set; } = string.Empty;

    public string StartedDate { get; set; } = string.Empty;

    public string UserType { get; init; } = string.Empty;
}