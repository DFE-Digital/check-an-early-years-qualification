using System.Web;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Entities;
using Microsoft.Extensions.Logging;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class ContentfulContentService(
    IContentfulClient contentfulClient,
    ILogger<ContentfulContentService> logger)
    : IContentService
{
    private readonly Dictionary<object, string> _contentTypes
        = new()
          {
              { typeof(StartPage), "startPage" },
              { typeof(Qualification), "Qualification" },
              { typeof(DetailsPage), "detailsPage" },
              { typeof(AdvicePage), "advicePage" },
              { typeof(RadioQuestionPage), "radioQuestionPage" },
              { typeof(AccessibilityStatementPage), "accessibilityStatementPage" },
              { typeof(NavigationLinks), "navigationLinks" },
              { typeof(CookiesPage), "cookiesPage" },
              { typeof(PhaseBanner), "phaseBanner" },
              { typeof(CookiesBanner), "cookiesBanner" },
              { typeof(DateQuestionPage), "dateQuestionPage" }
          };

    public async Task<StartPage?> GetStartPage()
    {
        var startPageEntries = await GetEntriesByType<StartPage>();
        if (startPageEntries is null || !startPageEntries.Any())
        {
            logger.LogWarning("No start page entry returned");
            return default;
        }

        return startPageEntries.First();
    }

    public async Task<DetailsPage?> GetDetailsPage()
    {
        var detailsPageEntries = await GetEntriesByType<DetailsPage>();
        if (detailsPageEntries is null || !detailsPageEntries.Any())
        {
            logger.LogWarning("No details page entry returned");
            return default;
        }

        var detailsPageContent = detailsPageEntries.First();
        return detailsPageContent;
    }

    public async Task<AccessibilityStatementPage?> GetAccessibilityStatementPage()
    {
        var accessibilityStatementEntities = await GetEntriesByType<AccessibilityStatementPage>();
        if (accessibilityStatementEntities is null || !accessibilityStatementEntities.Any())
        {
            logger.LogWarning("No accessibility statement page entry returned");
            return default;
        }

        return accessibilityStatementEntities.First();
    }

    public async Task<CookiesPage?> GetCookiesPage()
    {
        var cookiesEntities = await GetEntriesByType<CookiesPage>();
        if (cookiesEntities is null || !cookiesEntities.Any())
        {
            logger.LogWarning("No cookies page entry returned");
            return default;
        }

        var cookiesContent = cookiesEntities.First();
        return cookiesContent;
    }

    public async Task<List<NavigationLink>?> GetNavigationLinks()
    {
        var navigationLinkEntries = await GetEntriesByType<NavigationLinks>();
        if (navigationLinkEntries is not null && navigationLinkEntries.Any())
        {
            return navigationLinkEntries.First().Links;
        }

        logger.LogWarning("No navigation links returned");
        return default;
    }

    public async Task<Qualification?> GetQualificationById(string qualificationId)
    {
        var queryBuilder = new QueryBuilder<Qualification>().ContentTypeIs(_contentTypes[typeof(Qualification)])
                                                            .FieldEquals("fields.qualificationId",
                                                                         qualificationId.ToUpper());
        var qualifications = await GetEntriesByType(queryBuilder);

        if (qualifications is null || !qualifications.Any())
        {
            var encodedQualificationId = HttpUtility.HtmlEncode(qualificationId);
            logger.LogWarning("No qualifications returned for qualificationId: {QualificationId}",
                              encodedQualificationId);
            return default;
        }

        var qualification = qualifications.First();
        return qualification;
    }

    public async Task<AdvicePage?> GetAdvicePage(string entryId)
    {
        var advicePage = await GetEntryById<AdvicePage>(entryId);
        if (advicePage is null)
        {
            logger.LogWarning("Advice page with {EntryID} could not be found", entryId);
            return default;
        }

        return advicePage;
    }

    public async Task<RadioQuestionPage?> GetRadioQuestionPage(string entryId)
    {
        return await GetEntryById<RadioQuestionPage>(entryId);
    }

      public async Task<DateQuestionPage?> GetDateQuestionPage(string entryId)
      {
          return await GetEntryById<DateQuestionPage>(entryId);
      }

    public async Task<PhaseBanner?> GetPhaseBannerContent()
    {
        var phaseBannerEntities = await GetEntriesByType<PhaseBanner>();
        if (phaseBannerEntities is null || !phaseBannerEntities.Any())
        {
            logger.LogWarning("No phase banner entry returned");
            return default;
        }

        return phaseBannerEntities.First();
    }

    public async Task<CookiesBanner?> GetCookiesBannerContent()
    {
        var cookiesBannerEntry = await GetEntriesByType<CookiesBanner>();
        if (cookiesBannerEntry is null || !cookiesBannerEntry.Any())
        {
            logger.LogWarning("No cookies banner entry returned");
            return default;
        }

        return cookiesBannerEntry.First();
    }

    public async Task<List<Qualification>> GetQualifications(string? level)
    {

      var queryBuilder = new QueryBuilder<Qualification>();

      if (!string.IsNullOrEmpty(level))
      {
        queryBuilder.FieldEquals("fields.qualificationLevel", level);
      }

      var qualifications = await GetEntriesByType(queryBuilder);

      if (qualifications == null)
      {
        return new List<Qualification>();
      }

      return qualifications.ToList();
    }

    private async Task<T?> GetEntryById<T>(string entryId)
    {
        try
        {
            // NOTE: GetEntry doesn't bind linked references which is why we are using GetEntriesByType
            var queryBuilder = new QueryBuilder<T>().ContentTypeIs(_contentTypes[typeof(T)])
                                                    .FieldEquals("sys.id", entryId);
            var entry = await contentfulClient.GetEntriesByType(_contentTypes[typeof(T)], queryBuilder);
            return entry.FirstOrDefault();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception trying to retrieve entryId {EntryId} for type {Type} from Contentful.",
                            entryId, nameof(T));
            return default;
        }
    }

    private async Task<ContentfulCollection<T>?> GetEntriesByType<T>(QueryBuilder<T>? queryBuilder = null)
    {
        var type = typeof(T);
        try
        {
            var results = await contentfulClient.GetEntriesByType(_contentTypes[type], queryBuilder);
            return results;
        }
        catch (Exception ex)
        {
            var typeName = type.Name;
            logger.LogError(ex, "Exception trying to retrieve {TypeName} from Contentful.", typeName);
            return default;
        }
    }
}