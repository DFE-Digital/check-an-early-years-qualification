using Dfe.EarlyYearsQualification.Caching;
using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Dfe.EarlyYearsQualification.UnitTests.TestHelpers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging.Abstractions;

namespace Dfe.EarlyYearsQualification.UnitTests.HttpHandlers;

[TestClass]
public class CachingHandlerTests
{
    private static IDistributedCache GetCache()
    {
        return MockDistributedRepositoryHelper.GetMockDistributedCacheInstance();
    }

    [TestMethod]
    public async Task RequestWithNullAddress_ThrowsException()
    {
        var handler = new TestCachingHandler(GetCache(),
                                             new Mock<IUrlToKeyConverter>().Object,
                                             new Mock<ICachingOptionsManager>().Object,
                                             NullLogger<CachingHandler>.Instance);

        var action = async () =>
                         await handler.PublicSendAsync(new HttpRequestMessage { RequestUri = null },
                                                       CancellationToken.None);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [TestMethod]
    public async Task Request_ReadsFromCache_UsingUrlAsKey()
    {
        // Arrange...
        var address = new Uri("https://google.co.uk/path?query=q#fragment");

        var cachingOptionsManager = new Mock<ICachingOptionsManager>();
        cachingOptionsManager.Setup(m => m.GetCachingOption())
                             .ReturnsAsync(CachingOption.UseCache);

        const string key = "some key";

        var urlToKeyConverter = new Mock<IUrlToKeyConverter>();
        urlToKeyConverter
            .Setup(c => c.GetKeyAsync(It.IsAny<Uri>()))
            .ReturnsAsync(key);

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, address);

        var distributedCache = GetCache();

        byte[] value = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        await distributedCache.SetAsync(key, value, new DistributedCacheEntryOptions());

        var handler = new TestCachingHandler(distributedCache,
                                             urlToKeyConverter.Object,
                                             cachingOptionsManager.Object,
                                             NullLogger<CachingHandler>.Instance);

        // Act...
        var response = await handler.PublicSendAsync(httpRequestMessage,
                                                     CancellationToken.None);

        // Assert...
        Moq.Mock.Get(distributedCache)
           .Verify(c => c.Get(key), Times.Once);

        response.Content.ReadAsByteArrayAsync().Result.Should().ContainInOrder(value);
    }

    [TestMethod]
    public async Task Request_NotFoundInCache_CallsBaseSend_AndWritesToCache()
    {
        // Arrange...
        var address = new Uri("https://example.com/");
        // NB this test does a real request to that address, as you can't mock an HttpHandler's HttpClient

        var cachingOptionsManager = new Mock<ICachingOptionsManager>();
        cachingOptionsManager.Setup(m => m.GetCachingOption())
                             .ReturnsAsync(CachingOption.UseCache);

        const string key = "some key";

        var urlToKeyConverter = new Mock<IUrlToKeyConverter>();
        urlToKeyConverter
            .Setup(c => c.GetKeyAsync(It.IsAny<Uri>()))
            .ReturnsAsync(key);

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, address);

        var distributedCache = GetCache();

        var handler = new TestCachingHandler(distributedCache,
                                             urlToKeyConverter.Object,
                                             cachingOptionsManager.Object,
                                             NullLogger<CachingHandler>.Instance);

        // Act...
        await handler.PublicSendAsync(httpRequestMessage,
                                      CancellationToken.None);

        // Assert...
        var cache = Moq.Mock.Get(distributedCache);

        cache.Verify(c => c.Get(key), Times.Once);

        cache.Verify(c => c.SetAsync(key,
                                     It.IsAny<byte[]>(),
                                     It.IsAny<DistributedCacheEntryOptions>(),
                                     It.IsAny<CancellationToken>()),
                     Times.Once);

        byte[]? cached = await distributedCache.GetAsync(key);

        cached.Should().NotBeNull();
    }

    [TestMethod]
    public async Task Request_WhenBypassingCache_DoesNotReadCacheBeforeSend()
    {
        // Arrange...
        var address = new Uri("https://example.com/");
        // NB this test does a real request to that address, as you can't mock an HttpHandler's HttpClient

        var cachingOptionsManager = new Mock<ICachingOptionsManager>();
        cachingOptionsManager.Setup(m => m.GetCachingOption())
                             .ReturnsAsync(CachingOption.BypassCache);

        const string key = "some key";

        var urlToKeyConverter = new Mock<IUrlToKeyConverter>();
        urlToKeyConverter
            .Setup(c => c.GetKeyAsync(It.IsAny<Uri>()))
            .ReturnsAsync(key);

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, address);

        var distributedCache = GetCache();

        var handler = new TestCachingHandler(distributedCache,
                                             urlToKeyConverter.Object,
                                             cachingOptionsManager.Object,
                                             NullLogger<CachingHandler>.Instance);

        // Act...
        var response = await handler.PublicSendAsync(httpRequestMessage,
                                                     CancellationToken.None);

        // Assert...
        var cache = Moq.Mock.Get(distributedCache);

        cache.Verify(c => c.Get(key), Times.Never);

        cache.Verify(c => c.SetAsync(key,
                                     It.IsAny<byte[]>(),
                                     It.IsAny<DistributedCacheEntryOptions>(),
                                     It.IsAny<CancellationToken>()),
                     Times.Never);

        response.Should().NotBeNull();
    }

    private class TestCachingHandler(
        IDistributedCache cache,
        IUrlToKeyConverter converter,
        ICachingOptionsManager optionsManager,
        ILogger<CachingHandler> logger)
        : CachingHandler(cache, converter, optionsManager, logger, new HttpClientHandler())
    {
        public async Task<HttpResponseMessage> PublicSendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            return await base.SendAsync(request, cancellationToken);
        }
    }
}