using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationListModel
{
    public string Header { get; init; } = string.Empty;

    public UserJourneyModel? Filters { get; init; }

    public NavigationLink? BackButton { get; init; }
}