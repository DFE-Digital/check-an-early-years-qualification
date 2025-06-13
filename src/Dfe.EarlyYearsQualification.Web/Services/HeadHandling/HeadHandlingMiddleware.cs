using Microsoft.ApplicationInsights.DataContracts;

namespace Dfe.EarlyYearsQualification.Web.Services.HeadHandling;

public class HeadHandlingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    public async Task Invoke(HttpContext context)
    {
        bool methodSwitched = false;

        if (HttpMethods.IsHead(context.Request.Method))
        {
            methodSwitched = true;

            context.Request.Method = HttpMethods.Get;
            context.Response.Body = Stream.Null;
        }

        await _next(context);

        if (methodSwitched)
        {
            var requestTelemetry = context.Features.Get<RequestTelemetry>();
            requestTelemetry?.Properties.Add("WasHEADRequest", "true");
            context.Request.Method = HttpMethods.Head;
        }
    }
}