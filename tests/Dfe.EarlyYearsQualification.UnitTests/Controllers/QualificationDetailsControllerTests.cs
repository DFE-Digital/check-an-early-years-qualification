using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class QualificationDetailsControllerTests
{
    [TestMethod]
    public async Task Index_PassInNullQualificationId_ReturnsBadRequest()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller =
            new QualificationDetailsController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockContentParser.Object,
                                               mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        var result = await controller.Index(string.Empty);

        result.Should().NotBeNull();

        var resultType = result as BadRequestResult;
        resultType.Should().NotBeNull();
        resultType!.StatusCode.Should().Be(400);
    }

    [TestMethod]
    public async Task Index_ContentServiceReturnsNullDetailsPage_RedirectsToHomeError()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller =
            new QualificationDetailsController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockContentParser.Object,
                                               mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        mockContentService.Setup(s => s.GetDetailsPage())
                          .ReturnsAsync((DetailsPage?)null);

        var result = await controller.Index("X");

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Index");
        actionResult.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the qualification details page");
    }

    [TestMethod]
    public async Task Index_ContentServiceReturnsNoQualification_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller =
            new QualificationDetailsController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockContentParser.Object,
                                               mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        const string qualificationId = "eyq-145";
        mockRepository.Setup(x => x.GetById(qualificationId))
                      .ReturnsAsync((Qualification?)default);

        mockContentService.Setup(x => x.GetDetailsPage())
                          .ReturnsAsync(new DetailsPage());

        var result = await controller.Index(qualificationId);

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("Could not find details for qualification with ID: eyq-145");
    }

    [TestMethod]
    public async Task Index_NoDateOfQualificationSelectedPriorInTheJourney_RedirectToHome()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        const string qualificationId = "eyq-145";

        var qualificationResult = new Qualification(qualificationId,
                                                    "Qualification Name",
                                                    AwardingOrganisations.Ncfe,
                                                    2)
                                  {
                                      FromWhichYear = "2014", ToWhichYear = "2019",
                                      QualificationNumber = "ABC/547/900",
                                      AdditionalRequirements = "additional requirements"
                                  };

        mockRepository.Setup(x => x.GetById(qualificationId))
                      .ReturnsAsync(qualificationResult);

        mockContentService.Setup(x => x.GetDetailsPage())
                          .ReturnsAsync(new DetailsPage());

        mockUserJourneyCookieService.Setup(x => x.GetWhenWasQualificationStarted()).Returns((null, null));

        var controller =
            new QualificationDetailsController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockContentParser.Object,
                                               mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        var result = await controller.Index(qualificationId);

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Home");
    }

    [TestMethod]
    public async Task Index_NoDateMonthOfQualificationSelectedPriorInTheJourney_RedirectToHome()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        const string qualificationId = "eyq-145";

        var qualificationResult = new Qualification(qualificationId,
                                                    "Qualification Name",
                                                    AwardingOrganisations.Ncfe,
                                                    2)
                                  {
                                      FromWhichYear = "2014", ToWhichYear = "2019",
                                      QualificationNumber = "ABC/547/900",
                                      AdditionalRequirements = "additional requirements"
                                  };

        mockRepository.Setup(x => x.GetById(qualificationId))
                      .ReturnsAsync(qualificationResult);

        mockContentService.Setup(x => x.GetDetailsPage())
                          .ReturnsAsync(new DetailsPage());

        mockUserJourneyCookieService.Setup(x => x.GetWhenWasQualificationStarted()).Returns((null, 2012));

        var controller =
            new QualificationDetailsController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockContentParser.Object,
                                               mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        var result = await controller.Index(qualificationId);

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Home");
    }

    [TestMethod]
    public async Task Index_QualificationHasAdditionalQuestionsButNoneAnswered_RedirectTotTheAdditionalQuestionsPage()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        const string qualificationId = "eyq-145";

        var listOfAdditionalReqs = new List<AdditionalRequirementQuestion>
                                   {
                                       new()
                                       {
                                           Question = "Some Question"
                                       }
                                   };

        // Mismatch between the two lists here to simulate questions not being answered
        var listOfAdditionalReqsAnswered = new Dictionary<string, string>();

        var qualificationResult = new Qualification(qualificationId,
                                                    "Qualification Name",
                                                    AwardingOrganisations.Ncfe,
                                                    2)
                                  {
                                      FromWhichYear = "2014", ToWhichYear = "2019",
                                      QualificationNumber = "ABC/547/900",
                                      AdditionalRequirements = "additional requirements",
                                      AdditionalRequirementQuestions = listOfAdditionalReqs
                                  };

        mockRepository.Setup(x => x.GetById(qualificationId))
                      .ReturnsAsync(qualificationResult);

        mockContentService.Setup(x => x.GetDetailsPage())
                          .ReturnsAsync(new DetailsPage());

        mockUserJourneyCookieService.Setup(x => x.GetWhenWasQualificationStarted()).Returns((9, 2022));
        mockUserJourneyCookieService.Setup(x => x.GetAdditionalQuestionsAnswers())
                                    .Returns(listOfAdditionalReqsAnswered);

        var controller =
            new QualificationDetailsController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockContentParser.Object,
                                               mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        var result = await controller.Index(qualificationId);

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("CheckAdditionalRequirements");
        resultType.RouteValues.Should().Contain("qualificationId", qualificationId);
        resultType.RouteValues.Should().Contain("questionId", 1);
    }

    [TestMethod]
    [DataRow("no", "no")]
    [DataRow("yes", "yes")]
    [DataRow("no", "yes")]
    public async Task
        Index_Index_QualificationHasAdditionalQuestionsButAnswersAreNotCorrect_MarkAsNotRelevantAndReturn(
            string answer1, string answer2)
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        const string qualificationId = "eyq-145";

        var listOfAdditionalReqs = new List<AdditionalRequirementQuestion>
                                   {
                                       new()
                                       {
                                           Question = "Some Question",
                                           AnswerToBeFullAndRelevant = true
                                       },
                                       new()
                                       {
                                           Question = "Another Question",
                                           AnswerToBeFullAndRelevant = false
                                       }
                                   };

        // Question has been answered, but the answer is not what we want for the qualification to be full and relevant
        var listOfAdditionalReqsAnswered = new Dictionary<string, string>
                                           {
                                               { "Some Question", answer1 },
                                               { "Another Question", answer2 }
                                           };

        var qualificationResult = new Qualification(qualificationId,
                                                    "Qualification Name",
                                                    AwardingOrganisations.Ncfe,
                                                    2)
                                  {
                                      FromWhichYear = "2014", ToWhichYear = "2019",
                                      QualificationNumber = "ABC/547/900",
                                      AdditionalRequirements = "additional requirements",
                                      AdditionalRequirementQuestions = listOfAdditionalReqs
                                  };

        mockRepository.Setup(x => x.GetById(qualificationId))
                      .ReturnsAsync(qualificationResult);

        mockContentService.Setup(x => x.GetDetailsPage())
                          .ReturnsAsync(new DetailsPage());

        mockUserJourneyCookieService.Setup(x => x.GetWhenWasQualificationStarted())
                                    .Returns((9, 2022));

        mockUserJourneyCookieService.Setup(x => x.GetAdditionalQuestionsAnswers())
                                    .Returns(listOfAdditionalReqsAnswered);

        var controller =
            new QualificationDetailsController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockContentParser.Object,
                                               mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        var result = await controller.Index(qualificationId);

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as QualificationDetailsModel;
        model.Should().NotBeNull();

        model!.QualificationId.Should().Be(qualificationResult.QualificationId);
        model.QualificationName.Should().Be(qualificationResult.QualificationName);
        model.AwardingOrganisationTitle.Should().Be(qualificationResult.AwardingOrganisationTitle);
        model.QualificationLevel.Should().Be(qualificationResult.QualificationLevel);
        model.FromWhichYear.Should().Be(qualificationResult.FromWhichYear);
        model.QualificationNumber.Should().Be(qualificationResult.QualificationNumber);

        model.RatioRequirements.ApprovedForLevel2.Should().BeFalse();
        model.RatioRequirements.ApprovedForLevel3.Should().BeFalse();
        model.RatioRequirements.ApprovedForLevel6.Should().BeFalse();

        //Note unqualified ratios will always be approved no matter the answers
        model.RatioRequirements.ApprovedForUnqualified.Should().BeTrue();
    }

    [TestMethod]
    public async Task Index_BuildUpRatioRequirements_CantFindLevel2_LogsAndThrows()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        const string qualificationId = "eyq-145";
        const int level = 2;
        const int startDateYear = 2022;

        var ratioRequirements = new List<RatioRequirement>();

        var qualificationResult = new Qualification(qualificationId,
                                                    "Qualification Name",
                                                    AwardingOrganisations.Ncfe,
                                                    level)
                                  {
                                      FromWhichYear = "2014", ToWhichYear = "2019",
                                      QualificationNumber = "ABC/547/900",
                                      AdditionalRequirements = "additional requirements",
                                      RatioRequirements = ratioRequirements
                                  };

        mockRepository.Setup(x => x.GetById(qualificationId))
                      .ReturnsAsync(qualificationResult);

        mockContentService.Setup(x => x.GetDetailsPage())
                          .ReturnsAsync(new DetailsPage());

        mockUserJourneyCookieService.Setup(x => x.GetWhenWasQualificationStarted()).Returns((9, startDateYear));
        mockUserJourneyCookieService.Setup(x => x.WasStartedBeforeSeptember2014())
                                    .Returns(false);

        var controller =
            new QualificationDetailsController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockContentParser.Object,
                                               mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        await Assert.ThrowsExceptionAsync<NullReferenceException>(() => controller.Index(qualificationId));
        mockLogger.VerifyError($"Could not find property: FullAndRelevantForLevel{level}After2014 within Level 2 Ratio Requirements for qualification: {qualificationId}");
    }

    [TestMethod]
    public async Task Index_BuildUpRatioRequirements_CantFindLevel3_LogsAndThrows()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        const string qualificationId = "eyq-145";
        const int level = 2;
        const int startDateYear = 2022;

        var ratioRequirements = new List<RatioRequirement>
                                {
                                    new()
                                    {
                                        RatioRequirementName = "Level 2 Ratio Requirements",
                                        FullAndRelevantForLevel2After2014 = true
                                    }
                                };

        var qualificationResult = new Qualification(qualificationId,
                                                    "Qualification Name",
                                                    AwardingOrganisations.Ncfe,
                                                    level)
                                  {
                                      FromWhichYear = "2014", ToWhichYear = "2019",
                                      QualificationNumber = "ABC/547/900",
                                      AdditionalRequirements = "additional requirements",
                                      RatioRequirements = ratioRequirements
                                  };

        mockRepository.Setup(x => x.GetById(qualificationId))
                      .ReturnsAsync(qualificationResult);

        mockContentService.Setup(x => x.GetDetailsPage())
                          .ReturnsAsync(new DetailsPage());

        mockUserJourneyCookieService.Setup(x => x.GetWhenWasQualificationStarted())
                                    .Returns((9, startDateYear));

        mockUserJourneyCookieService.Setup(x => x.WasStartedBeforeSeptember2014())
                                    .Returns(false);

        var controller =
            new QualificationDetailsController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockContentParser.Object,
                                               mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        await Assert.ThrowsExceptionAsync<NullReferenceException>(() => controller.Index(qualificationId));
        mockLogger.VerifyError($"Could not find property: FullAndRelevantForLevel{level}After2014 within Level 3 Ratio Requirements for qualification: {qualificationId}");
    }

    [TestMethod]
    public async Task Index_BuildUpRatioRequirements_CantFindLevel6_LogsAndThrows()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        const string qualificationId = "eyq-145";
        const int level = 2;
        const int startDateYear = 2022;

        var ratioRequirements = new List<RatioRequirement>
                                {
                                    new()
                                    {
                                        RatioRequirementName = "Level 2 Ratio Requirements",
                                        FullAndRelevantForLevel2After2014 = true
                                    },
                                    new()
                                    {
                                        RatioRequirementName = "Level 3 Ratio Requirements",
                                        FullAndRelevantForLevel2After2014 = true
                                    }
                                };

        var qualificationResult = new Qualification(qualificationId,
                                                    "Qualification Name",
                                                    AwardingOrganisations.Ncfe,
                                                    level)
                                  {
                                      FromWhichYear = "2014", ToWhichYear = "2019",
                                      QualificationNumber = "ABC/547/900",
                                      AdditionalRequirements = "additional requirements",
                                      RatioRequirements = ratioRequirements
                                  };

        mockRepository.Setup(x => x.GetById(qualificationId)).ReturnsAsync(qualificationResult);
        mockContentService.Setup(x => x.GetDetailsPage()).ReturnsAsync(new DetailsPage());

        mockUserJourneyCookieService.Setup(x => x.GetWhenWasQualificationStarted()).Returns((9, startDateYear));
        mockUserJourneyCookieService.Setup(x => x.WasStartedBeforeSeptember2014())
                                    .Returns(false);

        var controller =
            new QualificationDetailsController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockContentParser.Object,
                                               mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        await Assert.ThrowsExceptionAsync<NullReferenceException>(() => controller.Index(qualificationId));
        mockLogger.VerifyError($"Could not find property: FullAndRelevantForLevel{level}After2014 within Level 6 Ratio Requirements for qualification: {qualificationId}");
    }

    [TestMethod]
    public async Task Index_AllRatiosFound_ReturnsView()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        const string qualificationId = "eyq-145";
        const int level = 2;
        const int startDateYear = 2022;

        var ratioRequirements = new List<RatioRequirement>
                                {
                                    new()
                                    {
                                        RatioRequirementName = "Level 2 Ratio Requirements",
                                        FullAndRelevantForLevel2After2014 = true,
                                        RequirementForLevel2After2014 = ContentfulContentHelper.Paragraph("Test")
                                    },
                                    new()
                                    {
                                        RatioRequirementName = "Level 3 Ratio Requirements",
                                        FullAndRelevantForLevel2After2014 = true,
                                        RequirementForLevel2After2014 = ContentfulContentHelper.Paragraph("Test")
                                    },
                                    new()
                                    {
                                        RatioRequirementName = "Level 6 Ratio Requirements",
                                        FullAndRelevantForLevel2After2014 = true,
                                        RequirementForLevel2After2014 = ContentfulContentHelper.Paragraph("Test")
                                    },
                                    new()
                                    {
                                        RatioRequirementName = "Unqualified Ratio Requirements",
                                        FullAndRelevantForLevel2After2014 = true,
                                        RequirementForLevel2After2014 = ContentfulContentHelper.Paragraph("Test")
                                    }
                                };

        var qualificationResult = new Qualification(qualificationId,
                                                    "Qualification Name",
                                                    AwardingOrganisations.Ncfe,
                                                    level)
                                  {
                                      FromWhichYear = "2014", ToWhichYear = "2019",
                                      QualificationNumber = "ABC/547/900",
                                      AdditionalRequirements = "additional requirements",
                                      RatioRequirements = ratioRequirements
                                  };

        var detailsPage = new DetailsPage
                          {
                              BackToAdditionalQuestionsLink = new NavigationLink
                                                              {
                                                                  Href = "/api/qualifications"
                                                              }
                          };

        mockRepository.Setup(x => x.GetById(qualificationId))
                      .ReturnsAsync(qualificationResult);

        mockContentService.Setup(x => x.GetDetailsPage())
                          .ReturnsAsync(detailsPage);

        mockUserJourneyCookieService.Setup(x => x.GetWhenWasQualificationStarted())
                                    .Returns((9, startDateYear));

        mockUserJourneyCookieService.Setup(x => x.WasStartedBeforeSeptember2014())
                                    .Returns(false);

        var controller =
            new QualificationDetailsController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockContentParser.Object,
                                               mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        var result = await controller.Index(qualificationId);

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as QualificationDetailsModel;
        model.Should().NotBeNull();

        model!.QualificationId.Should().Be(qualificationResult.QualificationId);
        model.QualificationName.Should().Be(qualificationResult.QualificationName);
        model.AwardingOrganisationTitle.Should().Be(qualificationResult.AwardingOrganisationTitle);
        model.QualificationLevel.Should().Be(qualificationResult.QualificationLevel);
        model.FromWhichYear.Should().Be(qualificationResult.FromWhichYear);
        model.QualificationNumber.Should().Be(qualificationResult.QualificationNumber);

        model.RatioRequirements.ApprovedForLevel2.Should().BeTrue();
        model.RatioRequirements.ApprovedForLevel3.Should().BeTrue();
        model.RatioRequirements.ApprovedForLevel6.Should().BeTrue();
        model.RatioRequirements.ApprovedForUnqualified.Should().BeTrue();
    }

    [TestMethod]
    public async Task Index_BackToAdditionalQuestionsLinkIncludesQualificationId()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        mockUserJourneyCookieService.Setup(x => x.UserHasAnsweredAdditionalQuestions())
                                    .Returns(true);

        const string qualificationId = "eyq-145";
        const int level = 2;
        const int startDateYear = 2022;

        var ratioRequirements = new List<RatioRequirement>
                                {
                                    new()
                                    {
                                        RatioRequirementName = "Level 2 Ratio Requirements",
                                        FullAndRelevantForLevel2After2014 = true,
                                        RequirementForLevel2After2014 = ContentfulContentHelper.Paragraph("Test")
                                    },
                                    new()
                                    {
                                        RatioRequirementName = "Level 3 Ratio Requirements",
                                        FullAndRelevantForLevel2After2014 = true,
                                        RequirementForLevel2After2014 = ContentfulContentHelper.Paragraph("Test")
                                    },
                                    new()
                                    {
                                        RatioRequirementName = "Level 6 Ratio Requirements",
                                        FullAndRelevantForLevel2After2014 = true,
                                        RequirementForLevel2After2014 = ContentfulContentHelper.Paragraph("Test")
                                    },
                                    new()
                                    {
                                        RatioRequirementName = "Unqualified Ratio Requirements",
                                        FullAndRelevantForLevel2After2014 = true,
                                        RequirementForLevel2After2014 = ContentfulContentHelper.Paragraph("Test")
                                    }
                                };

        var qualificationResult = new Qualification(qualificationId,
                                                    "Qualification Name",
                                                    AwardingOrganisations.Ncfe,
                                                    level)
                                  {
                                      FromWhichYear = "2014", ToWhichYear = "2019",
                                      QualificationNumber = "ABC/547/900",
                                      AdditionalRequirements = "additional requirements",
                                      RatioRequirements = ratioRequirements
                                  };

        var detailsPage = new DetailsPage
                          {
                              BackToAdditionalQuestionsLink = new NavigationLink
                                                              {
                                                                  Href = "/api/qualifications"
                                                              }
                          };

        mockRepository.Setup(x => x.GetById(qualificationId))
                      .ReturnsAsync(qualificationResult);

        mockContentService.Setup(x => x.GetDetailsPage())
                          .ReturnsAsync(detailsPage);

        mockUserJourneyCookieService.Setup(x => x.GetWhenWasQualificationStarted()).Returns((9, startDateYear));
        mockUserJourneyCookieService.Setup(x => x.WasStartedBeforeSeptember2014())
                                    .Returns(false);

        var controller =
            new QualificationDetailsController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockContentParser.Object,
                                               mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        var result = await controller.Index(qualificationId);

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as QualificationDetailsModel;

        model!.BackButton!.Href.Should().Be("/api/qualifications/eyq-145/1");
    }

    [TestMethod]
    public async Task Get_ReturnsView()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        mockRepository
            .Setup(x =>
                       x.Get(It.IsAny<int?>(),
                             It.IsAny<int?>(),
                             It.IsAny<int?>(),
                             It.IsAny<string?>(),
                             It.IsAny<string?>()))
            .ReturnsAsync([]);

        var controller =
            new QualificationDetailsController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockContentParser.Object,
                                               mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        mockContentService.Setup(x => x.GetQualificationListPage()).ReturnsAsync(new QualificationListPage
                 {
                     BackButton = new NavigationLink
                                  {
                                      DisplayText = "TEST",
                                      Href = "/",
                                      OpenInNewTab = false
                                  },
                     Header = "TEST"
                 });

        var result = await controller.Get();

        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [TestMethod]
    public async Task Get_NoContent_LogsAndRedirectsToError()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller =
            new QualificationDetailsController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockContentParser.Object,
                                               mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        mockContentService.Setup(x => x.GetQualificationListPage()).ReturnsAsync(default(QualificationListPage));

        var result = await controller.Get();

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Index");
        actionResult.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the qualification list page");
    }

    [TestMethod]
    public void Refine_SaveQualificationName_RedirectsToGet()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller =
            new QualificationDetailsController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockContentParser.Object,
                                               mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        var result = controller.Refine("Test");

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Get");
        mockUserJourneyCookieService.Verify(x => x.SetQualificationNameSearchCriteria("Test"), Times.Once);
    }

    [TestMethod]
    public void Refine_NullParam_RedirectsToGet()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller =
            new QualificationDetailsController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockContentParser.Object,
                                               mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        var result = controller.Refine(null);

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Get");
        mockUserJourneyCookieService.Verify(x => x.SetQualificationNameSearchCriteria(string.Empty), Times.Once);
    }

    [TestMethod]
    public void Refine_InvalidModel_LogsWarning()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller =
            new QualificationDetailsController(mockLogger.Object,
                                               mockRepository.Object,
                                               mockContentService.Object,
                                               mockContentParser.Object,
                                               mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        controller.ModelState.AddModelError("Key", "Error message");

        controller.Refine(null);

        mockLogger.VerifyWarning($"Invalid model state in {nameof(QualificationDetailsController)} POST");
    }
}