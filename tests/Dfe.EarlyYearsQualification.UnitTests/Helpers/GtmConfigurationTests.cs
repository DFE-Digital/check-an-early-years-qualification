using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Services.CookiesPreferenceService;
using Microsoft.Extensions.Configuration;
using FluentAssertions;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Helpers;

[TestClass]
public class GtmConfigurationTests
{
    [TestMethod]
    public void UseCookies_ReturnsFalse()
    {
        var mockCookiePreferenceService = new Mock<ICookiesPreferenceService>();
        var mockConfiguration = new Mock<IConfiguration>();

        mockCookiePreferenceService.Setup(x => x.GetCookie()).Returns(new DfeCookie());

        var gtmConfiguration = new GtmConfiguration(mockCookiePreferenceService.Object, mockConfiguration.Object);

        gtmConfiguration.UseCookies.Should().BeFalse();
    }
    
    [TestMethod]
    public void UseCookies_ReturnsTrue()
    {
        var mockCookiePreferenceService = new Mock<ICookiesPreferenceService>();
        var mockConfiguration = new Mock<IConfiguration>();

        mockCookiePreferenceService.Setup(x => x.GetCookie()).Returns(new DfeCookie { HasApproved = true });

        var gtmConfiguration = new GtmConfiguration(mockCookiePreferenceService.Object, mockConfiguration.Object);

        gtmConfiguration.UseCookies.Should().BeTrue();
    }
    
    [TestMethod]
    public void GtmTag_ReturnsEmpty()
    {
        var mockCookiePreferenceService = new Mock<ICookiesPreferenceService>();
        var mockConfiguration = new Mock<IConfiguration>();
        var mockSection = new Mock<IConfigurationSection>();
        mockSection.Setup(x => x.Value).Returns((string)null);
        mockConfiguration.Setup(x => x.GetSection("GTM:Tag")).Returns(mockSection.Object);
        
        var gtmConfiguration = new GtmConfiguration(mockCookiePreferenceService.Object, mockConfiguration.Object);

        gtmConfiguration.GtmTag.Should().BeEmpty();
    }
    
    [TestMethod]
    public void GtmTag_ReturnsConfigValue()
    {
        const string tag = "TEST-TAG";
        var mockCookiePreferenceService = new Mock<ICookiesPreferenceService>();
        var mockConfiguration = new Mock<IConfiguration>();
        var mockSection = new Mock<IConfigurationSection>();
        mockSection.Setup(x => x.Value).Returns(tag);
        mockConfiguration.Setup(x => x.GetSection("GTM:Tag")).Returns(mockSection.Object);
        
        var gtmConfiguration = new GtmConfiguration(mockCookiePreferenceService.Object, mockConfiguration.Object);

        gtmConfiguration.GtmTag.Should().Be(tag);
    }
}