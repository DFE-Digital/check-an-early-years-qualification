using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ServiceController
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}