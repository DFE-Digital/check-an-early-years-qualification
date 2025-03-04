using Dfe.EarlyYearsQualification.Content.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace Dfe.EarlyYearsQualification.Web.Services.Caching;

public static class SetupExtensions
{
    public static void UseDistributedCache(this WebApplicationBuilder builder)
    {
        string? cacheType = builder.Configuration.GetValue<string>("CacheType");

        if (cacheType is "Redis")
        {
            SetupRedisCache(builder);
            return;
        }

        if (cacheType is "Memory")
        {
            SetupInMemoryCache(builder);
            return;
        }

        builder.Services.AddSingleton<IDistributedCache, NoCache>();
    }

    private static void SetupInMemoryCache(WebApplicationBuilder builder)
    {
        builder.Services.AddDistributedMemoryCache();
    }

    private static void SetupRedisCache(WebApplicationBuilder builder)
    {
        string? redisConnectionString = builder.Configuration.GetConnectionString("RedisConnectionString");

        string? redisInstance = builder.Configuration.GetValue<string>("RedisInstanceName");

        builder.Services
               .AddStackExchangeRedisCache(options =>
                                           {
                                               options.Configuration = redisConnectionString;
                                               options.InstanceName = redisInstance;
                                           });
    }
}