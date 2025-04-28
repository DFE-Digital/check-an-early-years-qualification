using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.Filters;

public interface IQualificationFilterFactory
{
    List<Qualification> ApplyFilters(List<Qualification> qualifications, int? level, int? startDateMonth,
                                     int? startDateYear, string? awardingOrganisation, string? qualificationName);
}