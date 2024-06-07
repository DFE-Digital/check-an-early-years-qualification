using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dfe.EarlyYearsQualification.Web.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ChallengeResourceFilterAttribute(ILogger<ChallengeResourceFilterAttribute> logger)
    : Attribute, IResourceFilter
{
    public const string AuthSecretCookieName = "auth-secret";

    private const bool RedirectIsPermanent = false;
    private const bool RedirectPreservesMethod = false;

    public static string Challenge
    {
        get { return "CX"; }
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        var cookieIsPresent = context.HttpContext.Request.Cookies.ContainsKey(AuthSecretCookieName);

        if (cookieIsPresent && context.HttpContext.Request.Cookies[AuthSecretCookieName]!.Equals(Challenge))
        {
            return;
        }

        var warningMessage = $"Access denied by {nameof(ChallengeResourceFilterAttribute)}";

        if (cookieIsPresent)
        {
            warningMessage += " (incorrect value submitted)";
        }

        logger.LogWarning(warningMessage);

        var requestedUri = context.HttpContext.Request.GetEncodedUrl();

        context.Result = new RedirectToActionResult("Index", "Challenge",
                                                    new
                                                    {
                                                        redirectAddress = requestedUri
                                                    },
                                                    RedirectIsPermanent,
                                                    RedirectPreservesMethod);
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        // do nothing
    }
}