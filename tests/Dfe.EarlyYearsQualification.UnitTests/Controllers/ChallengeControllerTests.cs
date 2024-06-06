using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Filters;
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
        var controller = new ChallengeController(NullLogger<ChallengeController>.Instance);

        var result = await controller.Index("/", null);

        result.Should().BeAssignableTo<ContentResult>();
    }

    [TestMethod]
    public async Task GetChallenge_WithCorrectValue_RedirectsWithCookie()
    {
        var cookies = new Dictionary<string, string>();

        var cookiesMock = new Mock<IResponseCookies>();
        cookiesMock.Setup(c =>
                              c.Append(ChallengeResourceFilterAttribute.AuthSecretCookieName,
                                       ChallengeResourceFilterAttribute.Challenge))
                   .Callback((string k, string v) => cookies.Add(k, v));

        var mockContext = new Mock<HttpContext>();
        mockContext.SetupGet(c => c.Response.Cookies).Returns(cookiesMock.Object);

        var controller = new ChallengeController(NullLogger<ChallengeController>.Instance);
        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = mockContext.Object
                                       };

        var result = await controller.Index("/url", ChallengeResourceFilterAttribute.Challenge);

        result.Should().BeAssignableTo<RedirectResult>();

        cookies.Should().ContainKey(ChallengeResourceFilterAttribute.AuthSecretCookieName);
        cookies[ChallengeResourceFilterAttribute.AuthSecretCookieName].Should()
                                                                      .Be(ChallengeResourceFilterAttribute.Challenge);
    }
}