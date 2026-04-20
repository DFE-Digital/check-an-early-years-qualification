using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Dfe.EarlyYearsQualification.Web.Services.WebView;

namespace Dfe.EarlyYearsQualification.UnitTests.Services.WebView;

[TestClass]
public class WebViewServiceTests
{
    [TestMethod]
    public async Task GetWebViewPage_ReturnsPageFromContentService()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var expectedPage = new WebViewPage();
        mockContentService.Setup(x => x.GetWebViewPage()).ReturnsAsync(expectedPage);

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        var result = await service.GetWebViewPage();

        // Assert
        result.Should().Be(expectedPage);
        mockContentService.Verify(x => x.GetWebViewPage(), Times.Once);
    }

    [TestMethod]
    public async Task GetWebViewPage_ReturnsNull_WhenContentServiceReturnsNull()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        mockContentService.Setup(x => x.GetWebViewPage()).ReturnsAsync((WebViewPage?)null);

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        var result = await service.GetWebViewPage();

        // Assert
        result.Should().BeNull();
        mockContentService.Verify(x => x.GetWebViewPage(), Times.Once);
    }

    [TestMethod]
    public async Task MapWebViewPageContentToViewModelAsync_MapsContentCorrectly()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var content = new WebViewPage();
        var filters = new WebViewFilters
        {
            SearchTerm = "Qualification",
            QualificationLevel = "3",
            QualificationStartDate = "Pre-September 2014",
        };
        var qualifications = new List<Qualification>
        {
            new("EYQ-1", "Qualification 1", "Org 1", 3)
            {
                FromWhichYear = "Sep-11",
                ToWhichYear = "Sep-13",
                EyqlTabs = new List<Tab>
                {
                    new() { Heading = "Pre-September 2014" }
                }
            }
        };
        var expectedModel = new EarlyYearsQualificationListModel();

        mockUserJourneyCookieService.Setup(x => x.GetWebViewFilters()).Returns(filters);
        mockQualificationsRepository.Setup(x => x.Get(3, null, null, null, "Qualification"))
            .ReturnsAsync(qualifications);
        mockWebViewPageMapper.Setup(x => x.Map(content, filters, It.IsAny<List<Qualification>>()))
            .ReturnsAsync(expectedModel);

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        var result = await service.MapWebViewPageContentToViewModelAsync(content);

        // Assert
        result.Should().Be(expectedModel);
        mockUserJourneyCookieService.Verify(x => x.GetWebViewFilters(), Times.Once);
        mockQualificationsRepository.Verify(x => x.Get(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        mockWebViewPageMapper.Verify(x => x.Map(content, filters, It.IsAny<List<Qualification>>()), Times.Once);
    }

    [TestMethod]
    public async Task GetQualifications_WithEmptySearchTerm_PassesNullAsSearchCriteria()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var filters = new WebViewFilters
        {
            SearchTerm = "",
            QualificationLevel = "",
            QualificationStartDate = ""
        };

        var qualifications = new List<Qualification>
        {
            new("Q1", "Qualification 1", "Org 1", 3)
        };

        mockQualificationsRepository.Setup(x => x.Get(null, null, null, null, null))
            .ReturnsAsync(qualifications);

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        var result = await service.GetQualifications(filters);

        // Assert
        result.Should().HaveCount(1);
        result[0].Should().Be(qualifications[0]);
        mockQualificationsRepository.Verify(x => x.Get(null, null, null, null, null), Times.Once);
    }

    [TestMethod]
    public async Task GetQualifications_WithWhitespaceSearchTerm_PassesNullAsSearchCriteria()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var filters = new WebViewFilters
        {
            SearchTerm = "   ",
            QualificationLevel = "",
            QualificationStartDate = ""
        };

        var qualifications = new List<Qualification>
        {
            new("Q1", "Qualification 1", "Org 1", 3)
        };

        mockQualificationsRepository.Setup(x => x.Get(null, null, null, null, null))
            .ReturnsAsync(qualifications);

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        var result = await service.GetQualifications(filters);

        // Assert
        result.Should().HaveCount(1);
        mockQualificationsRepository.Verify(x => x.Get(null, null, null, null, null), Times.Once);
    }

    [TestMethod]
    public async Task GetQualifications_WithValidSearchTerm_PassesSearchTermAsSearchCriteria()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var filters = new WebViewFilters
        {
            SearchTerm = "test search",
            QualificationLevel = "",
            QualificationStartDate = ""
        };

        var qualifications = new List<Qualification>
        {
            new("Q1", "Qualification 1", "Org 1", 3)
        };

        mockQualificationsRepository.Setup(x => x.Get(null, null, null, null, "test search"))
            .ReturnsAsync(qualifications);

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        var result = await service.GetQualifications(filters);

        // Assert
        result.Should().HaveCount(1);
        mockQualificationsRepository.Verify(x => x.Get(null, null, null, null, "test search"), Times.Once);
    }

    [TestMethod]
    public async Task GetQualifications_WithValidQualificationLevel_ParsesLevelCorrectly()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var filters = new WebViewFilters
        {
            SearchTerm = "",
            QualificationLevel = "5",
            QualificationStartDate = ""
        };

        var qualifications = new List<Qualification>
        {
            new("Q1", "Qualification 1", "Org 1", 5)
        };

        mockQualificationsRepository.Setup(x => x.Get(5, null, null, null, null))
            .ReturnsAsync(qualifications);

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        var result = await service.GetQualifications(filters);

        // Assert
        result.Should().HaveCount(1);
        mockQualificationsRepository.Verify(x => x.Get(5, null, null, null, null), Times.Once);
    }

    [TestMethod]
    public async Task GetQualifications_WithInvalidQualificationLevel_UsesZero()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var filters = new WebViewFilters
        {
            SearchTerm = "",
            QualificationLevel = "invalid",
            QualificationStartDate = ""
        };

        var qualifications = new List<Qualification>
        {
            new("Q1", "Qualification 1", "Org 1", 3)
        };

        mockQualificationsRepository.Setup(x => x.Get(null, null, null, null, null))
            .ReturnsAsync(qualifications);

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        var result = await service.GetQualifications(filters);

        // Assert
        result.Should().HaveCount(1);
        mockQualificationsRepository.Verify(x => x.Get(null, null, null, null, null), Times.Once);
    }

    [TestMethod]
    public async Task GetQualifications_WithStartDateFilter_FiltersQualificationsByStartDate()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var filters = new WebViewFilters
        {
            SearchTerm = "",
            QualificationLevel = "",
            QualificationStartDate = "Pre-September 2014"
        };

        var qualifications = new List<Qualification>
        {
            new("Q1", "Qualification 1", "Org 1", 3)
            {
                EyqlTabs = new List<Tab>
                {
                    new() { Heading = "Pre-September 2014" }
                }
            },
            new("Q2", "Qualification 2", "Org 2", 3)
            {
                EyqlTabs = new List<Tab>
                {
                    new() { Heading = "Post-September 2014" }
                }
            },
            new("Q3", "Qualification 3", "Org 3", 3)
            {
                EyqlTabs = new List<Tab>
                {
                    new() { Heading = "Pre-September 2014" }
                }
            }
        };

        mockQualificationsRepository.Setup(x => x.Get(null, null, null, null, null))
            .ReturnsAsync(qualifications);

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        var result = await service.GetQualifications(filters);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(q => q.QualificationId == "Q1");
        result.Should().Contain(q => q.QualificationId == "Q3");
        result.Should().NotContain(q => q.QualificationId == "Q2");
    }

    [TestMethod]
    public async Task GetQualifications_WithEmptyStartDateFilter_ReturnsAllQualifications()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var filters = new WebViewFilters
        {
            SearchTerm = "",
            QualificationLevel = "",
            QualificationStartDate = ""
        };

        var qualifications = new List<Qualification>
        {
            new("Q1", "Qualification 1", "Org 1", 3),
            new("Q2", "Qualification 2", "Org 2", 3)
        };

        mockQualificationsRepository.Setup(x => x.Get(null, null, null, null, null))
            .ReturnsAsync(qualifications);

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        var result = await service.GetQualifications(filters);

        // Assert
        result.Should().HaveCount(2);
    }

    [TestMethod]
    public async Task GetQualifications_OrdersByLevelThenName()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var filters = new WebViewFilters
        {
            SearchTerm = "",
            QualificationLevel = "",
            QualificationStartDate = ""
        };

        var qualifications = new List<Qualification>
        {
            new("Q1", "Z Qualification", "Org 1", 5),
            new("Q2", "A Qualification", "Org 2", 5),
            new("Q3", "B Qualification", "Org 3", 3),
            new("Q4", "A Qualification", "Org 4", 3)
        };

        mockQualificationsRepository.Setup(x => x.Get(null, null, null, null, null))
            .ReturnsAsync(qualifications);

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        var result = await service.GetQualifications(filters);

        // Assert
        result.Should().HaveCount(4);
        result[0].QualificationId.Should().Be("Q4"); // Level 3, A Qualification
        result[1].QualificationId.Should().Be("Q3"); // Level 3, B Qualification
        result[2].QualificationId.Should().Be("Q2"); // Level 5, A Qualification
        result[3].QualificationId.Should().Be("Q1"); // Level 5, Z Qualification
    }

    [TestMethod]
    public void ApplyFilters_SetsFiltersCorrectly()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var existingFilters = new WebViewFilters
        {
            SearchTerm = "old",
            QualificationLevel = "2",
            QualificationStartDate = "Pre-September 2014"
        };

        mockUserJourneyCookieService.Setup(x => x.GetWebViewFilters()).Returns(existingFilters);

        var model = new EarlyYearsQualificationListModel
        {
            SearchTermFilter = "new search",
            QualificationStartDateFilter = "Post-September 2014",
            QualificationLevelFilter = "3"
        };

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        service.ApplyFilters(model);

        // Assert
        existingFilters.SearchTerm.Should().Be("new search");
        existingFilters.QualificationStartDate.Should().Be("Post-September 2014");
        existingFilters.QualificationLevel.Should().Be("3");
        mockUserJourneyCookieService.Verify(x => x.GetWebViewFilters(), Times.Once);
        mockUserJourneyCookieService.Verify(x => x.SetWebViewFilters(existingFilters), Times.Once);
    }

    [TestMethod]
    public void RemoveFilter_RemovesQualificationLevel_WhenFilterContainsQualificationLevel()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var filters = new WebViewFilters
        {
            SearchTerm = "test",
            QualificationLevel = "3",
            QualificationStartDate = "Pre-September 2014"
        };

        mockUserJourneyCookieService.Setup(x => x.GetWebViewFilters()).Returns(filters);

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        service.RemoveFilter("qualification-level");

        // Assert
        filters.QualificationLevel.Should().BeEmpty();
        filters.SearchTerm.Should().Be("test");
        filters.QualificationStartDate.Should().Be("Pre-September 2014");
        mockUserJourneyCookieService.Verify(x => x.SetWebViewFilters(filters), Times.Once);
    }

    [TestMethod]
    public void RemoveFilter_RemovesStartDate_WhenFilterContainsStartDate()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var filters = new WebViewFilters
        {
            SearchTerm = "test",
            QualificationLevel = "3",
            QualificationStartDate = "Post-September 2014"
        };

        mockUserJourneyCookieService.Setup(x => x.GetWebViewFilters()).Returns(filters);

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        service.RemoveFilter("start-date");

        // Assert
        filters.QualificationStartDate.Should().BeEmpty();
        filters.SearchTerm.Should().Be("test");
        filters.QualificationLevel.Should().Be("3");
        mockUserJourneyCookieService.Verify(x => x.SetWebViewFilters(filters), Times.Once);
    }

    [TestMethod]
    public void RemoveFilter_RemovesSearchTerm_WhenFilterContainsSearchTerm()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var filters = new WebViewFilters
        {
            SearchTerm = "test",
            QualificationLevel = "3",
            QualificationStartDate = "Post-September 2014"
        };

        mockUserJourneyCookieService.Setup(x => x.GetWebViewFilters()).Returns(filters);

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        service.RemoveFilter("search-term");

        // Assert
        filters.SearchTerm.Should().BeEmpty();
        filters.QualificationLevel.Should().Be("3");
        filters.QualificationStartDate.Should().Be("Post-September 2014");
        mockUserJourneyCookieService.Verify(x => x.SetWebViewFilters(filters), Times.Once);
    }

    [TestMethod]
    public void RemoveFilter_RemovesMultipleFilters_WhenFilterContainsMultipleKeywords()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var filters = new WebViewFilters
        {
            SearchTerm = "test",
            QualificationLevel = "3",
            QualificationStartDate = "Post-September 2014"
        };

        mockUserJourneyCookieService.Setup(x => x.GetWebViewFilters()).Returns(filters);

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        service.RemoveFilter("qualification-level-start-date");

        // Assert
        filters.QualificationLevel.Should().BeEmpty();
        filters.QualificationStartDate.Should().BeEmpty();
        filters.SearchTerm.Should().Be("test");
        mockUserJourneyCookieService.Verify(x => x.SetWebViewFilters(filters), Times.Once);
    }

    [TestMethod]
    public void RemoveFilter_DoesNothing_WhenFilterDoesNotMatchAnyKeyword()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var filters = new WebViewFilters
        {
            SearchTerm = "test",
            QualificationLevel = "3",
            QualificationStartDate = "Post-September 2014"
        };

        mockUserJourneyCookieService.Setup(x => x.GetWebViewFilters()).Returns(filters);

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        service.RemoveFilter("unknown-filter");

        // Assert
        filters.SearchTerm.Should().Be("test");
        filters.QualificationLevel.Should().Be("3");
        filters.QualificationStartDate.Should().Be("Post-September 2014");
        mockUserJourneyCookieService.Verify(x => x.SetWebViewFilters(filters), Times.Once);
    }

    [TestMethod]
    public async Task GetQualifications_WithStartDateFilter_FiltersOutQualificationsWithNullEyqlTabs()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var filters = new WebViewFilters
        {
            SearchTerm = "",
            QualificationLevel = "",
            QualificationStartDate = "Post-September 2014"
        };

        var qualifications = new List<Qualification>
        {
            new("Q1", "Qualification 1", "Org 1", 3)
            {
                EyqlTabs = new List<Tab>
                {
                    new() { Heading = "Post-September 2014" }
                }
            },
            new("Q2", "Qualification 2", "Org 2", 3), // No EyqlTabs
            new("Q3", "Qualification 3", "Org 3", 3)
            {
                EyqlTabs = new List<Tab>()
            }
        };

        mockQualificationsRepository.Setup(x => x.Get(null, null, null, null, null))
            .ReturnsAsync(qualifications);

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        var result = await service.GetQualifications(filters);

        // Assert
        result.Should().HaveCount(1);
        result[0].QualificationId.Should().Be("Q1");
    }

    [TestMethod]
    public async Task MapWebViewPageContentToViewModelAsync_WithEmptyFilters_ReturnsModel()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var content = new WebViewPage();
        var filters = new WebViewFilters
        {
            SearchTerm = "",
            QualificationLevel = "",
            QualificationStartDate = ""
        };
        var qualifications = new List<Qualification>
        {
            new("Q1", "Qualification 1", "Org 1", 3)
        };
        var expectedModel = new EarlyYearsQualificationListModel();

        mockUserJourneyCookieService.Setup(x => x.GetWebViewFilters()).Returns(filters);
        mockQualificationsRepository.Setup(x => x.Get(null, null, null, null, null))
            .ReturnsAsync(qualifications);
        mockWebViewPageMapper.Setup(x => x.Map(content, filters, It.IsAny<List<Qualification>>()))
            .ReturnsAsync(expectedModel);

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        var result = await service.MapWebViewPageContentToViewModelAsync(content);

        // Assert
        result.Should().Be(expectedModel);
        mockUserJourneyCookieService.Verify(x => x.GetWebViewFilters(), Times.Once);
        mockQualificationsRepository.Verify(x => x.Get(null, null, null, null, null), Times.Once);
    }

    [TestMethod]
    public void SetWebViewFilters_CallsUserJourneyCookieService()
    {
        // Arrange
        var mockContentService = new Mock<IContentService>();
        var mockWebViewPageMapper = new Mock<IWebViewPageMapper>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockQualificationsRepository = new Mock<IQualificationsRepository>();

        var filters = new WebViewFilters
        {
            SearchTerm = "test",
            QualificationLevel = "3",
            QualificationStartDate = "Post-September 2014"
        };

        var service = new WebViewService(
            mockContentService.Object,
            mockWebViewPageMapper.Object,
            mockUserJourneyCookieService.Object,
            mockQualificationsRepository.Object);

        // Act
        service.SetWebViewFilters(filters);

        // Assert
        mockUserJourneyCookieService.Verify(x => x.SetWebViewFilters(filters), Times.Once);
    }
}
