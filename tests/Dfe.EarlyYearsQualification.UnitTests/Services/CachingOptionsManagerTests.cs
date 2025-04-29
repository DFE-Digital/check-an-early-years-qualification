using Dfe.EarlyYearsQualification.Caching;
using Dfe.EarlyYearsQualification.TestSupport;
using Dfe.EarlyYearsQualification.Web.Services.Caching;
using Dfe.EarlyYearsQualification.Web.Services.Cookies;
using Microsoft.Extensions.Logging.Abstractions;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class CachingOptionsManagerTests
{
    [TestMethod]
    public async Task GetCachingOption_ReturnsValueFromCookie()
    {
        var cookies = new Dictionary<string, string>
                      {
                          { CachingOptionsManager.OptionsCookieKey, nameof(CachingOption.BypassCache) }
                      };

        var cookieManager = new Mock<ICookieManager>();
        cookieManager.Setup(m => m.ReadInboundCookies())
                     .Returns(cookies);

        var sut = new CachingOptionsManager(NullLogger<CachingOptionsManager>.Instance, cookieManager.Object);

        var option = await sut.GetCachingOption();

        option.Should().Be(CachingOption.BypassCache);
    }

    [TestMethod]
    public async Task WhenCookiesIsNull_GetCachingOption_ReturnsDefault()
    {
        var cookieManager = new Mock<ICookieManager>();
        cookieManager.Setup(m => m.ReadInboundCookies())
                     .Returns((IDictionary<string, string>)null!);

        var sut = new CachingOptionsManager(NullLogger<CachingOptionsManager>.Instance, cookieManager.Object);

        var option = await sut.GetCachingOption();

        option.Should().Be(CachingOption.UseCache);
    }

    [TestMethod]
    public async Task WhenCookiesHasNoOptionValue_GetCachingOption_ReturnsDefault()
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
    public async Task WhenCookiesHasEmptyOptionValue_GetCachingOption_ReturnsDefault()
    {
        var cookies = new Dictionary<string, string>
                      {
                          { CachingOptionsManager.OptionsCookieKey, "" }
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
    public async Task WhenCookiesHasUnexpectedOptionValue_GetCachingOption_ReturnsDefault()
    {
        var cookies = new Dictionary<string, string>
                      {
                          { CachingOptionsManager.OptionsCookieKey, "NotAValidValue" }
                      };

        var cookieManager = new Mock<ICookieManager>();
        cookieManager.Setup(m => m.ReadInboundCookies())
                     .Returns(cookies);

        var sut = new CachingOptionsManager(NullLogger<CachingOptionsManager>.Instance, cookieManager.Object);

        var option = await sut.GetCachingOption();

        option.Should().Be(CachingOption.UseCache);
    }

    [TestMethod]
    public async Task WhenCookiesHasUnexpectedOptionValue_GetCachingOption_LogsWarning()
    {
        const string invalidOptionValue = "NotAValidValue";

        var cookies = new Dictionary<string, string>
                      {
                          { CachingOptionsManager.OptionsCookieKey, invalidOptionValue }
                      };

        var cookieManager = new Mock<ICookieManager>();
        cookieManager.Setup(m => m.ReadInboundCookies())
                     .Returns(cookies);

        var logger = new Mock<ILogger<CachingOptionsManager>>();

        var sut = new CachingOptionsManager(logger.Object, cookieManager.Object);

        await sut.GetCachingOption();

        logger.VerifyWarning($@"User's caching option set to unexpected value '{invalidOptionValue}'");
    }

    [TestMethod]
    public async Task WhenCookiesHasOptionValueDefault_GetCachingOption_ReturnsDefault()
    {
        var cookies = new Dictionary<string, string>
                      {
                          { CachingOptionsManager.OptionsCookieKey, nameof(CachingOption.UseCache) }
                      };

        var cookieManager = new Mock<ICookieManager>();
        cookieManager.Setup(m => m.ReadInboundCookies())
                     .Returns(cookies);

        var sut = new CachingOptionsManager(NullLogger<CachingOptionsManager>.Instance, cookieManager.Object);

        var option = await sut.GetCachingOption();

        option.Should().Be(CachingOption.UseCache);
    }
}