using Contentful.Core;
using Contentful.Core.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using DistributedCacheExtensions = Dfe.EarlyYearsQualification.Content.Services.Extensions.DistributedCacheExtensions;

namespace Dfe.EarlyYearsQualification.UnitTests.TestHelpers;

public static class MockDistributedRepositoryHelper
{
    private static readonly object SyncObject = new();
    private static bool _cacheSerializationIsSetUp;

    public static IDistributedCache GetMockDistributedCacheInstance()
    {
        EnsureCacheTestSetup();

        var dic = new Dictionary<string, byte[]>();

        var distributedCache = new Mock<IDistributedCache>();

        distributedCache.Reset();

        distributedCache.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(),
                                               It.IsAny<DistributedCacheEntryOptions>(),
                                               It.IsAny<CancellationToken>()))
                        .Callback((string key,
                                   byte[] value,
                                   DistributedCacheEntryOptions _,
                                   CancellationToken _) => dic[key] = value);

        distributedCache.Setup(x => x.Get(It.IsAny<string>()))
                        .Returns((string key) =>
                                 {
                                     bool ok = dic.TryGetValue(key, out byte[]? expression);
                                     return ok ? expression : null;
                                 });

        return distributedCache.Object;
    }

    private static void EnsureCacheTestSetup()
    {
        lock (SyncObject)
        {
            if (_cacheSerializationIsSetUp)
            {
                return;
            }

            var contentfulClient = new ContentfulClient(new HttpClient(), new ContentfulOptions());

            DistributedCacheExtensions.Serializer = contentfulClient.Serializer;
            DistributedCacheExtensions.SerializerSettings = contentfulClient.SerializerSettings;

            _cacheSerializationIsSetUp = true;
        }
    }
}