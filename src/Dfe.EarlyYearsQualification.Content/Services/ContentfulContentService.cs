using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
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

  public async Task<ResultPage> GetResultPage()
  {
    var resultPageEntries = await _contentfulClient.GetEntriesByType<ResultPage>("resultPage");
    var resultPageContent = resultPageEntries.First();
    return resultPageContent;
  }

  public async Task<CourseSummaryPage> GetCourseSummaryPage()
  {
    var courseSummaryPageEntries = await _contentfulClient.GetEntriesByType<CourseSummaryPage>("courseSummaryPage");
    var courseSummaryPageContent = courseSummaryPageEntries.First();
    return courseSummaryPageContent;
  }

  public async Task<List<CourseSummary>> GetCourseResults(string searchText)
  {
    var queryBuilder = new QueryBuilder<CourseSummary>().ContentTypeIs("courseSummary").FullTextSearch(searchText);
    var searchResult = await _contentfulClient.GetEntries(queryBuilder);
    return searchResult.ToList();
  }

  public async Task<CourseSummary> GetCourseById(int courseId)
  {
    var queryBuilder = new QueryBuilder<CourseSummary>().ContentTypeIs("courseSummary").FieldEquals("fields.courseId", courseId.ToString());
    var test = queryBuilder.Build();
    Console.WriteLine(test);
    var searchResult = await _contentfulClient.GetEntries(queryBuilder);

    return searchResult.First();
  }
}
