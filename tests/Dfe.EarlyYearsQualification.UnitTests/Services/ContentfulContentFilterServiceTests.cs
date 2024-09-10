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
        queryString.Count.Should().Be(2);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("limit", "500");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInLevel_ClientContainsLevelInQuery()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items = new[]
                                  {
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
                          Items = new[]
                                  {
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
                          Items = new[]
                                  {
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
                          Items = new[]
                                  {
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
                          Items = new[]
                                  {
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
        queryString.Count.Should().Be(3);
        queryString.Should().Contain("content_type", "Qualification");
        queryString.Should().Contain("limit", "500");
        queryString.Should().Contain("fields.awardingOrganisationTitle[in]",
                                     $"{AwardingOrganisations.AllHigherEducation},{AwardingOrganisations.Various},{AwardingOrganisations.Ncfe}");
    }

    [TestMethod]
    public async Task GetFilteredQualifications_PassInCacheAndNoStartDate_QueryDoesntContainNcfe()
    {
        var results = new ContentfulCollection<Qualification>
                      {
                          Items = new[]
                                  {
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
                          Items = new[]
                                  {
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
                          Items = new[]
                                  {
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
                          Items = new[]
                                  {
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
                          Items = new[]
                                  {
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
                          Items = new[]
                                  {
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
                                  }
                      };

        var mockContentfulClient = new Mock<IContentfulClient>();
        mockContentfulClient.Setup(x => x.GetEntries(
                                                     It.IsAny<QueryBuilder<Qualification>>(),
                                                     It.IsAny<CancellationToken>()))
                            .ReturnsAsync(results);

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        mockFuzzyAdapter.Setup(a => a.PartialRatio(qualificationSearch.ToLower(), technicalDiplomaInChildCare.ToLower())).Returns(80);

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