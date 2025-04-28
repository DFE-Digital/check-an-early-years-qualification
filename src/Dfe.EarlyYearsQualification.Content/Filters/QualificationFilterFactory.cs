using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Content.Validators;

namespace Dfe.EarlyYearsQualification.Content.Filters;

public class QualificationFilterFactory(IFuzzyAdapter fuzzyAdapter, IDateValidator dateValidator) : IQualificationFilterFactory
{
    public List<Qualification> ApplyFilters(List<Qualification> qualifications, int? level, int? startDateMonth, int? startDateYear,
                                            string? awardingOrganisation, string? qualificationName)
    {
        var filteredQualifications = qualifications;
        
        filteredQualifications = FilterQualificationsByLevel(level, filteredQualifications);
        filteredQualifications = FilterQualificationsByAwardingOrganisation(startDateMonth, startDateYear, awardingOrganisation, filteredQualifications);
        filteredQualifications = FilterQualificationsByDate(startDateMonth, startDateYear, filteredQualifications);
        filteredQualifications = FilterQualificationsByName(filteredQualifications, qualificationName);

        return filteredQualifications;
    }

    private static List<Qualification> FilterQualificationsByLevel(int? level, List<Qualification> filteredQualifications)
    {
        if (level is > 0)
        {
            filteredQualifications =
                filteredQualifications.Where(x => x.QualificationLevel == level).Select(x => x).ToList();
        }

        return filteredQualifications;
    }

    private static List<Qualification> FilterQualificationsByAwardingOrganisation(int? startDateMonth, int? startDateYear,
                                                                                    string? awardingOrganisation,
                                                                                    List<Qualification> filteredQualifications)
    {
        if (!string.IsNullOrEmpty(awardingOrganisation))
        {
            var awardingOrganisations = new List<string>
                                        {
                                            AwardingOrganisations.AllHigherEducation,
                                            AwardingOrganisations.Various
                                        };
            awardingOrganisations.AddRange(IncludeLinkedOrganisations(awardingOrganisation, startDateMonth,
                                                                      startDateYear));

            filteredQualifications =
                filteredQualifications.Where(x => awardingOrganisations.Contains(x.AwardingOrganisationTitle)).ToList();
        }

        return filteredQualifications;
    }

    private List<Qualification> FilterQualificationsByName(
        List<Qualification> qualifications,
        string? qualificationName)
    {
        if (string.IsNullOrEmpty(qualificationName)) return qualifications;

        var matchedQualifications = new List<Qualification>();
        foreach (var qualification in qualifications)
        {
            var weight =
                fuzzyAdapter.PartialRatio(qualificationName.ToLower(), qualification.QualificationName.ToLower());
            if (weight > 70)
            {
                matchedQualifications.Add(qualification);
            }
        }

        return matchedQualifications;
    }

    private List<Qualification> FilterQualificationsByDate(int? startDateMonth, int? startDateYear,
                                                           List<Qualification> qualifications)
    {
        if (!startDateMonth.HasValue || !startDateYear.HasValue) return qualifications;

        var matchedQualifications = new List<Qualification>();
        var enteredStartDate = new DateOnly(startDateYear.Value, startDateMonth.Value, dateValidator.GetDay());
        foreach (var qualification in qualifications)
        {
            var qualificationStartDate = dateValidator.GetDate(qualification.FromWhichYear);
            var qualificationEndDate = dateValidator.GetDate(qualification.ToWhichYear);

            var result = dateValidator.ValidateDateEntry(qualificationStartDate, qualificationEndDate, enteredStartDate,
                                                        qualification);
            if (result is not null)
            {
                matchedQualifications.Add(result);
            }
        }

        return matchedQualifications;
    }
    
    private static List<string> IncludeLinkedOrganisations(string awardingOrganisation, int? startDateMonth,
                                                           int? startDateYear)
    {
        var result = new List<string>();

        switch (awardingOrganisation)
        {
            case AwardingOrganisations.Edexcel or AwardingOrganisations.Pearson:
            {
                result.AddRange(new List<string> { AwardingOrganisations.Edexcel, AwardingOrganisations.Pearson });
                break;
            }
            case AwardingOrganisations.Ncfe or AwardingOrganisations.Cache
                when startDateMonth.HasValue && startDateYear.HasValue:
            {
                var cutOffDate = new DateOnly(2014, 9, 1);
                var date = new DateOnly(startDateYear.Value, startDateMonth.Value, 1);
                if (date >= cutOffDate)
                {
                    result.AddRange(new List<string> { AwardingOrganisations.Ncfe, AwardingOrganisations.Cache });
                }
                else
                {
                    result.Add(awardingOrganisation);
                }

                break;
            }
            default:
            {
                result.Add(awardingOrganisation);
                break;
            }
        }

        return result;
    }
}