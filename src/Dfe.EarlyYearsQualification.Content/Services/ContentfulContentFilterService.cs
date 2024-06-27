using Contentful.Core;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class ContentfulContentFilterService (
    IContentfulClient contentfulClient) 
    : IContentFilterService
{
    private readonly Dictionary<int, string> _months = new()
                                                      {
                                                          { 1, "Jan" },
                                                          { 2, "Feb" },
                                                          { 3, "Mar" },
                                                          { 4, "Apr" },
                                                          { 5, "May" },
                                                          { 6, "Jun" },
                                                          { 7, "Jul" },
                                                          { 8, "Aug" },
                                                          { 9, "Sep" },
                                                          { 10, "Oct" },
                                                          { 11, "Nov" },
                                                          { 12, "Dec" }
                                                      };

    private const int Day = 28;

    // Used by the unit tests to inject a mock builder that returns the query params
    public QueryBuilder<Qualification> QueryBuilder { get; set; } = QueryBuilder<Qualification>.New;
    
    public async Task<List<Qualification>> GetFilteredQualifications(int? level, int? startDateMonth, int? startDateYear)
    {
        // create query builder
        var queryBuilder = QueryBuilder.ContentTypeIs(ContentTypes.Qualification);

        if (level is > 0)
        {
            queryBuilder = queryBuilder.FieldEquals("fields.qualificationLevel", level.Value.ToString());
        }
        
        // get qualifications
        var qualifications = await contentfulClient.GetEntries(queryBuilder);

        if (!startDateMonth.HasValue || !startDateYear.HasValue) return qualifications.ToList();
        
        // apply start date filtering
        var results = new List<Qualification>();
        var enteredStartDate = new DateTime(startDateYear.Value, startDateMonth.Value, Day);
        foreach (var qualification in qualifications)
        {
            var qualificationStartDate = GetQualificationDate(qualification.FromWhichYear);
            var qualificationEndDate = GetQualificationDate(qualification.ToWhichYear);
            if (qualificationStartDate is not null && qualificationEndDate is not null)
            {
                // check start date falls between those dates & add to results
                if (enteredStartDate >= qualificationStartDate && enteredStartDate <= qualificationEndDate)
                {
                    results.Add(qualification);
                };
            }
            else if (qualificationStartDate is null && qualificationEndDate is not null)
            {
                // if qualification start date is null, check entered start date is <= ToWhichYear & add to results
                if (enteredStartDate <= qualificationEndDate)
                {
                    results.Add(qualification);
                };
            }
            else if (qualificationStartDate is not null && qualificationEndDate is null)
            {
                // if qualification end date is null, check entered start date is >= FromWhichYear & add to results
                if (enteredStartDate >= qualificationStartDate)
                {
                    results.Add(qualification);
                };
            }
        }

        return results;
    }

    private DateTime? GetQualificationDate(string? qualificationDate)
    {
        if (string.IsNullOrEmpty(qualificationDate) || qualificationDate == "null")
        {
            return null;
        }

        return ConvertToDateTime(qualificationDate);
    }

    private DateTime? ConvertToDateTime(string qualificationDate)
    {
        var splitQualificationDate = qualificationDate.Split('-');
        if (splitQualificationDate.Length != 2) return null;
        
        var month = _months.FirstOrDefault(x => x.Value == splitQualificationDate[0]).Key;
        var year = Convert.ToInt32(splitQualificationDate[1]) + 2000;
        
        return new DateTime(year, month, Day);
    }
}