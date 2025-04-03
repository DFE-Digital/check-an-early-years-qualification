using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Dfe.EarlyYearsQualification.Caching.Services;

public class RedisCacheInvalidator(
    IOptions<RedisCacheOptions> redisCacheOptions,
    ILogger<RedisCacheInvalidator> logger) : ICacheInvalidator
{
    private const string ClearCacheLuaScript =
        "redis.call('del', unpack(redis.call('keys', ARGV[1])))";

    private readonly RedisCacheOptions _options = redisCacheOptions.Value;

    public async Task ClearCacheAsync(string keyPrefix)
    {
        if (string.IsNullOrWhiteSpace(keyPrefix))
        {
            throw new ArgumentException("Must not be null or empty.", nameof(keyPrefix));
        }

        logger.LogInformation("Clearing cache items with prefix {KeyPrefix}.", keyPrefix);

        var connection = await ConnectionMultiplexer.ConnectAsync(_options.Configuration!);
        var database = connection.GetDatabase();

        RedisValue[] redisValues = [$"{keyPrefix}*"];

        var result = await database.ScriptEvaluateAsync(ClearCacheLuaScript, values: redisValues);

        logger.LogInformation("Redis result {Result}", result);

        await connection.CloseAsync();
    }
}