using Microsoft.AspNetCore.Mvc;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Result;
using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

public class ResultController : Controller
{
    private readonly ILogger<ResultController> _logger;
    private readonly IContentService _contentService;

    public ResultController(ILogger<ResultController> logger, IContentService contentService)
    {
        _logger = logger;
        _contentService = contentService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string courseName)
    {
      var pageContent = await _contentService.GetResultPage();
      var searchResult = await _contentService.GetCourseResults(courseName);
      var model = new ResultPageModel()
      {
        Header = pageContent.Header,
        SearchResults = searchResult!
      };

      return View(model);
    }
}