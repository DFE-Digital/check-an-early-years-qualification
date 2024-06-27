using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.Services;

public interface IContentFilterService
{
    Task<List<Qualification>> GetFilteredQualifications(int? level, int? startDateMonth, int? startDateYear);
}