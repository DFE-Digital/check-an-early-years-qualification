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

        if (cacheType is "Redis")
        {
            var instanceName = cacheConfiguration?.GetValue<string>("Instance");

            if (instanceName == null)
            {
                throw new ConfigurationErrorsException("For Redis cache, Cache.Instance must be configured");
            }

            SetupRedisCache(builder, instanceName);
        }
        else if (cacheType is "Memory")
        {
            SetupInMemoryCache(builder);
        }
        else
        {
            builder.Services.AddSingleton<IDistributedCache, NoCache>();
            builder.Services.AddSingleton<ICacheInvalidator, NoCacheInvalidator>();
        }
    }

    private static void SetupInMemoryCache(WebApplicationBuilder builder)
    {
        builder.Services.AddDistributedMemoryCache();

        // There is no easy way to use the in-memory implementation of IDistributedCache,
        // _and_ invalidate the underlying Memory cache
        builder.Services.AddSingleton<ICacheInvalidator, NoCacheInvalidator>();
    }

    private static void SetupRedisCache(WebApplicationBuilder builder, string instanceName)
    {
        var redisDnsEndPoint = GetRedisDnsEndPoint(instanceName);

        builder.Services
               .AddStackExchangeRedisCache(options => { options.SetupRedisConnection(redisDnsEndPoint); });

        builder.Services
               .AddSingleton<ICacheInvalidator, RedisCacheInvalidator>();
    }

    private static DnsEndPoint GetRedisDnsEndPoint(string instanceName)
    {
        var hostName = $"{instanceName}.redis.cache.windows.net";

        var redisDnsEndPoint = new DnsEndPoint(hostName, 6380);
        return redisDnsEndPoint;
    }

    private static void SetupRedisConnection(
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
            async () => await CreateRedisMultiplexer(options);
    }

    private static async Task<ConnectionMultiplexer> CreateRedisMultiplexer(RedisCacheOptions options)
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
}