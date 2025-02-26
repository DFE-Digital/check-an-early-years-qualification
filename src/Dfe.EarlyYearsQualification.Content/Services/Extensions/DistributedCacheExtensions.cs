using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Dfe.EarlyYearsQualification.Content.Services.Extensions;

public static class DistributedCacheExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
                                                                      {
                                                                          WriteIndented = true
                                                                      };

    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value)
    {
        return SetAsync(cache, key, value, new DistributedCacheEntryOptions()
                                           .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                                           .SetAbsoluteExpiration(TimeSpan.FromHours(1)));
    }

    private static Task SetAsync<T>(IDistributedCache cache, string key, T value,
                                    DistributedCacheEntryOptions options)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, SerializerOptions);
        return cache.SetAsync(key, bytes, options);
    }

    private static bool TryGetValue<T>(IDistributedCache cache, string key, out T? value)
    {
        var val = cache.Get(key);
        value = default;

        if (val == null)
        {
            return false;
        }

        using var memoryStream = new MemoryStream(val);
        using var streamReader = new StreamReader(memoryStream);

        var json = streamReader.ReadToEnd();

        value = JsonSerializer.Deserialize<T>(json, SerializerOptions);

        return true;
    }

    public static async Task<T?> GetOrSetAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> task,
                                                  DistributedCacheEntryOptions? options = null)
    {
        options ??= new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(12))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(24));

        if (TryGetValue(cache, key, out T? value) && value is not null)
        {
            return value;
        }

        value = await task();
        if (value is not null)
        {
            await SetAsync<T>(cache, key, value, options);
        }

        return value;
    }
}