using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Helpers;

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
        QualificationNumber = StringFormattingHelper.FormatSlashedNumbers(qualification.QualificationNumber);
    }

    public string QualificationId { get; init; } = string.Empty;

    public string QualificationName { get; init; } = string.Empty;

    public string AwardingOrganisationTitle { get; init; } = string.Empty;

    public int QualificationLevel { get; init; }

    public string? QualificationNumber { get; init; } = string.Empty;

    public bool IsQualificationNameDuplicate { get; set; }
}