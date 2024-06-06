using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dfe.EarlyYearsQualification.Web.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ChallengeResourceFilterAttribute(ILogger<ChallengeResourceFilterAttribute> logger)
    : Attribute, IResourceFilter
{
    public const string AuthSecretCookieName = "auth-secret";
    public const string Challenge = "CX";

    private const bool RedirectIsPermanent = false;
    private const bool RedirectPreservesMethod = true;

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        if (context.HttpContext.Request.Cookies.ContainsKey(AuthSecretCookieName)
            && context.HttpContext.Request.Cookies[AuthSecretCookieName]!.Equals(Challenge))
        {
            return;
        }

        logger.LogWarning($"Access denied by {nameof(ChallengeResourceFilterAttribute)}");

        var requestedUri = context.HttpContext.Request.GetEncodedUrl();

        var uriBuilder = new UriBuilder(requestedUri)
                         {
                             Path = "/challenge",
                             Query = $"from={requestedUri}"
                         };

        var redirectUri = uriBuilder.Uri;

        context.Result = new RedirectResult(redirectUri.ToString(),
                                            RedirectIsPermanent,
                                            RedirectPreservesMethod);
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        // do nothing
    }
}