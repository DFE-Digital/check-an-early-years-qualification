using Microsoft.AspNetCore.Mvc;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

public class DetailsController : Controller
{
    private readonly ILogger<DetailsController> _logger;
    private readonly IContentService _contentService;

    public DetailsController(ILogger<DetailsController> logger, IContentService contentService)
    {
        _logger = logger;
        _contentService = contentService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int courseId)
    {
      var pageContent = await _contentService.GetCourseSummaryPage();
      var courseSummary = await _contentService.GetCourseById(courseId);
      var model = new DetailsPageModel()
      {
        Header = pageContent.Header,
        CourseSummary = courseSummary
      };

      return View(model);
    }
}