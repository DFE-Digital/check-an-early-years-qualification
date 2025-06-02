using Microsoft.Extensions.Caching.Distributed;

namespace Dfe.EarlyYearsQualification.Caching;

public static class DistributedCacheExtensions
{
    private static readonly DistributedCacheEntryOptions DefaultCacheOptions
        = new DistributedCacheEntryOptions()
          .SetSlidingExpiration(TimeSpan.FromHours(18))
          .SetAbsoluteExpiration(TimeSpan.FromHours(24));

    private static async Task SetAsync(
        IDistributedCache cache,
        string key,
        byte[] value,
        DistributedCacheEntryOptions options)
    {
        // fire and forget!
        await cache.SetAsync(key, value, options);
    }

    private static async Task<byte[]?> GetValueAsync(IDistributedCache cache, string key)
    {
        const int timeout = 500; // ½ second

        var tokenSource = new CancellationTokenSource(timeout);
        var cancellationToken = tokenSource.Token;

        var task = cache.GetAsync(key, cancellationToken);

        if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken)) == task)
        {
            // task completed within timeout, so return
            byte[]? value = await task;

            return value;
        }

        await tokenSource.CancelAsync();

        return null; // cache miss
    }

    public static async Task<HttpResponseMessage?> GetOrSetAsync(
        this IDistributedCache cache,
        string key,
        Func<Task<HttpResponseMessage>> task,
        DistributedCacheEntryOptions? options = null)
    {
        var cacheOptions = options ?? DefaultCacheOptions;

        byte[]? value = await GetValueAsync(cache, key);

        if (value is not null)
        {
            var responseFromCache = new HttpResponseMessage { Content = new ByteArrayContent(value) };

            return responseFromCache;
        }

        var responseFromContentful = await task();

        if (responseFromContentful.IsSuccessStatusCode) // don't cache unsuccessful requests
        {
            try
            {
                await cache.SetAsync(key,
                                     await responseFromContentful.Content.ReadAsByteArrayAsync(),
                                     cacheOptions);
            }
            catch (Exception)
            {
                // failing to write the response to the cache doesn't really matter
            }
        }

        return responseFromContentful;
    }
}