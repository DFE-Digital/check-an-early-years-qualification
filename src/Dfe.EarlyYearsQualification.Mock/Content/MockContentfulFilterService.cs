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
                CreateQualification("EYQ-100", "CACHE", 2, null, endDate),
                CreateQualification("EYQ-101", "NCFE", 2, startDate, endDate),
                CreateQualification("EYQ-240", "Pearson", 3, startDate, endDate),
                CreateQualification("EYQ-103", "NCFE", 3, startDate, endDate),
                CreateQualification("EYQ-104", "City & Guilds", 4, startDate, endDate),
                CreateQualification("EYQ-105", "Montessori Centre International", 4, startDate, endDate),
                CreateQualification("EYQ-106", "Various Awarding Organisations", 5, startDate, endDate),
                CreateQualification("EYQ-107", "Edexcel (now Pearson Education Ltd)", 5, startDate, endDate),
                CreateQualification("EYQ-108", "Kent Sussex Montessori Centre", 6, startDate, endDate),
                CreateQualification("EYQ-109", "NNEB National Nursery Examination Board", 6, startDate, endDate),
                CreateQualification("EYQ-110", "Various Awarding Organisations", 7, startDate, endDate),
                CreateQualification("EYQ-111", "City & Guilds", 7, startDate, endDate),
                CreateQualification("EYQ-112", "Pearson", 8, startDate, endDate),
                CreateQualification("EYQ-113", "CACHE", 8, startDate, endDate)
            };

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