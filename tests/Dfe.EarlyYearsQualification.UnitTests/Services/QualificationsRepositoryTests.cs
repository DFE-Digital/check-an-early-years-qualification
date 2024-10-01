using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

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

        var service = new QualificationsRepository(ClientMock.Object,
                                                   new Mock<IFuzzyAdapter>().Object,
                                                   Logger.Object);

        var result = await service.GetQualificationById("SomeId");

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

        var service = new QualificationsRepository(ClientMock.Object,
                                                   new Mock<IFuzzyAdapter>().Object,
                                                   Logger.Object);

        var result = await service.GetQualificationById("SomeId");

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
                             client.GetEntriesByType(
                                                     It.IsAny<string>(),
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<Qualification>
                                { Items = new List<Qualification> { qualification } });

        var service = new QualificationsRepository(ClientMock.Object,
                                                   new Mock<IFuzzyAdapter>().Object,
                                                   Logger.Object);

        var result = await service.GetQualificationById("SomeId");

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
                             c.GetEntriesByType(It.IsAny<string>(),
                                                It.IsAny<QueryBuilder<Qualification>>(),
                                                It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ContentfulCollection<Qualification> { Items = [qualification] });

        var service = new QualificationsRepository(ClientMock.Object,
                                                   new Mock<IFuzzyAdapter>().Object,
                                                   Logger.Object);

        var result = await service.GetQualifications();

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

        var service = new QualificationsRepository(ClientMock.Object,
                                                   new Mock<IFuzzyAdapter>().Object,
                                                   Logger.Object);

        var result = await service.GetQualifications();

        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInNullParameters_ReturnsAllQualifications()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Ncfe,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              },
                              new Qualification("EYQ-741",
                                                "test",
                                                AwardingOrganisations.Pearson,
                                                3)
                              {
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "def/456/951",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };
        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();

        var repository =
            new QualificationsRepository(mockContentfulClient.Object,
                                         mockFuzzyAdapter.Object,
                                         mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        var filteredQualifications =
            await repository.GetFilteredQualifications(null, null, null, null, null);

        filteredQualifications.Should().NotBeNull();
        filteredQualifications.Count.Should().Be(2);
        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(2);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("limit", "500");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInLevel_ClientContainsLevelInQuery()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Ncfe,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object,
                                         mockFuzzyAdapter.Object,
                                         mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await repository.GetFilteredQualifications(4, null, null, null, null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(3);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("limit", "500");
        queryString.Should().Contain("fields.qualificationLevel", "4");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInAwardingOrganisation_ClientContainsAwardingOrganisationInQuery()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Ncfe,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await repository.GetFilteredQualifications(null, null, null, AwardingOrganisations.Ncfe, null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(3);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("limit", "500");
        queryString.Should().Contain("fields.awardingOrganisationTitle[in]",
                                     $"{AwardingOrganisations.AllHigherEducation},{AwardingOrganisations.Various},{AwardingOrganisations.Ncfe}");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInLevelMonthAndNullParameters_ReturnsAllQualifications()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Ncfe,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              },
                              new Qualification("EYQ-741",
                                                "test",
                                                AwardingOrganisations.Pearson,
                                                4)
                              {
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "def/456/951",
                                  AdditionalRequirements = "requirements"
                              },
                              new Qualification("EYQ-752",
                                                "test",
                                                AwardingOrganisations.Cache,
                                                4)
                              {
                                  FromWhichYear = "Sep-21",
                                  QualificationNumber = "ghi/456/951",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };
        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        var filteredQualifications = await repository.GetFilteredQualifications(4, 9, null, null, null);

        filteredQualifications.Should().NotBeNull();
        filteredQualifications.Count.Should().Be(3);
        filteredQualifications[0].QualificationId.Should().Be("EYQ-123");
        filteredQualifications[1].QualificationId.Should().Be("EYQ-741");
        filteredQualifications[2].QualificationId.Should().Be("EYQ-752");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInLevelNullAndYearParameters_ReturnsAllQualifications()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Ncfe,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              },
                              new Qualification("EYQ-741",
                                                "test",
                                                AwardingOrganisations.Pearson,
                                                4)
                              {
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "def/456/951",
                                  AdditionalRequirements = "requirements"
                              },
                              new Qualification("EYQ-752",
                                                "test",
                                                AwardingOrganisations.Cache,
                                                4)
                              {
                                  FromWhichYear = "Sep-21",
                                  QualificationNumber = "ghi/456/951",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };
        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        var filteredQualifications = await repository.GetFilteredQualifications(4, null, 2014, null, null);

        filteredQualifications.Should().NotBeNull();
        filteredQualifications.Count.Should().Be(3);
        filteredQualifications[0].QualificationId.Should().Be("EYQ-123");
        filteredQualifications[1].QualificationId.Should().Be("EYQ-741");
        filteredQualifications[2].QualificationId.Should().Be("EYQ-752");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_FilterOnDates_ReturnsFilteredQualifications()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Ncfe,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              },
                              new Qualification("EYQ-741",
                                                "test",
                                                AwardingOrganisations.Pearson,
                                                4)
                              {
                                  ToWhichYear = "Sep-19",
                                  QualificationNumber = "def/456/951",
                                  AdditionalRequirements = "requirements"
                              },
                              new Qualification("EYQ-746",
                                                "test",
                                                AwardingOrganisations.Cache,
                                                4)
                              {
                                  FromWhichYear = "Sep-15",
                                  QualificationNumber = "ghi/456/951",
                                  AdditionalRequirements = "requirements"
                              },
                              new Qualification("EYQ-752",
                                                "test",
                                                AwardingOrganisations.Cache,
                                                4)
                              {
                                  FromWhichYear = "Sep-21",
                                  QualificationNumber = "ghi/456/951",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };
        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        var filteredQualifications = await repository.GetFilteredQualifications(4, 5, 2016, null, null);

        filteredQualifications.Should().NotBeNull();
        filteredQualifications.Count.Should().Be(3);
        filteredQualifications[0].QualificationId.Should().Be("EYQ-123");
        filteredQualifications[1].QualificationId.Should().Be("EYQ-741");
        filteredQualifications[2].QualificationId.Should().Be("EYQ-746");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_FilterOnDates_MonthIsCaseInsensitive_ReturnsFilteredQualifications()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Ncfe,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              },
                              new Qualification("EYQ-741",
                                                "test",
                                                AwardingOrganisations.Pearson,
                                                4)
                              {
                                  ToWhichYear = "seP-19",
                                  QualificationNumber = "def/456/951",
                                  AdditionalRequirements = "requirements"
                              },
                              new Qualification("EYQ-746",
                                                "test",
                                                AwardingOrganisations.Cache,
                                                4)
                              {
                                  FromWhichYear = "sEp-15",
                                  QualificationNumber = "ghi/456/951",
                                  AdditionalRequirements = "requirements"
                              },
                              new Qualification("EYQ-752",
                                                "test",
                                                AwardingOrganisations.Cache,
                                                4)
                              {
                                  FromWhichYear = "SEP-21",
                                  QualificationNumber = "ghi/456/951",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };
        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        var filteredQualifications = await repository.GetFilteredQualifications(4, 5, 2016, null, null);

        filteredQualifications.Should().NotBeNull();
        filteredQualifications.Count.Should().Be(3);
        filteredQualifications[0].QualificationId.Should().Be("EYQ-123");
        filteredQualifications[1].QualificationId.Should().Be("EYQ-741");
        filteredQualifications[2].QualificationId.Should().Be("EYQ-746");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_ContentfulClientThrowsException_ReturnsEmptyList()
    {
        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ThrowsAsync(new Exception());

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object);

        var filteredQualifications = await repository.GetFilteredQualifications(4, 5, 2016, null, null);

        filteredQualifications.Should().NotBeNull();
        filteredQualifications.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetFilteredQualifications_DataContainsInvalidDateFormat_LogsError()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Ncfe,
                                                4)
                              {
                                  FromWhichYear =
                                      "Sep15", // We expect Mmm-yy, e.g. "Sep-15"
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await repository.GetFilteredQualifications(4, 5, 2016, null, null);

        mockLogger.VerifyError("Qualification date Sep15 has unexpected format");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_DataContainsInvalidMonth_LogsError()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Ncfe,
                                                4)
                              {
                                  FromWhichYear =
                                      "Sept-15", // "Sept" in the data: we expect "Sep"
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await repository.GetFilteredQualifications(4, 5, 2016, null, null);

        mockLogger.VerifyError("Qualification date Sept-15 contains unexpected month value");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_DataContainsInvalidYear_LogsError()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Ncfe,
                                                4)
                              {
                                  FromWhichYear = "Sep-15",
                                  ToWhichYear = "Aug-1a", // invalid year typo
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await repository.GetFilteredQualifications(4, 5, 2016, null, null);

        mockLogger.VerifyError("Qualification date Aug-1a contains unexpected year value");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInEdexcel_QueryIncludesPearson()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Pearson,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await repository.GetFilteredQualifications(null, null, null, AwardingOrganisations.Edexcel, null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(3);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("limit", "500");
        queryString.Should().Contain("fields.awardingOrganisationTitle[in]",
                                     $"{AwardingOrganisations.AllHigherEducation},{AwardingOrganisations.Various},{AwardingOrganisations.Edexcel},{AwardingOrganisations.Pearson}");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInPearson_QueryIncludesEdexcel()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Pearson,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await repository.GetFilteredQualifications(null, null, null, AwardingOrganisations.Pearson, null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(3);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("limit", "500");
        queryString.Should().Contain("fields.awardingOrganisationTitle[in]",
                                     $"{AwardingOrganisations.AllHigherEducation},{AwardingOrganisations.Various},{AwardingOrganisations.Edexcel},{AwardingOrganisations.Pearson}");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInNcfeAndNoStartDate_QueryDoesntContainCache()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Ncfe,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await repository.GetFilteredQualifications(null, null, null, AwardingOrganisations.Ncfe, null);

        var queryString = mockQueryBuilder.GetQueryString();

        var awardingOrganisations = queryString.First(q => q.Key == "fields.awardingOrganisationTitle[in]");
        awardingOrganisations.Value.Should().NotContain(AwardingOrganisations.Cache);
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInCacheAndNoStartDate_QueryDoesntContainNcfe()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Cache,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await repository.GetFilteredQualifications(null, null, null, AwardingOrganisations.Cache, null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(3);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("limit", "500");
        queryString.Should().Contain("fields.awardingOrganisationTitle[in]",
                                     $"{AwardingOrganisations.AllHigherEducation},{AwardingOrganisations.Various},{AwardingOrganisations.Cache}");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInNcfeAndStartDateLessThanSept14_QueryDoesntContainCache()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Ncfe,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await repository.GetFilteredQualifications(null, 8, 2014, AwardingOrganisations.Ncfe, null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(3);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("limit", "500");
        queryString.Should().Contain("fields.awardingOrganisationTitle[in]",
                                     $"{AwardingOrganisations.AllHigherEducation},{AwardingOrganisations.Various},{AwardingOrganisations.Ncfe}");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInCacheAndStartDateLessThanSept14_QueryDoesntContainNcfe()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Cache,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await repository.GetFilteredQualifications(null, 8, 2014, AwardingOrganisations.Cache, null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(3);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("limit", "500");
        queryString.Should().Contain("fields.awardingOrganisationTitle[in]",
                                     $"{AwardingOrganisations.AllHigherEducation},{AwardingOrganisations.Various},{AwardingOrganisations.Cache}");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInNcfeAndStartDateGreaterThanSept14_QueryContainsCache()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Ncfe,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await repository.GetFilteredQualifications(null, 9, 2014, AwardingOrganisations.Ncfe, null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(3);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("limit", "500");
        queryString.Should().Contain("fields.awardingOrganisationTitle[in]",
                                     $"{AwardingOrganisations.AllHigherEducation},{AwardingOrganisations.Various},{AwardingOrganisations.Ncfe},{AwardingOrganisations.Cache}");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInCacheAndStartDateGreaterThanSept14_QueryContainsNcfe()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Cache,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await repository.GetFilteredQualifications(null, 9, 2014, AwardingOrganisations.Cache, null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(3);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("limit", "500");
        queryString.Should().Contain("fields.awardingOrganisationTitle[in]",
                                     $"{AwardingOrganisations.AllHigherEducation},{AwardingOrganisations.Various},{AwardingOrganisations.Ncfe},{AwardingOrganisations.Cache}");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInQualificationNameThatProducesWeightAbove70_ReturnsQualification()
    {
        // ReSharper disable once StringLiteralTypo
        const string qualificationSearch = "teknical";

        const string technicalDiplomaInChildCare = "Technical Diploma in Child Care";

        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                technicalDiplomaInChildCare,
                                                AwardingOrganisations.Cache,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              },
                              new Qualification("EYQ-123",
                                                "Diploma in Early Years Child Care",
                                                AwardingOrganisations.Cache,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        mockFuzzyAdapter
            .Setup(a => a.PartialRatio(qualificationSearch.ToLower(), technicalDiplomaInChildCare.ToLower()))
            .Returns(80);

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        var qualifications =
            await repository.GetFilteredQualifications(null, null, null, AwardingOrganisations.Cache,
                                                       qualificationSearch);

        qualifications.Should().NotBeNull();
        qualifications.Count.Should().Be(1);
    }

    [TestMethod]
    public async Task
        GetFilteredQualifications_PassInQualificationNameThatProducesWeightEqual70_DoesNotReturnQualification()
    {
        // ReSharper disable once StringLiteralTypo
        const string qualificationSearch = "teknical";

        const string technicalDiplomaInChildCare = "Technical Diploma in Child Care";

        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                technicalDiplomaInChildCare,
                                                AwardingOrganisations.Cache,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              },
                              new Qualification("EYQ-123",
                                                "Diploma in Early Years Child Care",
                                                AwardingOrganisations.Cache,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        mockFuzzyAdapter.Setup(a => a.PartialRatio(qualificationSearch, technicalDiplomaInChildCare)).Returns(70);

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        var qualifications =
            await repository.GetFilteredQualifications(null, null, null, AwardingOrganisations.Cache,
                                                       qualificationSearch);

        qualifications.Should().NotBeNull();
        qualifications.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetFilteredQualifications_StartDateAfterExpiryExpiration_ResultsDontIncludeQualification()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Ncfe,
                                                4)
                              {
                                  FromWhichYear = "Apr-15",
                                  ToWhichYear = "Aug-19",
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        var filteredQualifications =
            await repository.GetFilteredQualifications(4, 09, 2024, AwardingOrganisations.Ncfe, null);

        filteredQualifications.Should().NotBeNull();
        filteredQualifications.Count.Should().Be(0);
    }

    [TestMethod]
    public async Task GetFilteredQualifications_StartDateIsNotNullEndDateIsNull_ResultIncludesQualification()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items =
                          [
                              new Qualification("EYQ-123",
                                                "test",
                                                AwardingOrganisations.Ncfe,
                                                4)
                              {
                                  FromWhichYear = "Aug-15",
                                  ToWhichYear = null,
                                  QualificationNumber = "abc/123/987",
                                  AdditionalRequirements = "requirements"
                              }
                          ]
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<QualificationsRepository>>();
        var repository =
            new QualificationsRepository(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        var filteredQualifications =
            await repository.GetFilteredQualifications(4, 08, 2019, AwardingOrganisations.Ncfe, null);

        filteredQualifications.Should().NotBeNull();
        filteredQualifications.Count.Should().Be(1);
        filteredQualifications[0].AwardingOrganisationTitle.Should().Be(AwardingOrganisations.Ncfe);
    }
}

public class MockQueryBuilder : QueryBuilder<Qualification>
{
    public List<KeyValuePair<string, string>> GetQueryString()
    {
        return _querystringValues;
    }
}