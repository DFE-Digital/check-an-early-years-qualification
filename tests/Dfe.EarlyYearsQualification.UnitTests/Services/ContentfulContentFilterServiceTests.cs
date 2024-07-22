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

[TestClass]
public class ContentfulContentFilterServiceTests
{
    [TestMethod]
    public async Task GetFilteredQualifications_PassInNullParameters_ReturnsAllQualifications()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        "test",
                                                        "NCFE",
                                                        4,
                                                        "Apr-15",
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null),
                                      new Qualification(
                                                        "EYQ-741",
                                                        "test",
                                                        "Pearson",
                                                        3,
                                                        null,
                                                        "Aug-19",
                                                        "def/456/951",
                                                        "requirements",
                                                        null, 
                                                        null)
                                  }
                      };
        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        var filteredQualifications = await filterService.GetFilteredQualifications(null, null, null, null, null);

        filteredQualifications.Should().NotBeNull();
        filteredQualifications.Count.Should().Be(2);
        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(1);
        queryString.Should().Contain("content_type", "Qualification");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInLevel_ClientContainsLevelInQuery()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        "test",
                                                        "NCFE",
                                                        4,
                                                        "Apr-15",
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await filterService.GetFilteredQualifications(4, null, null, null, null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(2);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("fields.qualificationLevel", "4");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInAwardingOrganisation_ClientContainsAwardingOrganisationInQuery()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        "test",
                                                        "NCFE",
                                                        4,
                                                        "Apr-15",
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await filterService.GetFilteredQualifications(null, null, null, "NCFE", null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(2);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("fields.awardingOrganisationTitle[in]",
                                     "All Higher Education Institutes,Various Awarding Organisations,NCFE");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInLevelMonthAndNullParameters_ReturnsAllQualifications()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        "test",
                                                        "NCFE",
                                                        4,
                                                        "Apr-15",
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null),
                                      new Qualification(
                                                        "EYQ-741",
                                                        "test",
                                                        "Pearson",
                                                        4,
                                                        null,
                                                        "Aug-19",
                                                        "def/456/951",
                                                        "requirements",
                                                        null,
                                                        null),
                                      new Qualification(
                                                        "EYQ-752",
                                                        "test",
                                                        "CACHE",
                                                        4,
                                                        "Sep-21",
                                                        null,
                                                        "ghi/456/951",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };
        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        var filteredQualifications = await filterService.GetFilteredQualifications(4, 9, null, null, null);

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
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        "test",
                                                        "NCFE",
                                                        4,
                                                        "Apr-15",
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null),
                                      new Qualification(
                                                        "EYQ-741",
                                                        "test",
                                                        "Pearson",
                                                        4,
                                                        null,
                                                        "Aug-19",
                                                        "def/456/951",
                                                        "requirements",
                                                        null,
                                                        null),
                                      new Qualification(
                                                        "EYQ-752",
                                                        "test",
                                                        "CACHE",
                                                        4,
                                                        "Sep-21",
                                                        null,
                                                        "ghi/456/951",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };
        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        var filteredQualifications = await filterService.GetFilteredQualifications(4, null, 2014, null, null);

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
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        "test",
                                                        "NCFE",
                                                        4,
                                                        "Apr-15",
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null),
                                      new Qualification(
                                                        "EYQ-741",
                                                        "test",
                                                        "Pearson",
                                                        4,
                                                        null,
                                                        "Sep-19",
                                                        "def/456/951",
                                                        "requirements",
                                                        null,
                                                        null),
                                      new Qualification(
                                                        "EYQ-746",
                                                        "test",
                                                        "CACHE",
                                                        4,
                                                        "Sep-15",
                                                        null,
                                                        "ghi/456/951",
                                                        "requirements",
                                                        null,
                                                        null),
                                      new Qualification(
                                                        "EYQ-752",
                                                        "test",
                                                        "CACHE",
                                                        4,
                                                        "Sep-21",
                                                        null,
                                                        "ghi/456/951",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };
        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        var filteredQualifications = await filterService.GetFilteredQualifications(4, 5, 2016, null, null);

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
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        "test",
                                                        "NCFE",
                                                        4,
                                                        "Apr-15",
                                                        "aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null),
                                      new Qualification(
                                                        "EYQ-741",
                                                        "test",
                                                        "Pearson",
                                                        4,
                                                        null,
                                                        "seP-19",
                                                        "def/456/951",
                                                        "requirements",
                                                        null,
                                                        null),
                                      new Qualification(
                                                        "EYQ-746",
                                                        "test",
                                                        "CACHE",
                                                        4,
                                                        "sEp-15",
                                                        null,
                                                        "ghi/456/951",
                                                        "requirements",
                                                        null,
                                                        null),
                                      new Qualification(
                                                        "EYQ-752",
                                                        "test",
                                                        "CACHE",
                                                        4,
                                                        "SEP-21",
                                                        null,
                                                        "ghi/456/951",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };
        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        var filteredQualifications = await filterService.GetFilteredQualifications(4, 5, 2016, null, null);

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

        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object);

        var filteredQualifications = await filterService.GetFilteredQualifications(4, 5, 2016, null, null);

        filteredQualifications.Should().NotBeNull();
        filteredQualifications.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetFilteredQualifications_DataContainsInvalidDateFormat_LogsError()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        "test",
                                                        "NCFE",
                                                        4,
                                                        "Sep15", // We expect Mmm-yy, e.g. "Sep-15"
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await filterService.GetFilteredQualifications(4, 5, 2016, null, null);

        mockLogger.VerifyError("Qualification date Sep15 has unexpected format");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_DataContainsInvalidMonth_LogsError()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        "test",
                                                        "NCFE",
                                                        4,
                                                        "Sept-15", // "Sept" in the data: we expect "Sep"
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await filterService.GetFilteredQualifications(4, 5, 2016, null, null);

        mockLogger.VerifyError("Qualification date Sept-15 contains unexpected month value");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_DataContainsInvalidYear_LogsError()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        "test",
                                                        "NCFE",
                                                        4,
                                                        "Sep-15",
                                                        "Aug-1a", // invalid year typo
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await filterService.GetFilteredQualifications(4, 5, 2016, null, null);

        mockLogger.VerifyError("Qualification date Aug-1a contains unexpected year value");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInEdexcel_QueryIncludesPearson()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        "test",
                                                        "Pearson",
                                                        4,
                                                        "Apr-15",
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await filterService.GetFilteredQualifications(null, null, null, AwardingOrganisations.Edexcel, null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(2);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("fields.awardingOrganisationTitle[in]",
                                     "All Higher Education Institutes,Various Awarding Organisations,Edexcel (now Pearson Education Ltd),Pearson Education Ltd");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInPearson_QueryIncludesEdexcel()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        "test",
                                                        "Pearson",
                                                        4,
                                                        "Apr-15",
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await filterService.GetFilteredQualifications(null, null, null, AwardingOrganisations.Pearson, null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(2);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("fields.awardingOrganisationTitle[in]",
                                     "All Higher Education Institutes,Various Awarding Organisations,Edexcel (now Pearson Education Ltd),Pearson Education Ltd");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInNCFEAndNoStartDate_QueryDoesntContainCache()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        "test",
                                                        "NCFE",
                                                        4,
                                                        "Apr-15",
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await filterService.GetFilteredQualifications(null, null, null, AwardingOrganisations.Ncfe, null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(2);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("fields.awardingOrganisationTitle[in]",
                                     "All Higher Education Institutes,Various Awarding Organisations,NCFE");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInCACHEAndNoStartDate_QueryDoesntContainNCFE()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        "test",
                                                        "CACHE",
                                                        4,
                                                        "Apr-15",
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await filterService.GetFilteredQualifications(null, null, null, AwardingOrganisations.Cache, null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(2);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("fields.awardingOrganisationTitle[in]",
                                     "All Higher Education Institutes,Various Awarding Organisations,CACHE Council for Awards in Care Health and Education");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInNCFEAndStartDateLessThanSept14_QueryDoesntContainCache()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        "test",
                                                        "NCFE",
                                                        4,
                                                        "Apr-15",
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await filterService.GetFilteredQualifications(null, 8, 2014, AwardingOrganisations.Ncfe, null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(2);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("fields.awardingOrganisationTitle[in]",
                                     "All Higher Education Institutes,Various Awarding Organisations,NCFE");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInCACHEAndStartDateLessThanSept14_QueryDoesntContainNCFE()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        "test",
                                                        "CACHE",
                                                        4,
                                                        "Apr-15",
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await filterService.GetFilteredQualifications(null, 8, 2014, AwardingOrganisations.Cache, null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(2);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("fields.awardingOrganisationTitle[in]",
                                     "All Higher Education Institutes,Various Awarding Organisations,CACHE Council for Awards in Care Health and Education");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInNCFEAndStartDateGreaterThanSept14_QueryContainsCache()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        "test",
                                                        "NCFE",
                                                        4,
                                                        "Apr-15",
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await filterService.GetFilteredQualifications(null, 9, 2014, AwardingOrganisations.Ncfe, null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(2);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("fields.awardingOrganisationTitle[in]",
                                     "All Higher Education Institutes,Various Awarding Organisations,NCFE,CACHE Council for Awards in Care Health and Education");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInCACHEAndStartDateGreaterThanSept14_QueryContainsNCFE()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        "test",
                                                        "CACHE",
                                                        4,
                                                        "Apr-15",
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        await filterService.GetFilteredQualifications(null, 9, 2014, AwardingOrganisations.Cache, null);

        var queryString = mockQueryBuilder.GetQueryString();
        queryString.Count.Should().Be(2);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("fields.awardingOrganisationTitle[in]",
                                     "All Higher Education Institutes,Various Awarding Organisations,NCFE,CACHE Council for Awards in Care Health and Education");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInQualificationNameThatProducesWeightAbove70_ReturnsQualification()
    {
        // ReSharper disable once StringLiteralTypo
        const string qualificationSearch = "teknical";

        const string technicalDiplomaInChildCare = "Technical Diploma in Child Care";

        var results = new ContentfulCollection<Qualification>
                      {
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        technicalDiplomaInChildCare,
                                                        "CACHE",
                                                        4,
                                                        "Apr-15",
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null),
                                      new Qualification(
                                                        "EYQ-123",
                                                        "Diploma in Early Years Child Care",
                                                        "CACHE",
                                                        4,
                                                        "Apr-15",
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        mockFuzzyAdapter.Setup(a => a.PartialRatio(qualificationSearch, technicalDiplomaInChildCare)).Returns(80);

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        var qualifications =
            await filterService.GetFilteredQualifications(null, null, null, AwardingOrganisations.Cache,
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
                          Items = new[]
                                  {
                                      new Qualification(
                                                        "EYQ-123",
                                                        technicalDiplomaInChildCare,
                                                        "CACHE",
                                                        4,
                                                        "Apr-15",
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null),
                                      new Qualification(
                                                        "EYQ-123",
                                                        "Diploma in Early Years Child Care",
                                                        "CACHE",
                                                        4,
                                                        "Apr-15",
                                                        "Aug-19",
                                                        "abc/123/987",
                                                        "requirements",
                                                        null,
                                                        null)
                                  }
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        mockFuzzyAdapter.Setup(a => a.PartialRatio(qualificationSearch, technicalDiplomaInChildCare)).Returns(70);

        var mockQueryBuilder = new MockQueryBuilder();
        var mockLogger = new Mock<ILogger<ContentfulContentFilterService>>();
        var filterService =
            new ContentfulContentFilterService(mockContentfulClient.Object, mockFuzzyAdapter.Object, mockLogger.Object)
            {
                QueryBuilder = mockQueryBuilder
            };

        var qualifications =
            await filterService.GetFilteredQualifications(null, null, null, AwardingOrganisations.Cache,
                                                          qualificationSearch);

        qualifications.Should().NotBeNull();
        qualifications.Should().BeEmpty();
    }
}

public class MockQueryBuilder : QueryBuilder<Qualification>
{
    public List<KeyValuePair<string, string>> GetQueryString()
    {
        return _querystringValues;
    }
}