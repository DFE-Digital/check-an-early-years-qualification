using Dfe.EarlyYearsQualification.Web.Services.Contentful;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class ContentfulUrlToPathAndQueryCacheKeyConverterTests
{
    [TestMethod]
    public void KeyPrefix_IsExpectedValue()
    {
        /*
         * Please reflect whether this is intentional.
         *
         * If the key prefix used when clearing the cache changes, any values actually in the distributed
         * cache when a new version of the code is released will never thereafter be actively cleared.
         * Entries using the old prefix won't get in the way of functionality, but they will use up Redis
         * memory until they are ejected from the cache when they expire.
         */
        ContentfulUrlToPathAndQueryCacheKeyConverter
            .KeyPrefix
            .Should().Be("contentful:", "Extra action required if this value changes");
    }

    [TestMethod]
    public async Task ContentfulUrl_IsConvertedTo_PathAndQueryKey()
    {
        var sut = new ContentfulUrlToPathAndQueryCacheKeyConverter();

        var uriBuilder = new UriBuilder("https://www.contentful.com/s/b/get?x=12&y=24#fragment");

        var key = await sut.GetKeyAsync(uriBuilder.Uri);

        key.Should().Be($"{ContentfulUrlToPathAndQueryCacheKeyConverter.KeyPrefix}/s/b/get?x=12&y=24");
    }
}