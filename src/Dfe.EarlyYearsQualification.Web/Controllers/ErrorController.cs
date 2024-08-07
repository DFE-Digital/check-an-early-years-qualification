using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/error")]
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public class ErrorController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("ProblemWithTheService");
    }
    
    [Route("{statusCode:int}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        HttpContext.Response.StatusCode = statusCode;

        return statusCode switch
               {
                   404 => View("NotFound"),
                   _ => View("ProblemWithTheService")
               };
    }
}