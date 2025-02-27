using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Dfe.EarlyYearsQualification.Content.Caching;

public static class DistributedCacheExtensions
{
    public static JsonSerializer? ContentfulSerializer { get; set; }
    public static JsonSerializerSettings? ContentfulSerializerSettings { get; set; }

    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value)
    {
        CheckSetup();
        return SetAsync(cache, key, value, new DistributedCacheEntryOptions()
                                           .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                                           .SetAbsoluteExpiration(TimeSpan.FromHours(1)));
    }

    private static Task SetAsync<T>(IDistributedCache cache, string key, T value,
                                    DistributedCacheEntryOptions options)
    {
        using var stringWriter = new StringWriter();
        using var jsonTextWriter = new JsonTextWriter(stringWriter);

        ContentfulSerializer!.Serialize(jsonTextWriter, value, typeof(T));

        var bytes = Encoding.UTF8.GetBytes(stringWriter.ToString());

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
        using var jsonReader = new JsonTextReader(streamReader);

#if DEBUG
        var json = streamReader.ReadToEnd();
        Debug.WriteLine($"Value from cache: {json}");

        memoryStream.Seek(0, SeekOrigin.Begin);
#endif

        value = ContentfulSerializer!.Deserialize<T>(jsonReader);

        return true;
    }

    public static async Task<T?> GetOrSetAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> task,
                                                  DistributedCacheEntryOptions? cacheOptions = null)
    {
        CheckSetup();

        cacheOptions ??= new DistributedCacheEntryOptions()
                         .SetSlidingExpiration(TimeSpan.FromHours(12))
                         .SetAbsoluteExpiration(TimeSpan.FromHours(24));

        if (TryGetValue(cache, key, out T? value) && value is not null)
        {
            return value;
        }

        value = await task();
        if (value is not null)
        {
            await SetAsync<T>(cache, key, value, cacheOptions);
        }

        return value;
    }

    private static void CheckSetup()
    {
        if (ContentfulSerializer == null || ContentfulSerializerSettings == null)
        {
            throw new InvalidOperationException("Distributed cache serialization not initialised");
        }
    }
}