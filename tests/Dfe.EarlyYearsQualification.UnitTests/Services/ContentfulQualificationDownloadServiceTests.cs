using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Download;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;

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

        mockContentfulClient
            .Setup(x => x.GetEntries(It.IsAny<QueryBuilder<Qualification>>()))
            .ReturnsAsync(new ContentfulCollection<Qualification>{ Items = new List<Qualification>() });

        mockDownloadGenerator.Setup(x => x.GenerateQualificationListContent(It.IsAny<List<Qualification>>()))
                             .Returns(string.Empty);
        
        var service = new ContentfulQualificationDownloadService(mockContentfulClient.Object, mockContentfulManagementClient.Object, mockDownloadGenerator.Object, mockLogger.Object);

        await service.GenerateEyqlDownload();
        
        mockContentfulClient.Verify(x => x.GetEntries(It.IsAny<QueryBuilder<Qualification>>()), Times.Once);
        mockDownloadGenerator.Verify(x => x.GenerateQualificationListContent(It.IsAny<List<Qualification>>()), Times.Once);
        mockContentfulManagementClient.Verify(x => x.UploadFileAndCreateAsset(It.IsAny<ManagementAsset>(), It.IsAny<byte[]>()), Times.Never);
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

        mockContentfulClient
            .Setup(x => x.GetEntries(It.IsAny<QueryBuilder<Qualification>>()))
            .ReturnsAsync(new ContentfulCollection<Qualification>{ Items = new List<Qualification>() });

        mockDownloadGenerator.Setup(x => x.GenerateQualificationListContent(It.IsAny<List<Qualification>>()))
                             .Returns("Some content");
        
        var service = new ContentfulQualificationDownloadService(mockContentfulClient.Object, mockContentfulManagementClient.Object, mockDownloadGenerator.Object, mockLogger.Object);

        await service.GenerateEyqlDownload();
        
        mockContentfulClient.Verify(x => x.GetEntries(It.IsAny<QueryBuilder<Qualification>>()), Times.Once);
        mockDownloadGenerator.Verify(x => x.GenerateQualificationListContent(It.IsAny<List<Qualification>>()), Times.Once);
        mockContentfulManagementClient.Verify(x => x.UploadFileAndCreateAsset(It.Is<ManagementAsset>(ma => ma.SystemProperties.Id == Assets.EarlyYearsQualificationList), It.IsAny<byte[]>()), Times.Once);
        mockContentfulManagementClient.Verify(x => x.DeleteAsset(Assets.EarlyYearsQualificationList, 1), Times.Once);
    }
    
    [TestMethod]
    public async Task GenerateEyqlDownload_ContentfulClientThrowsException_LogsError()
    {
        var mockLogger = new Mock<ILogger<ContentfulQualificationDownloadService>>();
        var mockContentfulManagementClient = new Mock<IContentfulManagementClient>();
        var mockContentfulClient = new Mock<IContentfulClient>();
        var mockDownloadGenerator = new Mock<IDownloadGenerator>();

        var exception = new Exception("Test Exception");

        mockContentfulClient
            .Setup(x => x.GetEntries(It.IsAny<QueryBuilder<Qualification>>()))
            .ThrowsAsync(exception);
        
        var service = new ContentfulQualificationDownloadService(mockContentfulClient.Object, mockContentfulManagementClient.Object, mockDownloadGenerator.Object, mockLogger.Object);

        await service.GenerateEyqlDownload();
        
        mockContentfulClient.Verify(x => x.GetEntries(It.IsAny<QueryBuilder<Qualification>>()), Times.Once);
        mockDownloadGenerator.Verify(x => x.GenerateQualificationListContent(It.IsAny<List<Qualification>>()), Times.Never);
        mockContentfulManagementClient.Verify(x => x.UploadFileAndCreateAsset(It.IsAny<ManagementAsset>(), It.IsAny<byte[]>()), Times.Never);
        mockContentfulManagementClient.Verify(x => x.DeleteAsset(Assets.EarlyYearsQualificationList, 1), Times.Never);
        mockLogger.VerifyError("Error generating EYQL download.", exception);
    }
}