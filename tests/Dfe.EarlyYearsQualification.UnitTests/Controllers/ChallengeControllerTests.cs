using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Filters;
using Dfe.EarlyYearsQualification.Web.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class ChallengeControllerTests
{
    [TestMethod]
    public async Task GetChallenge_Returns_Page()
    {
        const string from = "/";

        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper.Setup(u => u.IsLocalUrl(It.IsAny<string?>()))
                     .Returns(true);

        var controller = new ChallengeController(NullLogger<ChallengeController>.Instance,
                                                 mockUrlHelper.Object);

        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = new DefaultHttpContext()
                                       };

        var result = await controller.Index(new ChallengeModel { RedirectAddress = "/", Value = null });

        result.Should().BeAssignableTo<ViewResult>();

        var content = (ViewResult)result;

        content.Model.Should().BeAssignableTo<ChallengeModel>()
               .Which
               .RedirectAddress.Should().Be(from);
    }

    [TestMethod]
    public async Task GetChallenge_NonLocalFrom_Returns_Page_With_BaseFrom()
    {
        const string from = "https://google.co.uk";

        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper.Setup(u => u.IsLocalUrl(It.IsAny<string?>()))
                     .Returns(false);

        var controller = new ChallengeController(NullLogger<ChallengeController>.Instance,
                                                 mockUrlHelper.Object);

        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = new DefaultHttpContext()
                                       };

        var result = await controller.Index(new ChallengeModel { RedirectAddress = from, Value = null });

        result.Should().BeAssignableTo<ViewResult>();

        var content = (ViewResult)result;

        content.Model.Should().BeAssignableTo<ChallengeModel>()
               .Which
               .RedirectAddress.Should().Be("/");
    }

    [TestMethod]
    public async Task PostChallenge_WithCorrectValue_RedirectsWithCookie()
    {
        const string from = "/cookies";
        const string accessKey = "CX";

        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper.Setup(u => u.IsLocalUrl(It.IsAny<string?>()))
                     .Returns(true);

        var cookies = new Dictionary<string, string>();

        var cookiesMock = new Mock<IResponseCookies>();
        cookiesMock.Setup(c =>
                              c.Append(ChallengeResourceFilterAttribute.AuthSecretCookieName,
                                       accessKey))
                   .Callback((string k, string v) => cookies.Add(k, v));

        var mockContext = new Mock<HttpContext>();
        mockContext.SetupGet(c => c.Response.Cookies).Returns(cookiesMock.Object);

        var controller = new ChallengeController(NullLogger<ChallengeController>.Instance,
                                                 mockUrlHelper.Object);
        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = mockContext.Object
                                       };

        var result = await controller.Post(new ChallengeModel
                                           {
                                               RedirectAddress = from,
                                               Value = accessKey
                                           });

        result.Should().BeAssignableTo<RedirectResult>();

        var redirect = (RedirectResult)result;
        redirect.Url.Should().Be("/cookies");

        cookies.Should().ContainKey(ChallengeResourceFilterAttribute.AuthSecretCookieName);
        cookies[ChallengeResourceFilterAttribute.AuthSecretCookieName].Should()
                                                                      .Be(accessKey);
    }

    [TestMethod]
    public async Task PostChallenge_WithCorrectValue_ButNonLocalFrom_RedirectsWithCookie_ToBaseUrl()
    {
        const string from = "https://google.co.uk";
        const string accessKey = "CX";

        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper.Setup(u => u.IsLocalUrl(It.IsAny<string?>()))
                     .Returns(false); // NB: behaviour relies on UrlHelper correctly determining non-local URLs

        var cookies = new Dictionary<string, string>();

        var cookiesMock = new Mock<IResponseCookies>();
        cookiesMock.Setup(c =>
                              c.Append(ChallengeResourceFilterAttribute.AuthSecretCookieName,
                                       accessKey))
                   .Callback((string k, string v) => cookies.Add(k, v));

        var mockContext = new Mock<HttpContext>();
        mockContext.SetupGet(c => c.Response.Cookies).Returns(cookiesMock.Object);

        var controller = new ChallengeController(NullLogger<ChallengeController>.Instance,
                                                 mockUrlHelper.Object);
        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = mockContext.Object
                                       };

        var result = await controller.Post(new ChallengeModel
                                           {
                                               RedirectAddress = from,
                                               Value = accessKey
                                           });

        result.Should().BeAssignableTo<RedirectResult>();

        var redirect = (RedirectResult)result;
        redirect.Url.Should().Be("/");

        cookies.Should().ContainKey(ChallengeResourceFilterAttribute.AuthSecretCookieName);
        cookies[ChallengeResourceFilterAttribute.AuthSecretCookieName].Should()
                                                                      .Be(accessKey);
    }
}