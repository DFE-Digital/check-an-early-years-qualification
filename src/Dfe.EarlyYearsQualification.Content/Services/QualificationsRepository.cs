using System.Web;
using Contentful.Core;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Filters;
using Dfe.EarlyYearsQualification.Content.Services.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class QualificationsRepository(
    ILogger<QualificationsRepository> logger,
    [FromKeyedServices(Clients.ContentfulDefaultClient)]IContentfulClient contentfulClient,
    IQualificationListFilter qualificationListFilter)
    : ContentfulContentServiceBase(logger, contentfulClient), IQualificationsRepository
{
    public async Task<Qualification?> GetById(string qualificationId)
    {
        var qualifications = await GetAllQualifications(false);
        if (qualifications.Count == 0)
        {
            var encodedQualificationId = HttpUtility.HtmlEncode(qualificationId);
            Logger.LogWarning("No qualifications returned for qualificationId: {QualificationId}",
                              encodedQualificationId);
            return null;
        }

        return qualifications.FirstOrDefault(x => string.Equals(x.QualificationId, qualificationId, StringComparison.CurrentCultureIgnoreCase));
    }

    public async Task<List<Qualification>> Get(QualificationFilterOptions filterOptions)
    {
        Logger.LogInformation("Filtering options passed in - level: {Level}, startDateMonth: {StartDateMonth}, startDateYear: {StartDateYear}, awardingOrganisation: {AwardingOrganisation}, qualificationName: {QualificationName}, nation: {Nation}, includeAllQualifications: {includeAllQualifications}",
                              filterOptions.Level,
                              filterOptions.StartDateMonth,
                              filterOptions.StartDateYear,
                              filterOptions.AwardingOrganisation,
                              filterOptions.QualificationName,
                              filterOptions.Nation,
                              filterOptions.IncludeAllQualifications);
        
        var qualifications = await GetAllQualifications(filterOptions.IncludeAllQualifications);

        if (qualifications.Count == 0)
        {
            return qualifications;
        }

        var filteredQualifications =
            qualificationListFilter.ApplyFilters(qualifications, 
                                                 filterOptions.Level,
                                                 filterOptions.StartDateMonth,
                                                 filterOptions.StartDateYear,
                                                 filterOptions.AwardingOrganisation,
                                                 filterOptions.QualificationName,
                                                 filterOptions.Nation);
        
        return filteredQualifications;
    }

    private async Task<List<Qualification>> GetAllQualifications(bool includeAllQualifications)
    {
        var ratioRequirements = await GetEntriesByType<RatioRequirement>();
        if (ratioRequirements == null)
        {
            logger.LogWarning("No ratio requirements returned");
            return [];
        }
        var queryBuilder = QueryBuilder<Qualification>.New.ContentTypeIs(ContentTypes.Qualification)
                                                      .Include(2)
                                                      .Limit(1000);
        
        // Some qualifications have this flag set which means they shouldn't show in the main service.
        // However, we still want them in the webview and csv download.
        if (!includeAllQualifications)
        {
            queryBuilder.FieldExcludes("fields.excludeFromShowingInMainService", ["1"]);
        }
                                                      
        try
        {
            var qualifications = await ContentfulClient.GetEntries(queryBuilder);
            qualifications.ToList().ForEach(x => x.RatioRequirements = ratioRequirements.ToList());
            // In the test environments, a qualificationId can be null when a new Qualification is in the progress of being created.
            // When the list of qualifications is being iterated on, it can cause an error hence the filter below.
            return qualifications.Where(x => !string.IsNullOrEmpty(x.QualificationId)).ToList();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error getting qualifications");
            return [];
        }
    }
}