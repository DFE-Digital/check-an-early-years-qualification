using Dfe.EarlyYearsQualification.Caching;
using Dfe.EarlyYearsQualification.Web.Services.Caching;
using Dfe.EarlyYearsQualification.Web.Services.Cookies;
using Microsoft.Extensions.Logging.Abstractions;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class CachingOptionsManagerTests
{
    [TestMethod]
    public async Task GetCachingOption_ReturnsExpectedValue()
    {
        var cookies = new Dictionary<string, string>
                      {
                          { "option", nameof(CachingOption.BypassCache) }
                      };

        var cookieManager = new Mock<ICookieManager>();
        cookieManager.Setup(m => m.ReadInboundCookies())
                     .Returns(cookies);

        var sut = new CachingOptionsManager(NullLogger<CachingOptionsManager>.Instance, cookieManager.Object);

        var option = await sut.GetCachingOption();

        option.Should().Be(CachingOption.BypassCache);
    }

    [TestMethod]
    public async Task WhenCookiesIsNull_GetCachingOption_ReturnsNone()
    {
        var cookieManager = new Mock<ICookieManager>();
        cookieManager.Setup(m => m.ReadInboundCookies())
                     .Returns((IDictionary<string, string>)null!);

        var sut = new CachingOptionsManager(NullLogger<CachingOptionsManager>.Instance, cookieManager.Object);

        var option = await sut.GetCachingOption();

        option.Should().Be(CachingOption.UseCache);
    }

    [TestMethod]
    public async Task WhenCookiesHasNoOptionValue_GetCachingOption_ReturnsNone()
    {
        var cookies = new Dictionary<string, string>();

        var cookieManager = new Mock<ICookieManager>();
        cookieManager.Setup(m => m.ReadInboundCookies())
                     .Returns(cookies);

        var sut = new CachingOptionsManager(NullLogger<CachingOptionsManager>.Instance, cookieManager.Object);

        var option = await sut.GetCachingOption();

        option.Should().Be(CachingOption.UseCache);
    }

    [TestMethod]
    public async Task WhenCookiesHasEmptyOptionValue_GetCachingOption_ReturnsNone()
    {
        var cookies = new Dictionary<string, string>
                      {
                          { "option", "" }
                      };

        var cookieManager = new Mock<ICookieManager>();
        cookieManager.Setup(m => m.ReadInboundCookies())
                     .Returns(cookies);

        var sut =
            new CachingOptionsManager(NullLogger<CachingOptionsManager>.Instance, cookieManager.Object);

        var option = await sut.GetCachingOption();

        option.Should().Be(CachingOption.UseCache);
    }

    [TestMethod]
    public async Task WhenCookiesHasUnexpectedOptionValue_GetCachingOption_ReturnsNone()
    {
        var cookies = new Dictionary<string, string>
                      {
                          { "option", "NotAValidValue" }
                      };

        var cookieManager = new Mock<ICookieManager>();
        cookieManager.Setup(m => m.ReadInboundCookies())
                     .Returns(cookies);

        var sut = new CachingOptionsManager(NullLogger<CachingOptionsManager>.Instance, cookieManager.Object);

        var option = await sut.GetCachingOption();

        option.Should().Be(CachingOption.UseCache);
    }

    [TestMethod]
    public async Task WhenCookiesHasOptionValueNone_GetCachingOption_ReturnsNone()
    {
        var cookies = new Dictionary<string, string>
                      {
                          { "option", nameof(CachingOption.UseCache) }
                      };

        var cookieManager = new Mock<ICookieManager>();
        cookieManager.Setup(m => m.ReadInboundCookies())
                     .Returns(cookies);

        var sut = new CachingOptionsManager(NullLogger<CachingOptionsManager>.Instance, cookieManager.Object);

        var option = await sut.GetCachingOption();

        option.Should().Be(CachingOption.UseCache);
    }
}