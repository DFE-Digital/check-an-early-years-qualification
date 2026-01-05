using Dfe.EarlyYearsQualification.Caching.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Logging.Abstractions;

namespace Dfe.EarlyYearsQualifications.Caching.UnitTests;

[TestClass]
public class RedisCacheInvalidatorTests
{
    [TestMethod]
    public async Task Passed_EmptyKeyPrefix1_Throws()
    {
        var sut = new RedisCacheInvalidator(new RedisCacheOptions(), new NullLogger<RedisCacheInvalidator>());

        var action = async () => await sut.ClearCacheAsync("");

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [TestMethod]
    public async Task Passed_EmptyKeyPrefix2_Throws()
    {
        var sut = new RedisCacheInvalidator(new RedisCacheOptions(), new NullLogger<RedisCacheInvalidator>());

        var action = async () => await sut.ClearCacheAsync(" ");

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [TestMethod]
    public async Task Passed_NullKeyPrefix_Throws()
    {
        var sut = new RedisCacheInvalidator(new RedisCacheOptions(), new NullLogger<RedisCacheInvalidator>());

        var action = async () => await sut.ClearCacheAsync(null!);

        await action.Should().ThrowAsync<ArgumentException>();
    }
}