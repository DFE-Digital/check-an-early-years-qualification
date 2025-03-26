using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Dfe.EarlyYearsQualification.Content.Caching;

public class CachingHandler(
    IDistributedCache cache,
    ILogger<CachingHandler> logger)
    : DelegatingHandler(new HttpClientHandler())
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken)
    {
        if (request.RequestUri is null)
        {
            throw new ArgumentException(nameof(request.RequestUri));
        }

        string cacheKey = request.RequestUri.ToString();

        var response =
            await cache.GetOrSetAsync(cacheKey,
                                      async () => await InternalSendAsync(request, cancellationToken));

        return response!;
    }

    private async Task<HttpResponseMessage> InternalSendAsync(HttpRequestMessage request,
                                                              CancellationToken cancellationToken)
    {
        logger.LogInformation("Caching handler invoking Contentful API");
        return await base.SendAsync(request, cancellationToken);
    }
}