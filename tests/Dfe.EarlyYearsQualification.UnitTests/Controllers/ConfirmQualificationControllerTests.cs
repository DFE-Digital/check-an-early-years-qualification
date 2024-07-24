using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

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
        var mockContentService = new Mock<IContentService>();

        var controller = new ConfirmQualificationController(mockLogger.Object, mockContentService.Object);

        var result = await controller.Index(id);

        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BadRequestResult));
    }

    [TestMethod]
    public async Task Index_ContentServiceCantFindPageDetails_LogsAndReturnsErrorPage()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockContentService = new Mock<IContentService>();

        mockContentService.Setup(x => x.GetConfirmQualificationPage()).ReturnsAsync(default(ConfirmQualificationPage?));

        var controller = new ConfirmQualificationController(mockLogger.Object, mockContentService.Object);

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
        var mockContentService = new Mock<IContentService>();

        mockContentService.Setup(x => x.GetConfirmQualificationPage()).ReturnsAsync(new ConfirmQualificationPage
                 {
                     QualificationLabel = "Test qualification label",
                     BackButton = new NavigationLink
                                  {
                                      DisplayText = "Test back button",
                                      OpenInNewTab = false,
                                      Href = "/qualifications"
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
                     ErrorBannerLink = "Test error banner link"
                 });

        mockContentService.Setup(x => x.GetQualificationById("Some ID")).ReturnsAsync(default(Qualification?));

        var controller = new ConfirmQualificationController(mockLogger.Object, mockContentService.Object);

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
        var mockContentService = new Mock<IContentService>();

        mockContentService.Setup(x => x.GetConfirmQualificationPage()).ReturnsAsync(new ConfirmQualificationPage
                 {
                     QualificationLabel = "Test qualification label",
                     BackButton = new NavigationLink
                                  {
                                      DisplayText = "Test back button",
                                      OpenInNewTab = false,
                                      Href = "/qualifications"
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
                     ErrorBannerLink = "Test error banner link"
                 });

        mockContentService.Setup(x => x.GetQualificationById("Some ID")).ReturnsAsync(new Qualification("Some ID",
                  "Qualification Name", "NCFE", 2, "2014", "2019",
                  "ABC/547/900", "additional requirements", null, null));

        var controller = new ConfirmQualificationController(mockLogger.Object, mockContentService.Object);

        var result = await controller.Index("Some ID");

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as ConfirmQualificationPageModel;
        model.Should().NotBeNull();

        model!.BackButton.Should().BeEquivalentTo(new NavigationLink
                                                  {
                                                      DisplayText = "Test back button",
                                                      OpenInNewTab = false,
                                                      Href = "/qualifications"
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
        model.Heading.Should().Be("Test heading");
        model.ButtonText.Should().Be("Test button text");
        model.ErrorText.Should().Be("Test error text");
        model.LevelLabel.Should().Be("Test level label");
        model.QualificationId.Should().Be("Some ID");
        model.QualificationLabel.Should().Be("Test qualification label");
        model.QualificationLevel.Should().Be("2");
        model.QualificationName.Should().Be("Qualification Name");
        model.RadioHeading.Should().Be("Test radio heading");
        model.AwardingOrganisationLabel.Should().Be("Test awarding organisation label");
        model.ConfirmQualificationAnswer.Should().Be(string.Empty);
        model.DateAddedLabel.Should().Be("Test date added label");
        model.ErrorBannerHeading.Should().Be("Test error banner heading");
        model.ErrorBannerLink.Should().Be("Test error banner link");
        model.QualificationAwardingOrganisation.Should().Be("NCFE");
        model.QualificationDateAdded.Should().Be("2014");
    }

    [TestMethod]
    public async Task Post_InvalidModel_CantGetPageContent_LogsAndReturnsError()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockContentService = new Mock<IContentService>();

        mockContentService.Setup(x => x.GetConfirmQualificationPage()).ReturnsAsync(default(ConfirmQualificationPage?));
        mockContentService.Setup(x => x.GetQualificationById("Some ID")).ReturnsAsync(new Qualification("Some ID",
         "Qualification Name", "NCFE", 2, "2014", "2019",
         "ABC/547/900", "additional requirements", null, null));

        var controller = new ConfirmQualificationController(mockLogger.Object, mockContentService.Object);

        controller.ModelState.AddModelError("test", "error");

        var result = await controller.Confirm(new ConfirmQualificationPageModel() { QualificationId = "Some ID"});

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
        var mockContentService = new Mock<IContentService>();

        var controller = new ConfirmQualificationController(mockLogger.Object, mockContentService.Object);

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
        var mockContentService = new Mock<IContentService>();

        mockContentService.Setup(x => x.GetQualificationById("Some ID")).ReturnsAsync(default(Qualification?));

        var controller = new ConfirmQualificationController(mockLogger.Object, mockContentService.Object);

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
        var mockContentService = new Mock<IContentService>();

        mockContentService.Setup(x => x.GetConfirmQualificationPage()).ReturnsAsync(new ConfirmQualificationPage
                 {
                     QualificationLabel = "Test qualification label",
                     BackButton = new NavigationLink
                                  {
                                      DisplayText = "Test back button",
                                      OpenInNewTab = false,
                                      Href = "/qualifications"
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
                     ErrorBannerLink = "Test error banner link"
                 });

        mockContentService.Setup(x => x.GetQualificationById("Some ID")).ReturnsAsync(new Qualification("Some ID",
                  "Qualification Name", "NCFE", 2, "2014", "2019",
                  "ABC/547/900", "additional requirements", null, null));

        var controller = new ConfirmQualificationController(mockLogger.Object, mockContentService.Object);

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

        model!.BackButton.Should().BeEquivalentTo(new NavigationLink
                                                  {
                                                      DisplayText = "Test back button",
                                                      OpenInNewTab = false,
                                                      Href = "/qualifications"
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
        model.Heading.Should().Be("Test heading");
        model.ButtonText.Should().Be("Test button text");
        model.ErrorText.Should().Be("Test error text");
        model.LevelLabel.Should().Be("Test level label");
        model.QualificationId.Should().Be("Some ID");
        model.QualificationLabel.Should().Be("Test qualification label");
        model.QualificationLevel.Should().Be("2");
        model.QualificationName.Should().Be("Qualification Name");
        model.RadioHeading.Should().Be("Test radio heading");
        model.AwardingOrganisationLabel.Should().Be("Test awarding organisation label");
        model.ConfirmQualificationAnswer.Should().Be(string.Empty);
        model.DateAddedLabel.Should().Be("Test date added label");
        model.ErrorBannerHeading.Should().Be("Test error banner heading");
        model.ErrorBannerLink.Should().Be("Test error banner link");
        model.QualificationAwardingOrganisation.Should().Be("NCFE");
        model.QualificationDateAdded.Should().Be("2014");
    }

    [TestMethod]
    public async Task Post_ValidModel_PassedYesAndQualificationHasAdditionalRequirements_RedirectsToCheckAdditionalRequirements()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockContentService = new Mock<IContentService>();
        var additionalRequirements = new List<AdditionalRequirementQuestion> { new () };
        mockContentService.Setup(x => x.GetQualificationById("TEST-123")).ReturnsAsync(new Qualification("Some ID",
         "Qualification Name", "NCFE", 2, "2014", "2019",
         "ABC/547/900", "additional requirements", additionalRequirements, null));
        
        var controller = new ConfirmQualificationController(mockLogger.Object, mockContentService.Object);

        var result = await controller.Confirm(new ConfirmQualificationPageModel
                                              {
                                                  QualificationId = "TEST-123",
                                                  ConfirmQualificationAnswer = "yes"
                                              });

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Index");
        actionResult.ControllerName.Should().Be("CheckAdditionalRequirements");
        actionResult.RouteValues.Should().ContainSingle("qualificationId", "TEST-123");
    }
    
    [TestMethod]
    public async Task Post_ValidModel_PassedYes_RedirectsToQualificationDetailsAction()
    {
        var mockLogger = new Mock<ILogger<ConfirmQualificationController>>();
        var mockContentService = new Mock<IContentService>();
        mockContentService.Setup(x => x.GetQualificationById("TEST-123")).ReturnsAsync(new Qualification("Some ID",
         "Qualification Name", "NCFE", 2, "2014", "2019",
         "ABC/547/900", "additional requirements", null, null));
        
        var controller = new ConfirmQualificationController(mockLogger.Object, mockContentService.Object);

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
        var mockContentService = new Mock<IContentService>();
        mockContentService.Setup(x => x.GetQualificationById("TEST-123")).ReturnsAsync(new Qualification("Some ID",
         "Qualification Name", "NCFE", 2, "2014", "2019",
         "ABC/547/900", "additional requirements", null, null));

        var controller = new ConfirmQualificationController(mockLogger.Object, mockContentService.Object);

        var result = await controller.Confirm(new ConfirmQualificationPageModel
                                              {
                                                  QualificationId = "TEST-123",
                                                  ConfirmQualificationAnswer = "not yes"
                                              });

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Get");
        actionResult.ControllerName.Should().Be("QualificationDetails");
    }
}