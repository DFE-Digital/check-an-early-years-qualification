using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Extensions;
using Dfe.EarlyYearsQualification.Content.Renderers;
using Microsoft.Extensions.Logging;
using System.Web;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class ContentfulContentService : IContentService
{
    private readonly IContentfulClient _contentfulClient;
    private readonly ILogger<ContentfulContentService> _logger;

    private readonly Dictionary<object, string> _contentTypes = new()
    {
        {typeof(StartPage), "startPage"},
        {typeof(NavigationLink), "navigationLink"},
        {typeof(Qualification), "Qualification"},
        {typeof(DetailsPage), "detailsPage"}
    };

    public ContentfulContentService(IContentfulClient contentfulClient, ILogger<ContentfulContentService> logger)
    {
        _contentfulClient = contentfulClient;
        _logger = logger;
    }

    public async Task<StartPage?> GetStartPage()
    {
        var startPageEntries = await GetEntriesByType<StartPage>();
        if (startPageEntries is null || !startPageEntries.Any())
        {
            _logger.LogWarning("No start page entry returned");
            return default;
        }
        var startPageContent = startPageEntries.First();
        var htmlRenderer = GetGeneralHtmlRenderer();
        startPageContent.PreCtaButtonContentHtml = await htmlRenderer.ToHtml(startPageContent.PreCtaButtonContent);
        startPageContent.PostCtaButtonContentHtml = await htmlRenderer.ToHtml(startPageContent.PostCtaButtonContent);
        startPageContent.RightHandSideContentHtml = await GetSideContentHtmlRenderer().ToHtml(startPageContent.RightHandSideContent);
        return startPageContent;
    }

    public async Task<DetailsPage?> GetDetailsPage()
    {
      var detailsPageEntries = await GetEntriesByType<DetailsPage>();
      if (detailsPageEntries is null || !detailsPageEntries.Any())
      {
          _logger.LogWarning("No details page entry returned");
          return default;
      }
      var detailsPageContent = detailsPageEntries.First();
      var htmlRenderer = GetGeneralHtmlRenderer();
      htmlRenderer.AddRenderer(new GovUkInsetTextRenderer(_contentfulClient) { Order = 18 });
      detailsPageContent.CheckAnotherQualificationTextHtml = await htmlRenderer.ToHtml(detailsPageContent.CheckAnotherQualificationText);
      detailsPageContent.FurtherInfoTextHtml = await htmlRenderer.ToHtml(detailsPageContent.FurtherInfoText);
      return detailsPageContent;
    }

    public async Task<List<NavigationLink>?> GetNavigationLinks()
    {
        var navigationLinkEntries = await GetEntriesByType<NavigationLink>();
        if (navigationLinkEntries is not null && navigationLinkEntries.Any())
        {
            return navigationLinkEntries.ToList();
        }

        _logger.LogWarning("No navigation links returned");
        return default;
    }

    public async Task<Qualification?> GetQualificationById(string qualificationId)
    {
        var queryBuilder = new QueryBuilder<Qualification>().ContentTypeIs(_contentTypes[typeof(Qualification)]).FieldEquals("fields.qualificationId", qualificationId.ToUpper());
        var qualifications = await GetEntriesByType(queryBuilder);

        if (qualifications is null || !qualifications.Any())
        {
            var encodedQualificationId = HttpUtility.HtmlEncode(qualificationId);
            _logger.LogWarning("No qualifications returned for qualificationId: {QualificationId}", encodedQualificationId);
            return default;
        }
        var qualification = qualifications.First();
        return qualification;
    }

    private async Task<ContentfulCollection<T>?> GetEntriesByType<T>(QueryBuilder<T>? queryBuilder = null)
    {
        var type = typeof(T);
        try
        {
            var results = await _contentfulClient.GetEntriesByType(_contentTypes[type], queryBuilder);
            return results;
        }
        catch (Exception ex)
        {
            var typeName = type.Name;
            _logger.LogError(ex, "Exception trying to retrieve {TypeName} from Contentful.", typeName);
            return default;
        }
    }

    private static HtmlRenderer GetGeneralHtmlRenderer()
    {
        var htmlRenderer = new HtmlRenderer();      
        htmlRenderer.AddCommonRenderers().AddRenderer(new UnorderedListRenderer { Order = 17 });
        return htmlRenderer;
    }

    private static HtmlRenderer GetSideContentHtmlRenderer()
    {
        var htmlRenderer = new HtmlRenderer();
        htmlRenderer.AddCommonRenderers().AddRenderer(new UnorderedListHyperlinksRenderer { Order = 17 });
        return htmlRenderer;
    }
}
