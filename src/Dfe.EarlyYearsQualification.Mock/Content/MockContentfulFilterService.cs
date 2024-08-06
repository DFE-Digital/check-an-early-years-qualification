using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;

namespace Dfe.EarlyYearsQualification.Mock.Content;

public class MockContentfulFilterService : IContentFilterService
{
    public Task<List<Qualification>> GetFilteredQualifications(int? level, int? startDateMonth, int? startDateYear,
                                                               string? awardingOrganisation, string? qualificationName)
    {
        const string startDate = "Sep-14";
        const string endDate = "Aug-19";

        var qualifications =
            new List<Qualification>
            {
                CreateQualification("EYQ-100", AwardingOrganisations.Cache, 2, null, endDate),
                CreateQualification("EYQ-101", AwardingOrganisations.Ncfe, 2, startDate, endDate),
                CreateQualification("EYQ-240", AwardingOrganisations.Pearson, 3, startDate, endDate),
                CreateQualification("EYQ-103", AwardingOrganisations.Ncfe, 3, startDate, endDate),
                CreateQualification("EYQ-104", "City & Guilds", 4, startDate, endDate),
                CreateQualification("EYQ-105", "Montessori Centre International", 4, startDate, endDate),
                CreateQualification("EYQ-106", AwardingOrganisations.Various, 5, startDate, endDate),
                CreateQualification("EYQ-107", AwardingOrganisations.Edexcel, 5, startDate, endDate),
                CreateQualification("EYQ-108", "Kent Sussex Montessori Centre", 6, startDate, endDate),
                CreateQualification("EYQ-109", "NNEB National Nursery Examination Board", 6, startDate, endDate),
                CreateQualification("EYQ-110", AwardingOrganisations.Various, 7, startDate, endDate),
                CreateQualification("EYQ-111", "City & Guilds", 7, startDate, endDate),
                CreateQualification("EYQ-112", AwardingOrganisations.Pearson, 8, startDate, endDate),
                CreateQualification("EYQ-113", AwardingOrganisations.Cache, 8, startDate, endDate)
            };

        // For now, inbound parameters startDateMonth and startDateYear are ignored
        return Task.FromResult(qualifications.Where(x => x.QualificationLevel == level).ToList());
    }

    private static Qualification CreateQualification(string qualificationId, string awardingOrganisation, int level,
                                                     string? startDate, string endDate)
    {
        return new Qualification(qualificationId,
                                 $"{qualificationId}-test",
                                 awardingOrganisation,
                                 level,
                                 startDate,
                                 endDate,
                                 "ghi/456/951",
                                 "additional requirements",
                                 null,
                                 null);
    }
}