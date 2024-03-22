using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController : Controller
{    
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Search([FromForm] string qualificationName)
    {
      return RedirectToAction("Index", "SearchResult", new {qualificationName});
    }
}