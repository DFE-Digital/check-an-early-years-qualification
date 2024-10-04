using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Content;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Filters;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        var contentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();

        mockUrlHelper.Setup(u => u.IsLocalUrl(It.IsAny<string?>()))
                     .Returns(true);

        var mockContentfulService = new MockContentfulService();

        contentService.Setup(x => x.GetChallengePage()).Returns(mockContentfulService.GetChallengePage());

        var controller = new ChallengeController(NullLogger<ChallengeController>.Instance,
                                                 mockUrlHelper.Object,
                                                 contentService.Object,
                                                 mockContentParser.Object,
                                                 accessKeysHelper.Object);

        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = new DefaultHttpContext()
                                       };

        var result = await controller.Index(new ChallengePageModel { RedirectAddress = "/" });

        result.Should().BeAssignableTo<ViewResult>();

        var content = (ViewResult)result;

        content.Model.Should().BeAssignableTo<ChallengePageModel>()
               .Which
               .RedirectAddress.Should().Be(from);
    }

    [TestMethod]
    public async Task GetChallenge_NonLocalFrom_Returns_Page_With_BaseFrom()
    {
        const string from = "https://google.co.uk";

        var mockUrlHelper = new Mock<IUrlHelper>();
        var contentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();

        var mockContentfulService = new MockContentfulService();

        contentService.Setup(x => x.GetChallengePage()).Returns(mockContentfulService.GetChallengePage());

        mockUrlHelper.Setup(u => u.IsLocalUrl(It.IsAny<string?>()))
                     .Returns(false);

        var controller = new ChallengeController(NullLogger<ChallengeController>.Instance,
                                                 mockUrlHelper.Object,
                                                 contentService.Object,
                                                 mockContentParser.Object,
                                                 accessKeysHelper.Object);

        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = new DefaultHttpContext()
                                       };

        var result = await controller.Index(new ChallengePageModel { RedirectAddress = from });

        result.Should().BeAssignableTo<ViewResult>();

        var content = (ViewResult)result;

        content.Model.Should().BeAssignableTo<ChallengePageModel>()
               .Which
               .RedirectAddress.Should().Be("/");
    }

    [TestMethod]
    public async Task GetChallenge_WithInvalidModel_LogsWarning()
    {
        var mockLogger = new Mock<ILogger<ChallengeController>>();

        var mockUrlHelper = new Mock<IUrlHelper>();
        var contentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();

        var mockContentfulService = new MockContentfulService();

        contentService.Setup(x => x.GetChallengePage()).Returns(mockContentfulService.GetChallengePage());

        var controller = new ChallengeController(mockLogger.Object,
                                                 mockUrlHelper.Object,
                                                 contentService.Object,
                                                 mockContentParser.Object,
                                                 accessKeysHelper.Object);

        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = new DefaultHttpContext()
                                       };

        controller.ModelState.AddModelError("test", "error");

        await controller.Index(new ChallengePageModel { RedirectAddress = "/" });

        mockLogger.VerifyWarning("Invalid challenge model (get)");
    }

    [TestMethod]
    public async Task PostChallenge_WithSecretValue_RedirectsWithKeyInCookie()
    {
        const string from = "/cookies";
        const string accessKey = "Key";

        var mockUrlHelper = new Mock<IUrlHelper>();
        var contentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();

        var accessKeys = new List<string>
                         {
                             accessKey
                         };

        accessKeysHelper.Setup(x => x.ConfiguredKeys).Returns(accessKeys);

        var mockContentfulService = new MockContentfulService();

        contentService.Setup(x => x.GetChallengePage()).Returns(mockContentfulService.GetChallengePage());

        mockUrlHelper.Setup(u => u.IsLocalUrl(It.IsAny<string?>()))
                     .Returns(true);

        var cookies = new Dictionary<string, Tuple<string, CookieOptions>>();

        var cookiesMock = new Mock<IResponseCookies>();
        cookiesMock.Setup(c =>
                              c.Append(ChallengeResourceFilterAttribute.AuthSecretCookieName,
                                       accessKey,
                                       It.IsAny<CookieOptions>()))
                   .Callback((string k, string v, CookieOptions o) =>
                                 cookies.Add(k, new Tuple<string, CookieOptions>(v, o)));

        var mockContext = new Mock<HttpContext>();
        mockContext.SetupGet(c => c.Response.Cookies).Returns(cookiesMock.Object);

        var controller = new ChallengeController(NullLogger<ChallengeController>.Instance,
                                                 mockUrlHelper.Object,
                                                 contentService.Object,
                                                 mockContentParser.Object,
                                                 accessKeysHelper.Object);

        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = mockContext.Object
                                       };

        var result = await controller.Post(new ChallengePageModel
                                           {
                                               RedirectAddress = from,
                                               PasswordValue = accessKey
                                           });

        result.Should().BeAssignableTo<RedirectResult>();

        var redirect = (RedirectResult)result;
        redirect.Url.Should().Be("/cookies");

        cookies.Should().ContainKey(ChallengeResourceFilterAttribute.AuthSecretCookieName);
        cookies[ChallengeResourceFilterAttribute.AuthSecretCookieName].Item1
                                                                      .Should().Be(accessKey);
        cookies[ChallengeResourceFilterAttribute.AuthSecretCookieName].Item2.HttpOnly
                                                                      .Should().BeTrue();
    }

    [TestMethod]
    public async Task PostChallenge_WithSecretValueWithWhitespaceStartAndEnd_RedirectsWithTrimmedKeyInCookie()
    {
        const string from = "/cookies";
        const string accessKey = "Key";

        var mockUrlHelper = new Mock<IUrlHelper>();
        var contentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();

        var accessKeys = new List<string>
                         {
                             accessKey
                         };

        accessKeysHelper.Setup(x => x.ConfiguredKeys).Returns(accessKeys);

        var mockContentfulService = new MockContentfulService();

        contentService.Setup(x => x.GetChallengePage()).Returns(mockContentfulService.GetChallengePage());

        mockUrlHelper.Setup(u => u.IsLocalUrl(It.IsAny<string?>()))
                     .Returns(true);

        var cookies = new Dictionary<string, Tuple<string, CookieOptions>>();

        var cookiesMock = new Mock<IResponseCookies>();
        cookiesMock.Setup(c =>
                              c.Append(ChallengeResourceFilterAttribute.AuthSecretCookieName,
                                       accessKey,
                                       It.IsAny<CookieOptions>()))
                   .Callback((string k, string v, CookieOptions o) =>
                                 cookies.Add(k, new Tuple<string, CookieOptions>(v, o)));

        var mockContext = new Mock<HttpContext>();
        mockContext.SetupGet(c => c.Response.Cookies).Returns(cookiesMock.Object);

        var controller = new ChallengeController(NullLogger<ChallengeController>.Instance,
                                                 mockUrlHelper.Object,
                                                 contentService.Object,
                                                 mockContentParser.Object,
                                                 accessKeysHelper.Object);

        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = mockContext.Object
                                       };

        var result = await controller.Post(new ChallengePageModel
                                           {
                                               RedirectAddress = from,
                                               PasswordValue = $"  {accessKey}  "
                                           });

        result.Should().BeAssignableTo<RedirectResult>();

        var redirect = (RedirectResult)result;
        redirect.Url.Should().Be("/cookies");

        cookies.Should().ContainKey(ChallengeResourceFilterAttribute.AuthSecretCookieName);
        cookies[ChallengeResourceFilterAttribute.AuthSecretCookieName].Item1
                                                                      .Should().Be(accessKey);
        cookies[ChallengeResourceFilterAttribute.AuthSecretCookieName].Item2.HttpOnly
                                                                      .Should().BeTrue();
    }

    [TestMethod]
    public async Task PostChallenge_WithSecretValue_ButNonLocalFrom_RedirectsWithSecretInCookie_ToBaseUrl()
    {
        const string from = "https://google.co.uk";
        const string accessKey = "CX";

        var mockUrlHelper = new Mock<IUrlHelper>();
        var contentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();

        var accessKeys = new List<string>
                         {
                             accessKey
                         };

        accessKeysHelper.Setup(x => x.ConfiguredKeys).Returns(accessKeys);

        var mockContentfulService = new MockContentfulService();

        contentService.Setup(x => x.GetChallengePage()).Returns(mockContentfulService.GetChallengePage());

        mockUrlHelper.Setup(u => u.IsLocalUrl(It.IsAny<string?>()))
                     .Returns(false); // NB: behaviour relies on UrlHelper correctly determining non-local URLs

        var cookies = new Dictionary<string, Tuple<string, CookieOptions>>();

        var cookiesMock = new Mock<IResponseCookies>();
        cookiesMock.Setup(c =>
                              c.Append(ChallengeResourceFilterAttribute.AuthSecretCookieName,
                                       accessKey,
                                       It.IsAny<CookieOptions>()))
                   .Callback((string k, string v, CookieOptions o) =>
                                 cookies.Add(k, new Tuple<string, CookieOptions>(v, o)));

        var mockContext = new Mock<HttpContext>();
        mockContext.SetupGet(c => c.Response.Cookies).Returns(cookiesMock.Object);

        var controller = new ChallengeController(NullLogger<ChallengeController>.Instance,
                                                 mockUrlHelper.Object,
                                                 contentService.Object,
                                                 mockContentParser.Object,
                                                 accessKeysHelper.Object);

        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = mockContext.Object
                                       };

        var result = await controller.Post(new ChallengePageModel
                                           {
                                               RedirectAddress = from,
                                               PasswordValue = accessKey
                                           });

        result.Should().BeAssignableTo<RedirectResult>();

        var redirect = (RedirectResult)result;
        redirect.Url.Should().Be("/");

        cookies.Should().ContainKey(ChallengeResourceFilterAttribute.AuthSecretCookieName);
        cookies[ChallengeResourceFilterAttribute.AuthSecretCookieName].Item1
                                                                      .Should().Be(accessKey);
        cookies[ChallengeResourceFilterAttribute.AuthSecretCookieName].Item2.HttpOnly
                                                                      .Should().BeTrue();
    }

    [TestMethod]
    public async Task PostChallenge_WithEmptySecretValue_ReturnsPageWithErrorMessage()
    {
        var mockUrlHelper = new Mock<IUrlHelper>();
        var contentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();

        var mockContentfulService = new MockContentfulService();

        var content = await mockContentfulService.GetChallengePage();

        contentService.Setup(x => x.GetChallengePage()).ReturnsAsync(content);

        mockUrlHelper.Setup(u => u.IsLocalUrl(It.IsAny<string?>()))
                     .Returns(true);

        var controller = new ChallengeController(NullLogger<ChallengeController>.Instance,
                                                 mockUrlHelper.Object,
                                                 contentService.Object,
                                                 mockContentParser.Object,
                                                 accessKeysHelper.Object);

        var result = await controller.Post(new ChallengePageModel { RedirectAddress = "/", PasswordValue = " \t " });

        result.Should().BeAssignableTo<ViewResult>();

        var viewResult = result as ViewResult;

        viewResult!.Model.Should().BeAssignableTo<ChallengePageModel>();

        var modelToReturn = viewResult.Model as ChallengePageModel;

        modelToReturn!.ErrorSummaryModel.Should().NotBeNull();
        modelToReturn.ErrorSummaryModel!.ErrorBannerLinkText.Should().Be(content!.MissingPasswordText);
    }

    [TestMethod]
    public async Task PostChallenge_WithInvalidModel_LogsWarning()
    {
        var mockLogger = new Mock<ILogger<ChallengeController>>();

        var mockUrlHelper = new Mock<IUrlHelper>();
        var contentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();

        var mockContentfulService = new MockContentfulService();

        contentService.Setup(x => x.GetChallengePage()).Returns(mockContentfulService.GetChallengePage());

        var controller = new ChallengeController(mockLogger.Object,
                                                 mockUrlHelper.Object,
                                                 contentService.Object,
                                                 mockContentParser.Object,
                                                 accessKeysHelper.Object);

        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = new DefaultHttpContext()
                                       };

        controller.ModelState.AddModelError("test", "error");

        await controller.Post(new ChallengePageModel { RedirectAddress = "/" });

        mockLogger.VerifyWarning("Invalid challenge model (post)");
    }

    [TestMethod]
    public async Task Index_NoContent_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<ChallengeController>>();

        var mockUrlHelper = new Mock<IUrlHelper>();
        var contentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();

        var controller = new ChallengeController(mockLogger.Object,
                                                 mockUrlHelper.Object,
                                                 contentService.Object,
                                                 mockContentParser.Object,
                                                 accessKeysHelper.Object);

        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = new DefaultHttpContext()
                                       };

        var result = await controller.Index(new ChallengePageModel { RedirectAddress = "/" });

        result.Should().BeEquivalentTo(new RedirectToActionResult("Index", "Error", null),
                                       options => options.ExcludingMissingMembers());

        mockLogger.VerifyError("No content for the challenge page");
    }

    [TestMethod]
    public async Task Post_NoContent_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<ChallengeController>>();

        var mockUrlHelper = new Mock<IUrlHelper>();
        var contentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();

        var controller = new ChallengeController(mockLogger.Object,
                                                 mockUrlHelper.Object,
                                                 contentService.Object,
                                                 mockContentParser.Object,
                                                 accessKeysHelper.Object);

        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = new DefaultHttpContext()
                                       };

        var result = await controller.Post(new ChallengePageModel { RedirectAddress = "/" });

        result.Should().BeEquivalentTo(new RedirectToActionResult("Index", "Error", null),
                                       options => options.ExcludingMissingMembers());

        mockLogger.VerifyError("No content for the challenge page");
    }

    [TestMethod]
    public async Task Post_PasswordPassedIsNotCorrect_ReturnsWithErrorMessage()
    {
        var mockLogger = new Mock<ILogger<ChallengeController>>();

        var mockUrlHelper = new Mock<IUrlHelper>();
        var contentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();

        var mockContentfulService = new MockContentfulService();
        var content = await mockContentfulService.GetChallengePage();

        contentService.Setup(x => x.GetChallengePage()).ReturnsAsync(content);

        accessKeysHelper.Setup(x => x.ConfiguredKeys).Returns(new List<string>
                                                              {
                                                                  "Some Key"
                                                              });

        var controller = new ChallengeController(mockLogger.Object,
                                                 mockUrlHelper.Object,
                                                 contentService.Object,
                                                 mockContentParser.Object,
                                                 accessKeysHelper.Object);

        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = new DefaultHttpContext()
                                       };

        var result = await controller.Post(new ChallengePageModel
                                           { RedirectAddress = "/", PasswordValue = "Not A Correct Key" });

        result.Should().BeAssignableTo<ViewResult>();

        var viewResult = result as ViewResult;

        viewResult!.Model.Should().BeAssignableTo<ChallengePageModel>();

        var modelToReturn = viewResult.Model as ChallengePageModel;

        modelToReturn!.ErrorSummaryModel.Should().NotBeNull();
        modelToReturn.ErrorSummaryModel!.ErrorBannerLinkText.Should().Be(content!.IncorrectPasswordText);
    }
}