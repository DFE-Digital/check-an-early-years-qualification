using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class CacheControllerTests
{
    [TestMethod]
    public async Task NoSecretSubmittedInHeaderOrQuery_ReturnsUnauthorized()
    {
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(r => r.Headers).Returns(new HeaderDictionary());
        mockRequest.Setup(r => r.Query).Returns(new QueryCollection());

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);

        var cacheInvalidator = new Mock<ICacheInvalidator>();
        var configuration = new Mock<IConfiguration>();

        var controllerContext = new ControllerContext
                                {
                                    HttpContext = mockHttpContext.Object
                                };

        var controller =
            new CacheController(new NullLogger<CacheController>(),
                                cacheInvalidator.Object,
                                configuration.Object)
            {
                ControllerContext = controllerContext
            };

        var result = await controller.Index();

        result.Should().BeOfType<UnauthorizedResult>();
    }

    [TestMethod]
    public async Task SecretSubmitted_ButSecretValueNotConfigured_ReturnsUnauthorized()
    {
        const string secret = "secret";

        var mockRequest = new Mock<HttpRequest>();
        var headerDictionary = new HeaderDictionary { { "Cache-Secret", secret } };

        mockRequest.SetupGet(r => r.Headers).Returns(headerDictionary);
        mockRequest.SetupGet(r => r.Query).Returns(new QueryCollection());

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);

        var cacheInvalidator = new Mock<ICacheInvalidator>();
        var configuration = new Mock<IConfiguration>();

        var controllerContext = new ControllerContext
                                {
                                    HttpContext = mockHttpContext.Object
                                };

        var controller =
            new CacheController(new NullLogger<CacheController>(),
                                cacheInvalidator.Object,
                                configuration.Object)
            {
                ControllerContext = controllerContext
            };

        var action = async () => await controller.Index();

        await action.Should().ThrowAsync<InvalidOperationException>();
    }

    [TestMethod]
    public async Task CorrectSecretSubmittedInHeader_CallsCacheInvalidator()
    {
        const string secret = "secret";

        var headerDictionary = new HeaderDictionary { { "Cache-Secret", secret } };

        var mockRequest = new Mock<HttpRequest>();
        mockRequest.SetupGet(r => r.Headers).Returns(headerDictionary);
        mockRequest.SetupGet(r => r.Query).Returns(new QueryCollection());

        var mockHttpContext = new Mock<HttpContext>();

        mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);

        var cacheInvalidator = new Mock<ICacheInvalidator>();

        var section = new Mock<IConfigurationSection>();
        section.Setup(s => s["AuthSecret"]).Returns(secret);

        var configuration = new Mock<IConfiguration>();
        configuration.Setup(c => c.GetSection("Cache")).Returns(section.Object);

        var controllerContext = new ControllerContext
                                {
                                    HttpContext = mockHttpContext.Object
                                };

        var controller =
            new CacheController(new NullLogger<CacheController>(),
                                cacheInvalidator.Object,
                                configuration.Object)
            {
                ControllerContext = controllerContext
            };

        var result = await controller.Index();

        cacheInvalidator
            .Verify(x => x.ClearCacheAsync(It.IsAny<string>()),
                    Times.Once);

        result.Should().BeOfType<NoContentResult>();
    }

    [TestMethod]
    public async Task CorrectSecretSubmittedInQuery_CallsCacheInvalidator()
    {
        const string secret = "secret";

        var dic = new Dictionary<string, StringValues>
                  {
                      { "cache-secret", secret }
                  };

        var queryCollection = new QueryCollection(dic);
        var mockRequest = new Mock<HttpRequest>();

        mockRequest.Setup(r => r.Headers).Returns(new HeaderDictionary());
        mockRequest.Setup(r => r.Query).Returns(queryCollection);

        var mockHttpContext = new Mock<HttpContext>();

        mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);

        var cacheInvalidator = new Mock<ICacheInvalidator>();

        var section = new Mock<IConfigurationSection>();
        section.Setup(s => s["AuthSecret"]).Returns(secret);

        var configuration = new Mock<IConfiguration>();
        configuration.Setup(c => c.GetSection("Cache")).Returns(section.Object);

        var controllerContext = new ControllerContext
                                {
                                    HttpContext = mockHttpContext.Object
                                };

        var controller =
            new CacheController(new NullLogger<CacheController>(),
                                cacheInvalidator.Object,
                                configuration.Object)
            {
                ControllerContext = controllerContext
            };

        var result = await controller.Index();

        cacheInvalidator
            .Verify(x => x.ClearCacheAsync(It.IsAny<string>()),
                    Times.Once);

        result.Should().BeOfType<NoContentResult>();
    }

    [TestMethod]
    public async Task SecretSubmittedInHeaderAndQuery_IgnoresQuery_WhenHeaderSecretCorrect_CallsCacheInvalidator()
    {
        const string configuredSecret = "secret";
        const string querySecret = "different secret";

        var dic = new Dictionary<string, StringValues>
                  {
                      { "cache-secret", querySecret }
                  };

        var queryCollection = new QueryCollection(dic);

        var headerDictionary = new HeaderDictionary { { "Cache-Secret", configuredSecret } };

        var mockRequest = new Mock<HttpRequest>();

        mockRequest.Setup(r => r.Headers).Returns(headerDictionary);
        mockRequest.Setup(r => r.Query).Returns(queryCollection);

        var mockHttpContext = new Mock<HttpContext>();

        mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);

        var cacheInvalidator = new Mock<ICacheInvalidator>();

        var section = new Mock<IConfigurationSection>();
        section.Setup(s => s["AuthSecret"]).Returns(configuredSecret);

        var configuration = new Mock<IConfiguration>();
        configuration.Setup(c => c.GetSection("Cache")).Returns(section.Object);

        var controllerContext = new ControllerContext
                                {
                                    HttpContext = mockHttpContext.Object
                                };

        var controller =
            new CacheController(new NullLogger<CacheController>(),
                                cacheInvalidator.Object,
                                configuration.Object)
            {
                ControllerContext = controllerContext
            };

        var result = await controller.Index();

        cacheInvalidator
            .Verify(x => x.ClearCacheAsync(It.IsAny<string>()),
                    Times.Once);

        result.Should().BeOfType<NoContentResult>();
    }

    [TestMethod]
    public async Task IncorrectSecretSubmittedInHeader_ReturnsUnauthorized()
    {
        const string configuredSecret = "secret";
        const string submittedSecret = "wrong secret";

        var headerDictionary = new HeaderDictionary { { "Cache-Secret", submittedSecret } };

        var mockRequest = new Mock<HttpRequest>();
        mockRequest.SetupGet(r => r.Headers).Returns(headerDictionary);
        mockRequest.SetupGet(r => r.Query).Returns(new QueryCollection());

        var mockHttpContext = new Mock<HttpContext>();

        mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);

        var cacheInvalidator = new Mock<ICacheInvalidator>();

        var section = new Mock<IConfigurationSection>();
        section.Setup(s => s["AuthSecret"]).Returns(configuredSecret);

        var configuration = new Mock<IConfiguration>();
        configuration.Setup(c => c.GetSection("Cache")).Returns(section.Object);

        var controllerContext = new ControllerContext
                                {
                                    HttpContext = mockHttpContext.Object
                                };

        var controller =
            new CacheController(new NullLogger<CacheController>(),
                                cacheInvalidator.Object,
                                configuration.Object)
            {
                ControllerContext = controllerContext
            };

        var result = await controller.Index();

        cacheInvalidator
            .Verify(x => x.ClearCacheAsync(It.IsAny<string>()),
                    Times.Never);

        result.Should().BeOfType<UnauthorizedResult>();
    }

    [TestMethod]
    public async Task IncorrectSecretSubmittedInQuery_ReturnsUnauthorized()
    {
        const string configuredSecret = "secret";
        const string submittedSecret = "wrong secret";

        var dic = new Dictionary<string, StringValues>
                  {
                      { "cache-secret", submittedSecret }
                  };

        var queryCollection = new QueryCollection(dic);
        var mockRequest = new Mock<HttpRequest>();

        mockRequest.Setup(r => r.Headers).Returns(new HeaderDictionary());
        mockRequest.Setup(r => r.Query).Returns(queryCollection);

        var mockHttpContext = new Mock<HttpContext>();

        mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);

        var cacheInvalidator = new Mock<ICacheInvalidator>();

        var section = new Mock<IConfigurationSection>();
        section.Setup(s => s["AuthSecret"]).Returns(configuredSecret);

        var configuration = new Mock<IConfiguration>();
        configuration.Setup(c => c.GetSection("Cache")).Returns(section.Object);

        var controllerContext = new ControllerContext
                                {
                                    HttpContext = mockHttpContext.Object
                                };

        var controller =
            new CacheController(new NullLogger<CacheController>(),
                                cacheInvalidator.Object,
                                configuration.Object)
            {
                ControllerContext = controllerContext
            };

        var result = await controller.Index();

        cacheInvalidator
            .Verify(x => x.ClearCacheAsync(It.IsAny<string>()),
                    Times.Never);

        result.Should().BeOfType<UnauthorizedResult>();
    }

    [TestMethod]
    public void CacheController_Has_IgnoreAntiForgeryAttribute()
    {
        object[] customAttributes = typeof(CacheController).GetCustomAttributes(false);

        customAttributes.Should().Contain(x => x is IgnoreAntiforgeryTokenAttribute,
                                          "Anti-forgery not used for API POST");
    }
}