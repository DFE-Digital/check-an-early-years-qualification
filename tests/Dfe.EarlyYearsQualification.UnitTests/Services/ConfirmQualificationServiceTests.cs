using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Services.ConfirmQualification;
using Dfe.EarlyYearsQualification.Web.Services.QualificationSearch;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class ConfirmQualificationServiceTests
{
    private Mock<IContentService> _mockContentService = new Mock<IContentService>();
    private Mock<IUserJourneyCookieService> _mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

    private Mock<IConfirmQualificationPageMapper> _mockConfirmQualificationPageMapper =
        new Mock<IConfirmQualificationPageMapper>();

    private Mock<IQualificationSearchService> _mockQualificationSearchService = new Mock<IQualificationSearchService>();

    [TestMethod]
    public async Task GetConfirmQualificationPageAsync_Calls_ContentService_GetConfirmQualificationPage()
    {
        // Act
        _ = await GetSut().GetConfirmQualificationPageAsync();

        // Assert
        _mockContentService.Verify(o => o.GetConfirmQualificationPage(), Times.Once);
    }

    [TestMethod]
    public async Task GetFilteredQualifications_Calls_QualificationSearchService_GetFilteredQualifications()
    {
        // Act
        _ = await GetSut().GetFilteredQualifications();

        // Assert
        _mockQualificationSearchService.Verify(o => o.GetFilteredQualifications(), Times.Once);
    }
    
    [TestMethod]
    public async Task GetFilteredQualifications_CallsWithOverride_QualificationSearchService_GetFilteredQualifications()
    {
        // Arrange
        const string searchCriteriaOverride = "override";
        
        // Act
        _ = await GetSut().GetFilteredQualifications(searchCriteriaOverride);

        // Assert
        _mockQualificationSearchService.Verify(o => o.GetFilteredQualifications(searchCriteriaOverride), Times.Once);
    }
    
    [TestMethod]
    public async Task GetQualificationById_Calls_QualificationSearchService_GetQualificationById()
    {
        // Arrange
        const string qualificationId = "ABC-123";
        
        // Act
        _ = await GetSut().GetQualificationById(qualificationId);

        // Assert
        _mockQualificationSearchService.Verify(o => o.GetQualificationById(qualificationId), Times.Once);
    }

    [TestMethod]
    public void GetHelpFormEnquiry_Calls_UserJourneyCookieService_GetHelpFormEnquiry()
    {
        // Act
        GetSut().GetHelpFormEnquiry();

        // Assert
        _mockUserJourneyCookieService.Verify(o => o.GetHelpFormEnquiry(), Times.Once);
    }

    [TestMethod]
    public async Task GetQualificationById_Returns_Expected()
    {
        // Arrange
        var qualificationResult = new Qualification("2", "qualification name 2", "org title 2", 3);
        
        _mockQualificationSearchService.Setup(x => x.GetQualificationById(It.IsAny<string>())).ReturnsAsync(qualificationResult);

        // Act
        var qualification = await GetSut().GetQualificationById("2");

        // Assert
        qualification.Should().NotBeNull();
        qualification.QualificationId.Should().Be("2");
        qualification.QualificationName.Should().Be("qualification name 2");
        qualification.AwardingOrganisationTitle.Should().Be("org title 2");
        qualification.QualificationLevel.Should().Be(3);
    }

    [TestMethod]
    public async Task Map_Calls_ConfirmQualificationPageMapper_Map()
    {
        // Arrange
        var content = It.IsAny<ConfirmQualificationPage>();
        var qualification = It.IsAny<Qualification>();
        var qualifications = It.IsAny<List<Qualification>>();

        // Act
        _ = await GetSut().Map(content, qualification, qualifications);

        // Assert
        _mockConfirmQualificationPageMapper.Verify(o => o.Map(content, qualification, qualifications), Times.Once);
    }

    [TestMethod]
    public void SetHelpFormEnquiry_Calls_UserJourneyCookieService()
    {
        // Act
        GetSut().SetHelpFormEnquiry(new HelpFormEnquiry());

        // Assert
        _mockUserJourneyCookieService.Verify(x => x.SetHelpFormEnquiry(It.IsAny<HelpFormEnquiry>()), Times.Once);
    }

    [TestMethod]
    public void GetAwardingOrganisation_Calls_UserJourneyCookieService()
    {
        // Act
        GetSut().GetAwardingOrganisation();

        // Assert
        _mockUserJourneyCookieService.Verify(x => x.GetAwardingOrganisation(), Times.Once);
    }

    [TestMethod]
    public void ValidSubmitSetCookieValues_Calls_UserJourneyCookieService()
    {
        // Act
        GetSut().ValidSubmitSetCookieValues();

        // Assert
        _mockUserJourneyCookieService.Verify(x => x.SetUserSelectedQualificationFromList(YesOrNo.Yes), Times.Once);
        _mockUserJourneyCookieService.Verify(x => x.ClearAdditionalQuestionsAnswers(), Times.Once);
    }

    [TestMethod]
    [DataRow("Open University OU", AwardingOrganisations.Various, "Open University OU")]
    [DataRow("Open University OU", AwardingOrganisations.Ncfe, "Open University OU")]
    [DataRow(null, AwardingOrganisations.Various, "")]
    [DataRow(null, AwardingOrganisations.Ncfe, AwardingOrganisations.Ncfe)]
    public void
        SetHelpFormAwardingQualificationVariousOrganisationPrepopulates_HelpForm_WithSelectedAwardedOrganisation(
            string? awardingOrgDropdownValue, string pageTitle, string expected)
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(x => x.GetAwardingOrganisation()).Returns(awardingOrgDropdownValue);

        // Act
        var result = GetSut().SetHelpFormAwardingQualification(pageTitle);

        // Assert
        result.Should().Be(expected);
    }

    private ConfirmQualificationService GetSut()
    {
        return new ConfirmQualificationService(
                                               _mockContentService.Object,
                                               _mockUserJourneyCookieService.Object,
                                               _mockConfirmQualificationPageMapper.Object,
                                               _mockQualificationSearchService.Object
                                              );
    }
}