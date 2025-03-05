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
        if (cacheConfiguration == null)
        {
            return;
        }

        string? cacheType = cacheConfiguration.GetValue<string>("CacheType");

        if (cacheType is "Redis")
        {
            string? instanceName = cacheConfiguration.GetValue<string>("InstanceName");

            if (instanceName == null)
            {
                throw new ConfigurationErrorsException("For Redis cache, InstanceName must be configured");
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
        var redisDnsEndPoint = new
            DnsEndPoint($"{instanceName}.redis.cache.windows.net",
                        6380);

        builder.Services
               .AddStackExchangeRedisCache(options =>
                                           {
                                               options.SetRedisEndpoint(redisDnsEndPoint);

                                               options.ConnectionMultiplexerFactory =
                                                   async () => await SetupRedisConnection(options);
                                           });
    }

    private static async Task<IConnectionMultiplexer> SetupRedisConnection(RedisCacheOptions options)
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

    private static void SetRedisEndpoint(this RedisCacheOptions options, DnsEndPoint redisDnsEndPoint)
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
    }
}