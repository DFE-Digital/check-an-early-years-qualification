using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Entities;

namespace Dfe.EarlyYearsQualification.Content.Services.Interfaces;

public interface IQualificationsRepository
{
    Task<Qualification?> GetById(string qualificationId);

    Task<List<Qualification>> Get(QualificationFilterOptions filterOptions);
}