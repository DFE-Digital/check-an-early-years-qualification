using Microsoft.Extensions.Caching.Distributed;

namespace Dfe.EarlyYearsQualification.UnitTests.TestHelpers;

public static class MockDistributedRepositoryHelper
{
    public static IDistributedCache GetMockDistributedCacheInstance()
    {
        var dic = new Dictionary<string, byte[]>();

        var distributedCache = new Mock<IDistributedCache>();

        distributedCache.Reset();

        distributedCache.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(),
                                               It.IsAny<DistributedCacheEntryOptions>(),
                                               It.IsAny<CancellationToken>()))
                        .Callback((string key, byte[] value, DistributedCacheEntryOptions _, CancellationToken _)
                                      => dic[key] = value);

        distributedCache.Setup(x => x.Get(It.IsAny<string>()))
                        .Returns((string key) => dic.TryGetValue(key, out var expression) ? expression : null);

        return distributedCache.Object;
    }
}