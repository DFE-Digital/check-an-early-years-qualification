using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class CheckAdditionalRequirementsControllerTests
{
    [TestMethod]
    public async Task Index_ModelStateInValid_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object, mockContentService.Object,
                                                                   mockHtmlRenderer.Object,
                                                                   mockUserJourneyCookieService.Object);
        controller.ModelState.AddModelError("test", "error");

        var result = await controller.Index("");

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No qualificationId passed in");
    }

    [TestMethod]
    public async Task Index_UnableToFindQualification_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        mockContentService.Setup(x => x.GetQualificationById("Test-123")).ReturnsAsync(value: null).Verifiable();

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object, mockContentService.Object,
                                                                   mockHtmlRenderer.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.Index("Test-123");

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockContentService.VerifyAll();
        mockLogger.VerifyError("Could not find details for qualification with ID: Test-123");
    }

    [TestMethod]
    public async Task Index_QualificationHasNullAdditionalRequirements_RedirectsToQualificationDetailsPage()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        mockContentService.Setup(x => x.GetQualificationById("Test-123")).ReturnsAsync(CreateQualification(null));

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object, mockContentService.Object,
                                                                   mockHtmlRenderer.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.Index("Test-123");

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("QualificationDetails");
    }

    [TestMethod]
    public async Task Index_PageContentIsNull_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        mockContentService.Setup(x => x.GetQualificationById("Test-123"))
                          .ReturnsAsync(CreateQualification(CreateAdditionalRequirementQuestions()));
        mockContentService.Setup(x => x.GetCheckAdditionalRequirementsPage()).ReturnsAsync(value: null);

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object, mockContentService.Object,
                                                                   mockHtmlRenderer.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.Index("Test-123");

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockContentService.VerifyAll();
        mockLogger.VerifyError("No content for the check additional requirements page");
    }

    [TestMethod]
    public async Task Index_PageContentIsReturned_MapsModelAndReturnsView()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var qualification = CreateQualification(CreateAdditionalRequirementQuestions());
        var pageContent = CreatePageContent();
        mockContentService.Setup(x => x.GetQualificationById("Test-123")).ReturnsAsync(qualification);
        mockContentService.Setup(x => x.GetCheckAdditionalRequirementsPage()).ReturnsAsync(pageContent);

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object, mockContentService.Object,
                                                                   mockHtmlRenderer.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.Index("Test-123");

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();
        resultType!.ViewName.Should().Be("Index");

        resultType.Model.Should().NotBeNull();
        resultType.Model.Should().BeAssignableTo<CheckAdditionalRequirementsPageModel>();

        var model = resultType.Model as CheckAdditionalRequirementsPageModel;
        model!.Heading.Should().BeSameAs(pageContent.Heading);
        model.AwardingOrganisationLabel.Should().BeSameAs(pageContent.AwardingOrganisationLabel);
        model.AwardingOrganisation.Should().BeSameAs(qualification.AwardingOrganisationTitle);
        model.BackButton.Should().NotBeNull();
        model.BackButton.Should().BeOfType<NavigationLinkModel>();
        model.BackButton!.Href.Should().BeSameAs(pageContent.BackButton!.Href);
        model.BackButton.DisplayText.Should().BeSameAs(pageContent.BackButton.DisplayText);
        model.BackButton.OpenInNewTab.Should().Be(pageContent.BackButton.OpenInNewTab);
        model.Answers.Should().NotBeNull();
        model.Answers.Count.Should().Be(qualification.AdditionalRequirementQuestions!.Count);
        model.ErrorMessage.Should().BeSameAs(pageContent.ErrorMessage);
        model.ErrorSummaryHeading.Should().BeSameAs(pageContent.ErrorSummaryHeading);
        model.InformationMessage.Should().BeSameAs(pageContent.InformationMessage);
        model.QualificationId.Should().BeSameAs(qualification.QualificationId);
        model.QualificationLevel.Should().Be(qualification.QualificationLevel);
        model.QualificationLevelLabel.Should().Be(pageContent.QualificationLevelLabel);
        model.QualificationLabel.Should().BeSameAs(pageContent.QualificationLabel);
        model.QualificationName.Should().BeSameAs(qualification.QualificationName);
        model.CtaButtonText.Should().BeSameAs(pageContent.CtaButtonText);
        model.QuestionSectionHeading.Should().BeSameAs(pageContent.QuestionSectionHeading);
        model.AdditionalRequirementQuestions.Should().NotBeNull();
        model.AdditionalRequirementQuestions.Count.Should().Be(qualification.AdditionalRequirementQuestions.Count);
        model.HasErrors.Should().BeFalse();
    }

    [TestMethod]
    public async Task Post_ModelStateIsValid_RedirectsToQualificationDetails()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object, mockContentService.Object,
                                                                   mockHtmlRenderer.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.Post(new CheckAdditionalRequirementsPageModel { QualificationId = "Test-123" });
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("QualificationDetails");
        resultType.RouteValues.Should().ContainSingle("qualificationId", "Test-123");
        mockUserJourneyCookieService
            .Verify(x => x.SetAdditionalQuestionsAnswers(It.IsAny<Dictionary<string, string>>()), Times.Once);
    }

    [TestMethod]
    public async Task Post_UnableToFindQualification_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        mockContentService.Setup(x => x.GetQualificationById("Test-123")).ReturnsAsync(value: null).Verifiable();

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object, mockContentService.Object,
                                                                   mockHtmlRenderer.Object,
                                                                   mockUserJourneyCookieService.Object);

        controller.ModelState.AddModelError("test", "test");
        var result = await controller.Post(new CheckAdditionalRequirementsPageModel { QualificationId = "Test-123" });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockContentService.VerifyAll();
        mockLogger.VerifyError("Could not find details for qualification with ID: Test-123");
    }

    [TestMethod]
    public async Task Post_QualificationHasNullAdditionalRequirements_RedirectsToQualificationDetailsPage()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        mockContentService.Setup(x => x.GetQualificationById("Test-123")).ReturnsAsync(CreateQualification(null));

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object, mockContentService.Object,
                                                                   mockHtmlRenderer.Object,
                                                                   mockUserJourneyCookieService.Object);

        controller.ModelState.AddModelError("test", "test");
        var result = await controller.Post(new CheckAdditionalRequirementsPageModel { QualificationId = "Test-123" });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("QualificationDetails");
    }

    [TestMethod]
    public async Task Post_PageContentIsNull_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        mockContentService.Setup(x => x.GetQualificationById("Test-123"))
                          .ReturnsAsync(CreateQualification(CreateAdditionalRequirementQuestions()));
        mockContentService.Setup(x => x.GetCheckAdditionalRequirementsPage()).ReturnsAsync(value: null);

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object, mockContentService.Object,
                                                                   mockHtmlRenderer.Object,
                                                                   mockUserJourneyCookieService.Object);

        controller.ModelState.AddModelError("test", "test");
        var result = await controller.Post(new CheckAdditionalRequirementsPageModel { QualificationId = "Test-123" });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockContentService.VerifyAll();
        mockLogger.VerifyError("No content for the check additional requirements page");
    }

    [TestMethod]
    public async Task Post_PageContentIsReturned_MapsModelAndReturnsView()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var qualification = CreateQualification(CreateAdditionalRequirementQuestions());
        var pageContent = CreatePageContent();
        mockContentService.Setup(x => x.GetQualificationById("Test-123")).ReturnsAsync(qualification);
        mockContentService.Setup(x => x.GetCheckAdditionalRequirementsPage()).ReturnsAsync(pageContent);

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object, mockContentService.Object,
                                                                   mockHtmlRenderer.Object,
                                                                   mockUserJourneyCookieService.Object);

        controller.ModelState.AddModelError("test", "test");

        var answers = new Dictionary<string, string> { { "Test question", "yes" } };

        var result = await controller.Post(new CheckAdditionalRequirementsPageModel
                                           { QualificationId = "Test-123", Answers = answers });

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();
        resultType!.ViewName.Should().Be("Index");

        resultType.Model.Should().NotBeNull();
        resultType.Model.Should().BeAssignableTo<CheckAdditionalRequirementsPageModel>();

        var model = resultType.Model as CheckAdditionalRequirementsPageModel;
        model!.Heading.Should().BeSameAs(pageContent.Heading);
        model.AwardingOrganisationLabel.Should().BeSameAs(pageContent.AwardingOrganisationLabel);
        model.AwardingOrganisation.Should().BeSameAs(qualification.AwardingOrganisationTitle);
        model.BackButton.Should().NotBeNull();
        model.BackButton.Should().BeOfType<NavigationLinkModel>();
        model.BackButton!.Href.Should().BeSameAs(pageContent.BackButton!.Href);
        model.BackButton.DisplayText.Should().BeSameAs(pageContent.BackButton.DisplayText);
        model.BackButton.OpenInNewTab.Should().Be(pageContent.BackButton.OpenInNewTab);
        model.Answers.Should().NotBeNull();
        model.Answers.Count.Should().Be(qualification.AdditionalRequirementQuestions!.Count);
        model.ErrorMessage.Should().BeSameAs(pageContent.ErrorMessage);
        model.ErrorSummaryHeading.Should().BeSameAs(pageContent.ErrorSummaryHeading);
        model.InformationMessage.Should().BeSameAs(pageContent.InformationMessage);
        model.QualificationId.Should().BeSameAs(qualification.QualificationId);
        model.QualificationLevel.Should().Be(qualification.QualificationLevel);
        model.QualificationLevelLabel.Should().Be(pageContent.QualificationLevelLabel);
        model.QualificationLabel.Should().BeSameAs(pageContent.QualificationLabel);
        model.QualificationName.Should().BeSameAs(qualification.QualificationName);
        model.CtaButtonText.Should().BeSameAs(pageContent.CtaButtonText);
        model.QuestionSectionHeading.Should().BeSameAs(pageContent.QuestionSectionHeading);
        model.AdditionalRequirementQuestions.Should().NotBeNull();
        model.AdditionalRequirementQuestions.Count.Should().Be(qualification.AdditionalRequirementQuestions.Count);
        model.HasErrors.Should().BeTrue();
    }

    private static Qualification CreateQualification(
        List<AdditionalRequirementQuestion>? additionalRequirementQuestions)
    {
        return new Qualification("Test-123",
                                 "Test name",
                                 "Awarding Org",
                                 3)
               {
                   FromWhichYear = "Aug-14", QualificationNumber = "ABC/123/789",
                   ToWhichYear = "Additional requirements",
                   AdditionalRequirementQuestions = additionalRequirementQuestions,
                   RatioRequirements = new List<RatioRequirement>()
               };
    }

    private static List<AdditionalRequirementQuestion> CreateAdditionalRequirementQuestions()
    {
        return
        [
            new AdditionalRequirementQuestion
            {
                Question = "Test question",
                Answers =
                [
                    new Option
                    {
                        Label = "Yes",
                        Value = "yes"
                    },
                    new Option
                    {
                        Label = "No",
                        Value = "no"
                    }
                ],
                DetailsContent = new Document(),
                DetailsHeading = "Details heading",
                HintText = "Hint text",
                ConfirmationStatement = "Confirmation statement",
                AnswerToBeFullAndRelevant = true
            },
            new AdditionalRequirementQuestion
            {
                Question = "Test question 2",
                Answers =
                [
                    new Option
                    {
                        Label = "Yes",
                        Value = "yes"
                    },
                    new Option
                    {
                        Label = "No",
                        Value = "no"
                    }
                ],
                DetailsContent = new Document(),
                DetailsHeading = "Details heading",
                HintText = "Hint text",
                ConfirmationStatement = "Confirmation statement",
                AnswerToBeFullAndRelevant = true
            }
        ];
    }

    private static CheckAdditionalRequirementsPage CreatePageContent()
    {
        return new CheckAdditionalRequirementsPage
               {
                   QualificationLabel = "Qualification",
                   BackButton = new NavigationLink
                                {
                                    DisplayText = "Back",
                                    OpenInNewTab = false,
                                    Href = "/"
                                },
                   AwardingOrganisationLabel = "Awarding organisation",
                   Heading = "This is the heading",
                   ErrorMessage = "This is an error",
                   InformationMessage = "This is the information message",
                   CtaButtonText = "Get result",
                   QualificationLevelLabel = "Qualification level",
                   QuestionSectionHeading = "Answer the following questions",
                   ErrorSummaryHeading = "There was a problem"
               };
    }
}