using Microsoft.AspNetCore.Mvc;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Result;
using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

public class ResultController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IContentService _contentService;

    public ResultController(ILogger<HomeController> logger, IContentService contentService)
    {
        _logger = logger;
        _contentService = contentService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string CourseName)
    {
      var pageContent = await _contentService.GetResultPage();
      var searchResult = await _contentService.GetCourseResults(CourseName);
      var model = new ResultPageModel()
      {
        Header = pageContent.Header,
        SearchResults = searchResult!
      };

      return View(model);
    }
}