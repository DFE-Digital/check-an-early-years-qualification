using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Download;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class ContentfulQualificationDownloadServiceTests
{
    [TestMethod]
    public async Task GenerateEyqlDownload_EmptyStringReturnedFromGenerator_Returns()
    {
        var mockLogger = new Mock<ILogger<ContentfulQualificationDownloadService>>();
        var mockContentfulManagementClient = new Mock<IContentfulManagementClient>();
        var mockDownloadGenerator = new Mock<IDownloadGenerator>();

        mockContentfulManagementClient
            .Setup(x => x.GetEntriesCollection(It.IsAny<QueryBuilder<Qualification>>(), null))
            .ReturnsAsync(new ContentfulCollection<Qualification>{ Items = new List<Qualification>() });

        mockDownloadGenerator.Setup(x => x.GenerateQualificationListContent(It.IsAny<List<Qualification>>()))
                             .Returns(string.Empty);
        
        var service = new ContentfulQualificationDownloadService(mockContentfulManagementClient.Object, mockDownloadGenerator.Object, mockLogger.Object);

        await service.GenerateEyqlDownload();
        
        mockContentfulManagementClient.Verify(x => x.GetEntriesCollection(It.IsAny<QueryBuilder<Qualification>>()), Times.Once);
        mockContentfulManagementClient.Verify(x => x.UploadFileAndCreateAsset(It.IsAny<ManagementAsset>(), It.IsAny<byte[]>()), Times.Never);
        mockLogger.VerifyWarning("EYQL not generated. No content found.");
    }
    
    [TestMethod]
    public async Task GenerateEyqlDownload_GeneratesContent_CallsUploadFileAndCreateAsset()
    {
        var mockLogger = new Mock<ILogger<ContentfulQualificationDownloadService>>();
        var mockContentfulManagementClient = new Mock<IContentfulManagementClient>();
        var mockDownloadGenerator = new Mock<IDownloadGenerator>();

        mockContentfulManagementClient
            .Setup(x => x.GetEntriesCollection(It.IsAny<QueryBuilder<Qualification>>(), null))
            .ReturnsAsync(new ContentfulCollection<Qualification>{ Items = new List<Qualification>() });

        mockDownloadGenerator.Setup(x => x.GenerateQualificationListContent(It.IsAny<List<Qualification>>()))
                             .Returns("Some content");
        
        var service = new ContentfulQualificationDownloadService(mockContentfulManagementClient.Object, mockDownloadGenerator.Object, mockLogger.Object);

        await service.GenerateEyqlDownload();
        
        mockContentfulManagementClient.Verify(x => x.GetEntriesCollection(It.IsAny<QueryBuilder<Qualification>>()), Times.Once);
        mockContentfulManagementClient.Verify(x => x.UploadFileAndCreateAsset(It.Is<ManagementAsset>(ma => ma.SystemProperties.Id == Assets.EarlyYearsQualificationList), It.IsAny<byte[]>()), Times.Once);
    }
}