using System.Net;
using System.Net.Http.Json;
using System.Text;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Download;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Moq.Protected;
using File = Contentful.Core.Models.File;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class ContentfulQualificationDownloadServiceTests
{
    [TestMethod]
    public async Task GenerateEyqlDownload_EmptyStringReturnedFromGenerator_Returns()
    {
        var mockLogger = new Mock<ILogger<ContentfulQualificationDownloadService>>();
        var mockContentfulManagementClient = new Mock<IContentfulManagementClient>();
        var mockContentfulClient = new Mock<IContentfulClient>();
        var mockDownloadGenerator = new Mock<IDownloadGenerator>();
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();

        mockContentfulClient
            .Setup(x => x.GetEntries(It.IsAny<QueryBuilder<Qualification>>()))
            .ReturnsAsync(new ContentfulCollection<Qualification> { Items = new List<Qualification>() });

        mockDownloadGenerator.Setup(x => x.GenerateQualificationListContent(It.IsAny<List<Qualification>>()))
                             .Returns(string.Empty);

        var service = new ContentfulQualificationDownloadService(mockContentfulClient.Object,
                                                                 mockContentfulManagementClient.Object,
                                                                 mockDownloadGenerator.Object, mockLogger.Object,
                                                                 mockHttpClientFactory.Object);

        await service.GenerateEyqlDownload();

        mockContentfulClient.Verify(x => x.GetEntries(It.IsAny<QueryBuilder<Qualification>>()), Times.Once);
        mockDownloadGenerator.Verify(x => x.GenerateQualificationListContent(It.IsAny<List<Qualification>>()),
                                     Times.Once);
        mockContentfulManagementClient.Verify(x => x.UploadFileAndCreateAsset(It.IsAny<ManagementAsset>(),
                                                                              It.IsAny<byte[]>()), Times.Never);
        mockContentfulManagementClient.Verify(x => x.DeleteAsset(Assets.EarlyYearsQualificationList, 1), Times.Never);
        mockLogger.VerifyWarning("EYQL not generated. No content found.");
    }

    [TestMethod]
    public async Task GenerateEyqlDownload_GeneratesContent_CallsUploadFileAndCreateAsset()
    {
        var mockLogger = new Mock<ILogger<ContentfulQualificationDownloadService>>();
        var mockContentfulManagementClient = new Mock<IContentfulManagementClient>();
        var mockContentfulClient = new Mock<IContentfulClient>();
        var mockDownloadGenerator = new Mock<IDownloadGenerator>();
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();

        mockContentfulClient
            .Setup(x => x.GetEntries(It.IsAny<QueryBuilder<Qualification>>()))
            .ReturnsAsync(new ContentfulCollection<Qualification> { Items = new List<Qualification>() });

        mockDownloadGenerator.Setup(x => x.GenerateQualificationListContent(It.IsAny<List<Qualification>>()))
                             .Returns("Some content");

        mockContentfulManagementClient.Setup(x => x.GetAssetsCollection(It.IsAny<QueryBuilder<ManagementAsset>>()))
                                      .ReturnsAsync(new ContentfulCollection<ManagementAsset>
                                                    {
                                                        Items = new List<ManagementAsset>
                                                                {
                                                                    new ManagementAsset
                                                                    {
                                                                        SystemProperties = new SystemProperties
                                                                            {
                                                                                Id = Assets
                                                                                    .EarlyYearsQualificationList,
                                                                                FieldStatus = new FieldStatus
                                                                                    {
                                                                                        Status =
                                                                                            new Dictionary<string,
                                                                                                FieldStatusType>
                                                                                            {
                                                                                                {
                                                                                                    "Status",
                                                                                                    FieldStatusType
                                                                                                        .Published
                                                                                                }
                                                                                            }
                                                                                    }
                                                                            }
                                                                    }
                                                                }
                                                    });

        var service = new ContentfulQualificationDownloadService(mockContentfulClient.Object,
                                                                 mockContentfulManagementClient.Object,
                                                                 mockDownloadGenerator.Object, mockLogger.Object,
                                                                 mockHttpClientFactory.Object);

        await service.GenerateEyqlDownload();

        mockContentfulClient.Verify(x => x.GetEntries(It.IsAny<QueryBuilder<Qualification>>()), Times.Once);
        mockDownloadGenerator.Verify(x => x.GenerateQualificationListContent(It.IsAny<List<Qualification>>()),
                                     Times.Once);
        mockContentfulManagementClient.Verify(x =>
                                                  x.UploadFileAndCreateAsset(It.Is<ManagementAsset>(ma =>
                                                                                 ma.SystemProperties.Id ==
                                                                                 Assets
                                                                                     .EarlyYearsQualificationList),
                                                                             It.IsAny<byte[]>()), Times.Once);
        mockContentfulManagementClient.Verify(x => x.DeleteAsset(Assets.EarlyYearsQualificationList, 1), Times.Once);
        mockContentfulManagementClient.Verify(x => x.UnpublishAsset(Assets.EarlyYearsQualificationList, 1), Times.Once);
        mockContentfulManagementClient.Verify(x => x.PublishAsset(Assets.EarlyYearsQualificationList, 2), Times.Once);
    }

    [TestMethod]
    public async Task GenerateEyqlDownload_ContentfulClientThrowsException_LogsError()
    {
        var mockLogger = new Mock<ILogger<ContentfulQualificationDownloadService>>();
        var mockContentfulManagementClient = new Mock<IContentfulManagementClient>();
        var mockContentfulClient = new Mock<IContentfulClient>();
        var mockDownloadGenerator = new Mock<IDownloadGenerator>();
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();

        var exception = new Exception("Test Exception");

        mockContentfulClient
            .Setup(x => x.GetEntries(It.IsAny<QueryBuilder<Qualification>>()))
            .ThrowsAsync(exception);

        var service = new ContentfulQualificationDownloadService(mockContentfulClient.Object,
                                                                 mockContentfulManagementClient.Object,
                                                                 mockDownloadGenerator.Object, mockLogger.Object,
                                                                 mockHttpClientFactory.Object);

        await service.GenerateEyqlDownload();

        mockContentfulClient.Verify(x => x.GetEntries(It.IsAny<QueryBuilder<Qualification>>()), Times.Once);
        mockDownloadGenerator.Verify(x => x.GenerateQualificationListContent(It.IsAny<List<Qualification>>()),
                                     Times.Never);
        mockContentfulManagementClient.Verify(x => x.UploadFileAndCreateAsset(It.IsAny<ManagementAsset>(),
                                                                              It.IsAny<byte[]>()), Times.Never);
        mockContentfulManagementClient.Verify(x => x.DeleteAsset(Assets.EarlyYearsQualificationList, 1), Times.Never);
        mockLogger.VerifyError("Error generating EYQL download.", exception);
    }
    
    [TestMethod]
    public async Task GetEyqlDownloadAsByteArray_NotFound_LogsWarning()
    {
        var mockLogger = new Mock<ILogger<ContentfulQualificationDownloadService>>();
        var mockContentfulManagementClient = new Mock<IContentfulManagementClient>();
        var mockContentfulClient = new Mock<IContentfulClient>();
        var mockDownloadGenerator = new Mock<IDownloadGenerator>();
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();

        var service = new ContentfulQualificationDownloadService(mockContentfulClient.Object,
                                                                 mockContentfulManagementClient.Object,
                                                                 mockDownloadGenerator.Object, mockLogger.Object,
                                                                 mockHttpClientFactory.Object);

        var result = await service.GetEyqlDownloadAsByteArray();

        result.Should().BeEmpty();
        mockLogger.VerifyWarning("EYQL not found.");
    }
    
    [TestMethod]
    public async Task GetEyqlDownloadAsByteArray_CallsClient_ReturnsFileContent()
    {
        var mockLogger = new Mock<ILogger<ContentfulQualificationDownloadService>>();
        var mockContentfulManagementClient = new Mock<IContentfulManagementClient>();
        var mockContentfulClient = new Mock<IContentfulClient>();
        var mockDownloadGenerator = new Mock<IDownloadGenerator>();
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        
        mockContentfulManagementClient.Setup(x => x.GetAssetsCollection(It.IsAny<QueryBuilder<ManagementAsset>>()))
                                      .ReturnsAsync(new ContentfulCollection<ManagementAsset>
                                                    {
                                                        Items = new List<ManagementAsset>
                                                                {
                                                                    new ManagementAsset
                                                                    {
                                                                        SystemProperties = new SystemProperties
                                                                            {
                                                                                Id = Assets
                                                                                    .EarlyYearsQualificationList,
                                                                                FieldStatus = new FieldStatus
                                                                                    {
                                                                                        Status =
                                                                                            new Dictionary<string,
                                                                                                FieldStatusType>
                                                                                            {
                                                                                                {
                                                                                                    "Status",
                                                                                                    FieldStatusType
                                                                                                        .Published
                                                                                                }
                                                                                            }
                                                                                    }
                                                                            },
                                                                        Files = new Dictionary<string, File>
                                                                            {
                                                                                {"en-GB", new File { Url = "//test/url" }}
                                                                            }
                                                                    }
                                                                }
                                                    });
        const string output = "Test Content";
        var mockHandler = new Mock<HttpMessageHandler>();
        var mockResponse = new HttpResponseMessage
                           {
                               StatusCode = HttpStatusCode.OK,
                               Content = JsonContent.Create<string>(output)
                           };

        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                                              "SendAsync",
                                              ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                                              ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        // Inject the handler or client into your application code
        var httpClient = new HttpClient(mockHandler.Object);
        
        mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var service = new ContentfulQualificationDownloadService(mockContentfulClient.Object,
                                                                 mockContentfulManagementClient.Object,
                                                                 mockDownloadGenerator.Object, mockLogger.Object,
                                                                 mockHttpClientFactory.Object);
        
        

        var result = await service.GetEyqlDownloadAsByteArray();

        result.Should().NotBeEmpty();
        var resultString = Encoding.Default.GetString(result);
        resultString.Should().Contain(output);
    }
}