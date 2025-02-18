using System.Globalization;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Services.QualificationSearch;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class QualificationSearchServiceTests
{
    private Mock<IQualificationsRepository> _mockRepository = new();
    private Mock<IContentService> _mockContentService = new();
    private Mock<IGovUkContentParser> _mockContentParser = new();
    private Mock<IUserJourneyCookieService> _mockUserJourneyCookieService = new();

    private QualificationSearchService GetSut()
    {
        return new QualificationSearchService(
                                              _mockRepository.Object,
                                              _mockContentService.Object,
                                              _mockContentParser.Object,
                                              _mockUserJourneyCookieService.Object
                                             );
    }

    [TestInitialize]
    public void Initialize()
    {
        _mockRepository = new Mock<IQualificationsRepository>();
        _mockContentService = new Mock<IContentService>();
        _mockContentParser = new Mock<IGovUkContentParser>();
        _mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
    }

    [TestMethod]
    public void Refine_Calls_CookieService_With_RefineSearch()
    {
        const string refineSearch = "Test";
        var sut = GetSut();

        sut.Refine(refineSearch);

        _mockUserJourneyCookieService.Verify(o => o.SetQualificationNameSearchCriteria(refineSearch));
    }

    [TestMethod]
    public async Task Get_Calls_ContentService_GetQualificationListPage()
    {
        var sut = GetSut();

        await sut.GetQualifications();

        _mockContentService.Verify(o => o.GetQualificationListPage(), Times.Once);
    }

    [TestMethod]
    public async Task Get_NullListPage_Returns_Null()
    {
        _mockContentService.Setup(o => o.GetQualificationListPage()).ReturnsAsync((QualificationListPage)null!);
        var sut = GetSut();

        var result = await sut.GetQualifications();

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetQualifications_GotList_Calls_Repository_Get()
    {
        _mockContentService.Setup(o => o.GetQualificationListPage()).ReturnsAsync(new QualificationListPage());
        var sut = GetSut();
        await sut.GetQualifications();

        _mockRepository.Verify(o => o.Get(
                                          It.IsAny<int?>(),
                                          It.IsAny<int?>(),
                                          It.IsAny<int?>(),
                                          It.IsAny<string?>(),
                                          It.IsAny<string?>()
                                         ), Times.Once);
    }

    [TestMethod]
    public async Task GetFilteredQualifications_GetsDetails_From_CookieService()
    {
        var sut = GetSut();
        await sut.GetFilteredQualifications();

        _mockUserJourneyCookieService.Verify(o => o.GetLevelOfQualification(), Times.Once);
        _mockUserJourneyCookieService.Verify(o => o.GetWhenWasQualificationStarted(), Times.Once);
        _mockUserJourneyCookieService.Verify(o => o.GetAwardingOrganisation(), Times.Once);
        _mockUserJourneyCookieService.Verify(o => o.GetSearchCriteria(), Times.Once);
    }

    [TestMethod]
    public async Task GetFilteredQualifications_Calls_Repository_Get_WithCorrectParams()
    {
        const int levelOfQualification = 123;
        const int startDateMonth = 3;
        const int startDateYear = 2016;
        const string awardingOrganisation = "awarding organisation";
        const string qualificationName = "qualification name";

        _mockUserJourneyCookieService.Setup(o => o.GetLevelOfQualification()).Returns(levelOfQualification);
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((startDateMonth, startDateYear));
        _mockUserJourneyCookieService.Setup(o => o.GetAwardingOrganisation()).Returns(awardingOrganisation);
        _mockUserJourneyCookieService.Setup(o => o.GetSearchCriteria()).Returns(qualificationName);

        var sut = GetSut();
        await sut.GetFilteredQualifications();

        _mockRepository.Verify(o => o.Get(
                                          levelOfQualification,
                                          startDateMonth,
                                          startDateYear,
                                          awardingOrganisation,
                                          qualificationName
                                         ), Times.Once);
    }

    [TestMethod]
    public async Task MapList_Qualifications_Returns_Correct_List()
    {
        var qualifications = new List<Qualification>
                             {
                                 new("qual-1", "qual-name-1", "org-1", 1),
                                 new("qual-2", "qual-name-2", "org-2", 2),
                                 new("qual-3", "qual-name-3", "org-2", 3),
                             };

        var sut = GetSut();

        var result = await sut.MapList(new QualificationListPage(), qualifications);

        var quals = result.Qualifications;
        quals.Count.Should().Be(qualifications.Count);

        for (int i = 0; i < quals.Count; i++)
        {
            var thisResult = quals[i];
            var expectedResult = qualifications[i];

            thisResult.QualificationId.Should().Be(expectedResult.QualificationId);
            thisResult.QualificationName.Should().Be(expectedResult.QualificationName);
            thisResult.AwardingOrganisationTitle.Should().Be(expectedResult.AwardingOrganisationTitle);
            thisResult.QualificationLevel.Should().Be(expectedResult.QualificationLevel);
        }
    }

    [TestMethod]
    public void GetFilterModel_Calls_CookieService()
    {
        var qualificationListPage = new QualificationListPage();
        var sut = GetSut();
        sut.GetFilterModel(qualificationListPage);

        _mockUserJourneyCookieService.Verify(o => o.GetWhereWasQualificationAwarded(), Times.Once);
        _mockUserJourneyCookieService.Verify(o => o.GetWhenWasQualificationStarted(), Times.Once);
        _mockUserJourneyCookieService.Verify(o => o.GetLevelOfQualification(), Times.Once);
        _mockUserJourneyCookieService.Verify(o => o.GetAwardingOrganisation(), Times.Once);
    }

    [TestMethod]
    public void GetFilterModel_BasicModel_IsCorrect()
    {
        const string country = "where awarded";
        const string anyLevelHeading = "any heading";
        const string anyAwardingOrganisation = "any awarding organisation";

        _mockUserJourneyCookieService.Setup(o => o.GetWhereWasQualificationAwarded()).Returns(country);

        var qualificationListPage = new QualificationListPage
                                    {
                                        AnyLevelHeading = anyLevelHeading,
                                        AnyAwardingOrganisationHeading = anyAwardingOrganisation,
                                    };
        var sut = GetSut();
        var result = sut.GetFilterModel(qualificationListPage);

        result.Country.Should().Be(country);
        result.Level.Should().Be(anyLevelHeading);
        result.AwardingOrganisation.Should().Be(anyAwardingOrganisation);
    }

    [TestMethod]
    public void GetFilterModel_GotStartDates_Sets_StartDate()
    {
        const int startDateMonth = 3;
        const int startDateYear = 2016;
        var qualificationListPage = new QualificationListPage
                                    {
                                        StartDatePrefixText = "started"
                                    };
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((startDateMonth, startDateYear));
        var sut = GetSut();
        var result = sut.GetFilterModel(qualificationListPage);

        var expectedDt = new DateOnly(startDateYear, startDateMonth, 1);
        var expectedStartDate = $"{qualificationListPage.StartDatePrefixText} {expectedDt.ToString("MMMM", CultureInfo.InvariantCulture)} {startDateYear}";

        result.StartDate.Should().Be(expectedStartDate);
    }
    
    [TestMethod]
    public void GetFilterModel_GotAwardedDates_Sets_AwardedDate()
    {
        const int awardedDateMonth = 3;
        const int awardedDateYear = 2016;
        var qualificationListPage = new QualificationListPage
                                    {
                                        AwardedDatePrefixText = "awarded"
                                    };
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationAwarded()).Returns((awardedDateMonth, awardedDateYear));
        var sut = GetSut();
        var result = sut.GetFilterModel(qualificationListPage);

        var expectedDt = new DateOnly(awardedDateYear, awardedDateMonth, 1);
        var expectedAwardedDate = $"{qualificationListPage.AwardedDatePrefixText} {expectedDt.ToString("MMMM", CultureInfo.InvariantCulture)} {awardedDateYear}";

        result.AwardedDate.Should().Be(expectedAwardedDate);
    }

    [TestMethod]
    public void GetFilterModel_NotGotStartDates_Ignores_StartDate()
    {
        var qualificationListPage = new QualificationListPage();
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((null, null));
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationAwarded()).Returns((null, null));
        var sut = GetSut();
        var result = sut.GetFilterModel(qualificationListPage);

        result.StartDate.Should().Be(string.Empty);
        result.AwardedDate.Should().Be(string.Empty);
    }

    [TestMethod]
    public void GetFilterModel_GotLevel_Sets_Level()
    {
        const int level = 3;
        var qualificationListPage = new QualificationListPage();
        _mockUserJourneyCookieService.Setup(o => o.GetLevelOfQualification()).Returns(level);
        var sut = GetSut();
        var result = sut.GetFilterModel(qualificationListPage);

        var expectedLevel = $"Level {level}";

        result.Level.Should().Be(expectedLevel);
    }

    [TestMethod]
    public void GetFilterModel_NotGotLevel_Ignores_Level()
    {
        var qualificationListPage = new QualificationListPage();
        _mockUserJourneyCookieService.Setup(o => o.GetLevelOfQualification()).Returns((int?)null);
        var sut = GetSut();
        var result = sut.GetFilterModel(qualificationListPage);

        result.Level.Should().Be(string.Empty);
    }

    [TestMethod]
    public void GetFilterModel_GotAwardingOrganisation_Sets_AwardingOrganisation()
    {
        const string awardingOrganisation = "awarding organisation";
        var qualificationListPage = new QualificationListPage();
        _mockUserJourneyCookieService.Setup(o => o.GetAwardingOrganisation()).Returns(awardingOrganisation);
        var sut = GetSut();
        var result = sut.GetFilterModel(qualificationListPage);

        result.AwardingOrganisation.Should().Be(awardingOrganisation);
    }

    [TestMethod]
    public void GetFilterModel_NotGotAwardingOrganisation_Ignores_AwardingOrganisation()
    {
        var qualificationListPage = new QualificationListPage();
        _mockUserJourneyCookieService.Setup(o => o.GetAwardingOrganisation()).Returns((string?)null);
        var sut = GetSut();
        var result = sut.GetFilterModel(qualificationListPage);

        result.AwardingOrganisation.Should().Be(string.Empty);
    }
}