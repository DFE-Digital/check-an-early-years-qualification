using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
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
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);
        controller.ModelState.AddModelError("test", "error");

        var result = await controller.Index("", 1);

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
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        mockRepository.Setup(x => x.GetById("Test-123"))
                      .ReturnsAsync(value: null)
                      .Verifiable();

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.Index("Test-123", 1);

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
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        mockRepository.Setup(x => x.GetById("Test-123"))
                      .ReturnsAsync(CreateQualification(null));

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.Index("Test-123", 1);

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
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        mockRepository.Setup(x => x.GetById("Test-123"))
                      .ReturnsAsync(CreateQualification(CreateAdditionalRequirementQuestions()));

        mockContentService.Setup(x => x.GetCheckAdditionalRequirementsPage()).ReturnsAsync(value: null);

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.Index("Test-123", 1);

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
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var qualification = CreateQualification(CreateAdditionalRequirementQuestions());
        var pageContent = CreatePageContent();

        mockRepository.Setup(x => x.GetById("Test-123")).ReturnsAsync(qualification);

        mockContentService.Setup(x => x.GetCheckAdditionalRequirementsPage()).ReturnsAsync(pageContent);

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.Index("Test-123", 1);

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();
        resultType!.ViewName.Should().Be("Index");

        resultType.Model.Should().NotBeNull();
        resultType.Model.Should().BeAssignableTo<CheckAdditionalRequirementsPageModel>();

        var model = resultType.Model as CheckAdditionalRequirementsPageModel;
        model!.Heading.Should().BeSameAs(pageContent.Heading);
        model.BackButton.Should().NotBeNull();
        model.BackButton.Should().BeOfType<NavigationLinkModel>();
        model.BackButton!.Href.Should().BeSameAs(pageContent.BackButton!.Href);
        model.BackButton.DisplayText.Should().BeSameAs(pageContent.BackButton.DisplayText);
        model.BackButton.OpenInNewTab.Should().Be(pageContent.BackButton.OpenInNewTab);
        model.ErrorMessage.Should().BeSameAs(pageContent.ErrorMessage);
        model.ErrorSummaryHeading.Should().BeSameAs(pageContent.ErrorSummaryHeading);
        model.QualificationId.Should().BeSameAs(qualification.QualificationId);
        model.CtaButtonText.Should().BeSameAs(pageContent.CtaButtonText);
        model.QuestionSectionHeading.Should().BeSameAs(pageContent.QuestionSectionHeading);
        model.AdditionalRequirementQuestion.Should().NotBeNull();
        model.HasErrors.Should().BeFalse();
    }
    
    
    [TestMethod]
    public async Task Index_SecondQuestion_MapsPreviousQuestionBackButton()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var qualification = CreateQualification(CreateAdditionalRequirementQuestions());
        var pageContent = CreatePageContent();

        mockRepository.Setup(x => x.GetById("Test-123")).ReturnsAsync(qualification);

        mockContentService.Setup(x => x.GetCheckAdditionalRequirementsPage()).ReturnsAsync(pageContent);

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.Index("Test-123", 2);

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();
        resultType!.ViewName.Should().Be("Index");

        resultType.Model.Should().NotBeNull();
        resultType.Model.Should().BeAssignableTo<CheckAdditionalRequirementsPageModel>();

        var model = resultType.Model as CheckAdditionalRequirementsPageModel;
        model!.Heading.Should().BeSameAs(pageContent.Heading);
        model.BackButton.Should().NotBeNull();
        model.BackButton.Should().BeOfType<NavigationLinkModel>();
        model.BackButton!.Href.Should().BeSameAs(pageContent.PreviousQuestionBackButton!.Href);
        model.BackButton.DisplayText.Should().BeSameAs(pageContent.PreviousQuestionBackButton.DisplayText);
        model.BackButton.OpenInNewTab.Should().Be(pageContent.PreviousQuestionBackButton.OpenInNewTab);
        model.ErrorMessage.Should().BeSameAs(pageContent.ErrorMessage);
        model.ErrorSummaryHeading.Should().BeSameAs(pageContent.ErrorSummaryHeading);
        model.QualificationId.Should().BeSameAs(qualification.QualificationId);
        model.CtaButtonText.Should().BeSameAs(pageContent.CtaButtonText);
        model.QuestionSectionHeading.Should().BeSameAs(pageContent.QuestionSectionHeading);
        model.AdditionalRequirementQuestion.Should().NotBeNull();
        model.HasErrors.Should().BeFalse();
    }

    [TestMethod]
    public async Task Post_ModelStateIsValidAndHasAnotherAdditionalQuestion_RedirectsToIndex()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        mockRepository.Setup(x => x.GetById("Test-123"))
                      .ReturnsAsync(CreateQualification(CreateAdditionalRequirementQuestions()));

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.Post("Test-123", 1, new CheckAdditionalRequirementsPageModel { QualificationId = "Test-123" });
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("CheckAdditionalRequirements");
        resultType.RouteValues.Should().Contain("qualificationId", "Test-123");
        resultType.RouteValues.Should().Contain("questionIndex", 2);
        mockUserJourneyCookieService
            .Verify(x => x.SetAdditionalQuestionsAnswers(It.IsAny<Dictionary<string, string>>()), Times.Once);
    }
    
    [TestMethod]
    public async Task Post_ModelStateIsValidAndUnableToFindQualification_RedirectsToIndex()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        mockRepository.Setup(x => x.GetById("Test-123"))
                      .ReturnsAsync(value: null);

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.Post("Test-123", 1, new CheckAdditionalRequirementsPageModel { QualificationId = "Test-123" });
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");
    }
    
    [TestMethod]
    public async Task Post_ModelStateIsValid_RedirectsToQualificationDetails()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        mockRepository.Setup(x => x.GetById("Test-123"))
                      .ReturnsAsync(CreateQualification(new List<AdditionalRequirementQuestion> { new () }));

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.Post("Test-123", 1, new CheckAdditionalRequirementsPageModel { QualificationId = "Test-123" });
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("ConfirmAnswers");
        resultType.ControllerName.Should().Be("CheckAdditionalRequirements");
        resultType.RouteValues.Should().ContainSingle("qualificationId", "Test-123");
        mockUserJourneyCookieService
            .Verify(x => x.SetAdditionalQuestionsAnswers(It.IsAny<Dictionary<string, string>>()), Times.Once);
    }

    [TestMethod]
    public async Task Post_UnableToFindQualification_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        mockRepository.Setup(x => x.GetById("Test-123"))
                      .ReturnsAsync(value: null).Verifiable();

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);

        controller.ModelState.AddModelError("test", "test");
        var result = await controller.Post("Test-123", 1, new CheckAdditionalRequirementsPageModel { QualificationId = "Test-123" });

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
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        mockRepository.Setup(x => x.GetById("Test-123"))
                      .ReturnsAsync(CreateQualification(null));

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);

        controller.ModelState.AddModelError("test", "test");
        var result = await controller.Post("Test-123", 1, new CheckAdditionalRequirementsPageModel { QualificationId = "Test-123" });

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
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        mockRepository.Setup(x => x.GetById("Test-123"))
                      .ReturnsAsync(CreateQualification(CreateAdditionalRequirementQuestions()));

        mockContentService.Setup(x => x.GetCheckAdditionalRequirementsPage())
                          .ReturnsAsync(value: null);

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);

        controller.ModelState.AddModelError("test", "test");
        var result = await controller.Post("Test-123", 1, new CheckAdditionalRequirementsPageModel { QualificationId = "Test-123" });

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
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var qualification = CreateQualification(CreateAdditionalRequirementQuestions());
        var pageContent = CreatePageContent();

        mockRepository.Setup(x => x.GetById("Test-123"))
                      .ReturnsAsync(qualification);

        mockContentService.Setup(x => x.GetCheckAdditionalRequirementsPage())
                          .ReturnsAsync(pageContent);

        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);

        controller.ModelState.AddModelError("test", "test");

        var result = await controller.Post("Test-123", 1, new CheckAdditionalRequirementsPageModel { QualificationId = "Test-123" });

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();
        resultType!.ViewName.Should().Be("Index");

        resultType.Model.Should().NotBeNull();
        resultType.Model.Should().BeAssignableTo<CheckAdditionalRequirementsPageModel>();

        var model = resultType.Model as CheckAdditionalRequirementsPageModel;
        model!.Heading.Should().BeSameAs(pageContent.Heading);
        model.BackButton.Should().NotBeNull();
        model.BackButton.Should().BeOfType<NavigationLinkModel>();
        model.BackButton!.Href.Should().BeSameAs(pageContent.BackButton!.Href);
        model.BackButton.DisplayText.Should().BeSameAs(pageContent.BackButton.DisplayText);
        model.BackButton.OpenInNewTab.Should().Be(pageContent.BackButton.OpenInNewTab);
        model.ErrorMessage.Should().BeSameAs(pageContent.ErrorMessage);
        model.ErrorSummaryHeading.Should().BeSameAs(pageContent.ErrorSummaryHeading);
        model.QualificationId.Should().BeSameAs(qualification.QualificationId);
        model.CtaButtonText.Should().BeSameAs(pageContent.CtaButtonText);
        model.QuestionSectionHeading.Should().BeSameAs(pageContent.QuestionSectionHeading);
        model.AdditionalRequirementQuestion.Should().NotBeNull();
        model.HasErrors.Should().BeTrue();
    }

    [TestMethod]
    public async Task ConfirmAnswers_NoContent_Errors()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        
        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.ConfirmAnswers("some id");
        
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockContentService.VerifyAll();
        mockLogger.VerifyError("No content for the check additional requirements answer page content");
    }

    [TestMethod]
    public async Task ConfirmAnswers_NoQualification_Errors()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        
        mockContentService.Setup(x => x.GetCheckAdditionalRequirementsAnswerPage())
                          .ReturnsAsync(new CheckAdditionalRequirementsAnswerPage());
        
        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.ConfirmAnswers("Test-123");
        
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockContentService.VerifyAll();
        mockLogger.VerifyError("Could not find details for qualification with ID: Test-123");
    }

    [TestMethod]
    public async Task ConfirmAnswers_QualificationHasNoQuestions_RedirectToDetailsPage()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var qualification = CreateQualification(null);
        
        mockRepository.Setup(x => x.GetById("Test-123"))
                      .ReturnsAsync(qualification);
        
        mockContentService.Setup(x => x.GetCheckAdditionalRequirementsAnswerPage())
                          .ReturnsAsync(new CheckAdditionalRequirementsAnswerPage());
        
        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.ConfirmAnswers("Test-123");
        
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("QualificationDetails");
        resultType.RouteValues.Should().ContainSingle("qualificationId", "Test-123");
    }

    [TestMethod]
    public async Task ConfirmAnswers_QualHasQuestionsButNoAnswers_RedirectToFirstQuestion()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        
        var qualification = CreateQualification(CreateAdditionalRequirementQuestions());
        
        mockRepository.Setup(x => x.GetById("Test-123"))
                      .ReturnsAsync(qualification);
        
        mockContentService.Setup(x => x.GetCheckAdditionalRequirementsAnswerPage())
                          .ReturnsAsync(new CheckAdditionalRequirementsAnswerPage());
        
        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.ConfirmAnswers("Test-123");
        
        result.Should().NotBeNull();
        
        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("CheckAdditionalRequirements");
        resultType.RouteValues.Should().Contain("qualificationId", "Test-123");
        resultType.RouteValues.Should().Contain("questionIndex", 1);
    }
    
    [TestMethod]
    public async Task ConfirmAnswers_QualHasQuestionsButNotAllAreAnswered_RedirectToFirstQuestion()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        
        var qualification = CreateQualification(CreateAdditionalRequirementQuestions());
        
        mockRepository.Setup(x => x.GetById("Test-123"))
                      .ReturnsAsync(qualification);
        
        mockContentService.Setup(x => x.GetCheckAdditionalRequirementsAnswerPage())
                          .ReturnsAsync(new CheckAdditionalRequirementsAnswerPage());

        mockUserJourneyCookieService.Setup(x => x.GetAdditionalQuestionsAnswers())
                                    .Returns(new Dictionary<string, string>()
                                             {
                                                 { "Some Question", "Some Answer" }
                                             });
        
        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.ConfirmAnswers("Test-123");
        
        result.Should().NotBeNull();
        
        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("CheckAdditionalRequirements");
        resultType.RouteValues.Should().Contain("qualificationId", "Test-123");
        resultType.RouteValues.Should().Contain("questionIndex", 1);
    }
    
    [TestMethod]
    public async Task ConfirmAnswers_QualHasQuestionsAndAllAreAnswered_RedirectToConfirmAnswersPage()
    {
        var mockLogger = new Mock<ILogger<CheckAdditionalRequirementsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        
        var qualification = CreateQualification(CreateAdditionalRequirementQuestions());
        
        mockRepository.Setup(x => x.GetById("Test-123"))
                      .ReturnsAsync(qualification);
        
        mockContentService.Setup(x => x.GetCheckAdditionalRequirementsAnswerPage())
                          .ReturnsAsync(new CheckAdditionalRequirementsAnswerPage
                                        {
                                            BackButton = new NavigationLink
                                                         {
                                                             DisplayText = "Test back button display text",
                                                             Href = "Test back button href",
                                                             OpenInNewTab = false,
                                                         },
                                            ButtonText = "Test button text",
                                            PageHeading = "Test page heading",
                                            AnswerDisclaimerText = "Test answer disclaimer text",
                                            ChangeAnswerText = "Test change answer text"
                                        });

        var answers = new Dictionary<string, string>()
                      {
                          { "Some Question", "Some Answer" },
                          { "Another Question", "Another Answer" }
                      };
        
        mockUserJourneyCookieService.Setup(x => x.GetAdditionalQuestionsAnswers())
                                    .Returns(answers);
        
        var controller = new CheckAdditionalRequirementsController(mockLogger.Object,
                                                                   mockRepository.Object,
                                                                   mockContentService.Object,
                                                                   mockContentParser.Object,
                                                                   mockUserJourneyCookieService.Object);

        var result = await controller.ConfirmAnswers("Test-123");

        result.Should().NotBeNull();
        
        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();
        resultType!.ViewName.Should().Be("ConfirmAnswers");

        resultType.Model.Should().NotBeNull();
        resultType.Model.Should().BeAssignableTo<CheckAdditionalRequirementsAnswerPageModel>();

        var model = resultType.Model as CheckAdditionalRequirementsAnswerPageModel;

        model!.PageHeading.Should().Be("Test page heading");
        model.AnswerDisclaimerText.Should().Be("Test answer disclaimer text");
        model.ChangeAnswerText.Should().Be("Test change answer text");
        model.ButtonText.Should().Be("Test button text");
        model.BackButton!.DisplayText.Should().Be("Test back button display text");
        model.BackButton.OpenInNewTab.Should().BeFalse();
        model.BackButton.Href.Should().Be("Test back button href/Test-123/2");
        model.ButtonText.Should().Be("Test button text");
        model.Answers.Should().BeEquivalentTo(answers);
        model.ChangeQuestionHref.Should().Be("/qualifications/check-additional-questions/Test-123/");
        model.GetResultsHref.Should().Be("/qualifications/qualification-details/Test-123");
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
                   RatioRequirements = []
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
                   PreviousQuestionBackButton = new NavigationLink
                                                {
                                                    DisplayText = "Previous",
                                                    OpenInNewTab = false,
                                                    Href = "/previous"
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