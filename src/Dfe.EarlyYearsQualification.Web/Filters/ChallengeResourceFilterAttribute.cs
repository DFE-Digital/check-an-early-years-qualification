using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dfe.EarlyYearsQualification.Web.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ChallengeResourceFilterAttribute(
    ILogger<ChallengeResourceFilterAttribute> logger,
    IConfiguration configuration)
    : Attribute, IChallengeResourceFilterAttribute
{
    public const string AuthSecretCookieName = "auth-secret";

    private const bool RedirectIsPermanent = false;
    private const bool RedirectPreservesMethod = false;

    private string[]? ChallengeValues
    {
        get
        {
            return configuration
                   .GetSection("ServiceAccess")
                   .GetSection("Keys")
                   .Get<string[]>();
        }
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        if (ChallengeValues == null || ChallengeValues.Length == 0)
        {
            logger.LogError("Service access keys not configured");
            context.Result = new RedirectToActionResult("Error",
                                                        "Home",
                                                        new { },
                                                        RedirectIsPermanent,
                                                        RedirectPreservesMethod);
        }

        var cookieIsPresent = context.HttpContext.Request.Cookies.ContainsKey(AuthSecretCookieName);

        if (cookieIsPresent && ChallengeValues!.Contains(context.HttpContext.Request.Cookies[AuthSecretCookieName]))
        {
            return;
        }

        var warningMessage = $"Access denied by {nameof(ChallengeResourceFilterAttribute)}";

        if (cookieIsPresent)
        {
            warningMessage += " (incorrect value submitted)";
        }

        logger.LogWarning(warningMessage);

        var requestedPath = context.HttpContext.Request.Path;

        context.Result = new RedirectToActionResult("Index", "Challenge",
                                                    new
                                                    {
                                                        redirectAddress = requestedPath
                                                    },
                                                    RedirectIsPermanent,
                                                    RedirectPreservesMethod);
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        // do nothing
    }
}