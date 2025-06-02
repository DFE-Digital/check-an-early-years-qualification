using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

public class SecurityTextController(IConfiguration configuration) : ServiceController
{
    [HttpGet("security.txt")]
    [HttpGet(".well-known/security.txt")]
    public IActionResult GetSecurityText()
    {
        var url = configuration.GetSection("SecurityTxtUrl").Get<string>();
        if (string.IsNullOrEmpty(url))
        {
            return NotFound("Security file was not found");
        }

        return Redirect(url);
    }
}