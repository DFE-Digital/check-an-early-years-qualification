using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}