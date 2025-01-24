using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Services.CookiesPreferenceService;
using Microsoft.Extensions.Configuration;

namespace Dfe.EarlyYearsQualification.UnitTests.Helpers;

[TestClass]
public class TrackingConfigurationTests
{
    [TestMethod]
    public void UseCookies_ReturnsFalse()
    {
        var mockCookiePreferenceService = new Mock<ICookiesPreferenceService>();
        var mockConfiguration = new Mock<IConfiguration>();

        mockCookiePreferenceService.Setup(x => x.GetCookie()).Returns(new DfeCookie());

        var trackingConfiguration = new TrackingConfiguration(mockCookiePreferenceService.Object, mockConfiguration.Object);

        trackingConfiguration.UseCookies.Should().BeFalse();
    }
    
    [TestMethod]
    public void UseCookies_ReturnsTrue()
    {
        var mockCookiePreferenceService = new Mock<ICookiesPreferenceService>();
        var mockConfiguration = new Mock<IConfiguration>();

        mockCookiePreferenceService.Setup(x => x.GetCookie()).Returns(new DfeCookie { HasApproved = true });

        var trackingConfiguration = new TrackingConfiguration(mockCookiePreferenceService.Object, mockConfiguration.Object);

        trackingConfiguration.UseCookies.Should().BeTrue();
    }
    
    [TestMethod]
    public void GtmTag_ReturnsEmpty()
    {
        var mockCookiePreferenceService = new Mock<ICookiesPreferenceService>();
        var mockConfiguration = new Mock<IConfiguration>();
        var mockSection = new Mock<IConfigurationSection>();
        mockSection.Setup(x => x.Value).Returns(value: null);
        mockConfiguration.Setup(x => x.GetSection("GTM:Tag")).Returns(mockSection.Object);
        
        var trackingConfiguration = new TrackingConfiguration(mockCookiePreferenceService.Object, mockConfiguration.Object);

        trackingConfiguration.GtmTag.Should().BeEmpty();
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
        
        var trackingConfiguration = new TrackingConfiguration(mockCookiePreferenceService.Object, mockConfiguration.Object);

        trackingConfiguration.GtmTag.Should().Be(tag);
    }
    
    [TestMethod]
    public void ClarityTag_ReturnsEmpty()
    {
        var mockCookiePreferenceService = new Mock<ICookiesPreferenceService>();
        var mockConfiguration = new Mock<IConfiguration>();
        var mockSection = new Mock<IConfigurationSection>();
        mockSection.Setup(x => x.Value).Returns(value: null);
        mockConfiguration.Setup(x => x.GetSection("Clarity:Tag")).Returns(mockSection.Object);
        
        var trackingConfiguration = new TrackingConfiguration(mockCookiePreferenceService.Object, mockConfiguration.Object);

        trackingConfiguration.ClarityTag.Should().BeEmpty();
    }
    
    [TestMethod]
    public void ClarityTag_ReturnsConfigValue()
    {
        const string tag = "TEST-TAG";
        var mockCookiePreferenceService = new Mock<ICookiesPreferenceService>();
        var mockConfiguration = new Mock<IConfiguration>();
        var mockSection = new Mock<IConfigurationSection>();
        mockSection.Setup(x => x.Value).Returns(tag);
        mockConfiguration.Setup(x => x.GetSection("Clarity:Tag")).Returns(mockSection.Object);
        
        var trackingConfiguration = new TrackingConfiguration(mockCookiePreferenceService.Object, mockConfiguration.Object);

        trackingConfiguration.ClarityTag.Should().Be(tag);
    }
}