using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.ContractResolvers;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Dfe.EarlyYearsQualification.Web.Filters;

public class ApplicationInsightsActionFilterAttribute(ILogger<ApplicationInsightsActionFilterAttribute> logger)
    : ActionFilterAttribute
{
    private const string RequestBodyTelemetryKey = "Request-Body";
    private const string CookieValueTelemetryKey = "Cookie-Value";
    private const string CookieNotFoundMessage = "Not found";

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        try
        {
            if (context.HttpContext.Request.Method != HttpMethods.Post)
            {
                return;
            }
            
            var fromFormParameter = context.ActionDescriptor.Parameters
                                           .FirstOrDefault(item => item.BindingInfo?.BindingSource == BindingSource.Form)
                                           ?.Name.ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(fromFormParameter))
            {
                return;
            }
            
            var request = context.HttpContext.Features.Get<RequestTelemetry>();

            var cookieValue = context.HttpContext.Request.Cookies.ContainsKey(CookieKeyNames.UserJourneyKey) 
                                  ? context.HttpContext.Request.Cookies[CookieKeyNames.UserJourneyKey] 
                                  : CookieNotFoundMessage;
            
            request?.Properties.Add(CookieValueTelemetryKey, cookieValue);
            
            var arguments = context.ActionArguments
                                   .FirstOrDefault(x => x.Key.Equals(fromFormParameter, StringComparison.InvariantCultureIgnoreCase));

            if (arguments.Value is not null)
            {
                var argument = SerializeDataForTelemetry(arguments.Value);
                request?.Properties.Add(RequestBodyTelemetryKey, argument);
            }
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Error executing the application insights telemetry task");
        }
        finally
        {
            await next();
        }
    }

    private static string SerializeDataForTelemetry(object value)
    {
        return JsonConvert.SerializeObject(value,
                                           new JsonSerializerSettings
                                           {
                                               ContractResolver = new TelemetryContractResolver()
                                           });
    }
}