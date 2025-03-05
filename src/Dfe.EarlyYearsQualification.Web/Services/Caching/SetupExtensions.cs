using Dfe.EarlyYearsQualification.Content.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace Dfe.EarlyYearsQualification.Web.Services.Caching;

public static class SetupExtensions
{
    public static void UseDistributedCache(this WebApplicationBuilder builder, string cacheType)
    {
        if (cacheType.Equals("Redis"))
        {
            builder.Services
                   .AddStackExchangeRedisCache(options =>
                                               {
                                                   options.Configuration =
                                                       builder.Configuration
                                                              .GetConnectionString("RedisConnectionString");
                                                   options.InstanceName =
                                                       builder.Configuration.GetValue<string>("RedisInstanceName");
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