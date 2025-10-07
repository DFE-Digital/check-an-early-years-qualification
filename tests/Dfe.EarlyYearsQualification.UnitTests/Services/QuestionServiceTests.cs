using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Controllers.Questions;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.Help;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class QuestionServiceTests
{
    private Mock<ILogger<QuestionsController>> _mockLogger = new Mock<ILogger<QuestionsController>>();
    private Mock<IContentService> _mockContentService = new Mock<IContentService>();
    private Mock<IUserJourneyCookieService> _mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
    private Mock<IQualificationsRepository> _mockQualificationsRepository = new Mock<IQualificationsRepository>();
    private Mock<IDateQuestionModelValidator> _mockDateQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
    private Mock<IPlaceholderUpdater> _mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();
    private Mock<IRadioQuestionMapper> _mockRadioQuestionMapper = new Mock<IRadioQuestionMapper>();
    private Mock<IDropdownQuestionMapper> _mockDropdownQuestionMapper = new Mock<IDropdownQuestionMapper>();
    private Mock<IPreCheckPageMapper> _mockPreCheckPageMapper = new Mock<IPreCheckPageMapper>();

    [TestMethod]
    public async Task GetRadioView_Returns_RedirectToActionResult_When_ContentNull()
    {
        // Arrange
        _mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.AreYouCheckingYourOwnQualification)).ReturnsAsync(() => null);

        // Act
        var result = await GetSut().GetRadioView(QuestionPages.AreYouCheckingYourOwnQualification, nameof(QuestionsController.AreYouCheckingYourOwnQualification),
                                  "Questions", "false");

        // Assert
        result.Should().NotBeNull();
        var resultType = result as RedirectToActionResult;

        resultType!.ActionName.Should().Be("Index");
        resultType!.ControllerName.Should().Be("Error");

        _mockContentService.Verify(x => x.GetRadioQuestionPage(It.IsAny<string>()), Times.Once);
        _mockRadioQuestionMapper.Verify(x => x.Map(It.IsAny<RadioQuestionModel>(), It.IsAny<RadioQuestionPage>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task GetRadioView_Returns_ViewResult()
    {
        // Arrange
        _mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.AreYouCheckingYourOwnQualification)).ReturnsAsync(new RadioQuestionPage());

        // Act
        var result = await GetSut().GetRadioView(QuestionPages.AreYouCheckingYourOwnQualification, nameof(QuestionsController.AreYouCheckingYourOwnQualification),
                                  "Questions", "false");

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();
        resultType.ViewName.Should().Be("Radio");

        _mockContentService.Verify(x => x.GetRadioQuestionPage(It.IsAny<string>()), Times.Once);
        _mockRadioQuestionMapper.Verify(x => x.Map(It.IsAny<RadioQuestionModel>(), It.IsAny<RadioQuestionPage>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public async Task GetRadioQuestionPageContent_Calls_ContentService_GetRadioQuestionPage()
    {
        // Act
        var result = await GetSut().GetRadioQuestionPageContent(QuestionPages.AreYouCheckingYourOwnQualification);

        // Assert
        _mockContentService.Verify(x => x.GetRadioQuestionPage(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void ResetUserJourneyCookie_Calls_UserJourneyCookieService_ResetUserJourneyCookie()
    {
        // Act
        GetSut().ResetUserJourneyCookie();

        // Assert
        _mockUserJourneyCookieService.Verify(x => x.ResetUserJourneyCookie(), Times.Once);
    }

    [TestMethod]
    public void GetIsUserCheckingTheirOwnQualification_Calls_UserJourneyCookieService_GetIsUserCheckingTheirOwnQualification()
    {
        // Act
        GetSut().GetIsUserCheckingTheirOwnQualification();

        // Assert
        _mockUserJourneyCookieService.Verify(x => x.GetIsUserCheckingTheirOwnQualification(), Times.Once);
    }

    [TestMethod]
    public void SetIsUserCheckingTheirOwnQualification_Calls_UserJourneyCookieService_SetIsUserCheckingTheirOwnQualification()
    {
        // Arrange
        var option = "true";

        // Act
        GetSut().SetIsUserCheckingTheirOwnQualification(option);

        // Assert
        _mockUserJourneyCookieService.Verify(x => x.SetIsUserCheckingTheirOwnQualification(option), Times.Once);
    }

    [TestMethod]
    public void GetLevelOfQualification_Calls_UserJourneyCookieService_GetLevelOfQualification()
    {
        // Act
        GetSut().GetLevelOfQualification();

        // Assert
        _mockUserJourneyCookieService.Verify(x => x.GetLevelOfQualification(), Times.Once);
    }

    [TestMethod]
    public void GetWhereWasQualificationAwarded_Calls_UserJourneyCookieService_GetWhereWasQualificationAwarded()
    {
        // Act
        GetSut().GetWhereWasQualificationAwarded();

        // Assert
        _mockUserJourneyCookieService.Verify(x => x.GetWhereWasQualificationAwarded(), Times.Once);
    }

    [TestMethod]
    public void GetAwardingOrganisation_Calls_UserJourneyCookieService_GetAwardingOrganisation()
    {
        // Act
        GetSut().GetAwardingOrganisation();

        // Assert
        _mockUserJourneyCookieService.Verify(x => x.GetAwardingOrganisation(), Times.Once);
    }

    [TestMethod]
    public void GetAwardingOrganisationIsNotOnList_Calls_UserJourneyCookieService_GetAwardingOrganisationIsNotOnList()
    {
        // Act
        GetSut().GetAwardingOrganisationIsNotOnList();

        // Assert
        _mockUserJourneyCookieService.Verify(x => x.GetAwardingOrganisationIsNotOnList(), Times.Once);
    }

    [TestMethod]
    public async Task Map_Calls_RadioQuestionMapper_Map()
    {
        // Arrange
        var radioQuestionModel = new RadioQuestionModel();
        var radioQuestionPage = new RadioQuestionPage();
        var actionName = "";
        var controllerName = "";
        var selectedAnswer = "";

        // Act
        _ = await GetSut().Map(radioQuestionModel, radioQuestionPage, actionName, controllerName, selectedAnswer);

        // Assert
        _mockRadioQuestionMapper.Verify(x => x.Map(radioQuestionModel, radioQuestionPage, actionName, controllerName, selectedAnswer), Times.Once);
    }

    [TestMethod]
    public void RedirectBasedOnQualificationLevel2Selected_StartedBetweenSeptember2014AndAugust2019_ReturnsExpected()
    {
        var option = "2";
        _mockUserJourneyCookieService.Setup(x => x.WasStartedBetweenSeptember2014AndAugust2019()).Returns(true);

        // Act
        var result = GetSut().RedirectBasedOnQualificationLevelSelected(option);

        // Assert
        result.Should().NotBeNull();
        var resultType = result as RedirectToActionResult;

        resultType!.ActionName.Should().Be("QualificationsStartedBetweenSept2014AndAug2019");
        resultType!.ControllerName.Should().Be("Advice");

        _mockUserJourneyCookieService.Verify(x => x.SetLevelOfQualification(option), Times.Once);
    }

    [TestMethod]
    public void RedirectBasedOnQualificationLevel7Selected_WasStartedBetweenSeptember2014AndAugust2019_ReturnsExpected()
    {
        var option = "7";
        _mockUserJourneyCookieService.Setup(x => x.WasStartedBetweenSeptember2014AndAugust2019()).Returns(true);

        // Act
        var result = GetSut().RedirectBasedOnQualificationLevelSelected(option);

        // Assert
        result.Should().NotBeNull();
        var resultType = result as RedirectToActionResult;

        resultType!.ActionName.Should().Be(nameof(AdviceController.Level7QualificationStartedBetweenSept2014AndAug2019));
        resultType!.ControllerName.Should().Be("Advice");

        _mockUserJourneyCookieService.Verify(x => x.SetLevelOfQualification(option), Times.Once);
    }

    [TestMethod]
    public void RedirectBasedOnQualificationLevel7Selected_WasStartedOnOrAfterSeptember2019_ReturnsExpected()
    {
        var option = "7";
        _mockUserJourneyCookieService.Setup(x => x.WasStartedBetweenSeptember2014AndAugust2019()).Returns(false);
        _mockUserJourneyCookieService.Setup(x => x.WasStartedOnOrAfterSeptember2019()).Returns(true);

        // Act
        var result = GetSut().RedirectBasedOnQualificationLevelSelected(option);

        // Assert
        result.Should().NotBeNull();
        var resultType = result as RedirectToActionResult;

        resultType!.ActionName.Should().Be(nameof(AdviceController.Level7QualificationAfterAug2019));
        resultType!.ControllerName.Should().Be("Advice");

        _mockUserJourneyCookieService.Verify(x => x.SetLevelOfQualification(option), Times.Once);
    }

    [TestMethod]
    [DataRow(QualificationAwardLocation.OutsideOfTheUnitedKingdom, "QualificationOutsideTheUnitedKingdom")]
    [DataRow(QualificationAwardLocation.Scotland, "QualificationsAchievedInScotland")]
    [DataRow(QualificationAwardLocation.Wales, "QualificationsAchievedInWales")]
    [DataRow(QualificationAwardLocation.NorthernIreland, "QualificationsAchievedInNorthernIreland")]
    public void RedirectBasedOnWhereTheQualificationWasAwarded_ReturnsExpected(string option, string action)
    {
        _mockUserJourneyCookieService.Setup(x => x.SetWhereWasQualificationAwarded(option));

        // Act
        var result = GetSut().RedirectBasedOnWhereTheQualificationWasAwarded(option);

        // Assert
        result.Should().NotBeNull();
        var resultType = result as RedirectToActionResult;

        resultType!.ActionName.Should().Be(action);
        resultType!.ControllerName.Should().Be("Advice");

        _mockUserJourneyCookieService.Verify(x => x.SetWhereWasQualificationAwarded(option), Times.Once);
    }

    [TestMethod]
    public async Task GetPreCheckView_Returns_RedirectToActionResult_When_ContentNull()
    {
        // Arrange
        _mockContentService.Setup(x => x.GetPreCheckPage()).ReturnsAsync(() => null);

        // Act
        var result = await GetSut().GetPreCheckView();

        // Assert

        result.Should().NotBeNull();
        var resultType = result as RedirectToActionResult;

        resultType!.ActionName.Should().Be("Index");
        resultType!.ControllerName.Should().Be("Error");

        _mockContentService.Verify(x => x.GetPreCheckPage(), Times.Once);
        _mockPreCheckPageMapper.Verify(x => x.Map(It.IsAny<PreCheckPage>()), Times.Never);
    }

    [TestMethod]
    public async Task GetPreCheckView_Returns_ViewResult()
    {
        // Arrange
        _mockContentService.Setup(x => x.GetPreCheckPage()).ReturnsAsync(() => new PreCheckPage());

        // Act
        var result = await GetSut().GetPreCheckView();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();
        resultType.ViewName.Should().Be("PreCheck");

        _mockContentService.Verify(x => x.GetPreCheckPage(), Times.Once);
        _mockPreCheckPageMapper.Verify(x => x.Map(It.IsAny<PreCheckPage>()), Times.Once);
    }

    [TestMethod]
    public async Task MapPreCheckModel_Calls_PreCheckPageMapper_Map()
    {
        // Arrange
        var preCheckPage = new PreCheckPage();

        // Act
        _ = await GetSut().MapPreCheckModel(preCheckPage);

        // Assert
        _mockPreCheckPageMapper.Verify(x => x.Map(preCheckPage), Times.Once);
    }

    [TestMethod]
    public async Task GetFilteredQualifications_Calls_QualificationsRepository_Get()
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(x => x.GetLevelOfQualification()).Returns(3);
        _mockUserJourneyCookieService.Setup(x => x.GetWhenWasQualificationStarted()).Returns((3, 2002));

        // Act
        _ = await GetSut().GetFilteredQualifications();

        // Assert
        _mockQualificationsRepository.Verify(x => x.Get(3, 3, 2002, null, null), Times.Once);
    }

    [TestMethod]
    public async Task MapDropdownModel_Calls_DropdownQuestionMapper_Get()
    {
        // Arrange
        DropdownQuestionModel model = new();
        DropdownQuestionPage question = new();
        Qualification qualification = new Qualification("2", "", "", 3);
        List<Qualification> qualifications = new(); 
        string actionName = "";
        string controllerName = "";
        string? selectedAwardingOrganisation = "";
        bool selectedNotOnTheList = true;

        // Act
        _ = await GetSut().MapDropdownModel(model, question, qualifications, actionName, controllerName, selectedAwardingOrganisation, selectedNotOnTheList);

        // Assert
        _mockDropdownQuestionMapper.Verify(x => x.Map(model, question, actionName, controllerName, It.IsAny<IOrderedEnumerable<string>>() , selectedAwardingOrganisation, selectedNotOnTheList), Times.Once);
    }

    [TestMethod]
    public async Task GetDropdownQuestionPage_Calls_ContentService_GetDropdownQuestionPage()
    {
        // Act
        var result = await GetSut().GetDropdownQuestionPage(It.IsAny<string>());

        // Assert
        _mockContentService.Verify(x => x.GetDropdownQuestionPage(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void SetWhatIsTheAwardingOrganisationValuesInCookie_Calls_UserJourneyCookieService_ToSetRelevantDetails()
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());

        // Act
        GetSut().SetWhatIsTheAwardingOrganisationValuesInCookie(new DropdownQuestionModel());

        // Assert
        _mockUserJourneyCookieService.Verify(x => x.SetQualificationNameSearchCriteria(string.Empty), Times.Once);
        _mockUserJourneyCookieService.Verify(x => x.SetAwardingOrganisation(It.IsAny<string>()), Times.Once);
        _mockUserJourneyCookieService.Verify(x => x.SetAwardingOrganisationNotOnList(It.IsAny<bool>()), Times.Once);
        _mockUserJourneyCookieService.Verify(x => x.GetHelpFormEnquiry(), Times.Once);
        _mockUserJourneyCookieService.Verify(x => x.SetHelpFormEnquiry(It.IsAny<HelpFormEnquiry>()), Times.Once);
    }

    [TestMethod]
    public async Task GetDropdownQuestionPage_Calls_ContentService_GetPreCheckPage()
    {
        // Act
        var result = await GetSut().GetPreCheckPage();

        // Assert
        _mockContentService.Verify(x => x.GetPreCheckPage(), Times.Once);
    }

    [TestMethod]
    public void MapDatesModel_Calls_DatesQuestionMapper_Map()
    {
        // Arrange
        DatesQuestionModel model = new();
        DatesQuestionPage question = new()
        {
            StartedQuestion = new DateQuestion()
            {
                ErrorMessage = "Started Error Message"
            },
            AwardedQuestion = new DateQuestion()
            {
                ErrorMessage = "Awarded Error Message"
            }
        };
        string actionName = "";
        string controllerName = "";
        DatesValidationResult? validationResult = null;

        _mockUserJourneyCookieService.Setup(x => x.GetWhenWasQualificationStarted()).Returns((3, 2002));
        _mockUserJourneyCookieService.Setup(x => x.GetWhenWasQualificationAwarded()).Returns((5, 2005));

        // Act
        var result = GetSut().MapDatesModel(model, question, actionName, controllerName, validationResult);

        // Assert
        result.Should().NotBeNull();

        result.StartedQuestion.Should().NotBeNull();
        result.StartedQuestion.SelectedMonth.Should().Be(3);
        result.StartedQuestion.SelectedYear.Should().Be(2002);
        result.AwardedQuestion.Should().NotBeNull();
        result.AwardedQuestion.SelectedMonth.Should().Be(5);
        result.AwardedQuestion.SelectedYear.Should().Be(2005);

        _mockUserJourneyCookieService.Verify(x => x.GetWhenWasQualificationStarted(), Times.Once);
        _mockUserJourneyCookieService.Verify(x => x.GetWhenWasQualificationAwarded(), Times.Once);
    }

    [TestMethod]
    public async Task GetDatesQuestionPage_Calls_ContentService_GetDatesQuestionPage()
    {
        // Act
        var result = await GetSut().GetDatesQuestionPage(It.IsAny<string>());

        // Assert
        _mockContentService.Verify(x => x.GetDatesQuestionPage(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void IsValid_Calls_DateQuestionModelValidator_IsValid()
    {
        // Act
        var result = GetSut().IsValid(It.IsAny<DatesQuestionModel>(), It.IsAny<DatesQuestionPage>());

        // Assert
        _mockDateQuestionModelValidator.Verify(x => x.IsValid(It.IsAny<DatesQuestionModel>(), It.IsAny<DatesQuestionPage>()), Times.Once);
    }

    [TestMethod]
    public void SetWhenWasQualificationStarted_Calls_UserJourneyCookieService_SetWhenWasQualificationStarted()
    {
        // Arrange
        DateQuestionModel question = new DateQuestionModel()
        {
            SelectedMonth = 2,
            SelectedYear = 2003
        };

        // Act
        GetSut().SetWhenWasQualificationStarted(question);

        // Assert
        _mockUserJourneyCookieService.Verify(x => x.SetWhenWasQualificationStarted(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void SetWhenWasQualificationAwarded_Calls_UserJourneyCookieService_SetWhenWasQualificationAwarded()
    {
        // Arrange
        DateQuestionModel question = new DateQuestionModel()
        {
            SelectedMonth = 2,
            SelectedYear = 2006
        };

        // Act
        GetSut().SetWhenWasQualificationAwarded(question);

        // Assert
        _mockUserJourneyCookieService.Verify(x => x.SetWhenWasQualificationAwarded(It.IsAny<string>()), Times.Once);
    }

    private QuestionService GetSut()
    {
        return new QuestionService(
                                _mockLogger.Object,
                                _mockContentService.Object,
                                _mockUserJourneyCookieService.Object,
                                _mockQualificationsRepository.Object,
                                _mockDateQuestionModelValidator.Object,
                                _mockPlaceholderUpdater.Object,
                                _mockRadioQuestionMapper.Object,
                                _mockDropdownQuestionMapper.Object,
                                _mockPreCheckPageMapper.Object
                                );
    }
}