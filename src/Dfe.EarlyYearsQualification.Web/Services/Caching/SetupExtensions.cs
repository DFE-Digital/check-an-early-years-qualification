using System.Configuration;
using System.Net;
using Azure.Identity;
using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Dfe.EarlyYearsQualification.Caching.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;

namespace Dfe.EarlyYearsQualification.Web.Services.Caching;

public static class SetupExtensions
{
    public static void UseDistributedCache(this WebApplicationBuilder builder,
                                           IConfigurationSection? cacheConfiguration)
    {
        var cacheType = cacheConfiguration?.GetValue<string>("Type") ?? "None";

        switch (cacheType)
        {
            case "Redis":
                SetupRedisCache(builder, cacheConfiguration);
                break;
            case "Memory":
                SetupInMemoryCache(builder);
                break;
            default:
                SetupNoCache(builder);
                break;
        }

        builder.Services.AddScoped<ICachingOptionsManager, CachingOptionsManager>();
        // ...when running in production, this code need to add a NeverBypassCacheManager singleton here instead
    }

    private static void SetupRedisCache(WebApplicationBuilder builder, IConfigurationSection? cacheConfiguration)
    {
        var isLocal = cacheConfiguration?.GetValue<bool>("IsLocal") ?? false;

        if (isLocal)
        {
            SetupLocalRedisCache(builder);
        }
        else
        {
            SetupAzureRedisCache(builder, cacheConfiguration);
        }

        /*
         * A side effect of the Redis cache setup is that the correct RedisCacheOptions object is
         * also placed into the services at start-up, so the RedisCacheInvalidator also receives
         * it and therefore uses the same configuration to connect to Redis.
         */
        builder.Services.AddSingleton<ICacheInvalidator, RedisCacheInvalidator>();
    }

    private static void SetupInMemoryCache(WebApplicationBuilder builder)
    {
        builder.Services.AddDistributedMemoryCache();

        // There is no easy way to use the in-memory implementation of IDistributedCache,
        // _and_ invalidate the underlying Memory cache
        builder.Services.AddSingleton<ICacheInvalidator, NoCacheInvalidator>();
    }

    private static void SetupNoCache(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IDistributedCache, NoCache>();
        builder.Services.AddSingleton<ICacheInvalidator, NoCacheInvalidator>();
    }

    private static void SetupAzureRedisCache(WebApplicationBuilder builder, IConfigurationSection? cacheConfiguration)
    {
        var instanceName = cacheConfiguration?.GetValue<string>("Instance");
        if (instanceName == null)
        {
            throw new ConfigurationErrorsException("For Azure Redis cache, Cache.Instance must be configured");
        }

        var redisDnsEndPoint = GetAzureRedisDnsEndPoint(instanceName);

        builder.Services
               .AddStackExchangeRedisCache(options => { options.SetupAzureRedisConnection(redisDnsEndPoint); });
    }

    private static DnsEndPoint GetAzureRedisDnsEndPoint(string instanceName)
    {
        var hostName = $"{instanceName}.redis.cache.windows.net";

        var redisDnsEndPoint = new DnsEndPoint(hostName, 6380);
        return redisDnsEndPoint;
    }

    private static void SetupAzureRedisConnection(
        this RedisCacheOptions options, DnsEndPoint redisDnsEndPoint)
    {
        options.ConfigurationOptions
            = new ConfigurationOptions
              {
                  EndPoints =
                      new EndPointCollection(
                                             [
                                                 redisDnsEndPoint
                                             ])
              };

        options.ConnectionMultiplexerFactory =
            async () => await CreateAzureRedisMultiplexer(options);
    }

    private static async Task<ConnectionMultiplexer> CreateAzureRedisMultiplexer(RedisCacheOptions options)
    {
        if (options.ConfigurationOptions == null)
        {
            throw new InvalidOperationException("Redis configuration options was not provided.");
        }

        await options.ConfigurationOptions
                     .ConfigureForAzureWithTokenCredentialAsync(new DefaultAzureCredential())
                     .ConfigureAwait(false);

        return await ConnectionMultiplexer
                     .ConnectAsync(options.ConfigurationOptions)
                     .ConfigureAwait(false);
    }

    private static void SetupLocalRedisCache(this WebApplicationBuilder builder)
    {
        builder.Services.AddStackExchangeRedisCache(o => o.Configuration = "localhost:6379");
    }
}