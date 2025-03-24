using System.Configuration;
using System.Net;
using Azure.Identity;
using Dfe.EarlyYearsQualification.Content.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;

namespace Dfe.EarlyYearsQualification.Web.Services.Caching;

public static class SetupExtensions
{
    public static void UseDistributedCache(this WebApplicationBuilder builder,
                                           IConfigurationSection? cacheConfiguration)
    {
        string cacheType = cacheConfiguration?.GetValue<string>("CacheType") ?? "None";

        if (cacheType is "Redis")
        {
            string? instanceName = cacheConfiguration?.GetValue<string>("Instance");

            if (instanceName == null)
            {
                throw new ConfigurationErrorsException("For Redis cache, Cache.Instance must be configured");
            }

            SetupRedisCache(builder, instanceName);
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

    private static void SetupRedisCache(WebApplicationBuilder builder, string instanceName)
    {
        string hostName = $"{instanceName}.redis.cache.windows.net";

        var redisDnsEndPoint = new DnsEndPoint(hostName, 6380);

        builder.Services
               .AddStackExchangeRedisCache(options => { options.SetupRedisConnection(redisDnsEndPoint); });
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