using Contentful.Core;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class ContentfulContentService : IContentService
{
    private readonly IContentfulClient _contentfulClient;

    public ContentfulContentService(IContentfulClient contentfulClient)
    {
        _contentfulClient = contentfulClient;
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
}
