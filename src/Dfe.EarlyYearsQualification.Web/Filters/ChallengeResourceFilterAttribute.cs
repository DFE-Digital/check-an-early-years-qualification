using System.Diagnostics.CodeAnalysis;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dfe.EarlyYearsQualification.Web.Filters;

/// <summary>
///     Filter attribute that will check the HTTP request for a cookie whose value matches
///     one of the configured keys. Up to four allowable keys are set up in the service config:
///     <code>
///   "ServiceAccess":
///   {
///     "IsPublic": false,
///     "Keys": [
///       "Key-value-1",
///       "Key-value-2"
///       "Key-value-3"
///       "Key-value-4"
///     ]
///   }
///     </code>
///     "IsPublic" defaults to false. If "IsPublic" is true, it is more efficient to
///     add <see cref="NoChallengeResourceFilterAttribute" /> to the pipeline instead.
/// </summary>
/// <param name="logger"></param>
/// <param name="accessKeysHelper"></param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ChallengeResourceFilterAttribute(
    ILogger<ChallengeResourceFilterAttribute> logger,
    ICheckServiceAccessKeysHelper accessKeysHelper)
    : Attribute, IChallengeResourceFilterAttribute
{
    public const string AuthSecretCookieName = "auth-secret";

    private const bool RedirectsArePermanent = false;
    private const bool RedirectsPreserveMethod = false;

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        if (accessKeysHelper.AllowPublicAccess)
        {
            return;
        }

        if (!accessKeysHelper.ConfiguredKeys.Any())
        {
            logger.LogError("Service access keys not configured");
            context.Result = new RedirectToActionResult("Index",
                                                        "Error",
                                                        new { },
                                                        RedirectsArePermanent,
                                                        RedirectsPreserveMethod);
            return;
        }

        var cookieIsPresent = context.HttpContext.Request.Cookies.ContainsKey(AuthSecretCookieName);

        switch (cookieIsPresent)
        {
            case true
                when accessKeysHelper.ConfiguredKeys.Contains(context.HttpContext.Request.Cookies[AuthSecretCookieName])
                :
                return;
            case true:
                logger.LogWarning($"Access denied by {nameof(ChallengeResourceFilterAttribute)} (incorrect value submitted)");
                break;
            default:
                logger.LogWarning($"Access denied by {nameof(ChallengeResourceFilterAttribute)}");
                break;
        }

        var requestedPath = context.HttpContext.Request.Path;

        context.Result = new RedirectToActionResult("Index",
                                                    "Challenge",
                                                    new
                                                    {
                                                        redirectAddress = requestedPath
                                                    },
                                                    RedirectsArePermanent,
                                                    RedirectsPreserveMethod);
    }

    [ExcludeFromCodeCoverage]
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        // do nothing
    }
}