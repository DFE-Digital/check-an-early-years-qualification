using Dfe.EarlyYearsQualification.Content.Caching;
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
        var handler = new TestCachingHandler(GetCache(), NullLogger<CachingHandler>.Instance);

        var action = async () =>
                         await handler.PublicSendAsync(new HttpRequestMessage { RequestUri = null },
                                                       CancellationToken.None);

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [TestMethod]
    public async Task Request_ReadsFromCache_UsingUrlAsKey()
    {
        var address = new Uri("https://google.co.uk");

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, address);

        var distributedCache = GetCache();

        byte[] value = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        await distributedCache.SetAsync(address.ToString(), value, new DistributedCacheEntryOptions());

        var handler = new TestCachingHandler(distributedCache, NullLogger<CachingHandler>.Instance);

        var response = await handler.PublicSendAsync(httpRequestMessage,
                                                     CancellationToken.None);

        Moq.Mock.Get(distributedCache)
           .Verify(c => c.Get(address.ToString()), Times.Once);

        response.Content.ReadAsByteArrayAsync().Result.Should().ContainInOrder(value);
    }

    private class TestCachingHandler(IDistributedCache cache, ILogger<CachingHandler> logger)
        : CachingHandler(cache, logger)
    {
        public async Task<HttpResponseMessage> PublicSendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            return await base.SendAsync(request, cancellationToken);
        }
    }
}