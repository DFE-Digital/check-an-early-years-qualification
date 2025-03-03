using Dfe.EarlyYearsQualification.Content.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace Dfe.EarlyYearsQualification.Web.Services.Caching;

public static class SetupExtensions
{
    public static void UseDistributedCache(this WebApplicationBuilder builder, string cacheType)
    {
        if (cacheType.Equals("Redis"))
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
        else if (cacheType.Equals("Memory"))
        {
            builder.Services.AddDistributedMemoryCache();
        }
        else
        {
            builder.Services.AddSingleton<IDistributedCache, NoCache>();
        }
    }
}