using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dfe.EarlyYearsQualification.Web.Filters;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class NoChallengeResourceFilterAttribute : Attribute, IChallengeResourceFilterAttribute
{
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        // do nothing
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        // do nothing
    }
}