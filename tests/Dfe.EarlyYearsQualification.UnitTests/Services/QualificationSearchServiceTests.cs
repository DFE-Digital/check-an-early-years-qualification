using System.Globalization;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Services.QualificationSearch;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class QualificationSearchServiceTests
{
    private Mock<IGovUkContentParser> _mockContentParser = new();
    private Mock<IContentService> _mockContentService = new();
    private Mock<IQualificationsRepository> _mockRepository = new();
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
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationStarted())
                                     .Returns((startDateMonth, startDateYear));
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
                                 new("qual-3", "qual-name-3", "org-2", 3)
                             };

        var sut = GetSut();

        var result = await sut.MapList(new QualificationListPage(), qualifications);

        var resultQualifications = result.Qualifications;
        resultQualifications.Count.Should().Be(qualifications.Count);

        for (var i = 0; i < resultQualifications.Count; i++)
        {
            var thisResult = resultQualifications[i];
            var expectedResult = qualifications[i];

            thisResult.QualificationId.Should().Be(expectedResult.QualificationId);
            thisResult.QualificationName.Should().Be(expectedResult.QualificationName);
            thisResult.AwardingOrganisationTitle.Should().Be(expectedResult.AwardingOrganisationTitle);
            thisResult.QualificationLevel.Should().Be(expectedResult.QualificationLevel);
        }
    }

    private const string Pre2014Heading = "Pre 2014 Heading";
    private const string Post2014Heading = "Post 2014 Heading";
    private const string Pre2014Body = "Pre 2014 Body";
    private const string Post2014Body = "Post 2014 Body";
    
    [TestMethod]
    [DataRow(6, true, Pre2014Heading, Pre2014Body, true)]
    [DataRow(6, false, Post2014Heading, Post2014Body, true)]
    [DataRow(0, true, Pre2014Heading, Pre2014Body, true)]
    [DataRow(0, false, Post2014Heading, Post2014Body, true)]
    [DataRow(3, true, "", "", false)]
    [DataRow(3, false, "", "", false)]
    public async Task MapList_UserSearchedForLevel_IsPreOrPost2014(int level, bool isPre2014, string expectedHeading, string expectedBody, bool showL6OrNotSureContent)
    {
        var qualifications = new List<Qualification>
                             {
                                 new("qual-1", "qual-name-1", "org-1", 1)
                             };
        
        _mockUserJourneyCookieService.Setup(x => x.GetLevelOfQualification()).Returns(level).Verifiable();
        _mockUserJourneyCookieService.Setup(x => x.WasStartedBeforeSeptember2014()).Returns(isPre2014).Verifiable();

        var sut = GetSut();

        var qualificationListPage = new QualificationListPage
                                    {
                                        Pre2014L6OrNotSureContentHeading = Pre2014Heading,
                                        Pre2014L6OrNotSureContent = ContentfulContentHelper.Paragraph(Pre2014Body),
                                        Post2014L6OrNotSureContentHeading = Post2014Heading,
                                        Post2014L6OrNotSureContent = ContentfulContentHelper.Paragraph(Post2014Body)
                                    };
        
        _mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == qualificationListPage.Pre2014L6OrNotSureContent)))
                          .ReturnsAsync(Pre2014Body);
        
        _mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == qualificationListPage.Post2014L6OrNotSureContent)))
                          .ReturnsAsync(Post2014Body);
        
        var result = await sut.MapList(qualificationListPage, qualifications);

        _mockUserJourneyCookieService.VerifyAll();
        var resultQualifications = result.Qualifications;
        resultQualifications.Count.Should().Be(qualifications.Count);
        result.L6OrNotSureContentHeading.Should().Be(expectedHeading);
        result.L6OrNotSureContent.Should().Be(expectedBody);
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
        const string awardedIn = "awarded in";
        const string awardedBy = "awarded by";
        const string country = "England";
        const string anyLevelHeading = "any level";
        const string anyAwardingOrganisation = "various awarding organisations";

        _mockUserJourneyCookieService.Setup(o => o.GetWhereWasQualificationAwarded()).Returns(country);

        var qualificationListPage = new QualificationListPage
                                    {
                                        AwardedLocationPrefixText = awardedIn,
                                        AwardedByPrefixText = awardedBy,
                                        AnyLevelHeading = anyLevelHeading,
                                        AnyAwardingOrganisationHeading = anyAwardingOrganisation
                                    };
        var sut = GetSut();
        var result = sut.GetFilterModel(qualificationListPage);

        const string expectedCountryResult = $"{awardedIn} {country}";
        const string expectedAwardingOrganisationResult = $"{awardedBy} {anyAwardingOrganisation}";

        result.Country.Should().Be(expectedCountryResult);
        result.Level.Should().Be(anyLevelHeading);
        result.AwardingOrganisation.Should().Be(expectedAwardingOrganisationResult);
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
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationStarted())
                                     .Returns((startDateMonth, startDateYear));
        var sut = GetSut();
        var result = sut.GetFilterModel(qualificationListPage);

        var expectedDt = new DateOnly(startDateYear, startDateMonth, 1);
        var expectedStartDate =
            $"{qualificationListPage.StartDatePrefixText} {expectedDt.ToString("MMMM", CultureInfo.InvariantCulture)} {startDateYear}";

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
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationAwarded())
                                     .Returns((awardedDateMonth, awardedDateYear));
        var sut = GetSut();
        var result = sut.GetFilterModel(qualificationListPage);

        var expectedDt = new DateOnly(awardedDateYear, awardedDateMonth, 1);
        var expectedAwardedDate =
            $"{qualificationListPage.AwardedDatePrefixText} {expectedDt.ToString("MMMM", CultureInfo.InvariantCulture)} {awardedDateYear}";

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
        var qualificationListPage = new QualificationListPage { LevelPrefixText = "level" };
        _mockUserJourneyCookieService.Setup(o => o.GetLevelOfQualification()).Returns(level);
        var sut = GetSut();
        var result = sut.GetFilterModel(qualificationListPage);

        var expectedLevel = $"level {level}";

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
        const string awardedBy = "awarded by";
        const string awardingOrganisation = "awarding organisation";
        var qualificationListPage = new QualificationListPage { AwardedByPrefixText = awardedBy };
        _mockUserJourneyCookieService.Setup(o => o.GetAwardingOrganisation()).Returns(awardingOrganisation);
        var sut = GetSut();
        var result = sut.GetFilterModel(qualificationListPage);

        const string expectedResult = $"{awardedBy} {awardingOrganisation}";

        result.AwardingOrganisation.Should().Be(expectedResult);
    }

    [TestMethod]
    public void GetFilterModel_NotGotAwardingOrganisation_Ignores_AwardingOrganisation()
    {
        const string awardedBy = "awarded by";
        const string awardingOrganisation = "various awarding organisations";
        var qualificationListPage = new QualificationListPage
                                    {
                                        AwardedByPrefixText = "awarded by",
                                        AnyAwardingOrganisationHeading = awardingOrganisation
                                    };
        _mockUserJourneyCookieService.Setup(o => o.GetAwardingOrganisation()).Returns((string?)null);
        var sut = GetSut();
        var result = sut.GetFilterModel(qualificationListPage);

        const string expectedResult = $"{awardedBy} {awardingOrganisation}";

        result.AwardingOrganisation.Should().Be(expectedResult);
    }
}