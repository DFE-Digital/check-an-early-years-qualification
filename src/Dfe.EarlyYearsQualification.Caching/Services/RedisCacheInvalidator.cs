using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Dfe.EarlyYearsQualification.Caching.Services;

public class RedisCacheInvalidator(IOptions<RedisCacheOptions> redisCacheOptions) : ICacheInvalidator
{
    private const string ClearCacheLuaScript =
        "redis.call('del', unpack(redis.call('keys', ARGV[1])))";

    private readonly RedisCacheOptions _options = redisCacheOptions.Value;

    public async Task ClearCacheAsync()
    {
        var connection = await ConnectionMultiplexer.ConnectAsync(_options.Configuration!);
        var database = connection.GetDatabase();

        await database.ScriptEvaluateAsync(ClearCacheLuaScript,
                                           values: ["contentful:*"]
                                          );

        await connection.CloseAsync();
    }
}