using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dfe.EarlyYearsQualification.Web.Filters;

public class ChallengeResourceFilterAttribute : Attribute, IResourceFilter
{
    private const string ChallengeSecret = "CX";

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        if (!context.HttpContext.Request.Cookies.ContainsKey("auth-secret")
            || !context.HttpContext.Request.Cookies["auth-secret"]!.Equals(ChallengeSecret))
        {
            context.Result = new RedirectResult("/challenge");
        }
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        // do nothing
    }
}