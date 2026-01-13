using Microsoft.AspNetCore.Mvc.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dfe.EarlyYearsQualification.Web.Attributes;

public class LogAntiForgeryFailureAttribute(ILogger<LogAntiForgeryFailureAttribute> logger) : Attribute, IAlwaysRunResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is IAntiforgeryValidationFailedResult)
        {
            logger.LogError("The antiforgery token was not validated successfully.");
        }
    }

    public void OnResultExecuted(ResultExecutedContext context) { }
}