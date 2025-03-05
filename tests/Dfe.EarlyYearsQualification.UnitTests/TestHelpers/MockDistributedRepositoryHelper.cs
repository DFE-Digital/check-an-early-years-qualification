using Contentful.Core;
using Contentful.Core.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Serialization;
using DistributedCacheExtensions = Dfe.EarlyYearsQualification.Content.Caching.DistributedCacheExtensions;

namespace Dfe.EarlyYearsQualification.UnitTests.TestHelpers;

public static class MockDistributedRepositoryHelper
{
    private static bool _serializationIsSetUp;
    private static readonly object SyncObject = new();

    public static IDistributedCache GetMockDistributedCacheInstance()
    {
        EnsureCacheSerialization();

        var dic = new Dictionary<string, byte[]>();

        var distributedCache = new Mock<IDistributedCache>();

        distributedCache.Reset();

        distributedCache.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(),
                                               It.IsAny<DistributedCacheEntryOptions>(),
                                               It.IsAny<CancellationToken>()))
                        .Callback((string key, byte[] value, DistributedCacheEntryOptions _, CancellationToken _) =>
                                      dic[key] = value);

        distributedCache.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<byte[]>(),
                                          It.IsAny<DistributedCacheEntryOptions>()))
                        .Callback((string key, byte[] value, DistributedCacheEntryOptions _)
                                      => dic[key] = value);

        distributedCache.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((string key, CancellationToken _)
                                          => dic.TryGetValue(key, out byte[]? expression) ? expression : null);

        distributedCache.Setup(x => x.Get(It.IsAny<string>()))
                        .Returns((string key)
                                     => dic.TryGetValue(key, out byte[]? expression) ? expression : null);

        return distributedCache.Object;
    }

    private static void EnsureCacheSerialization()
    {
        lock (SyncObject)
        {
            if (_serializationIsSetUp)
            {
                return;
            }

            var contentfulClient = new ContentfulClient(new HttpClient(), new ContentfulOptions());

            contentfulClient.SerializerSettings.Converters.Add(new AssetJsonConverter());
            contentfulClient.SerializerSettings.Converters.Add(new ContentJsonConverter());

            contentfulClient.Serializer.TraceWriter = new DiagnosticsTraceWriter();

            DistributedCacheExtensions.ContentfulSerializer = contentfulClient.Serializer;
            DistributedCacheExtensions.ContentfulSerializerSettings = contentfulClient.SerializerSettings;

            _serializationIsSetUp = true;
        }
    }
}