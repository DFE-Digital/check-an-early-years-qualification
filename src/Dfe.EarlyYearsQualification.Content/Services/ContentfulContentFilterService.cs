using System.Collections.ObjectModel;
using System.Globalization;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Microsoft.Extensions.Logging;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class ContentfulContentFilterService(
    IContentfulClient contentfulClient,
    ILogger<ContentfulContentFilterService> logger)
    : IContentFilterService
{
    private const int Day = 28;

    private readonly ReadOnlyDictionary<string, int>
        _months = new(
                      new Dictionary<string, int>
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

    public async Task<List<Qualification>> GetFilteredQualifications(int? level, int? startDateMonth,
                                                                     int? startDateYear)
    {
        logger.LogInformation("Filtering options passed in - level: {Level}, startDateMonth: {StartDateMonth}, startDateYear: {StartDateYear}",
                              level,
                              startDateMonth,
                              startDateYear);

        // create query builder
        var queryBuilder = QueryBuilder.ContentTypeIs(ContentTypes.Qualification);

        if (level is > 0)
        {
            queryBuilder = queryBuilder.FieldEquals("fields.qualificationLevel", level.Value.ToString());
        }

        // get qualifications
        ContentfulCollection<Qualification>? qualifications;
        try
        {
            qualifications = await contentfulClient.GetEntries(queryBuilder);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting qualifications");
            return [];
        }

        if (!startDateMonth.HasValue || !startDateYear.HasValue) return qualifications.ToList();

        // apply start date filtering
        var results = FilterQualificationsByDate(startDateMonth.Value, startDateYear.Value, qualifications);

        return results;
    }

    private List<Qualification> FilterQualificationsByDate(int startDateMonth, int startDateYear,
                                                           ContentfulCollection<Qualification> qualifications)
    {
        var results = new List<Qualification>();
        var enteredStartDate = new DateOnly(startDateYear, startDateMonth, Day);
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
                     && enteredStartDate <= qualificationEndDate)
            {
                // if qualification start date is null, check entered start date is <= ToWhichYear & add to results
                results.Add(qualification);
            }
            else
            {
                // if qualification end date is null, check entered start date is >= FromWhichYear & add to results
                if (enteredStartDate >= qualificationStartDate)
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
            logger.LogError("Found qualification date {QualificationDate} with unexpected format", qualificationDate);
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
            logger.LogError("Qualification date {QualificationDate} contains unexpected year value", qualificationDate);
            return (false, 0, 0);
        }

        if (!_months.TryGetValue(abbreviatedMonth, out var month))
        {
            logger.LogError("Qualification date {QualificationDate} contains unexpected month value",
                            qualificationDate);

            return (false, 0, 0);
        }

        return (true, month, yearPart);
    }
}