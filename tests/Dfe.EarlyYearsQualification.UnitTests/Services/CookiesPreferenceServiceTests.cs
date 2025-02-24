using System.Text.Json;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Services.Cookies;
using Dfe.EarlyYearsQualification.Web.Services.CookiesPreferenceService;
using Microsoft.AspNetCore.Http;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class CookieServiceTests
{
    private const string FakeGoogleAnalyticsSessionCookie = "_ga_TEST";

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void SetVisibility_PassedValue_SetsVisibilityToValuePassed(bool valueToSet)
    {
        var cookie = new DfeCookie { IsVisible = !valueToSet, HasApproved = true, IsRejected = false };
        var mockContext = SetContextWithPreferenceCookie(cookie);

        var cookieService = new CookiesPreferenceService(mockContext.Object);

        cookieService.SetVisibility(valueToSet);

        mockContext.Verify(x => x.DeleteOutboundCookie("cookies_preferences_set"), Times.Once);

        var serializedCookieToCheck = JsonSerializer.Serialize(new DfeCookie
                                                               {
                                                                   HasApproved = cookie.HasApproved,
                                                                   IsRejected = cookie.IsRejected,
                                                                   IsVisible = valueToSet
                                                               });

        CheckSerializedCookieWasSet(mockContext, serializedCookieToCheck);
    }

    [TestMethod]
    public void RejectCookies_MethodCalled_SetsCorrectCookie()
    {
        var cookie = new DfeCookie { IsVisible = false, HasApproved = true, IsRejected = false };
        var mockContext = SetContextWithPreferenceCookie(cookie);

        var cookieService = new CookiesPreferenceService(mockContext.Object);

        cookieService.RejectCookies();

        mockContext.Verify(x => x.DeleteOutboundCookie(CookieKeyNames.ClaritySessionKey), Times.Once);
        mockContext.Verify(x => x.DeleteOutboundCookie(CookieKeyNames.ClarityUserIdKey), Times.Once);
        mockContext.Verify(x => x.DeleteOutboundCookie(CookieKeyNames.GoogleAnalyticsKey), Times.Once);
        mockContext.Verify(x => x.DeleteOutboundCookie(FakeGoogleAnalyticsSessionCookie), Times.Once);
        mockContext.Verify(x => x.DeleteOutboundCookie(CookieKeyNames.CookiesPreferenceKey), Times.Once);

        var serializedCookieToCheck = JsonSerializer.Serialize(new DfeCookie
                                                               {
                                                                   HasApproved = false,
                                                                   IsRejected = true,
                                                                   IsVisible = true
                                                               });

        CheckSerializedCookieWasSet(mockContext, serializedCookieToCheck);
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void SetPreference_MethodCalled_CreatesNewCookieWithProvidedPreferences(bool preference)
    {
        var cookieToCheck = new DfeCookie
                            {
                                HasApproved = preference,
                                IsRejected = false,
                                IsVisible = true
                            };

        var mockContext = SetContextWithPreferenceCookie(cookieToCheck);
        var cookieService = new CookiesPreferenceService(mockContext.Object);

        cookieService.SetPreference(preference);

        var serializedCookieToCheck = JsonSerializer.Serialize(cookieToCheck);

        CheckSerializedCookieWasSet(mockContext, serializedCookieToCheck);
    }

    [TestMethod]
    public void GetCookie_HasCurrentCookie_GetsCookie()
    {
        var cookie = new DfeCookie { IsVisible = false, HasApproved = true, IsRejected = false };
        var mockContext = SetContextWithPreferenceCookie(cookie);

        var cookieService = new CookiesPreferenceService(mockContext.Object);

        var result = cookieService.GetCookie();

        result.Should().BeOfType<DfeCookie>();
        result.IsVisible.Should().BeFalse();
        result.HasApproved.Should().BeTrue();
        result.IsRejected.Should().BeFalse();
    }

    [TestMethod]
    public void GetCookie_CurrentCookieMalformed_ReturnsDefaultCookie()
    {
        var cookie = JsonSerializer.Serialize("Some randomness");

        var dictionary = new Dictionary<string, string> { { "cookies_preferences_set", cookie } };

        var cookiesMock = new Mock<IRequestCookieCollection>();
        cookiesMock.SetupGet(cookiesCollection => cookiesCollection["cookies_preferences_set"]).Returns(cookie);

        var mockContext = new Mock<ICookieManager>();
        mockContext.Setup(contextAccessor => contextAccessor.ReadInboundCookies()).Returns(dictionary);

        var cookieService = new CookiesPreferenceService(mockContext.Object);

        var result = cookieService.GetCookie();

        result.Should().BeOfType<DfeCookie>();
        result.IsVisible.Should().BeTrue();
        result.HasApproved.Should().BeFalse();
        result.IsRejected.Should().BeFalse();
    }

    [TestMethod]
    public void GetCookie_CurrentCookieNull_ReturnsDefaultCookie()
    {
        var cookie = JsonSerializer.Serialize<DfeCookie?>(null);
        var dictionary = new Dictionary<string, string> { { "cookies_preferences_set", cookie } };

        var mockContext = new Mock<ICookieManager>();
        mockContext.Setup(contextAccessor => contextAccessor.ReadInboundCookies()).Returns(dictionary);

        var cookieService = new CookiesPreferenceService(mockContext.Object);

        var result = cookieService.GetCookie();

        result.Should().BeOfType<DfeCookie>();
        result.IsVisible.Should().BeTrue();
        result.HasApproved.Should().BeFalse();
        result.IsRejected.Should().BeFalse();
    }

    [TestMethod]
    public void GetCookie_NoCookieFound_ReturnDefaultCookie()
    {
        var mockContext = new Mock<ICookieManager>();
        mockContext.Setup(c => c.ReadInboundCookies()).Returns(new Dictionary<string, string>());

        var cookieService = new CookiesPreferenceService(mockContext.Object);

        var result = cookieService.GetCookie();

        result.Should().BeOfType<DfeCookie>();
        result.IsVisible.Should().BeTrue();
        result.HasApproved.Should().BeFalse();
        result.IsRejected.Should().BeFalse();
    }

    private static Mock<ICookieManager> SetContextWithPreferenceCookie(DfeCookie model)
    {
        var serializedModel = JsonSerializer.Serialize(model);

        var mockManager = new Mock<ICookieManager>();

        var cookies = new Dictionary<string, string>
                      {
                          { CookieKeyNames.CookiesPreferenceKey, serializedModel },
                          { FakeGoogleAnalyticsSessionCookie, "test" }
                      };

        mockManager.Setup(m => m.ReadInboundCookies())
                   .Returns(cookies);

        mockManager.Setup(m => m.SetOutboundCookie(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()))
                   .Callback((string key, string value, CookieOptions _) => cookies[key] = value)
                   .Verifiable();

        return mockManager;
    }

    private static void CheckSerializedCookieWasSet(Mock<ICookieManager> mockContext,
                                                    string serializedCookieToCheck)
    {
        var in364Days = new DateTimeOffset(DateTime.Now.AddDays(364));
        var inOneYear = new DateTimeOffset(DateTime.Now.AddYears(1));

        mockContext
            .Verify(cookieManager =>
                        cookieManager.SetOutboundCookie(CookieKeyNames.CookiesPreferenceKey,
                                                        serializedCookieToCheck,
                                                        It.Is<CookieOptions>(
                                                                             options =>
                                                                                 options.Secure
                                                                                 && options.HttpOnly
                                                                                 && options.Expires > in364Days
                                                                                 && options.Expires < inOneYear)
                                                       ),
                    Times.Once);
    }
}