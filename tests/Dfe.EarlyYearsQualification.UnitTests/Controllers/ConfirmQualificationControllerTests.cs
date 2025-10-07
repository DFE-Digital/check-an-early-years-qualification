using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.ConfirmQualification;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class ConfirmQualificationControllerTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public async Task Index_NullOrEmptyIdPassed_ReturnsBadRequest(string id)
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockConfirmQualificationService = new Mock<IConfirmQualificationService>();

        var controller = new ConfirmQualificationController(mockLogger.Object, mockConfirmQualificationService.Object);

        var result = await controller.Index(id);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BadRequestResult));
    }

    [TestMethod]
    public async Task Index_ContentServiceCantFindPageDetails_LogsAndReturnsErrorPage()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockConfirmQualificationService = new Mock<IConfirmQualificationService>();

        mockConfirmQualificationService.Setup(x => x.GetConfirmQualificationPageAsync())
                          .ReturnsAsync(default(ConfirmQualificationPage?));

        var controller = new ConfirmQualificationController(mockLogger.Object, mockConfirmQualificationService.Object);

        var result = await controller.Index("Some ID");

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Index");
        actionResult.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the cookies page");
    }

    [TestMethod]
    public async Task Index_CantFindQualification_LogsAndReturnsError()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockConfirmQualificationService = new Mock<IConfirmQualificationService>();

        mockConfirmQualificationService.Setup(x => x.GetConfirmQualificationPageAsync())
                          .ReturnsAsync(GetConfirmQualificationPageContent());

        mockConfirmQualificationService.Setup(o => o.GetFilteredQualifications())
                        .ReturnsAsync(new List<Qualification>());

        var controller =
            new ConfirmQualificationController(mockLogger.Object, mockConfirmQualificationService.Object);

        var result = await controller.Index("Some ID");

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Index");
        actionResult.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("Could not find details for qualification with ID: Some ID");
    }

    [TestMethod]
    public async Task Index_PageDetailsAndQualificationFound_MapsModelAndReturnsView()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockConfirmQualificationService = new Mock<IConfirmQualificationService>();

        mockConfirmQualificationService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());

        var expectedModel = new ConfirmQualificationPageModel { Heading = "Test" };
        mockConfirmQualificationService.Setup(x => x.Map(It.IsAny<ConfirmQualificationPage>(), It.IsAny<Qualification>(), It.IsAny<List<Qualification>>())).ReturnsAsync(expectedModel);

        var confirmQualificationPageContent = GetConfirmQualificationPageContent();

        mockConfirmQualificationService.Setup(x => x.GetConfirmQualificationPageAsync()).ReturnsAsync(confirmQualificationPageContent);

        var qualification = new Qualification("Some ID",
                                              "Qualification Name",
                                              AwardingOrganisations.Ncfe,
                                              2)
        {
            FromWhichYear = "2014",
            ToWhichYear = "2019",
            QualificationNumber = "ABC/547/900",
            AdditionalRequirements = "additional requirements"
        };

        var qualifications = new List<Qualification> { qualification };

        mockConfirmQualificationService.Setup(o => o.GetFilteredQualifications())
                                .ReturnsAsync(qualifications);

        mockConfirmQualificationService.Setup(o => o.GetQualificationById(qualifications, qualification.QualificationId))
                                .Returns(qualification);

        var controller =
            new ConfirmQualificationController(mockLogger.Object, mockConfirmQualificationService.Object);

        var result = await controller.Index("Some ID");

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as ConfirmQualificationPageModel;
        model.Should().NotBeNull();
        model.Should().BeEquivalentTo(expectedModel);
    }

    [TestMethod]
    [DataRow("Open University OU", AwardingOrganisations.Various, "Open University OU")]
    [DataRow("Open University OU", AwardingOrganisations.Ncfe, "Open University OU")]
    [DataRow(null, AwardingOrganisations.Various, "")]
    [DataRow(null, AwardingOrganisations.Ncfe, AwardingOrganisations.Ncfe)]
    public async Task Index_VariousOrganisationPrepopulates_HelpForm_WithSelectedAwardedOrganisation(string? awardingOrgDropdownValue, string pageTitle, string expected)
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockConfirmQualificationService = new Mock<IConfirmQualificationService>();

        mockConfirmQualificationService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());
        mockConfirmQualificationService.Setup(x => x.SetHelpFormAwardingQualification(pageTitle)).Returns(expected);


        var expectedModel = new ConfirmQualificationPageModel { Heading = "Test" };
        mockConfirmQualificationService.Setup(x => x.Map(It.IsAny<ConfirmQualificationPage>(), It.IsAny<Qualification>(), It.IsAny<List<Qualification>>())).ReturnsAsync(expectedModel);

        var confirmQualificationPageContent = GetConfirmQualificationPageContent();

        mockConfirmQualificationService.Setup(x => x.GetConfirmQualificationPageAsync()).ReturnsAsync(confirmQualificationPageContent);

        var qualification = new Qualification("Some ID",
                                              "Qualification Name",
                                              pageTitle,
                                              2)
        {
            FromWhichYear = "2014",
            ToWhichYear = "2019",
            QualificationNumber = "ABC/547/900",
            AdditionalRequirements = "additional requirements"
        };

        var qualifications = new List<Qualification> { qualification };

        mockConfirmQualificationService.Setup(o => o.GetFilteredQualifications())
                                .ReturnsAsync(qualifications);

        mockConfirmQualificationService.Setup(o => o.GetQualificationById(qualifications, qualification.QualificationId))
                                .Returns(qualification);

        var controller = new ConfirmQualificationController(mockLogger.Object, mockConfirmQualificationService.Object);

        var result = await controller.Index("Some ID");

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as ConfirmQualificationPageModel;
        model.Should().NotBeNull();
        model.Should().BeEquivalentTo(expectedModel);

        var enquiry = mockConfirmQualificationService.Object.GetHelpFormEnquiry();
        enquiry.QualificationName.Should().Be(qualification.QualificationName);
        enquiry.AwardingOrganisation.Should().Be(expected);
        mockConfirmQualificationService.Verify(x => x.SetHelpFormEnquiry(It.IsAny<HelpFormEnquiry>()), Times.Once);
    }

    [TestMethod]
    public async Task Post_InvalidModel_CantGetPageContent_LogsAndReturnsError()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockConfirmQualificationService = new Mock<IConfirmQualificationService>();

        mockConfirmQualificationService.Setup(x => x.GetConfirmQualificationPageAsync()).ReturnsAsync(default(ConfirmQualificationPage?));

        var qualification = new Qualification("Some ID",
                                              "Qualification Name",
                                              AwardingOrganisations.Ncfe,
                                              2)
        {
            FromWhichYear = "2014",
            ToWhichYear = "2019",
            QualificationNumber = "ABC/547/900",
            AdditionalRequirements = "additional requirements"
        };

        var qualifications = new List<Qualification> { qualification };

        mockConfirmQualificationService.Setup(o => o.GetFilteredQualifications())
                                .ReturnsAsync(qualifications);

        mockConfirmQualificationService.Setup(o => o.GetQualificationById(qualifications, qualification.QualificationId))
                                .Returns(qualification);

        var controller = new ConfirmQualificationController(mockLogger.Object, mockConfirmQualificationService.Object);

        controller.ModelState.AddModelError("test", "error");

        var result = await controller.Confirm(new ConfirmQualificationPageModel { QualificationId = "Some ID" });

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Index");
        actionResult.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the cookies page");
    }

    [TestMethod]
    public async Task Post_InvalidModel_NoQualificationId_LogsAndReturnsError()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockConfirmQualificationService = new Mock<IConfirmQualificationService>();

        var controller = new ConfirmQualificationController(mockLogger.Object, mockConfirmQualificationService.Object);

        var result = await controller.Confirm(new ConfirmQualificationPageModel());

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Index");
        actionResult.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No qualification id provided");
    }

    [TestMethod]
    public async Task Post_InvalidModel_CantFindQualificationId_LogsAndReturnsError()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockConfirmQualificationService = new Mock<IConfirmQualificationService>();

        var qualifications = new List<Qualification>();

        mockConfirmQualificationService.Setup(o => o.GetFilteredQualifications()).ReturnsAsync(qualifications);
        mockConfirmQualificationService.Setup(o => o.GetQualificationById(qualifications, "someinvalidid")).Returns((Qualification?)null);

        var controller = new ConfirmQualificationController(mockLogger.Object, mockConfirmQualificationService.Object);

        controller.ModelState.AddModelError("test", "error");

        var result = await controller.Confirm(new ConfirmQualificationPageModel
        {
            QualificationId = "Some ID"
        });

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Index");
        actionResult.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("Could not find details for qualification with ID: Some ID");
    }

    [TestMethod]
    public async Task Post_InvalidModel_BuildsModelWithHasErrorsAndReturns()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockConfirmQualificationService = new Mock<IConfirmQualificationService>();

        var expectedModel = new ConfirmQualificationPageModel { Heading = "Test" };
        mockConfirmQualificationService.Setup(x => x.Map(It.IsAny<ConfirmQualificationPage>(), It.IsAny<Qualification>(), It.IsAny<List<Qualification>>())).ReturnsAsync(expectedModel);

        var confirmQualificationPageContent = GetConfirmQualificationPageContent();

        mockConfirmQualificationService.Setup(x => x.GetConfirmQualificationPageAsync()).ReturnsAsync(confirmQualificationPageContent);

        var qualification = new Qualification("Some ID",
                                              "Qualification Name",
                                              AwardingOrganisations.Ncfe,
                                              2)
        {
            FromWhichYear = "2014",
            ToWhichYear = "2019",
            QualificationNumber = "ABC/547/900",
            AdditionalRequirements = "additional requirements"
        };


        var qualifications = new List<Qualification> { qualification };

        mockConfirmQualificationService.Setup(o => o.GetFilteredQualifications())
                                .ReturnsAsync(qualifications);

        mockConfirmQualificationService.Setup(o => o.GetQualificationById(qualifications, qualification.QualificationId))
                                .Returns(qualification);

        var controller = new ConfirmQualificationController(mockLogger.Object, mockConfirmQualificationService.Object);

        controller.ModelState.AddModelError("test", "error");

        var result = await controller.Confirm(new ConfirmQualificationPageModel
        {
            QualificationId = "Some ID"
        });

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as ConfirmQualificationPageModel;
        model.Should().NotBeNull();
        model.Should().BeEquivalentTo(expectedModel);
        model.HasErrors.Should().BeTrue();
    }

    [TestMethod]
    public async Task
        Post_ValidModel_PassedYesAndQualificationHasAdditionalRequirements_RedirectsToCheckAdditionalRequirements()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockConfirmQualificationService = new Mock<IConfirmQualificationService>();

        var additionalRequirements = new List<AdditionalRequirementQuestion> { new() };

        var qualification = new Qualification("TEST-123",
                                              "Qualification Name",
                                              AwardingOrganisations.Ncfe,
                                              2)
        {
            FromWhichYear = "2014",
            ToWhichYear = "2019",
            QualificationNumber = "ABC/547/900",
            AdditionalRequirements = "additional requirements",
            AdditionalRequirementQuestions = additionalRequirements
        };

        var qualifications = new List<Qualification> { qualification };

        mockConfirmQualificationService.Setup(o => o.GetFilteredQualifications())
                                .ReturnsAsync(qualifications);

        mockConfirmQualificationService.Setup(o => o.GetQualificationById(qualifications, qualification.QualificationId))
                                .Returns(qualification);

        var controller = new ConfirmQualificationController(mockLogger.Object, mockConfirmQualificationService.Object);

        var result = await controller.Confirm(new ConfirmQualificationPageModel
        {
            QualificationId = "TEST-123",
            ConfirmQualificationAnswer = "yes"
        });

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Index");
        actionResult.ControllerName.Should().Be("CheckAdditionalRequirements");
        actionResult.RouteValues.Should().Contain("qualificationId", "TEST-123");
        actionResult.RouteValues.Should().Contain("questionIndex", 1);
    }

    [TestMethod]
    public async Task
        Post_ValidModel_QualificationHasAdditionalRequirementsButAutomaticallyApprovedAtL6IsTrue_RedirectsToCheckAdditionalRequirements()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockConfirmQualificationService = new Mock<IConfirmQualificationService>();

        var additionalRequirements = new List<AdditionalRequirementQuestion> { new() };

        var qualification = new Qualification("TEST-123",
                                              "Qualification Name",
                                              AwardingOrganisations.Ncfe,
                                              2)
        {
            FromWhichYear = "2014",
            ToWhichYear = "2019",
            QualificationNumber = "ABC/547/900",
            AdditionalRequirements = "additional requirements",
            AdditionalRequirementQuestions = additionalRequirements,
            IsAutomaticallyApprovedAtLevel6 = true
        };


        var qualifications = new List<Qualification> { qualification };

        mockConfirmQualificationService.Setup(o => o.GetFilteredQualifications())
                                .ReturnsAsync(qualifications);

        mockConfirmQualificationService.Setup(o => o.GetQualificationById(qualifications, qualification.QualificationId))
                                .Returns(qualification);

        var controller = new ConfirmQualificationController(mockLogger.Object, mockConfirmQualificationService.Object);

        var result = await controller.Confirm(new ConfirmQualificationPageModel
        {
            QualificationId = "TEST-123",
            ConfirmQualificationAnswer = "yes"
        });

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Index");
        actionResult.ControllerName.Should().Be("QualificationDetails");
        actionResult.RouteValues.Should().ContainSingle("qualificationId", "TEST-123");
    }

    [TestMethod]
    public async Task Post_ValidModel_PassedYes_RedirectsToQualificationDetailsAction()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockConfirmQualificationService = new Mock<IConfirmQualificationService>();

        var qualification = new Qualification("TEST-123",
                                              "Qualification Name",
                                              AwardingOrganisations.Ncfe,
                                              2)
        {
            FromWhichYear = "2014",
            ToWhichYear = "2019",
            QualificationNumber = "ABC/547/900",
            AdditionalRequirements = "additional requirements"
        };


        var qualifications = new List<Qualification> { qualification };

        mockConfirmQualificationService.Setup(o => o.GetFilteredQualifications())
                                .ReturnsAsync(qualifications);

        mockConfirmQualificationService.Setup(o => o.GetQualificationById(qualifications, qualification.QualificationId))
                                .Returns(qualification);

        var controller = new ConfirmQualificationController(mockLogger.Object, mockConfirmQualificationService.Object);

        var result = await controller.Confirm(new ConfirmQualificationPageModel
        {
            QualificationId = "TEST-123",
            ConfirmQualificationAnswer = "yes"
        });

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Index");
        actionResult.ControllerName.Should().Be("QualificationDetails");
        actionResult.RouteValues.Should().ContainSingle("qualificationId", "TEST-123");
    }

    [TestMethod]
    public async Task Post_ValidModel_PassedAnythingButYes_RedirectsBackToTheQualificationDetailsAction()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockConfirmQualificationService = new Mock<IConfirmQualificationService>();

        var qualification = new Qualification("TEST-123",
                                              "Qualification Name",
                                              AwardingOrganisations.Ncfe,
                                              2)
        {
            FromWhichYear = "2014",
            ToWhichYear = "2019",
            QualificationNumber = "ABC/547/900",
            AdditionalRequirements = "additional requirements"
        };

        var qualifications = new List<Qualification> { qualification };

        mockConfirmQualificationService.Setup(o => o.GetFilteredQualifications())
                                .ReturnsAsync(qualifications);

        mockConfirmQualificationService.Setup(o => o.GetQualificationById(qualifications, qualification.QualificationId))
                                .Returns(qualification);

        var controller = new ConfirmQualificationController(mockLogger.Object, mockConfirmQualificationService.Object);


        var result = await controller.Confirm(new ConfirmQualificationPageModel
        {
            QualificationId = "TEST-123",
            ConfirmQualificationAnswer = "not yes"
        });

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Get");
        actionResult.ControllerName.Should().Be("QualificationSearch");
    }

    [TestMethod]
    public async Task Post_ValidModel_ClearsUserJourneyAdditionalAnswers()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockConfirmQualificationService = new Mock<IConfirmQualificationService>();

        var qualification = new Qualification("TEST-123",
                                              "Qualification Name",
                                              AwardingOrganisations.Ncfe,
                                              2)
        {
            FromWhichYear = "2014",
            ToWhichYear = "2019",
            QualificationNumber = "ABC/547/900",
            AdditionalRequirements = "additional requirements"
        };


        var qualifications = new List<Qualification> { qualification };

        mockConfirmQualificationService.Setup(o => o.GetFilteredQualifications())
                                .ReturnsAsync(qualifications);

        mockConfirmQualificationService.Setup(o => o.GetQualificationById(qualifications, qualification.QualificationId))
                                .Returns(qualification);

        var controller = new ConfirmQualificationController(mockLogger.Object, mockConfirmQualificationService.Object);

        await controller.Confirm(new ConfirmQualificationPageModel
        {
            QualificationId = "TEST-123",
            ConfirmQualificationAnswer = "yes"
        });

        mockConfirmQualificationService.Verify(x => x.ValidSubmitSetCookieValues(), Times.Once);
    }

    private static ConfirmQualificationPage GetConfirmQualificationPageContent()
    {
        return new ConfirmQualificationPage
        {
            QualificationLabel = "Test qualification label",
            BackButton = new NavigationLink
            {
                DisplayText = "Test back button",
                OpenInNewTab = false,
                Href = "/select-a-qualification-to-check"
            },
            ErrorText = "Test error text",
            ButtonText = "Test button text",
            LevelLabel = "Test level label",
            DateAddedLabel = "Test date added label",
            Heading = "Test heading",
            Options =
                   [
                       new Option
                       {
                           Label = "yes",
                           Value = "yes"
                       },
                       new Option
                       {
                           Label = "no",
                           Value = "no"
                       }
                   ],
            RadioHeading = "Test radio heading",
            AwardingOrganisationLabel = "Test awarding organisation label",
            ErrorBannerHeading = "Test error banner heading",
            ErrorBannerLink = "Test error banner link",
            AnswerDisclaimerText = "Answer disclaimer text",
            NoAdditionalRequirementsButtonText = "Get result",
            QualificationNumberLabel = "Qualification Number (QN)"
        };
    }
}