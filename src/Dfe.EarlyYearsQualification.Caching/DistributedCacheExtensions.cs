using Microsoft.Extensions.Caching.Distributed;

namespace Dfe.EarlyYearsQualification.Caching;

public static class DistributedCacheExtensions
{
    private static readonly DistributedCacheEntryOptions DefaultCacheOptions
        = new DistributedCacheEntryOptions()
          .SetSlidingExpiration(TimeSpan.FromHours(18))
          .SetAbsoluteExpiration(TimeSpan.FromHours(24));

    private static Task SetAsync(
        IDistributedCache cache,
        string key,
        byte[] value,
        DistributedCacheEntryOptions options)
    {
        return cache.SetAsync(key, value, options);
    }

    private static bool TryGetValue(IDistributedCache cache, string key, out byte[]? value)
    {
        value = null;

        var val = cache.Get(key);
        if (val == null)
        {
            return false;
        }

        value = val;

        return true;
    }

    public static async Task<HttpResponseMessage?> GetOrSetAsync(
        this IDistributedCache cache,
        string key,
        Func<Task<HttpResponseMessage>> task,
        DistributedCacheEntryOptions? options = null)
    {
        var cacheOptions = options ?? DefaultCacheOptions;
        if (TryGetValue(cache, key, out var bytes) && bytes is not null)
        {
            var responseFromCache = new HttpResponseMessage { Content = new ByteArrayContent(bytes) };

            return responseFromCache;
        }

        var responseFromContentful = await task();

        if (responseFromContentful.IsSuccessStatusCode) // don't cache unsuccessful requests
        {
            await SetAsync(cache, key, await responseFromContentful.Content.ReadAsByteArrayAsync(), cacheOptions);
        }

        return responseFromContentful;
    }
}