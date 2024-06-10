namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationSearchModel
{
    public List<QualificationCardModel> Qualifications { get; init; } = new List<QualificationCardModel>();
    public JourneySessionModel SearchParams { get; init; } = new JourneySessionModel();
}