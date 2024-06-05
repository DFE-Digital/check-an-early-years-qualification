using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/challenge")]
public class ChallengeController(
    ILogger<ChallengeController> logger)
    : Controller
{
    [HttpGet]
    public Task<IActionResult> Index()
    {
        logger.LogWarning("Challenge page invoked");
        return Task.FromResult<IActionResult>(new OkResult());
    }
}