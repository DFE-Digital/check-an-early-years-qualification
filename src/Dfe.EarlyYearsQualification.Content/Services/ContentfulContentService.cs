using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers;
using Microsoft.Extensions.Logging;
using FuzzySharp;

using System.Web;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class ContentfulContentService : IContentService
{
  private readonly IContentfulClient _contentfulClient;
  private readonly ILogger<ContentfulContentService> _logger;

  private Dictionary<object, string> ContentTypes = new()
    {
        {typeof(StartPage), "startPage"},
        {typeof(NavigationLink), "navigationLink"},
        {typeof(Qualification), "Qualification"}
    };

  public ContentfulContentService(IContentfulClient contentfulClient, ILogger<ContentfulContentService> logger)
  {
    _contentfulClient = contentfulClient;
    _logger = logger;
  }

  public async Task<StartPage?> GetStartPage()
  {
    var landingPageEntries = await GetEntriesByType<StartPage>();
    if (landingPageEntries is null || !landingPageEntries.Any())
    {
      _logger.LogWarning($"No landing page entry returned");
      return default;
    }
    var startPageContent = landingPageEntries.First();
    HtmlRenderer htmlRenderer = GetGeneralHtmlRenderer();
    startPageContent.PreCtaButtonContentHtml = await htmlRenderer.ToHtml(startPageContent.PreCtaButtonContent);
    startPageContent.PostCtaButtonContentHtml = await htmlRenderer.ToHtml(startPageContent.PostCtaButtonContent);
    startPageContent.RightHandSideContentHtml = await GetSideContentHtmlRenderer().ToHtml(startPageContent.RightHandSideContent);
    return startPageContent;
  }

  public async Task<List<NavigationLink>?> GetNavigationLinks()
  {
    var navigationLinkEntries = await GetEntriesByType<NavigationLink>();
    if (navigationLinkEntries is null || !navigationLinkEntries.Any())
    {
      _logger.LogWarning($"No navigation links returned");
      return default;
    }
    return navigationLinkEntries.ToList();
  }

  public async Task<Qualification?> GetQualificationById(string qualificationId)
  {
    var queryBuilder = new QueryBuilder<Qualification>().ContentTypeIs(ContentTypes[typeof(Qualification)]).FieldEquals("fields.qualificationId", qualificationId.ToUpper());
    var qualifications = await GetEntriesByType<Qualification>(queryBuilder);

    if (qualifications is null || !qualifications.Any())
    {
      _logger.LogWarning($"No qualifications returned for qualificationId: {HttpUtility.HtmlEncode(qualificationId)}");
      return default;
    }
    var qualification = qualifications.First();
    return qualification;
  }

  public async Task<SearchResultPage> GetSearchResultPage()
  {
    var resultPageEntries = await _contentfulClient.GetEntriesByType<SearchResultPage>("searchResultPage");
    var resultPageContent = resultPageEntries.First();
    return resultPageContent;
  }

  public async Task<List<Qualification>> SearchQualifications(string qualificationName)
  {
    try
    {
      // Get all qualifications from contentful
      var queryBuilder = new QueryBuilder<Qualification>().ContentTypeIs(ContentTypes[typeof(Qualification)]);
      var searchResult = await _contentfulClient.GetEntries(queryBuilder);
      var allQualifications = searchResult.ToList();

      var matchingQualifications = new Dictionary<Qualification, int>();

      foreach (var qualification in allQualifications)
      {
        //TODO: break down search term and qualification name to get best match

        var weight = Fuzz.WeightedRatio(qualificationName, qualification.QualificationName);

        if (weight >= 80)
        {
          matchingQualifications.Add(qualification, weight);
        }
      }

      return new List<Qualification>(matchingQualifications.OrderByDescending(x => x.Value).ToDictionary().Keys);
    }
    catch (Exception e)
    {
      _logger.LogError(e.Message);
      return new List<Qualification>();
    }
  }

  private async Task<ContentfulCollection<T>?> GetEntriesByType<T>(QueryBuilder<T>? queryBuilder = null)
  {
    try
    {
      var results = await _contentfulClient.GetEntriesByType<T>(ContentTypes[typeof(T)], queryBuilder);
      return results;
    }
    catch (Exception ex)
    {
      _logger.LogError($"Exception trying to retrieve {typeof(T)} from Contentful. Error: {ex}");
      return default;
    }
  }

  private static HtmlRenderer GetGeneralHtmlRenderer()
  {
    var htmlRenderer = new HtmlRenderer();
    htmlRenderer.AddRenderer(new Heading2Renderer() { Order = 10 });
    htmlRenderer.AddRenderer(new UnorderedListRenderer() { Order = 11 });
    htmlRenderer.AddRenderer(new HyperlinkRenderer() { Order = 12 });
    return htmlRenderer;
  }

  private static HtmlRenderer GetSideContentHtmlRenderer()
  {
    var htmlRenderer = new HtmlRenderer();
    htmlRenderer.AddRenderer(new HyperlinkRenderer() { Order = 10 });
    htmlRenderer.AddRenderer(new Heading2Renderer() { Order = 11 });
    htmlRenderer.AddRenderer(new UnorderedListHyperlinksRenderer() { Order = 12 });
    return htmlRenderer;
  }
}
