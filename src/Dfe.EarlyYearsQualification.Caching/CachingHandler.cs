using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Dfe.EarlyYearsQualification.Caching;

public class CachingHandler(
    IDistributedCache cache,
    IUrlToKeyConverter urlToKeyConverter,
    ICachingOptionsManager cachingOptionsManager,
    ILogger<CachingHandler> logger,
    HttpClientHandler httpClientHandler)
    : DelegatingHandler(httpClientHandler)
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken)
    {
        if (request.RequestUri is null)
        {
            throw new ArgumentException(nameof(request.RequestUri));
        }

        var cachingOption = await cachingOptionsManager.GetCachingOption();

        if (cachingOption == CachingOption.BypassCache)
        {
            return await InternalSendAsync(request, cancellationToken);
        }

        var cacheKey = await urlToKeyConverter.GetKeyAsync(request.RequestUri);

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