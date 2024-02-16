using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.Services;

public interface IContentService
{
    Task<LandingPage> GetLandingPage();

    Task<ResultPage> GetResultPage();

    Task<CourseSummaryPage> GetCourseSummaryPage();

    Task<List<CourseSummary>> GetCourseResults(string searchText);

    Task<CourseSummary> GetCourseById(int courseId);
    
    Task<List<NavigationLink>> GetNavigationLinks();
}
