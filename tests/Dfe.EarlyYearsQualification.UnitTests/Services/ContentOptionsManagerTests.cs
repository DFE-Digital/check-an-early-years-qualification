using Dfe.EarlyYearsQualification.Content.Options;
using Dfe.EarlyYearsQualification.Web.Services.Contentful;
using Dfe.EarlyYearsQualification.Web.Services.Cookies;
using Microsoft.Extensions.Logging.Abstractions;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class ContentOptionsManagerTests
{
    [TestMethod]
    public async Task GetContentOption_ReturnsValueFromCookie()
    {
        var cookies = new Dictionary<string, string>
                      {
                          { ContentOptionsManager.OptionsCookieKey, nameof(ContentOption.UsePreview) }
                      };

        var cookieManager = new Mock<ICookieManager>();
        cookieManager.Setup(m => m.ReadInboundCookies())
                     .Returns(cookies);

        var sut = new ContentOptionsManager(NullLogger<ContentOptionsManager>.Instance, cookieManager.Object);

        var option = await sut.GetContentOption();

        option.Should().Be(ContentOption.UsePreview);
    }

    [TestMethod]
    public async Task WhenCookiesIsNull_GetContentOption_ReturnsDefault()
    {
        var cookieManager = new Mock<ICookieManager>();
        cookieManager.Setup(m => m.ReadInboundCookies())
                     .Returns((IDictionary<string, string>)null!);

        var sut = new ContentOptionsManager(NullLogger<ContentOptionsManager>.Instance, cookieManager.Object);

        var option = await sut.GetContentOption();

        option.Should().Be(ContentOption.UsePublished);
    }

    [TestMethod]
    public async Task WhenCookiesHasNoOptionValue_GetContentOption_ReturnsDefault()
    {
        var cookies = new Dictionary<string, string>();

        var cookieManager = new Mock<ICookieManager>();
        cookieManager.Setup(m => m.ReadInboundCookies())
                     .Returns(cookies);

        var sut = new ContentOptionsManager(NullLogger<ContentOptionsManager>.Instance, cookieManager.Object);

        var option = await sut.GetContentOption();

        option.Should().Be(ContentOption.UsePublished);
    }

    [TestMethod]
    public async Task WhenCookiesHasEmptyOptionValue_GetContentOption_ReturnsDefault()
    {
        var cookies = new Dictionary<string, string>
                      {
                          { ContentOptionsManager.OptionsCookieKey, "" }
                      };

        var cookieManager = new Mock<ICookieManager>();
        cookieManager.Setup(m => m.ReadInboundCookies())
                     .Returns(cookies);

        var sut =
            new ContentOptionsManager(NullLogger<ContentOptionsManager>.Instance, cookieManager.Object);

        var option = await sut.GetContentOption();

        option.Should().Be(ContentOption.UsePublished);
    }

    [TestMethod]
    public async Task WhenCookiesHasUnexpectedOptionValue_GetContentOption_ReturnsDefault()
    {
        var cookies = new Dictionary<string, string>
                      {
                          { ContentOptionsManager.OptionsCookieKey, "NotAValidValue" }
                      };

        var cookieManager = new Mock<ICookieManager>();
        cookieManager.Setup(m => m.ReadInboundCookies())
                     .Returns(cookies);

        var sut = new ContentOptionsManager(NullLogger<ContentOptionsManager>.Instance, cookieManager.Object);

        var option = await sut.GetContentOption();

        option.Should().Be(ContentOption.UsePublished);
    }

    [TestMethod]
    public async Task WhenCookiesHasUnexpectedOptionValue_GetContentOption_LogsWarning()
    {
        const string invalidOptionValue = "NotAValidValue";

        var cookies = new Dictionary<string, string>
                      {
                          { ContentOptionsManager.OptionsCookieKey, invalidOptionValue }
                      };

        var cookieManager = new Mock<ICookieManager>();
        cookieManager.Setup(m => m.ReadInboundCookies())
                     .Returns(cookies);

        var logger = new Mock<ILogger<ContentOptionsManager>>();

        var sut = new ContentOptionsManager(logger.Object, cookieManager.Object);

        await sut.GetContentOption();

        logger.VerifyWarning($@"User's content option set to unexpected value '{invalidOptionValue}'");
    }

    [TestMethod]
    public async Task WhenCookiesHasOptionDefault_GetContentOption_ReturnsDefault()
    {
        var cookies = new Dictionary<string, string>
                      {
                          { ContentOptionsManager.OptionsCookieKey, nameof(ContentOption.UsePublished) }
                      };

        var cookieManager = new Mock<ICookieManager>();
        cookieManager.Setup(m => m.ReadInboundCookies())
                     .Returns(cookies);

        var sut = new ContentOptionsManager(NullLogger<ContentOptionsManager>.Instance, cookieManager.Object);

        var option = await sut.GetContentOption();

        option.Should().Be(ContentOption.UsePublished);
    }
}