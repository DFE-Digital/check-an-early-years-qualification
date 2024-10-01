using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.Services;

public interface IQualificationsRepository
{
    Task<Qualification?> GetQualificationById(string qualificationId);

    Task<List<Qualification>> GetQualifications();

    Task<List<Qualification>> GetFilteredQualifications(int? level, int? startDateMonth, int? startDateYear,
                                                        string? awardingOrganisation, string? qualificationName);
}