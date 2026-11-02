using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Filters;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Content.Services.Entities;
using Newtonsoft.Json;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class QualificationsRepositoryTests
{
    [TestMethod]
    public async Task GetQualificationById_NullRatioRequirements_LogsAndReturnsDefault()
    {
        var logger = new Mock<ILogger<QualificationsRepository>>();
        var clientMock = GetClientMock();
        
        clientMock.Setup(client =>
                             client.GetEntriesByType(ContentTypes.RatioRequirement,
                                                     It.IsAny<QueryBuilder<RatioRequirement>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync((ContentfulCollection<RatioRequirement>)null!);

        var service =
            new QualificationsRepository(logger.Object, clientMock.Object, new Mock<IQualificationListFilter>().Object);

        var result = await service.GetById("SomeId");

        logger.VerifyWarning("No ratio requirements returned");

        result.Should().BeNull();
    }
    
    [TestMethod]
    public async Task GetQualificationById_Null_LogsAndReturnsDefault()
    {
        var logger = new Mock<ILogger<QualificationsRepository>>();
        var clientMock = GetClientMock();
        
        clientMock.Setup(client =>
                             client.GetEntriesByType(ContentTypes.RatioRequirement,
                                                     It.IsAny<QueryBuilder<RatioRequirement>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<RatioRequirement>
                                { Items = [new RatioRequirement()] });
        
        clientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync((ContentfulCollection<Qualification>)null!);

        var service =
            new QualificationsRepository(logger.Object, clientMock.Object, new Mock<IQualificationListFilter>().Object);

        var result = await service.GetById("SomeId");

        logger.VerifyWarning("No qualifications returned for qualificationId: SomeId");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetQualificationById_NoContent_LogsAndReturnsDefault()
    {
        var logger = new Mock<ILogger<QualificationsRepository>>();
        var clientMock = GetClientMock();
        
        clientMock.Setup(client =>
                             client.GetEntriesByType(ContentTypes.RatioRequirement,
                                                     It.IsAny<QueryBuilder<RatioRequirement>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<RatioRequirement>
                                { Items = [new RatioRequirement()] });
        
        clientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<Qualification> { Items = new List<Qualification>() });

        var service =
            new QualificationsRepository(logger.Object, clientMock.Object, new Mock<IQualificationListFilter>().Object);

        var result = await service.GetById("SomeId");

        logger.VerifyWarning("No qualifications returned for qualificationId: SomeId");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetQualificationById_QualificationExists_Returns()
    {
        var logger = new Mock<ILogger<QualificationsRepository>>();
        var clientMock = GetClientMock();
        
        var qualification = new Qualification("SomeId",
                                              "Test qualification name",
                                              "Test awarding org",
                                              123)
                            {
                                FromWhichYear = "Test from which year",
                                ToWhichYear = "Test to which year",
                                QualificationNumber = "Test qualification number",
                            };

        clientMock.Setup(client =>
                             client.GetEntriesByType(ContentTypes.RatioRequirement,
                                                     It.IsAny<QueryBuilder<RatioRequirement>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<RatioRequirement>
                                { Items = [new RatioRequirement()] });

        clientMock.Setup(client =>
                             client.GetEntries(
                                               It.IsAny<QueryBuilder<Qualification>>(),
                                               It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<Qualification>
                                { Items = [qualification] });

        var service =
            new QualificationsRepository(logger.Object, clientMock.Object, new Mock<IQualificationListFilter>().Object);

        var result = await service.GetById("SomeId");

        result.Should().NotBeNull();
        result.Should().Be(qualification);
    }

    [TestMethod]
    public async Task GetQualificationById_QualificationsContainEmptyQualificationId_Exists_Returns()
    {
        var logger = new Mock<ILogger<QualificationsRepository>>();
        var clientMock = GetClientMock();
        
        var qualification = new Qualification("SomeId",
                                              "Test qualification name",
                                              "Test awarding org",
                                              123)
                            {
                                FromWhichYear = "Test from which year",
                                ToWhichYear = "Test to which year",
                                QualificationNumber = "Test qualification number",
                            };

        clientMock.Setup(client =>
                             client.GetEntriesByType(ContentTypes.RatioRequirement,
                                                     It.IsAny<QueryBuilder<RatioRequirement>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<RatioRequirement>
                                { Items = [new RatioRequirement()] });

        clientMock.Setup(client =>
                             client.GetEntries(
                                               It.IsAny<QueryBuilder<Qualification>>(),
                                               It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<Qualification>
                                {
                                    Items = [new Qualification(string.Empty, "Test name", "Test AO", 3), qualification]
                                });

        var service =
            new QualificationsRepository(logger.Object, clientMock.Object, new Mock<IQualificationListFilter>().Object);

        var result = await service.GetById("SomeId");

        result.Should().NotBeNull();
        result.Should().Be(qualification);
    }
    
    [TestMethod]
    public async Task GetQualifications_NullRatioRequirements_LogsAndReturnsDefault()
    {
        var logger = new Mock<ILogger<QualificationsRepository>>();
        var clientMock = GetClientMock();
        
        clientMock.Setup(client =>
                             client.GetEntriesByType(ContentTypes.RatioRequirement,
                                                     It.IsAny<QueryBuilder<RatioRequirement>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync((ContentfulCollection<RatioRequirement>)null!);

        var service =
            new QualificationsRepository(logger.Object, clientMock.Object, new Mock<IQualificationListFilter>().Object);

        var result = await service.Get(new QualificationFilterOptions{ IncludeAllQualifications = false });

        logger.VerifyWarning("No ratio requirements returned");

        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetQualifications_ReturnsQualifications()
    {
        var logger = new Mock<ILogger<QualificationsRepository>>();
        var clientMock = GetClientMock();
        
        var qualification = new Qualification("Id",
                                              "Name",
                                              "AO",
                                              6)
                            {
                                FromWhichYear = "2014", ToWhichYear = "2020",
                                QualificationNumber = "number",
                            };

        clientMock.Setup(client =>
                             client.GetEntriesByType(ContentTypes.RatioRequirement,
                                                     It.IsAny<QueryBuilder<RatioRequirement>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<RatioRequirement>
                                { Items = [new RatioRequirement()] });

        clientMock.Setup(c =>
                             c.GetEntries(It.IsAny<QueryBuilder<Qualification>>(),
                                          It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<Qualification> { Items = [qualification] });

        var mockQualificationFilterFactory = new Mock<IQualificationListFilter>();

        mockQualificationFilterFactory.Setup(x => x.ApplyFilters(It.IsAny<List<Qualification>>(), It.IsAny<int?>(),
                                                                 It.IsAny<int?>(), It.IsAny<int?>(),
                                                                 It.IsAny<string?>(),
                                                                 It.IsAny<string?>(),
                                                                 It.IsAny<string?>()))
                                      .Returns([qualification]);

        var service =
            new QualificationsRepository(logger.Object, clientMock.Object, mockQualificationFilterFactory.Object);

        var result = await service.Get(new QualificationFilterOptions{ IncludeAllQualifications = false });

        result.Should().HaveCount(1).And.Contain(qualification);
    }

    [TestMethod]
    public async Task GetQualifications_ContentfulHasNoQualifications_ReturnsEmpty()
    {
        var logger = new Mock<ILogger<QualificationsRepository>>();
        var clientMock = GetClientMock();
        
        clientMock.Setup(client =>
                             client.GetEntriesByType(ContentTypes.RatioRequirement,
                                                     It.IsAny<QueryBuilder<RatioRequirement>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<RatioRequirement>
                                { Items = [new RatioRequirement()] });
        
        clientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<Qualification>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<Qualification> { Items = [] });

        var service =
            new QualificationsRepository(logger.Object, clientMock.Object, new Mock<IQualificationListFilter>().Object);

        var result = await service.Get(new QualificationFilterOptions{ IncludeAllQualifications = false });

        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetFilteredQualifications_ContentfulClientThrowsException_ReturnsEmptyList()
    {
        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ThrowsAsync(new Exception());

        var mocklogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mocklogger.Object, mockContentfulClient.Object,
                                         new Mock<IQualificationListFilter>().Object);

        var filteredQualifications = await repository.Get(new QualificationFilterOptions
                                                          {
                                                              IncludeAllQualifications = false, 
                                                              Level = 4, 
                                                              StartDateMonth = 5, 
                                                              StartDateYear = 2016
                                                          });

        filteredQualifications.Should().NotBeNull();
        filteredQualifications.Should().BeEmpty();
    }
    
    private static Mock<IContentfulClient> GetClientMock()
    {
        var clientMock = new Mock<IContentfulClient>();
        clientMock.Setup(x => x.SerializerSettings)
                  .Returns(new JsonSerializerSettings { Converters = new List<JsonConverter>() });
        return clientMock;
    }
}