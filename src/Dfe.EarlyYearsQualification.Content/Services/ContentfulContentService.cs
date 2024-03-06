using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers;
using Microsoft.Extensions.Logging;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class ContentfulContentService : IContentService
{
    private readonly IContentfulClient _contentfulClient;
    private readonly ILogger<ContentfulContentService> _logger;

    public ContentfulContentService(IContentfulClient contentfulClient, ILogger<ContentfulContentService> logger)
    {
        _contentfulClient = contentfulClient;
        _logger = logger;
    }

    public async Task<LandingPage> GetLandingPage()
    {
        var landingPageEntries = await _contentfulClient.GetEntriesByType<LandingPage>("landingPage");
        var landingPageContent = landingPageEntries.First();
        var htmlRenderer = new HtmlRenderer();
        htmlRenderer.AddRenderer(new UnorderedListRenderer() { Order = 10 });
        landingPageContent.ServiceIntroductionHtml = await htmlRenderer.ToHtml(landingPageContent.ServiceIntroduction);
        return landingPageContent;
    }

    public async Task<List<NavigationLink>> GetNavigationLinks()
    {
        var navigationLinkEntries = await _contentfulClient.GetEntriesByType<NavigationLink>("navigationLink");
        return navigationLinkEntries.ToList();
    }

    public async Task<Qualification> GetQualification(string qualificationId)
    {
        var queryBuilder = new QueryBuilder<Qualification>().ContentTypeIs("Qualification").FieldEquals("fields.qualificationId", qualificationId.ToUpper());
        var qualifications = await _contentfulClient.GetEntriesByType("Qualification", queryBuilder);

        if (!qualifications.Any())
        {
            _logger.LogWarning($"No qualifications returned for qualificationId: {qualificationId}");
            return default;
        }
        var qualification = qualifications.First();
        return qualification;
    }
}
