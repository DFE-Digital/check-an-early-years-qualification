using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
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
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyService = new Mock<IUserJourneyCookieService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller =
            new ConfirmQualificationController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockUserJourneyService.Object,
                                               mockContentParser.Object);

        var result = await controller.Index(id);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BadRequestResult));
    }

    [TestMethod]
    public async Task Index_ContentServiceCantFindPageDetails_LogsAndReturnsErrorPage()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();

        mockContentService.Setup(x => x.GetConfirmQualificationPage())
                          .ReturnsAsync(default(ConfirmQualificationPage?));

        var mockUserJourneyService = new Mock<IUserJourneyCookieService>();

        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller =
            new ConfirmQualificationController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockUserJourneyService.Object,
                                               mockContentParser.Object);

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
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();

        mockContentService.Setup(x => x.GetConfirmQualificationPage())
                          .ReturnsAsync(GetConfirmQualificationPageContent());

        mockRepository.Setup(x => x.GetById("Some ID"))
                      .ReturnsAsync(default(Qualification?));

        var mockUserJourneyService = new Mock<IUserJourneyCookieService>();

        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller =
            new ConfirmQualificationController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockUserJourneyService.Object,
                                               mockContentParser.Object);

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
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();

        var confirmQualificationPageContent = GetConfirmQualificationPageContent();

        mockContentService.Setup(x => x.GetConfirmQualificationPage()).ReturnsAsync(confirmQualificationPageContent);

        var qualification = new Qualification("Some ID",
                                              "Qualification Name",
                                              AwardingOrganisations.Ncfe,
                                              2)
                            {
                                FromWhichYear = "2014", ToWhichYear = "2019",
                                QualificationNumber = "ABC/547/900",
                                AdditionalRequirements = "additional requirements"
                            };

        mockRepository.Setup(x => x.GetById("Some ID"))
                      .ReturnsAsync(qualification);

        var mockUserJourneyService = new Mock<IUserJourneyCookieService>();

        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller =
            new ConfirmQualificationController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockUserJourneyService.Object,
                                               mockContentParser.Object);

        var result = await controller.Index("Some ID");

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as ConfirmQualificationPageModel;
        model.Should().NotBeNull();

        model!.BackButton.Should().BeEquivalentTo(new NavigationLinkModel
                                                  {
                                                      DisplayText = "Test back button",
                                                      OpenInNewTab = false,
                                                      Href = "/select-a-qualification-to-check"
                                                  });
        model.HasErrors.Should().BeFalse();
        model.Options.Should().BeEquivalentTo([
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
                                              ]);
        model.Heading.Should().Be(confirmQualificationPageContent.Heading);
        model.ButtonText.Should().Be(confirmQualificationPageContent.NoAdditionalRequirementsButtonText);
        model.ErrorText.Should().Be(confirmQualificationPageContent.ErrorText);
        model.LevelLabel.Should().Be(confirmQualificationPageContent.LevelLabel);
        model.QualificationId.Should().Be("Some ID");
        model.QualificationLabel.Should().Be(confirmQualificationPageContent.QualificationLabel);
        model.QualificationLevel.Should().Be("2");
        model.QualificationName.Should().Be("Qualification Name");
        model.RadioHeading.Should().Be(confirmQualificationPageContent.RadioHeading);
        model.AwardingOrganisationLabel.Should().Be(confirmQualificationPageContent.AwardingOrganisationLabel);
        model.ConfirmQualificationAnswer.Should().Be(string.Empty);
        model.DateAddedLabel.Should().Be(confirmQualificationPageContent.DateAddedLabel);
        model.ErrorBannerHeading.Should().Be(confirmQualificationPageContent.ErrorBannerHeading);
        model.ErrorBannerLink.Should().Be(confirmQualificationPageContent.ErrorBannerLink);
        model.QualificationAwardingOrganisation.Should().Be(AwardingOrganisations.Ncfe);
        model.QualificationDateAdded.Should().Be("2014");
    }

    [TestMethod]
    public async Task Post_InvalidModel_CantGetPageContent_LogsAndReturnsError()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();

        mockContentService.Setup(x => x.GetConfirmQualificationPage()).ReturnsAsync(default(ConfirmQualificationPage?));

        var qualification = new Qualification("Some ID",
                                              "Qualification Name",
                                              AwardingOrganisations.Ncfe,
                                              2)
                            {
                                FromWhichYear = "2014", ToWhichYear = "2019",
                                QualificationNumber = "ABC/547/900",
                                AdditionalRequirements = "additional requirements"
                            };
        mockRepository.Setup(x => x.GetById("Some ID"))
                      .ReturnsAsync(qualification);

        var mockUserJourneyService = new Mock<IUserJourneyCookieService>();

        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller =
            new ConfirmQualificationController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockUserJourneyService.Object,
                                               mockContentParser.Object);

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
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyService = new Mock<IUserJourneyCookieService>();

        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller =
            new ConfirmQualificationController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockUserJourneyService.Object,
                                               mockContentParser.Object);

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
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();

        mockRepository.Setup(x => x.GetById("Some ID"))
                      .ReturnsAsync(default(Qualification?));

        var mockUserJourneyService = new Mock<IUserJourneyCookieService>();

        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller =
            new ConfirmQualificationController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockUserJourneyService.Object,
                                               mockContentParser.Object);

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
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();

        var confirmQualificationPageContent = GetConfirmQualificationPageContent();

        mockContentService.Setup(x => x.GetConfirmQualificationPage()).ReturnsAsync(confirmQualificationPageContent);

        var qualification = new Qualification("Some ID",
                                              "Qualification Name",
                                              AwardingOrganisations.Ncfe,
                                              2)
                            {
                                FromWhichYear = "2014", ToWhichYear = "2019",
                                QualificationNumber = "ABC/547/900",
                                AdditionalRequirements = "additional requirements"
                            };

        mockRepository.Setup(x => x.GetById("Some ID"))
                      .ReturnsAsync(qualification);

        var mockUserJourneyService = new Mock<IUserJourneyCookieService>();

        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller =
            new ConfirmQualificationController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockUserJourneyService.Object,
                                               mockContentParser.Object);

        controller.ModelState.AddModelError("test", "error");

        var result = await controller.Confirm(new ConfirmQualificationPageModel
                                              {
                                                  QualificationId = "Some ID"
                                              });

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as ConfirmQualificationPageModel;
        model.Should().NotBeNull();

        model!.BackButton.Should().BeEquivalentTo(new NavigationLinkModel
                                                  {
                                                      DisplayText = "Test back button",
                                                      OpenInNewTab = false,
                                                      Href = "/select-a-qualification-to-check"
                                                  });
        model.HasErrors.Should().BeTrue();
        model.Options.Should().BeEquivalentTo([
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
                                              ]);
        model.Heading.Should().Be(confirmQualificationPageContent.Heading);
        model.ButtonText.Should().Be(confirmQualificationPageContent.NoAdditionalRequirementsButtonText);
        model.ErrorText.Should().Be(confirmQualificationPageContent.ErrorText);
        model.LevelLabel.Should().Be(confirmQualificationPageContent.LevelLabel);
        model.QualificationId.Should().Be("Some ID");
        model.QualificationLabel.Should().Be(confirmQualificationPageContent.QualificationLabel);
        model.QualificationLevel.Should().Be("2");
        model.QualificationName.Should().Be("Qualification Name");
        model.RadioHeading.Should().Be(confirmQualificationPageContent.RadioHeading);
        model.AwardingOrganisationLabel.Should().Be(confirmQualificationPageContent.AwardingOrganisationLabel);
        model.ConfirmQualificationAnswer.Should().Be(string.Empty);
        model.DateAddedLabel.Should().Be(confirmQualificationPageContent.DateAddedLabel);
        model.ErrorBannerHeading.Should().Be(confirmQualificationPageContent.ErrorBannerHeading);
        model.ErrorBannerLink.Should().Be(confirmQualificationPageContent.ErrorBannerLink);
        model.QualificationAwardingOrganisation.Should().Be(AwardingOrganisations.Ncfe);
        model.QualificationDateAdded.Should().Be("2014");
    }

    [TestMethod]
    public async Task
        Post_ValidModel_PassedYesAndQualificationHasAdditionalRequirements_RedirectsToCheckAdditionalRequirements()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var additionalRequirements = new List<AdditionalRequirementQuestion> { new() };

        var qualification = new Qualification("Some ID",
                                              "Qualification Name",
                                              AwardingOrganisations.Ncfe,
                                              2)
                            {
                                FromWhichYear = "2014", ToWhichYear = "2019",
                                QualificationNumber = "ABC/547/900",
                                AdditionalRequirements = "additional requirements",
                                AdditionalRequirementQuestions = additionalRequirements
                            };

        mockRepository.Setup(x => x.GetById("TEST-123"))
                      .ReturnsAsync(qualification);

        var mockUserJourneyService = new Mock<IUserJourneyCookieService>();

        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller =
            new ConfirmQualificationController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockUserJourneyService.Object,
                                               mockContentParser.Object);

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
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var additionalRequirements = new List<AdditionalRequirementQuestion> { new() };

        var qualification = new Qualification("Some ID",
                                              "Qualification Name",
                                              AwardingOrganisations.Ncfe,
                                              2)
                            {
                                FromWhichYear = "2014", ToWhichYear = "2019",
                                QualificationNumber = "ABC/547/900",
                                AdditionalRequirements = "additional requirements",
                                AdditionalRequirementQuestions = additionalRequirements,
                                IsAutomaticallyApprovedAtLevel6 = true
                            };

        mockRepository.Setup(x => x.GetById("TEST-123"))
                      .ReturnsAsync(qualification);

        var mockUserJourneyService = new Mock<IUserJourneyCookieService>();

        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller =
            new ConfirmQualificationController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockUserJourneyService.Object,
                                               mockContentParser.Object);

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
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();

        var qualification = new Qualification("Some ID",
                                              "Qualification Name",
                                              AwardingOrganisations.Ncfe,
                                              2)
                            {
                                FromWhichYear = "2014", ToWhichYear = "2019",
                                QualificationNumber = "ABC/547/900",
                                AdditionalRequirements = "additional requirements"
                            };

        mockRepository.Setup(x => x.GetById("TEST-123"))
                      .ReturnsAsync(qualification);

        var mockUserJourneyService = new Mock<IUserJourneyCookieService>();

        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller =
            new ConfirmQualificationController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockUserJourneyService.Object,
                                               mockContentParser.Object);

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
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();

        var qualification = new Qualification("Some ID",
                                              "Qualification Name",
                                              AwardingOrganisations.Ncfe,
                                              2)
                            {
                                FromWhichYear = "2014", ToWhichYear = "2019",
                                QualificationNumber = "ABC/547/900",
                                AdditionalRequirements = "additional requirements"
                            };

        mockRepository.Setup(x => x.GetById("TEST-123"))
                      .ReturnsAsync(qualification);

        var mockUserJourneyService = new Mock<IUserJourneyCookieService>();

        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller =
            new ConfirmQualificationController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockUserJourneyService.Object,
                                               mockContentParser.Object);

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
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();

        var qualification = new Qualification("Some ID",
                                              "Qualification Name",
                                              AwardingOrganisations.Ncfe,
                                              2)
                            {
                                FromWhichYear = "2014", ToWhichYear = "2019",
                                QualificationNumber = "ABC/547/900",
                                AdditionalRequirements = "additional requirements"
                            };

        mockRepository.Setup(x => x.GetById("TEST-123"))
                      .ReturnsAsync(qualification);

        var mockUserJourneyService = new Mock<IUserJourneyCookieService>();

        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller =
            new ConfirmQualificationController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockUserJourneyService.Object,
                                               mockContentParser.Object);

        await controller.Confirm(new ConfirmQualificationPageModel
                                 {
                                     QualificationId = "TEST-123",
                                     ConfirmQualificationAnswer = "yes"
                                 });

        mockUserJourneyService.Verify(x => x.ClearAdditionalQuestionsAnswers(), Times.Once);
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
                   NoAdditionalRequirementsButtonText = "Get result"
               };
    }
}