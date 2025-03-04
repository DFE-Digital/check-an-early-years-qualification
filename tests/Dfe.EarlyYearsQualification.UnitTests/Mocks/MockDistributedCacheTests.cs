using Dfe.EarlyYearsQualification.UnitTests.TestHelpers;
using Microsoft.Extensions.Caching.Distributed;

namespace Dfe.EarlyYearsQualification.UnitTests.Mocks;

[TestClass]
public class MockDistributedCacheTests
{
    private static IDistributedCache GetCache()
    {
        return MockDistributedRepositoryHelper.GetMockDistributedCacheInstance();
    }

    [TestMethod]
    public void ReadFromCache_WhenKeyNotFound_ReturnsNull()
    {
        var cache = GetCache();

        byte[]? bytes = cache.Get("any_key");

        bytes.Should().BeNull();
    }

    [TestMethod]
    public async Task SetAsyncToCache_ThenRetrieveAsync_ReturnsCachedValue()
    {
        const string key = "cache_key";
        byte[] value = [2, 3, 5, 7, 11, 13];

        var cache = GetCache();

        await cache.SetAsync(key, value, new DistributedCacheEntryOptions(), CancellationToken.None);

        byte[]? bytes = await cache.GetAsync(key);

        bytes.Should().ContainInOrder(value);
    }

    [TestMethod]
    public void SetToCache_ThenRetrieve_ReturnsCachedValue()
    {
        const string key = "cache_key";
        byte[] value = [2, 3, 5, 7, 11, 13];

        var cache = GetCache();

        cache.Set(key, value, new DistributedCacheEntryOptions());

        byte[]? bytes = cache.Get(key);

        bytes.Should().ContainInOrder(value);
    }}