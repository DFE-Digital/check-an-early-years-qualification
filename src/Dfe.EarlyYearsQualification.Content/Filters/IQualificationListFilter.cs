using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.Filters;

public interface IQualificationListFilter
{
    List<Qualification> ApplyFilters(List<Qualification> qualifications, int? level, int? startDateMonth,
                                     int? startDateYear, string? awardingOrganisation, string? qualificationName);
}