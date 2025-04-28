using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Filters;
using Dfe.EarlyYearsQualification.Content.Services;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class QualificationsRepositoryTests : ContentfulContentServiceTestsBase<QualificationsRepository>
{
    [TestMethod]
    public async Task GetQualificationById_Null_LogsAndReturnsDefault()
    {
        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync((ContentfulCollection<Qualification>)null!);

        var service = new QualificationsRepository(Logger.Object, ClientMock.Object, new Mock<IQualificationFilterFactory>().Object);

        var result = await service.GetById("SomeId");

        Logger.VerifyWarning("No qualifications returned for qualificationId: SomeId");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetQualificationById_NoContent_LogsAndReturnsDefault()
    {
        ClientMock.Setup(client =>
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<Qualification> { Items = new List<Qualification>() });

        var service = new QualificationsRepository(Logger.Object, ClientMock.Object, new Mock<IQualificationFilterFactory>().Object);

        var result = await service.GetById("SomeId");

        Logger.VerifyWarning("No qualifications returned for qualificationId: SomeId");

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetQualificationById_QualificationExists_Returns()
    {
        var qualification = new Qualification("SomeId",
                                              "Test qualification name",
                                              "Test awarding org",
                                              123)
                            {
                                FromWhichYear = "Test from which year",
                                ToWhichYear = "Test to which year",
                                QualificationNumber = "Test qualification number",
                                AdditionalRequirements = "Test additional requirements"
                            };

        ClientMock.Setup(client =>
                             client.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<Qualification>
                                { Items = [qualification] });

        var service = new QualificationsRepository(Logger.Object, ClientMock.Object, new Mock<IQualificationFilterFactory>().Object);

        var result = await service.GetById("SomeId");

        result.Should().NotBeNull();
        result.Should().Be(qualification);
    }

    [TestMethod]
    public async Task GetQualifications_ReturnsQualifications()
    {
        var qualification = new Qualification("Id",
                                              "Name",
                                              "AO",
                                              6)
                            {
                                FromWhichYear = "2014", ToWhichYear = "2020",
                                QualificationNumber = "number", AdditionalRequirements = "Rq"
                            };

        ClientMock.Setup(c =>
                             c.GetEntries(It.IsAny<QueryBuilder<Qualification>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<Qualification> { Items = [qualification] });

        var mockQualificationFilterFactory = new Mock<IQualificationFilterFactory>();

        mockQualificationFilterFactory.Setup(x => x.ApplyFilters(It.IsAny<List<Qualification>>(), It.IsAny<int?>(),
                                                                 It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string?>(),
                                                                 It.IsAny<string?>()))
                                      .Returns([qualification]);

        var service = new QualificationsRepository(Logger.Object, ClientMock.Object, mockQualificationFilterFactory.Object);

        var result = await service.Get(null, null, null, null, null);

        result.Should().HaveCount(1).And.Contain(qualification);
    }

    [TestMethod]
    public async Task GetQualifications_ContentfulHasNoQualifications_ReturnsEmpty()
    {
        ClientMock.Setup(c =>
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<Qualification>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<Qualification> { Items = [] });

        var service = new QualificationsRepository(Logger.Object, ClientMock.Object, new Mock<IQualificationFilterFactory>().Object);

        var result = await service.Get(null, null, null, null, null);

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

        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockLogger.Object, mockContentfulClient.Object, new Mock<IQualificationFilterFactory>().Object);

        var filteredQualifications = await repository.Get(4, 5, 2016, null, null);

        filteredQualifications.Should().NotBeNull();
        filteredQualifications.Should().BeEmpty();
    }
    
}

public class MockQueryBuilder : QueryBuilder<Qualification>
{
    public List<KeyValuePair<string, string>> GetQueryString()
    {
        return _querystringValues;
    }
}