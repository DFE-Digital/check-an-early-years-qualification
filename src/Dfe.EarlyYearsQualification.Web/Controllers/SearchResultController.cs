using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchResultController : Controller
{    
    private readonly ILogger<SearchResultController> _logger;
    private readonly IContentService _contentService;

    public SearchResultController(ILogger<SearchResultController> logger, IContentService contentService)
    {
        _logger = logger;
        _contentService = contentService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string qualificationName)
    {
      var pageContent = await _contentService.GetSearchResultPage();
      var searchResult = await _contentService.SearchQualifications(qualificationName);
      var model = new SearchResultPageModel(pageContent.Header, searchResult);
      return View(model);
    }
}