using System.Net;
using System.Text;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Download;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Moq.Protected;
using File = Contentful.Core.Models.File;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class ContentfulQualificationDownloadServiceTests
{
    private const string Locale = "en-GB";

    [TestMethod]
    [DataRow("Production", Assets.EarlyYearsQualificationList, "Early-Years-Qualifications-List.csv", "EYQL Download")]
    [DataRow("Staging", Assets.EarlyYearsQualificationListStaging, "Early-Years-Qualifications-List-Staging.csv", "EYQL Download Staging")]
    [DataRow("Development", Assets.EarlyYearsQualificationListDevelopment, "Early-Years-Qualifications-List-Development.csv", "EYQL Download Development")]
    public async Task GenerateEyqlDownloadByEnvironment_Environment_GeneratesAndPublishesAsset(string environment, string assetId, string expectedFileName, string expectedTitle)
    {
        var clientMock = new Mock<IContentfulClient>();
        var managementClientMock = new Mock<IContentfulManagementClient>();
        var downloadGeneratorMock = new Mock<IDownloadGenerator>();
        var loggerMock = new Mock<ILogger<ContentfulQualificationDownloadService>>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var service = new ContentfulQualificationDownloadService(clientMock.Object,
                                                   managementClientMock.Object,
                                                   downloadGeneratorMock.Object,
                                                   loggerMock.Object,
                                                   httpClientFactoryMock.Object);
        
                                                   var qualifications = new ContentfulCollection<Qualification>
                                                                        {
                                                                            Items = [new Qualification("qualification-id", "Qualification", "Awarding organisation", 3)]
                                                                        };
        var existingAsset = CreateManagementAsset(assetId,
                                                  version: 5,
                                                  publishedVersion: 4,
                                                  isPublished: true);
        var uploadedAsset = CreateManagementAsset(assetId, version: 7);
        var generatedContent = "header,value";

        clientMock.Setup(client => client.GetEntries(It.IsAny<QueryBuilder<Qualification>>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(qualifications);
        downloadGeneratorMock.Setup(generator => generator.GenerateQualificationListContent(It.IsAny<List<Qualification>>()))
                              .Returns(generatedContent);
        managementClientMock.Setup(client => client.GetAssetsCollection(It.IsAny<QueryBuilder<ManagementAsset>>()))
                             .ReturnsAsync(new ContentfulCollection<ManagementAsset> { Items = [existingAsset] });

        ManagementAsset? createdAsset = null;
        byte[]? uploadedBytes = null;

        managementClientMock
            .Setup(client => client.UploadFileAndCreateAsset(It.IsAny<ManagementAsset>(),
                                                             It.IsAny<byte[]>(),
                                                             It.IsAny<string>(),
                                                             It.IsAny<CancellationToken>()))
            .Callback<ManagementAsset, byte[], string, CancellationToken>((asset, bytes, _, _) =>
            {
                createdAsset = asset;
                uploadedBytes = bytes;
            })
            .ReturnsAsync(uploadedAsset);

        await service.GenerateEyqlDownloadByEnvironment(environment);

        downloadGeneratorMock.Verify(generator => generator.GenerateQualificationListContent(
                                         It.Is<List<Qualification>>(items => items.Count == 1 && items[0].QualificationId == "qualification-id")),
                                     Times.Once);
        managementClientMock.Verify(client => client.UnpublishAsset(assetId, 4), Times.Once);
        managementClientMock.Verify(client => client.DeleteAsset(assetId, 5), Times.Once);
        managementClientMock.Verify(client => client.PublishAsset(assetId, 8), Times.Once);

        createdAsset.Should().NotBeNull();
        createdAsset.SystemProperties.Id.Should().Be(assetId);
        createdAsset.Title[Locale].Should().Be(expectedTitle);
        createdAsset.Description[Locale].Should().Be("The Early Years Qualifications List download.");
        createdAsset.Files[Locale].ContentType.Should().Be("text/csv");
        createdAsset.Files[Locale].FileName.Should().Be(expectedFileName);
        uploadedBytes.Should().Equal(Encoding.UTF8.GetBytes(generatedContent));
    }

    [TestMethod]
    public async Task GenerateEyqlDownloadByEnvironment_DownloadGeneratorReturnsEmptyContent_LogsWarningAndStops()
    {
        var clientMock = new Mock<IContentfulClient>();
        var managementClientMock = new Mock<IContentfulManagementClient>();
        var downloadGeneratorMock = new Mock<IDownloadGenerator>();
        var loggerMock = new Mock<ILogger<ContentfulQualificationDownloadService>>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var service = new ContentfulQualificationDownloadService(clientMock.Object,
                                                                 managementClientMock.Object,
                                                                 downloadGeneratorMock.Object,
                                                                 loggerMock.Object,
                                                                 httpClientFactoryMock.Object);
        
        clientMock.Setup(client => client.GetEntries(It.IsAny<QueryBuilder<Qualification>>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ContentfulCollection<Qualification>
                                 {
                                     Items = [new Qualification("qualification-id", "Qualification", "Awarding organisation", 3)]
                                 });
        downloadGeneratorMock.Setup(generator => generator.GenerateQualificationListContent(It.IsAny<List<Qualification>>()))
                              .Returns(string.Empty);

        await service.GenerateEyqlDownloadByEnvironment("Production");

        loggerMock.VerifyWarning("EYQL not generated. No content found.");
        managementClientMock.Verify(client => client.GetAssetsCollection(It.IsAny<QueryBuilder<ManagementAsset>>()), Times.Never);
        managementClientMock.Verify(client => client.UploadFileAndCreateAsset(It.IsAny<ManagementAsset>(),
                                                                               It.IsAny<byte[]>(),
                                                                               It.IsAny<string>(),
                                                                               It.IsAny<CancellationToken>()),
                                     Times.Never);
        managementClientMock.Verify(client => client.PublishAsset(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
    }

    [TestMethod]
    public async Task GenerateEyqlDownloadByEnvironment_UnknownEnvironment_LogsWarning()
    {
        var clientMock = new Mock<IContentfulClient>();
        var managementClientMock = new Mock<IContentfulManagementClient>();
        var downloadGeneratorMock = new Mock<IDownloadGenerator>();
        var loggerMock = new Mock<ILogger<ContentfulQualificationDownloadService>>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var service = new ContentfulQualificationDownloadService(clientMock.Object,
                                                                 managementClientMock.Object,
                                                                 downloadGeneratorMock.Object,
                                                                 loggerMock.Object,
                                                                 httpClientFactoryMock.Object);

        await service.GenerateEyqlDownloadByEnvironment("Test");

        loggerMock.VerifyWarning("Unknown environment: Test. No EYQL download generated.");
        clientMock.Verify(client => client.GetEntries(It.IsAny<QueryBuilder<Qualification>>(), It.IsAny<CancellationToken>()),
                           Times.Never);
    }

    [TestMethod]
    public async Task GenerateEyqlDownloadByEnvironment_WhenGenerationFails_LogsError()
    {
        var clientMock = new Mock<IContentfulClient>();
        var managementClientMock = new Mock<IContentfulManagementClient>();
        var downloadGeneratorMock = new Mock<IDownloadGenerator>();
        var loggerMock = new Mock<ILogger<ContentfulQualificationDownloadService>>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var service = new ContentfulQualificationDownloadService(clientMock.Object,
                                                                 managementClientMock.Object,
                                                                 downloadGeneratorMock.Object,
                                                                 loggerMock.Object,
                                                                 httpClientFactoryMock.Object);
        
        var exception = new InvalidOperationException("Failed to generate download");

        clientMock.Setup(client => client.GetEntries(It.IsAny<QueryBuilder<Qualification>>(), It.IsAny<CancellationToken>()))
                   .ThrowsAsync(exception);

        await service.GenerateEyqlDownloadByEnvironment("Production");

        loggerMock.VerifyError("Error generating EYQL download.", exception);
    }

    [TestMethod]
    public async Task GetEyqlDownload_Production_ReturnsFileContentsAndFileName()
    {
        var clientMock = new Mock<IContentfulClient>();
        var managementClientMock = new Mock<IContentfulManagementClient>();
        var downloadGeneratorMock = new Mock<IDownloadGenerator>();
        var loggerMock = new Mock<ILogger<ContentfulQualificationDownloadService>>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var service = new ContentfulQualificationDownloadService(clientMock.Object,
                                                                 managementClientMock.Object,
                                                                 downloadGeneratorMock.Object,
                                                                 loggerMock.Object,
                                                                 httpClientFactoryMock.Object);
        
        var expectedBytes = Encoding.UTF8.GetBytes("csv-content");
        var asset = CreateManagementAsset(Assets.EarlyYearsQualificationList,
                                          version: 2,
                                          url: "//images.ctfassets.net/spreadsheet.csv");
        var handler = new Mock<HttpMessageHandler>();

        handler.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync",
                                                ItExpr.Is<HttpRequestMessage>(request =>
                                                                                  request.Method == HttpMethod.Get
                                                                                  && request.RequestUri == new Uri("https://images.ctfassets.net/spreadsheet.csv")),
                                                ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                             {
                                 Content = new ByteArrayContent(expectedBytes)
                             });

        managementClientMock.Setup(client => client.GetAssetsCollection(It.IsAny<QueryBuilder<ManagementAsset>>()))
                             .ReturnsAsync(new ContentfulCollection<ManagementAsset> { Items = [asset] });
        httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>()))
                              .Returns(new HttpClient(handler.Object));

        var result = await service.GetEyqlDownload("Production");

        result.fileContents.Should().Equal(expectedBytes);
        result.fileName.Should().Be("Early-Years-Qualifications-List.csv");
        handler.Protected().Verify("SendAsync",
                                   Times.Once(),
                                   ItExpr.Is<HttpRequestMessage>(request =>
                                                                     request.Method == HttpMethod.Get
                                                                     && request.RequestUri == new Uri("https://images.ctfassets.net/spreadsheet.csv")),
                                   ItExpr.IsAny<CancellationToken>());
    }

    [TestMethod]
    public async Task GetEyqlDownload_WhenAssetDoesNotExist_LogsWarningAndReturnsEmptyContent()
    {
        var clientMock = new Mock<IContentfulClient>();
        var managementClientMock = new Mock<IContentfulManagementClient>();
        var downloadGeneratorMock = new Mock<IDownloadGenerator>();
        var loggerMock = new Mock<ILogger<ContentfulQualificationDownloadService>>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var service = new ContentfulQualificationDownloadService(clientMock.Object,
                                                                 managementClientMock.Object,
                                                                 downloadGeneratorMock.Object,
                                                                 loggerMock.Object,
                                                                 httpClientFactoryMock.Object);
        
        managementClientMock.Setup(client => client.GetAssetsCollection(It.IsAny<QueryBuilder<ManagementAsset>>()))
                             .ReturnsAsync(new ContentfulCollection<ManagementAsset> { Items = [] });

        var result = await service.GetEyqlDownload("Production");

        loggerMock.VerifyWarning("EYQL not found.");
        result.fileContents.Should().BeEmpty();
        result.fileName.Should().Be("Early-Years-Qualifications-List.csv");
    }

    [TestMethod]
    public async Task GetEyqlDownload_UnknownEnvironment_LogsWarningAndReturnsEmptyResult()
    {
        var clientMock = new Mock<IContentfulClient>();
        var managementClientMock = new Mock<IContentfulManagementClient>();
        var downloadGeneratorMock = new Mock<IDownloadGenerator>();
        var loggerMock = new Mock<ILogger<ContentfulQualificationDownloadService>>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var service = new ContentfulQualificationDownloadService(clientMock.Object,
                                                                 managementClientMock.Object,
                                                                 downloadGeneratorMock.Object,
                                                                 loggerMock.Object,
                                                                 httpClientFactoryMock.Object);

        var result = await service.GetEyqlDownload("ThisIsNotAValidEnvironment");

        loggerMock.VerifyWarning("Unknown environment: ThisIsNotAValidEnvironment. No EYQL asset found.");
        result.fileContents.Should().BeEmpty();
        result.fileName.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetEyqlDataForInternalDownload_DownloadGeneratorReturnsNull_ReturnsNull()
    {
        var clientMock = new Mock<IContentfulClient>();
        var managementClientMock = new Mock<IContentfulManagementClient>();
        var downloadGeneratorMock = new Mock<IDownloadGenerator>();
        var loggerMock = new Mock<ILogger<ContentfulQualificationDownloadService>>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var service = new ContentfulQualificationDownloadService(clientMock.Object,
                                                                 managementClientMock.Object,
                                                                 downloadGeneratorMock.Object,
                                                                 loggerMock.Object,
                                                                 httpClientFactoryMock.Object);
        
        clientMock.Setup(client => client.GetEntries(It.IsAny<QueryBuilder<Qualification>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ContentfulCollection<Qualification>
                    {
                        Items = [new Qualification("qualification-id", "Qualification", "Awarding organisation", 3)]
                    });
        
        downloadGeneratorMock.Setup(x => x.GenerateInternalQualificationListContent(It.IsAny<List<Qualification>>()))
                              .Returns(string.Empty);

        var result = await service.GetEyqlDataForInternalDownload();
        
        result.Should().BeNull();
        loggerMock.VerifyWarning("No content found for internal download.");
    }
    
    [TestMethod]
    public async Task GetEyqlDataForInternalDownload_DownloadGeneratorReturnsString_ReturnsByteArray()
    {
        var clientMock = new Mock<IContentfulClient>();
        var managementClientMock = new Mock<IContentfulManagementClient>();
        var downloadGeneratorMock = new Mock<IDownloadGenerator>();
        var loggerMock = new Mock<ILogger<ContentfulQualificationDownloadService>>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var service = new ContentfulQualificationDownloadService(clientMock.Object,
                                                                 managementClientMock.Object,
                                                                 downloadGeneratorMock.Object,
                                                                 loggerMock.Object,
                                                                 httpClientFactoryMock.Object);
        
        const string contentResult = "This is a test";
        clientMock.Setup(client => client.GetEntries(It.IsAny<QueryBuilder<Qualification>>(),
                                                      It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ContentfulCollection<Qualification>
                                 {
                                     Items = [new Qualification("qualification-id", "Qualification", "Awarding organisation", 3)]
                                 });
        
        downloadGeneratorMock.Setup(x => x.GenerateInternalQualificationListContent(It.IsAny<List<Qualification>>()))
                              .Returns(contentResult);

        var expectedByteArray = Encoding.UTF8.GetBytes(contentResult);
        
        var result = await service.GetEyqlDataForInternalDownload();
        
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedByteArray);
    }

    private static ManagementAsset CreateManagementAsset(string assetId,
                                                         int version,
                                                         int? publishedVersion = null,
                                                         bool isPublished = false,
                                                         string url = "//images.ctfassets.net/spreadsheet.csv")
    {
        return new ManagementAsset
               {
                   SystemProperties = new SystemProperties
                                      {
                                          Id = assetId,
                                          Version = version,
                                          PublishedVersion = publishedVersion,
                                          FieldStatus = new FieldStatus
                                                        {
                                                            Status = isPublished
                                                                         ? new Dictionary<string, FieldStatusType>
                                                                           {
                                                                               [Locale] = FieldStatusType.Published
                                                                           }
                                                                         : new Dictionary<string, FieldStatusType>()
                                                        }
                                      },
                   Files = new Dictionary<string, File>
                           {
                               [Locale] = new File
                                          {
                                              Url = url,
                                              FileName = "spreadsheet.csv",
                                              ContentType = "text/csv"
                                          }
                           },
                   Title = new Dictionary<string, string> { [Locale] = "Title" },
                   Description = new Dictionary<string, string> { [Locale] = "Description" }
               };
    }
}