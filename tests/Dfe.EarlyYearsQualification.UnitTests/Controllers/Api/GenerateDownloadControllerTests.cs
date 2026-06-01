using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers.Api;

[TestClass]
public class GenerateDownloadControllerTests
{
    [TestMethod]
    public async Task Index_NoSecretSubmittedInHeaderOrQuery_ReturnsUnauthorized()
    {
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(r => r.Headers).Returns(new HeaderDictionary());
        mockRequest.Setup(r => r.Query).Returns(new QueryCollection());

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);

        var mockQualificationDownloadService = new Mock<IQualificationDownloadService>();
        var mockConfiguration = new Mock<IConfiguration>();

        var controllerContext = new ControllerContext
                                {
                                    HttpContext = mockHttpContext.Object
                                };

        var controller =
            new GenerateDownloadController(new NullLogger<GenerateDownloadController>(),
                                           mockQualificationDownloadService.Object,
                                           mockConfiguration.Object)
            {
                ControllerContext = controllerContext
            };

        var result = await controller.Index();

        result.Should().BeOfType<UnauthorizedResult>();
    }
    
    [TestMethod]
    public async Task Index_SecretSubmitted_ButSecretValueNotConfigured_ReturnsUnauthorized()
    {
        const string secret = "secret";

        var mockRequest = new Mock<HttpRequest>();
        var headerDictionary = new HeaderDictionary { { "Download-Secret", secret } };

        mockRequest.SetupGet(r => r.Headers).Returns(headerDictionary);
        mockRequest.SetupGet(r => r.Query).Returns(new QueryCollection());

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);

        var mockQualificationDownloadService = new Mock<IQualificationDownloadService>();
        var mockConfiguration = new Mock<IConfiguration>();

        var controllerContext = new ControllerContext
                                {
                                    HttpContext = mockHttpContext.Object
                                };

        var controller =
            new GenerateDownloadController(new NullLogger<GenerateDownloadController>(),
                                           mockQualificationDownloadService.Object,
                                           mockConfiguration.Object)
            {
                ControllerContext = controllerContext
            };

        var action = async () => await controller.Index();

        await action.Should().ThrowAsync<InvalidOperationException>();
    }
    
    [TestMethod]
    public async Task Index_CorrectSecretSubmittedInHeader_CallsGenerateEyqlDownload()
    {
        const string secret = "secret";

        var headerDictionary = new HeaderDictionary { { "Download-Secret", secret } };

        var mockRequest = new Mock<HttpRequest>();
        mockRequest.SetupGet(r => r.Headers).Returns(headerDictionary);
        mockRequest.SetupGet(r => r.Query).Returns(new QueryCollection());

        var mockHttpContext = new Mock<HttpContext>();

        mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);
        
        var mockQualificationDownloadService = new Mock<IQualificationDownloadService>();
        var section = new Mock<IConfigurationSection>();
        section.Setup(s => s["AuthSecret"]).Returns(secret);

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c.GetSection("Download")).Returns(section.Object);

        var controllerContext = new ControllerContext
                                {
                                    HttpContext = mockHttpContext.Object
                                };

        var controller =
            new GenerateDownloadController(new NullLogger<GenerateDownloadController>(),
                                           mockQualificationDownloadService.Object,
                                           mockConfiguration.Object)
            {
                ControllerContext = controllerContext
            };

        var result = await controller.Index();

        mockQualificationDownloadService
            .Verify(x => x.GenerateEyqlDownload(),
                    Times.Once);

        result.Should().BeOfType<NoContentResult>();
    }
}