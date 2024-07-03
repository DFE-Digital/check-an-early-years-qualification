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
    private static readonly DateTimeFormatInfo CurrentFormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;

    private readonly ReadOnlyDictionary<int, string>
        _months = new(
                      new Dictionary<int, string>
                      {
                          { 1, CurrentFormatInfo.AbbreviatedMonthNames[0] },
                          { 2, CurrentFormatInfo.AbbreviatedMonthNames[1] },
                          { 3, CurrentFormatInfo.AbbreviatedMonthNames[2] },
                          { 4, CurrentFormatInfo.AbbreviatedMonthNames[3] },
                          { 5, CurrentFormatInfo.AbbreviatedMonthNames[4] },
                          { 6, CurrentFormatInfo.AbbreviatedMonthNames[5] },
                          { 7, CurrentFormatInfo.AbbreviatedMonthNames[6] },
                          { 8, CurrentFormatInfo.AbbreviatedMonthNames[7] },
                          { 9, CurrentFormatInfo.AbbreviatedMonthNames[8] },
                          { 10, CurrentFormatInfo.AbbreviatedMonthNames[9] },
                          { 11, CurrentFormatInfo.AbbreviatedMonthNames[10] },
                          { 12, CurrentFormatInfo.AbbreviatedMonthNames[11] }
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
                     // ...this more clearly reveals intention
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
        var splitQualificationDate = qualificationDate.Split('-');
        if (splitQualificationDate.Length != 2) return null;

        var month = _months.FirstOrDefault(x => x.Value == splitQualificationDate[0]).Key;
        var year = Convert.ToInt32(splitQualificationDate[1]) + 2000;

        return new DateOnly(year, month, Day);
    }
}