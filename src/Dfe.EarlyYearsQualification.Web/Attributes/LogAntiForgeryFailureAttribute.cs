using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dfe.EarlyYearsQualification.Web.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class LogAntiForgeryFailureAttribute(ILogger<LogAntiForgeryFailureAttribute> logger) : Attribute, IAlwaysRunResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is not IAntiforgeryValidationFailedResult) return;
        logger.LogError("The antiforgery token was not validated successfully.");
        context.Result = new ViewResult { ViewName = "ProblemWithTheService", StatusCode = 400 };
    }

    public void OnResultExecuted(ResultExecutedContext context) { }
}