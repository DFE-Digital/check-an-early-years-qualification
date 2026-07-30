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

    private Mock<IContentfulClient> _clientMock = null!;
    private Mock<IContentfulManagementClient> _managementClientMock = null!;
    private Mock<IDownloadGenerator> _downloadGeneratorMock = null!;
    private Mock<ILogger<ContentfulQualificationDownloadService>> _loggerMock = null!;
    private Mock<IHttpClientFactory> _httpClientFactoryMock = null!;

    [TestInitialize]
    public void SetUp()
    {
        _clientMock = new Mock<IContentfulClient>();
        _managementClientMock = new Mock<IContentfulManagementClient>();
        _downloadGeneratorMock = new Mock<IDownloadGenerator>();
        _loggerMock = new Mock<ILogger<ContentfulQualificationDownloadService>>();
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
    }

    [TestMethod]
    [DataRow("Production", Assets.EarlyYearsQualificationList, "Early-Years-Qualifications-List.csv", "EYQL Download")]
    [DataRow("Staging", Assets.EarlyYearsQualificationListStaging, "Early-Years-Qualifications-List-Staging.csv", "EYQL Download Staging")]
    [DataRow("Development", Assets.EarlyYearsQualificationListDevelopment, "Early-Years-Qualifications-List-Development.csv", "EYQL Download Development")]
    public async Task GenerateEyqlDownloadByEnvironment_Environment_GeneratesAndPublishesAsset(string environment, string assetId, string expectedFileName, string expectedTitle)
    {
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

        _clientMock.Setup(client => client.GetEntries(It.IsAny<QueryBuilder<Qualification>>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(qualifications);
        _downloadGeneratorMock.Setup(generator => generator.GenerateQualificationListContent(It.IsAny<List<Qualification>>()))
                              .Returns(generatedContent);
        _managementClientMock.Setup(client => client.GetAssetsCollection(It.IsAny<QueryBuilder<ManagementAsset>>()))
                             .ReturnsAsync(new ContentfulCollection<ManagementAsset> { Items = [existingAsset] });

        ManagementAsset? createdAsset = null;
        byte[]? uploadedBytes = null;

        _managementClientMock
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

        var service = CreateService();

        await service.GenerateEyqlDownloadByEnvironment(environment);

        _downloadGeneratorMock.Verify(generator => generator.GenerateQualificationListContent(
                                         It.Is<List<Qualification>>(items => items.Count == 1 && items[0].QualificationId == "qualification-id")),
                                     Times.Once);
        _managementClientMock.Verify(client => client.UnpublishAsset(assetId, 4), Times.Once);
        _managementClientMock.Verify(client => client.DeleteAsset(assetId, 5), Times.Once);
        _managementClientMock.Verify(client => client.PublishAsset(assetId, 8), Times.Once);

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
        _clientMock.Setup(client => client.GetEntries(It.IsAny<QueryBuilder<Qualification>>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ContentfulCollection<Qualification>
                                 {
                                     Items = [new Qualification("qualification-id", "Qualification", "Awarding organisation", 3)]
                                 });
        _downloadGeneratorMock.Setup(generator => generator.GenerateQualificationListContent(It.IsAny<List<Qualification>>()))
                              .Returns(string.Empty);

        var service = CreateService();

        await service.GenerateEyqlDownloadByEnvironment("Production");

        _loggerMock.VerifyWarning("EYQL not generated. No content found.");
        _managementClientMock.Verify(client => client.GetAssetsCollection(It.IsAny<QueryBuilder<ManagementAsset>>()), Times.Never);
        _managementClientMock.Verify(client => client.UploadFileAndCreateAsset(It.IsAny<ManagementAsset>(),
                                                                               It.IsAny<byte[]>(),
                                                                               It.IsAny<string>(),
                                                                               It.IsAny<CancellationToken>()),
                                     Times.Never);
        _managementClientMock.Verify(client => client.PublishAsset(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
    }

    [TestMethod]
    public async Task GenerateEyqlDownloadByEnvironment_UnknownEnvironment_LogsWarning()
    {
        var service = CreateService();

        await service.GenerateEyqlDownloadByEnvironment("Test");

        _loggerMock.VerifyWarning("Unknown environment: Test. No EYQL download generated.");
        _clientMock.Verify(client => client.GetEntries(It.IsAny<QueryBuilder<Qualification>>(), It.IsAny<CancellationToken>()),
                           Times.Never);
    }

    [TestMethod]
    public async Task GenerateEyqlDownloadByEnvironment_WhenGenerationFails_LogsError()
    {
        var exception = new InvalidOperationException("Failed to generate download");

        _clientMock.Setup(client => client.GetEntries(It.IsAny<QueryBuilder<Qualification>>(), It.IsAny<CancellationToken>()))
                   .ThrowsAsync(exception);

        var service = CreateService();

        await service.GenerateEyqlDownloadByEnvironment("Production");

        _loggerMock.VerifyError("Error generating EYQL download.", exception);
    }

    [TestMethod]
    public async Task GetEyqlDownload_Production_ReturnsFileContentsAndFileName()
    {
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

        _managementClientMock.Setup(client => client.GetAssetsCollection(It.IsAny<QueryBuilder<ManagementAsset>>()))
                             .ReturnsAsync(new ContentfulCollection<ManagementAsset> { Items = [asset] });
        _httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>()))
                              .Returns(new HttpClient(handler.Object));

        var service = CreateService();

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
        _managementClientMock.Setup(client => client.GetAssetsCollection(It.IsAny<QueryBuilder<ManagementAsset>>()))
                             .ReturnsAsync(new ContentfulCollection<ManagementAsset> { Items = [] });

        var service = CreateService();

        var result = await service.GetEyqlDownload("Production");

        _loggerMock.VerifyWarning("EYQL not found.");
        result.fileContents.Should().BeEmpty();
        result.fileName.Should().Be("Early-Years-Qualifications-List.csv");
    }

    [TestMethod]
    public async Task GetEyqlDownload_UnknownEnvironment_LogsWarningAndReturnsEmptyResult()
    {
        var service = CreateService();

        var result = await service.GetEyqlDownload("ThisIsNotAValidEnvironment");

        _loggerMock.VerifyWarning("Unknown environment: ThisIsNotAValidEnvironment. No EYQL asset found.");
        result.fileContents.Should().BeEmpty();
        result.fileName.Should().BeEmpty();
    }

    private ContentfulQualificationDownloadService CreateService()
    {
        return new ContentfulQualificationDownloadService(_clientMock.Object,
                                                          _managementClientMock.Object,
                                                          _downloadGeneratorMock.Object,
                                                          _loggerMock.Object,
                                                          _httpClientFactoryMock.Object);
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