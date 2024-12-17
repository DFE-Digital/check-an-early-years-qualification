using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class BasicQualificationModel
{
    protected BasicQualificationModel()
    {
    }

    public BasicQualificationModel(Qualification qualification)
    {
        QualificationId = qualification.QualificationId;
        QualificationLevel = qualification.QualificationLevel;
        QualificationName = qualification.QualificationName;
        AwardingOrganisationTitle = qualification.AwardingOrganisationTitle;
    }

    public string QualificationId { get; init; } = string.Empty;

    public string QualificationName { get; init; } = string.Empty;

    public string AwardingOrganisationTitle { get; init; } = string.Empty;

    public int QualificationLevel { get; init; }
}