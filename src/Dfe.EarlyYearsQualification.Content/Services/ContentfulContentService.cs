using System.Web;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Extensions;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;
using Microsoft.Extensions.Logging;
using TableRenderer = Dfe.EarlyYearsQualification.Content.Renderers.GovUk.TableRenderer;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class ContentfulContentService : IContentService
{
    private readonly IContentfulClient _contentfulClient;

    private readonly Dictionary<object, string> _contentTypes
        = new()
          {
              { typeof(StartPage), "startPage" },
              { typeof(Qualification), "Qualification" },
              { typeof(DetailsPage), "detailsPage" },
              { typeof(AdvicePage), "advicePage" },
              { typeof(QuestionPage), "questionPage" },
              { typeof(AccessibilityStatementPage), "accessibilityStatementPage" },
              { typeof(NavigationLinks), "navigationLinks" },
              { typeof(CookiesPage), "cookiesPage" },
              { typeof(PhaseBanner), "phaseBanner" }
          };

    private readonly ILogger<ContentfulContentService> _logger;

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

        return startPageEntries.First();
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
        htmlRenderer.AddRenderer(new InsetTextRenderer(_contentfulClient) { Order = 18 });
        detailsPageContent.CheckAnotherQualificationTextHtml =
            await htmlRenderer.ToHtml(detailsPageContent.CheckAnotherQualificationText);
        detailsPageContent.FurtherInfoTextHtml = await htmlRenderer.ToHtml(detailsPageContent.FurtherInfoText);
        return detailsPageContent;
    }

    public async Task<AccessibilityStatementPage?> GetAccessibilityStatementPage()
    {
        var accessibilityStatementEntities = await GetEntriesByType<AccessibilityStatementPage>();
        if (accessibilityStatementEntities is null || !accessibilityStatementEntities.Any())
        {
            _logger.LogWarning("No accessibility statement page entry returned");
            return default;
        }

        return accessibilityStatementEntities.First();
    }

    public async Task<CookiesPage?> GetCookiesPage()
    {
        var cookiesEntities = await GetEntriesByType<CookiesPage>();
        if (cookiesEntities is null || !cookiesEntities.Any())
        {
            _logger.LogWarning("No cookies page entry returned");
            return default;
        }

        var cookiesContent = cookiesEntities.First();
        var htmlRenderer = GetGeneralHtmlRenderer();
        htmlRenderer.AddRenderer(new TableRenderer { Order = 18 });
        cookiesContent.BodyHtml = await htmlRenderer.ToHtml(cookiesContent.Body);
        cookiesContent.SuccessBannerContentHtml =
            await GetSuccessBannerHtmlRenderer().ToHtml(cookiesContent.SuccessBannerContent);
        return cookiesContent;
    }

    public async Task<List<NavigationLink>?> GetNavigationLinks()
    {
        var navigationLinkEntries = await GetEntriesByType<NavigationLinks>();
        if (navigationLinkEntries is not null && navigationLinkEntries.Any())
        {
            return navigationLinkEntries.First().Links;
        }

        _logger.LogWarning("No navigation links returned");
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
            _logger.LogWarning("No qualifications returned for qualificationId: {QualificationId}",
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
            _logger.LogWarning("Advice page with {EntryID} could not be found", entryId);
            return default;
        }

        var htmlRenderer = GetGeneralHtmlRenderer();
        advicePage.BodyHtml = await htmlRenderer.ToHtml(advicePage.Body);
        return advicePage;
    }

    public async Task<QuestionPage?> GetQuestionPage(string entryId)
    {
        return await GetEntryById<QuestionPage>(entryId);
    }

    public async Task<PhaseBanner?> GetPhaseBannerContent()
    {
        var phaseBannerEntities = await GetEntriesByType<PhaseBanner>();
        if (phaseBannerEntities is null || !phaseBannerEntities.Any())
        {
            _logger.LogWarning("No phase banner entry returned");
            return default;
        }

        return phaseBannerEntities.First();
    }

    private async Task<T?> GetEntryById<T>(string entryId)
    {
        try
        {
            // NOTE: GetEntry doesn't bind linked references which is why we are using GetEntriesByType
            var queryBuilder = new QueryBuilder<T>().ContentTypeIs(_contentTypes[typeof(T)])
                                                    .FieldEquals("sys.id", entryId);
            var entry = await _contentfulClient.GetEntriesByType(_contentTypes[typeof(T)], queryBuilder);
            return entry.FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception trying to retrieve entryId {EntryId} for type {Type} from Contentful.",
                             entryId, nameof(T));
            return default;
        }
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
        htmlRenderer.AddCommonRenderers().AddRenderer(new UnorderedListRenderer { Order = 18 });
        return htmlRenderer;
    }

    private static HtmlRenderer GetSuccessBannerHtmlRenderer()
    {
        var htmlRenderer = new HtmlRenderer();
        htmlRenderer.AddRenderer(new SuccessBannerParagraphRenderer { Order = 1 });
        htmlRenderer.AddRenderer(new HyperlinkRenderer { Order = 2 });
        return htmlRenderer;
    }
}