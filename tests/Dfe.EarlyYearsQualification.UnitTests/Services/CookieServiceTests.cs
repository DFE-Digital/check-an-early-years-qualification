using System.Text.Json;
using Dfe.EarlyYearsQualification.Web.Services.CookiesPreferenceService;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class CookieServiceTests
{
    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void SetVisibility_PassedValue_SetsVisibilityToValuePassed(bool valueToSet)
    {
        var cookie = new DfeCookie { IsVisible = !valueToSet, HasApproved = true, IsRejected = false };
        var mockContext = SetContextWithPreferenceCookie(cookie);

        var cookieService = new CookiesPreferenceService(mockContext.Object);

        cookieService.SetVisibility(valueToSet);

        mockContext.Verify(x => x.HttpContext!.Response.Cookies.Delete("cookies_preferences_set"), Times.Once);

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

        mockContext.Verify(x => x.HttpContext!.Response.Cookies.Delete("cookies_preferences_set"), Times.Once);

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
        var cookiesMock = new Mock<IRequestCookieCollection>();
        cookiesMock.SetupGet(cookiesCollection => cookiesCollection["cookies_preferences_set"]).Returns(cookie);

        var mockContext = new Mock<IHttpContextAccessor>();
        mockContext.Setup(contextAccessor => contextAccessor.HttpContext!.Request.Cookies).Returns(cookiesMock.Object);
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
        var cookiesMock = new Mock<IRequestCookieCollection>();
        cookiesMock.SetupGet(cookiesCollection => cookiesCollection["cookies_preferences_set"]).Returns(cookie);

        var mockContext = new Mock<IHttpContextAccessor>();
        mockContext.Setup(contextAccessor => contextAccessor.HttpContext!.Request.Cookies).Returns(cookiesMock.Object);
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
        var mockContext = new Mock<IHttpContextAccessor>();
        var cookieService = new CookiesPreferenceService(mockContext.Object);

        var result = cookieService.GetCookie();

        result.Should().BeOfType<DfeCookie>();
        result.IsVisible.Should().BeTrue();
        result.HasApproved.Should().BeFalse();
        result.IsRejected.Should().BeFalse();
    }

    private static Mock<IHttpContextAccessor> SetContextWithPreferenceCookie(DfeCookie cookie)
    {
        var serializedCookie = JsonSerializer.Serialize(cookie);

        var requestCookiesMock = new Mock<IRequestCookieCollection>();
        var responseCookiesMock = new Mock<IResponseCookies>();

        requestCookiesMock.Setup(cookiesCollection => cookiesCollection["cookies_preferences_set"])
                          .Returns(serializedCookie);
        responseCookiesMock.Setup(x => x.Delete(It.IsAny<string>())).Verifiable();

        var httpContextMock = new Mock<IHttpContextAccessor>();
        httpContextMock.Setup(contextAccessor => contextAccessor.HttpContext!.Request.Cookies)
                       .Returns(requestCookiesMock.Object);
        httpContextMock.Setup(contextAccessor => contextAccessor.HttpContext!.Response.Cookies)
                       .Returns(responseCookiesMock.Object);
        return httpContextMock;
    }

    private static void CheckSerializedCookieWasSet(Mock<IHttpContextAccessor> mockContext,
                                                    string serializedCookieToCheck)
    {
        var in364Days = new DateTimeOffset(DateTime.Now.AddDays(364));
        var inOneYear = new DateTimeOffset(DateTime.Now.AddYears(1));

        mockContext
            .Verify(http =>
                        http.HttpContext!.Response.Cookies.Append("cookies_preferences_set",
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