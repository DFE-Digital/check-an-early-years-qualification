using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;

namespace Dfe.EarlyYearsQualification.Mock.Content;

public class MockContentfulFilterService : IContentFilterService
{
    public Task<List<Qualification>> GetFilteredQualifications(int? level, int? startDateMonth, int? startDateYear, string? awardingOrganisation, string? qualificationName)
    {
        var qualifications =
            new List<Qualification>
            {
                CreateQualification("EYQ-100", "CACHE", 2, null, "Aug-19"),
                CreateQualification("EYQ-101", "NCFE", 2, "Sep-14", "Aug-19"),
                CreateQualification("EYQ-102", "Pearson", 3, "Sep-14", "Aug-19"),
                CreateQualification("EYQ-103", "NCFE", 3, "Sep-14", "Aug-19"),
                CreateQualification("EYQ-104", "City & Guilds", 4, "Sep-14", "Aug-19"),
                CreateQualification("EYQ-105", "Montessori Centre International", 4, "Sep-14", "Aug-19"),
                CreateQualification("EYQ-106", "Various Awarding Organisations", 5, "Sep-14", "Aug-19"),
                CreateQualification("EYQ-107", "Edexcel (now Pearson Education Ltd)", 5, "Sep-14", "Aug-19"),
                CreateQualification("EYQ-108", "Kent Sussex Montessori Centre", 6, "Sep-14", "Aug-19"),
                CreateQualification("EYQ-109", "NNEB National Nursery Examination Board", 6, "Sep-14", "Aug-19"),
                CreateQualification("EYQ-110", "Various Awarding Organisations", 7, "Sep-14", "Aug-19"),
                CreateQualification("EYQ-111", "City & Guilds", 7, "Sep-14", "Aug-19"),
                CreateQualification("EYQ-112", "Pearson", 8, "Sep-14", "Aug-19"),
                CreateQualification("EYQ-113", "CACHE", 8, "Sep-14", "Aug-19")
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
                                 "additional requirements");
    }
}