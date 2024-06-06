using Dfe.EarlyYearsQualification.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

public class ChallengeController(
    ILogger<ChallengeController> logger)
    : Controller
{
    [Route("/challenge")]
    [HttpGet]
    public Task<IActionResult> Index([FromQuery(Name = "from")] string? from,
                                     [FromQuery(Name = "access-value")] string? accessValue)
    {
        from ??= "/";

        if (accessValue != null)
        {
            logger.LogInformation("Challenge secret access value entered successfully");

            HttpContext.Response.Cookies.Append(ChallengeResourceFilterAttribute.AuthSecretCookieName, accessValue);
            return Task.FromResult<IActionResult>(new RedirectResult(from));
        }

        logger.LogWarning("Challenge page invoked");

        return Task.FromResult<IActionResult>(Content($"""
            <html lang='en' />
            <body>
                <p>
                  If you have been invited to view this preview service, you will have been
                  given a value that will grant access. Please type in the value and click 'Submit'.
                </p>
                <form action='challenge' method='GET' enctype='application/x-www-form-urlencoded' >
                <input type='hidden' id='from' name='from' value='{from}' />
                <input type='text' id='access-value' name='access-value' placeholder='key' />
                <input type='submit' value='Submit' />
                </form>
            </body>
            </html>
            """,
                                                      "text/html"));
    }
}