using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;

namespace Dfe.EarlyYearsQualification.Content.Services.Extensions;

public static class DistributedCacheExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
                                                                      {
                                                                          PropertyNamingPolicy = null,
                                                                          WriteIndented = true,
                                                                          AllowTrailingCommas = true,
                                                                          DefaultIgnoreCondition =
                                                                              JsonIgnoreCondition.WhenWritingNull
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
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, SerializerOptions));
        return cache.SetAsync(key, bytes, options);
    }

    private static bool TryGetValue<T>(IDistributedCache cache, string key, out T? value)
    {
        var val = cache.Get(key);
        value = default;
        if (val == null) return false;
        value = JsonSerializer.Deserialize<T>(val, SerializerOptions);
        return true;
    }

    public static async Task<T?> GetOrSetAsync<T>(this IDistributedCache cache, string key, Func<Task<T>>? task,
                                                  DistributedCacheEntryOptions? options = null)
    {
        options ??= new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));

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