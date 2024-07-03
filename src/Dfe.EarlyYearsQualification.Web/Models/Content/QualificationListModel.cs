using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationListModel
{
    public string Header { get; set; } = string.Empty;

    public UserJourneyModel? Filters { get; set; }

    public NavigationLink? BackButton { get; set; }
}