using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.Services;

public interface IContentFilterService
{
    Task<Qualification> GetFilteredQualifications(string? level, string? country, string? startDateMonth,
                                                  string? startDateYear);
}