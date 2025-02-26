using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Dfe.EarlyYearsQualification.Content.Services.Extensions;

public static class DistributedCacheExtensions
{
    public static JsonSerializer Serializer { get; set; } = null!;
    public static JsonSerializerSettings SerializerSettings { get; set; } = null!;

    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value)
    {
        return SetAsync(cache, key, value, new DistributedCacheEntryOptions()
                                           .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                                           .SetAbsoluteExpiration(TimeSpan.FromHours(1)));
    }

    private static Task SetAsync<T>(IDistributedCache cache, string key, T value,
                                    DistributedCacheEntryOptions options)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value, SerializerSettings));
        return cache.SetAsync(key, bytes, options);
    }

    private static bool TryGetValue<T>(IDistributedCache cache, string key, out T? value)
    {
        byte[]? val = cache.Get(key);
        value = default;
        if (val == null)
        {
            return false;
        }

        using var memoryStream = new MemoryStream(val);
        using var streamReader = new StreamReader(memoryStream);
        using var jsonTextReader = new JsonTextReader(streamReader);

#if DEBUG
        string s = streamReader.ReadToEnd();
        Debug.Write($"Value from cache: {s}");

        memoryStream.Seek(0, SeekOrigin.Begin);
#endif

        value = Serializer.Deserialize<T>(jsonTextReader);
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