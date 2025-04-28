using System.Web;
using Contentful.Core;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Filters;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class QualificationsRepository(
    ILogger<QualificationsRepository> logger,
    IContentfulClient contentfulClient,
    IQualificationListFilter qualificationListFilter)
    : ContentfulContentServiceBase(logger, contentfulClient), IQualificationsRepository
{
    public async Task<Qualification?> GetById(string qualificationId)
    {
        var qualifications = await GetAllQualifications();
        if (qualifications.Count == 0)
        {
            var encodedQualificationId = HttpUtility.HtmlEncode(qualificationId);
            Logger.LogWarning("No qualifications returned for qualificationId: {QualificationId}",
                              encodedQualificationId);
            return null;
        }

        return qualifications.FirstOrDefault(x => x.QualificationId.ToUpper() == qualificationId.ToUpper());
    }

    public async Task<List<Qualification>> Get(int? level, int? startDateMonth, int? startDateYear, string? awardingOrganisation, string? qualificationName)
    {
        Logger.LogInformation("Filtering options passed in - level: {Level}, startDateMonth: {StartDateMonth}, startDateYear: {StartDateYear}, awardingOrganisation: {AwardingOrganisation}, qualificationName: {QualificationName}",
                              level,
                              startDateMonth,
                              startDateYear,
                              awardingOrganisation,
                              qualificationName);
        
        var qualifications = await GetAllQualifications();

        if (qualifications.Count == 0)
        {
            return qualifications;
        }

        var filteredQualifications =
            qualificationListFilter.ApplyFilters(qualifications, level, startDateMonth, startDateYear,
                                                    awardingOrganisation, qualificationName);
        
        return filteredQualifications;
    }

    private async Task<List<Qualification>> GetAllQualifications()
    {
        // TODO: Should limit be increased???
        var queryBuilder = QueryBuilder<Qualification>.New.ContentTypeIs(ContentTypes.Qualification).Include(2).Limit(500);
        try
        {
            var qualifications = await ContentfulClient.GetEntries(queryBuilder);
            return qualifications.ToList();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error getting qualifications");
            return [];
        }
    }
}