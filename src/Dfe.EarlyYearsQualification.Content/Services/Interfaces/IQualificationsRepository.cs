using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.Services.Interfaces;

public interface IQualificationsRepository
{
    Task<Qualification?> GetById(string qualificationId);

    Task<List<Qualification>> Get();

    Task<List<Qualification>> Get(int? level, int? startDateMonth, int? startDateYear,
                                  string? awardingOrganisation, string? qualificationName);
}