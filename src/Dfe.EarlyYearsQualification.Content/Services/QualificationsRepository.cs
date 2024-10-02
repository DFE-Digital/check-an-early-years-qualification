using System.Collections.ObjectModel;
using System.Globalization;
using System.Web;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class QualificationsRepository(
    ILogger<QualificationsRepository> logger,
    IContentfulClient contentfulClient,
    IFuzzyAdapter fuzzyAdapter)
    : ContentfulContentServiceBase(logger, contentfulClient), IQualificationsRepository
{
    private const int Day = 28;

    private static readonly ReadOnlyDictionary<string, int>
        Months = new(
                     new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
                     {
                         { "Jan", 1 },
                         { "Feb", 2 },
                         { "Mar", 3 },
                         { "Apr", 4 },
                         { "May", 5 },
                         { "Jun", 6 },
                         { "Jul", 7 },
                         { "Aug", 8 },
                         { "Sep", 9 },
                         { "Oct", 10 },
                         { "Nov", 11 },
                         { "Dec", 12 }
                     });

    // Used by the unit tests to inject a mock builder that returns the query params
    public QueryBuilder<Qualification> QueryBuilder { get; init; } = QueryBuilder<Qualification>.New;

    public async Task<Qualification?> GetById(string qualificationId)
    {
        var queryBuilder = new QueryBuilder<Qualification>().ContentTypeIs(ContentTypeLookup[typeof(Qualification)])
                                                            .Include(2)
                                                            .FieldEquals("fields.qualificationId",
                                                                         qualificationId.ToUpper());
        var qualifications = await GetEntriesByType(queryBuilder);

        if (qualifications is null || !qualifications.Any())
        {
            var encodedQualificationId = HttpUtility.HtmlEncode(qualificationId);
            Logger.LogWarning("No qualifications returned for qualificationId: {QualificationId}",
                              encodedQualificationId);
            return default;
        }

        var qualification = qualifications.First();
        return qualification;
    }

    public async Task<List<Qualification>> Get()
    {
        var qualifications = await GetEntriesByType<Qualification>();
        return qualifications!.ToList();
    }

    public async Task<List<Qualification>> Get(int? level, int? startDateMonth,
                                               int? startDateYear, string? awardingOrganisation,
                                               string? qualificationName)
    {
        Logger.LogInformation("Filtering options passed in - level: {Level}, startDateMonth: {StartDateMonth}, startDateYear: {StartDateYear}, awardingOrganisation: {AwardingOrganisation}, qualificationName: {QualificationName}",
                              level,
                              startDateMonth,
                              startDateYear,
                              awardingOrganisation,
                              qualificationName);

        // create query builder
        var queryBuilder = QueryBuilder.ContentTypeIs(ContentTypes.Qualification).Limit(500);

        if (level is > 0)
        {
            queryBuilder = queryBuilder.FieldEquals("fields.qualificationLevel", level.Value.ToString());
        }

        if (!string.IsNullOrEmpty(awardingOrganisation))
        {
            var awardingOrganisations = new List<string>
                                        {
                                            AwardingOrganisations.AllHigherEducation,
                                            AwardingOrganisations.Various
                                        };
            awardingOrganisations.AddRange(IncludeLinkedOrganisations(awardingOrganisation, startDateMonth,
                                                                      startDateYear));

            queryBuilder = queryBuilder.FieldIncludes("fields.awardingOrganisationTitle", awardingOrganisations);
        }

        // get qualifications
        ContentfulCollection<Qualification>? qualifications;
        try
        {
            qualifications = await ContentfulClient.GetEntries(queryBuilder);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error getting qualifications");
            return [];
        }

        // apply start date filtering
        var filteredQualifications = FilterQualificationsByDate(startDateMonth, startDateYear, qualifications.ToList());

        // Filter based on qualification name
        filteredQualifications = FilterQualificationsByName(filteredQualifications, qualificationName);

        return filteredQualifications;
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

        var results = new List<Qualification>();
        var enteredStartDate = new DateOnly(startDateYear.Value, startDateMonth.Value, Day);
        foreach (var qualification in qualifications)
        {
            var qualificationStartDate = GetQualificationDate(qualification.FromWhichYear);
            var qualificationEndDate = GetQualificationDate(qualification.ToWhichYear);

            if (qualificationStartDate is not null
                && qualificationEndDate is not null
                && enteredStartDate >= qualificationStartDate
                && enteredStartDate <= qualificationEndDate)
            {
                // check start date falls between those dates & add to results
                results.Add(qualification);
            }
            else if (qualificationStartDate is null
                     && qualificationEndDate is not null
                     // ReSharper disable once MergeSequentialChecks
                     // ...reveals the intention more clearly this way
                     && enteredStartDate <= qualificationEndDate)
            {
                // if qualification start date is null, check entered start date is <= ToWhichYear & add to results
                results.Add(qualification);
            }
            else
            {
                // if qualification end date is null, check entered start date is >= FromWhichYear & add to results
                if (qualificationStartDate is not null
                    && qualificationEndDate is null
                    && enteredStartDate >= qualificationStartDate)
                {
                    results.Add(qualification);
                }
            }
        }

        return results;
    }

    private DateOnly? GetQualificationDate(string? qualificationDate)
    {
        if (string.IsNullOrEmpty(qualificationDate) || qualificationDate == "null")
        {
            return null;
        }

        return ConvertToDateTime(qualificationDate);
    }

    private DateOnly? ConvertToDateTime(string qualificationDate)
    {
        var (isValid, month, yearMod2000) = ValidateQualificationDate(qualificationDate);

        if (!isValid)
        {
            return null;
        }

        var year = yearMod2000 + 2000;

        return new DateOnly(year, month, Day);
    }

    private (bool isValid, int month, int yearMod2000) ValidateQualificationDate(string qualificationDate)
    {
        var splitQualificationDate = qualificationDate.Split('-');
        if (splitQualificationDate.Length != 2)
        {
            Logger.LogError("Qualification date {QualificationDate} has unexpected format", qualificationDate);
            return (false, 0, 0);
        }

        var abbreviatedMonth = splitQualificationDate[0];
        var yearFilter = splitQualificationDate[1];

        var yearIsValid = int.TryParse(yearFilter,
                                       NumberStyles.Integer,
                                       NumberFormatInfo.InvariantInfo,
                                       out var yearPart);

        if (!yearIsValid)
        {
            Logger.LogError("Qualification date {QualificationDate} contains unexpected year value",
                            qualificationDate);
            return (false, 0, 0);
        }

        if (Months.TryGetValue(abbreviatedMonth, out var month))
        {
            return (true, month, yearPart);
        }

        Logger.LogError("Qualification date {QualificationDate} contains unexpected month value",
                        qualificationDate);

        return (false, 0, 0);
    }
}