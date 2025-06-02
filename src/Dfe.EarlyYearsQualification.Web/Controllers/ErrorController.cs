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
#pragma warning disable S6967
    // ...the model is an int, so no need to check ModelState.IsValid here
    public IActionResult HttpStatusCodeHandler(int statusCode)
#pragma warning restore S6967
    {
        HttpContext.Response.StatusCode = statusCode;

        return statusCode switch
               {
                   404 => View("NotFound"),
                   _ => View("ProblemWithTheService")
               };
    }
}