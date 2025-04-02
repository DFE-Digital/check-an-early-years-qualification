using Dfe.EarlyYearsQualification.Web.Services.Contentful;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class ContentfulUrlToCacheKeyConverterTests
{
    [TestMethod]
    public async Task ContentfulUrl_IsConvertedTo_PathAndQueryKey()
    {
        var sut = new ContentfulUrlToCacheKeyConverter();

        var uriBuilder = new UriBuilder("https://www.contentful.com/s/b/get?x=12&y=24#fragment");

        var key = await sut.GetKeyAsync(uriBuilder.Uri);

        key.Should().Be("contentful:/s/b/get?x=12&y=24");
    }
}