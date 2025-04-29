using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.TestSupport;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class CheckYourAnswersControllerTests
{
    [TestMethod]
    public async Task Index_NullWhereWasQualificationAwardedQuestion_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<CheckYourAnswersController>>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller =
            new CheckYourAnswersController(mockLogger.Object, mockContentService.Object,
                                           mockUserJourneyCookieService.Object);

        mockContentService.Setup(x => x.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationStartedAndAwarded))
                          .ReturnsAsync(new DatesQuestionPage { Question = "Date started question" });
        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhatLevelIsTheQualification))
                          .ReturnsAsync(new RadioQuestionPage { Question = "Level question" });
        mockContentService.Setup(x => x.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation))
                          .ReturnsAsync(new DropdownQuestionPage { Question = "Dropdown question" });
        mockContentService.Setup(x => x.GetCheckYourAnswersPage()).ReturnsAsync(new CheckYourAnswersPage());

        var result = await controller.Index();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new RedirectToActionResult("Index", "Error", null));

        mockLogger.VerifyError("No content for the check your answers page");
    }

    [TestMethod]
    public async Task Index_NullWhenWasTheQualificationStartedAndAwardedQuestion_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<CheckYourAnswersController>>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller =
            new CheckYourAnswersController(mockLogger.Object, mockContentService.Object,
                                           mockUserJourneyCookieService.Object);

        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhereWasTheQualificationAwarded))
                          .ReturnsAsync(new RadioQuestionPage { Question = "Awarded question" });
        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhatLevelIsTheQualification))
                          .ReturnsAsync(new RadioQuestionPage { Question = "Level question" });
        mockContentService.Setup(x => x.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation))
                          .ReturnsAsync(new DropdownQuestionPage { Question = "Dropdown question" });
        mockContentService.Setup(x => x.GetCheckYourAnswersPage()).ReturnsAsync(new CheckYourAnswersPage());

        var result = await controller.Index();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new RedirectToActionResult("Index", "Error", null));

        mockLogger.VerifyError("No content for the check your answers page");
    }

    [TestMethod]
    public async Task Index_NullWhatLevelIsTheQualificationQuestion_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<CheckYourAnswersController>>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller =
            new CheckYourAnswersController(mockLogger.Object, mockContentService.Object,
                                           mockUserJourneyCookieService.Object);

        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhereWasTheQualificationAwarded))
                          .ReturnsAsync(new RadioQuestionPage { Question = "Awarded question" });
        mockContentService.Setup(x => x.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationStartedAndAwarded))
                          .ReturnsAsync(new DatesQuestionPage { Question = "Date started question" });
        mockContentService.Setup(x => x.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation))
                          .ReturnsAsync(new DropdownQuestionPage { Question = "Dropdown question" });
        mockContentService.Setup(x => x.GetCheckYourAnswersPage()).ReturnsAsync(new CheckYourAnswersPage());

        var result = await controller.Index();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new RedirectToActionResult("Index", "Error", null));

        mockLogger.VerifyError("No content for the check your answers page");
    }

    [TestMethod]
    public async Task Index_NullWhatIsTheAwardingOrganisationQuestion_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<CheckYourAnswersController>>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller =
            new CheckYourAnswersController(mockLogger.Object, mockContentService.Object,
                                           mockUserJourneyCookieService.Object);

        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhereWasTheQualificationAwarded))
                          .ReturnsAsync(new RadioQuestionPage { Question = "Awarded question" });
        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhatLevelIsTheQualification))
                          .ReturnsAsync(new RadioQuestionPage { Question = "Level question" });
        mockContentService.Setup(x => x.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationStartedAndAwarded))
                          .ReturnsAsync(new DatesQuestionPage { Question = "Date started question" });
        mockContentService.Setup(x => x.GetCheckYourAnswersPage()).ReturnsAsync(new CheckYourAnswersPage());

        var result = await controller.Index();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new RedirectToActionResult("Index", "Error", null));

        mockLogger.VerifyError("No content for the check your answers page");
    }

    [TestMethod]
    public async Task Index_NullPageContent_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<CheckYourAnswersController>>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller =
            new CheckYourAnswersController(mockLogger.Object, mockContentService.Object,
                                           mockUserJourneyCookieService.Object);

        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhereWasTheQualificationAwarded))
                          .ReturnsAsync(new RadioQuestionPage { Question = "Awarded question" });
        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhatLevelIsTheQualification))
                          .ReturnsAsync(new RadioQuestionPage { Question = "Level question" });
        mockContentService.Setup(x => x.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationStartedAndAwarded))
                          .ReturnsAsync(new DatesQuestionPage { Question = "Date started question" });
        mockContentService.Setup(x => x.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation))
                          .ReturnsAsync(new DropdownQuestionPage { Question = "Dropdown question" });

        var result = await controller.Index();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new RedirectToActionResult("Index", "Error", null));

        mockLogger.VerifyError("No content for the check your answers page");
    }

    [TestMethod]
    public async Task Index_MapsDetails_ReturnsView()
    {
        var mockLogger = new Mock<ILogger<CheckYourAnswersController>>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller =
            new CheckYourAnswersController(mockLogger.Object, mockContentService.Object,
                                           mockUserJourneyCookieService.Object);

        const string pageHeading = "check your answers";

        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhereWasTheQualificationAwarded))
                          .ReturnsAsync(new RadioQuestionPage { Question = "Awarded question" });
        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhatLevelIsTheQualification))
                          .ReturnsAsync(new RadioQuestionPage { Question = "Level question" });
        mockContentService.Setup(x => x.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationStartedAndAwarded))
                          .ReturnsAsync(new DatesQuestionPage { Question = "Date started question" });
        mockContentService.Setup(x => x.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation))
                          .ReturnsAsync(new DropdownQuestionPage { Question = "Dropdown question" });
        mockContentService.Setup(x => x.GetCheckYourAnswersPage())
                          .ReturnsAsync(new CheckYourAnswersPage { PageHeading = pageHeading });

        var result = await controller.Index();

        result.Should().NotBeNull();

        var viewResult = result as ViewResult;
        object? data = viewResult!.Model;

        data.Should().NotBeNull();
        data.Should().BeOfType<CheckYourAnswersPageModel>();

        var model = data as CheckYourAnswersPageModel;
        model!.PageHeading.Should().Match(pageHeading);

        mockUserJourneyCookieService.Verify(x => x.GetWhereWasQualificationAwarded(), Times.Once);
        mockUserJourneyCookieService.Verify(x => x.GetWhenWasQualificationStarted(), Times.Once);
        mockUserJourneyCookieService.Verify(x => x.GetWhenWasQualificationAwarded(), Times.Once);
        mockUserJourneyCookieService.Verify(x => x.GetLevelOfQualification(), Times.Once);
        mockUserJourneyCookieService.Verify(x => x.GetAwardingOrganisation(), Times.Once);
    }
}