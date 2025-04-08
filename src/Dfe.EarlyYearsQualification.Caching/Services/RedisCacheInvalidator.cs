using System.Diagnostics.CodeAnalysis;
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
        """
        for _,k in ipairs(redis.call('keys', ARGV[1])) do
          redis.call('del', k)
        end
        """;

    private readonly RedisCacheOptions _options = redisCacheOptions.Value;

    public Task ClearCacheAsync(string keyPrefix)
    {
        if (string.IsNullOrWhiteSpace(keyPrefix))
        {
            throw new ArgumentException("Must not be null or empty.", nameof(keyPrefix));
        }

        return ClearCacheInternalAsync(keyPrefix);
    }

    [ExcludeFromCodeCoverage(Justification = "Cannot mock a physical Redis cache for unit testing.")]
    private async Task<RedisResult> ClearCacheInternalAsync(string keyPrefix)
    {
        logger.LogInformation("Clearing cache items with prefix {KeyPrefix}.", keyPrefix);

        RedisResult result;

        var connection = await ConnectionMultiplexer.ConnectAsync(_options.Configuration!);

        try
        {
            RedisValue[] redisValues = [$"{keyPrefix}*"];

            var database = connection.GetDatabase();

            result = await database.ScriptEvaluateAsync(ClearCacheLuaScript, values: redisValues);
        }
        finally
        {
            await connection.CloseAsync();
        }

        logger.LogInformation("Redis result {Result}", result);
        return result;
    }
}