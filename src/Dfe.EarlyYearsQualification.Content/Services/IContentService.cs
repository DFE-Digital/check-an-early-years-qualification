using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.Services;

public interface IContentService
{
    Task<LandingPage> GetLandingPage();

    Task<ResultPage> GetResultPage();

    Task<List<CourseSummary>> GetCourseResults(string searchText);
}
