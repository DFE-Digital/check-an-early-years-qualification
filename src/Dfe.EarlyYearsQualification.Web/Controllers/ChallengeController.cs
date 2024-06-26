using Dfe.EarlyYearsQualification.Web.Filters;
using Dfe.EarlyYearsQualification.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

public class ChallengeController(
    ILogger<ChallengeController> logger,
    IUrlHelper urlHelper)
    : Controller
{
    private const string DefaultRedirectAddress = "/";

    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public Task<IActionResult> Index([FromQuery] ChallengeModel model)
    {
        if (!ModelState.IsValid)
        {
            logger.LogWarning("Invalid challenge model (get)");
        }

        model.RedirectAddress = SanitiseReferralAddress(model.RedirectAddress);

        logger.LogWarning("Challenge page invoked");

        return Task.FromResult<IActionResult>(View("EntryForm", model));
    }

    [HttpPost]
    public Task<IActionResult> Post([FromForm] ChallengeModel model)
    {
        if (!ModelState.IsValid)
        {
            logger.LogWarning("Invalid challenge model (post)");
        }

        var referralAddress = SanitiseReferralAddress(model.RedirectAddress);

        if (string.IsNullOrWhiteSpace(model.Value))
        {
            return Index(model);
        }

        logger.LogInformation("Challenge secret access value entered");

        SetAuthSecretCookie(model.Value);
        return Task.FromResult<IActionResult>(new RedirectResult(referralAddress));
    }

    private void SetAuthSecretCookie(string accessValue)
    {
        HttpContext.Response
                   .Cookies
                   .Append(ChallengeResourceFilterAttribute.AuthSecretCookieName,
                           accessValue,
                           new CookieOptions { HttpOnly = true });
    }

    private string SanitiseReferralAddress(string from)
    {
        var redirectAddress = from;

        if (!urlHelper.IsLocalUrl(redirectAddress))
        {
            redirectAddress = DefaultRedirectAddress;
        }

        return redirectAddress;
    }
}