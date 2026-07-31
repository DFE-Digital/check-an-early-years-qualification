using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.QualificationDetails;
using Microsoft.AspNetCore.Http;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class QualificationDetailsControllerTests
{
    private readonly Mock<ILogger<QualificationDetailsController>> _mockLogger = new();
    private readonly Mock<IQualificationDetailsService> _mockQualificationDetailsService = new();

    private static Qualification DummyQualification
    {
        get
        {
            return new Qualification("qualificationId",
                                     It.IsAny<string>(),
                                     It.IsAny<string>(),
                                     It.IsAny<int>()
                                    );
        }
    }

    private static QualificationDetailsPage DummyDetailsPage
    {
        get { return new QualificationDetailsPage(); }
    }

    private static QualificationDetailsModel DummyDetails
    {
        get { return new QualificationDetailsModel { Content = new DetailsPageModel() }; }
    }

    private QualificationDetailsController GetSut()
    {
        return new QualificationDetailsController(_mockLogger.Object,
                                                  _mockQualificationDetailsService.Object)
               {
                   ControllerContext = new ControllerContext
                                       {
                                           HttpContext = new DefaultHttpContext()
                                       }
               };
    }

    [TestMethod]
    public async Task Index_NullId_Returns400BadRequest()
    {
        const string qualificationId = null!;
        var sut = GetSut();

        var result = await sut.Index(qualificationId!);

        result.Should().BeOfType<BadRequestResult>();
        var resultType = result as BadRequestResult;
        resultType.Should().NotBeNull();
        resultType.StatusCode.Should().Be(400);
    }

    [TestMethod]
    public async Task Index_EmptyId_Returns400BadRequest()
    {
        const string qualificationId = "";
        var sut = GetSut();

        var result = await sut.Index(qualificationId);

        result.Should().BeOfType<BadRequestResult>();
        var resultType = result as BadRequestResult;
        resultType.Should().NotBeNull();
        resultType.StatusCode.Should().Be(400);
    }

    [TestMethod]
    public async Task Index_Calls_QualificationDetailsService_HasStartDate()
    {
        const string qualificationId = "qualificationId";

        var sut = GetSut();

        _ = await sut.Index(qualificationId);

        _mockQualificationDetailsService.Verify(o => o.HasStartDate(), Times.Once);
    }

    [TestMethod]
    public async Task Index_MissingStartDate_RedirectsToHome()
    {
        const string qualificationId = "qualificationId";
        _mockQualificationDetailsService.Setup(o => o.HasStartDate()).Returns(false);

        var sut = GetSut();

        var result = await sut.Index(qualificationId);

        result.VerifyRedirect("Index", "Home");
    }

    [TestMethod]
    public async Task Index_Calls_QualificationDetailsService_GetDetailsPage()
    {
        const string qualificationId = "qualificationId";

        var qualifications = new List<Qualification>
                             {
                                 new Qualification(qualificationId, It.IsAny<string>(), It.IsAny<string>(),
                                                   It.IsAny<int>())
                             };
        _mockQualificationDetailsService.Setup(x => x.GetQualificationById(It.IsAny<string>()))
                                        .ReturnsAsync(new Qualification(qualificationId, "Name", "Awarding Org", 3));
        _mockQualificationDetailsService.Setup(o => o.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(o => o.GetFilteredQualifications()).ReturnsAsync(qualifications);
        _mockQualificationDetailsService.Setup(o => o.GetLevelOfQualification()).Returns(3);
        _mockQualificationDetailsService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((6, 2001));
        _mockQualificationDetailsService.Setup(o => o.GetWhenWasQualificationAwarded()).Returns((5, 2015));
        _mockQualificationDetailsService.Setup(o => o.GetUserIsCheckingOwnQualification()).Returns(false);

        var sut = GetSut();

        _ = await sut.Index(qualificationId);

        _mockQualificationDetailsService.Verify(o => o.GetQualificationDetailsPage(false, true, 3, 6, 2001,
                                                 5, 2015,
                                                 It.IsAny<Qualification>()), Times.Once);
    }

    [TestMethod]
    public async Task Index_Calls_QualificationDetailsService_LevelFromServiceIs0_CallsWithQualificationLevel()
    {
        const string qualificationId = "qualificationId";

        var qualifications = new List<Qualification>
                             {
                                 new Qualification(qualificationId, It.IsAny<string>(), It.IsAny<string>(),
                                                   5)
                             };
        _mockQualificationDetailsService.Setup(x => x.GetQualificationById(It.IsAny<string>()))
                                        .ReturnsAsync(new Qualification(qualificationId, "Name", "Awarding Org", 5));
        _mockQualificationDetailsService.Setup(o => o.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(o => o.GetFilteredQualifications()).ReturnsAsync(qualifications);
        _mockQualificationDetailsService.Setup(o => o.GetLevelOfQualification()).Returns(0);
        _mockQualificationDetailsService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((6, 2001));
        _mockQualificationDetailsService.Setup(o => o.GetWhenWasQualificationAwarded()).Returns((5, 2015));
        _mockQualificationDetailsService.Setup(o => o.GetUserIsCheckingOwnQualification()).Returns(false);

        var sut = GetSut();

        _ = await sut.Index(qualificationId);

        _mockQualificationDetailsService.Verify(o => o.GetQualificationDetailsPage(false, true, 5, 
                                                 6, 2001, 5, 2015,
                                                 It.IsAny<Qualification>()), Times.Once);
    }

    [TestMethod]
    public async Task Index_DetailsPage_IsNull_RedirectsToError()
    {
        const string qualificationId = "qualificationId";

        var qualifications = new List<Qualification>
                             {
                                 new Qualification(qualificationId, It.IsAny<string>(), It.IsAny<string>(),
                                                   It.IsAny<int>())
                             };

        _mockQualificationDetailsService.Setup(o => o.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(x => x.GetQualificationById(It.IsAny<string>()))
                                        .ReturnsAsync(new Qualification(qualificationId, "Name", "Awarding Org", 3));
        _mockQualificationDetailsService
            .Setup(o => o.GetQualificationDetailsPage(false, false, 3, 6, 2001, 5, 2019, It.IsAny<Qualification>()))
            .ReturnsAsync((QualificationDetailsPage)null!);
        _mockQualificationDetailsService.Setup(o => o.GetFilteredQualifications()).ReturnsAsync(qualifications);
        _mockQualificationDetailsService.Setup(o => o.GetLevelOfQualification()).Returns(3);
        _mockQualificationDetailsService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((6, 2001));
        _mockQualificationDetailsService.Setup(o => o.GetWhenWasQualificationAwarded()).Returns((5, 2019));
        _mockQualificationDetailsService.Setup(o => o.GetUserIsCheckingOwnQualification()).Returns(false);

        var sut = GetSut();

        var result = await sut.Index(qualificationId);

        _mockLogger.VerifyError("No content for the qualification details page");
        result.VerifyRedirect("Index", "Error");
    }

    [TestMethod]
    public async Task Index_Qualification_IsNull_RedirectsToError()
    {
        const string qualificationId = "qualificationId";
        _mockQualificationDetailsService.Setup(o => o.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(o => o.GetFilteredQualifications())
                                        .ReturnsAsync(new List<Qualification>());

        var sut = GetSut();

        var result = await sut.Index(qualificationId);

        _mockLogger.VerifyError("Could not find details for qualification with ID: qualificationId");
        result.VerifyRedirect("Index", "Error");
    }

    [TestMethod]
    public async Task Index_ValidateAdditionalQuestions_Valid_ReturnsView()
    {
        const string qualificationId = "qualificationId";
        _mockQualificationDetailsService.Setup(o => o.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(x => x.GetQualificationById(It.IsAny<string>()))
                                        .ReturnsAsync(new Qualification(qualificationId, "Name", "Awarding Org", 3));
        _mockQualificationDetailsService
            .Setup(o => o.GetQualificationDetailsPage(false, true, 3, 6, 2001, 5, 2019, It.IsAny<Qualification>()))
            .ReturnsAsync(DummyDetailsPage);
        _mockQualificationDetailsService.Setup(o => o.GetFilteredQualifications())
                                        .ReturnsAsync(new List<Qualification> { DummyQualification });
        _mockQualificationDetailsService.Setup(o => o.MapDetails(It.IsAny<Qualification>(),
                                                                 It.IsAny<QualificationDetailsPage>(), true))
                                        .ReturnsAsync(DummyDetails);
        _mockQualificationDetailsService.Setup(o => o.GetLevelOfQualification()).Returns(3);
        _mockQualificationDetailsService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((6, 2001));
        _mockQualificationDetailsService.Setup(o => o.GetWhenWasQualificationAwarded()).Returns((5, 2019));
        _mockQualificationDetailsService.Setup(o => o.GetUserIsCheckingOwnQualification()).Returns(false);

        var sut = GetSut();
        var result = await sut.Index(qualificationId);

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as QualificationDetailsModel;
        model.Should().NotBeNull();
    }

    [TestMethod]
    public async Task Index_ValidateAdditionalQuestions_InValid_Returns_View()
    {
        const string qualificationId = "qualificationId";
        var details = new QualificationDetailsModel
                      {
                          AdditionalRequirementAnswers = [new AdditionalRequirementAnswerModel()],
                          Content = new DetailsPageModel()
                      };
        _mockQualificationDetailsService.Setup(o => o.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(x => x.GetQualificationById(It.IsAny<string>()))
                                        .ReturnsAsync(new Qualification(qualificationId, "Name", "Awarding Org", 3));
        _mockQualificationDetailsService
            .Setup(o => o.GetQualificationDetailsPage(false, true, 3, 6, 2001, 5, 2019, It.IsAny<Qualification>()))
            .ReturnsAsync(DummyDetailsPage);
        _mockQualificationDetailsService.Setup(o => o.GetFilteredQualifications())
                                        .ReturnsAsync(new List<Qualification> { DummyQualification });
        _mockQualificationDetailsService.Setup(o => o.MapDetails(It.IsAny<Qualification>(),
                                                                 It.IsAny<QualificationDetailsPage>(), true))
                                        .ReturnsAsync(details);
        _mockQualificationDetailsService
            .Setup(o => o.AnswersIndicateNotFullAndRelevant(It.IsAny<List<AdditionalRequirementAnswerModel>>()))
            .Returns(true);

        _mockQualificationDetailsService.Setup(o => o.GetLevelOfQualification()).Returns(3);
        _mockQualificationDetailsService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((6, 2001));
        _mockQualificationDetailsService.Setup(o => o.GetWhenWasQualificationAwarded()).Returns((5, 2019));
        _mockQualificationDetailsService.Setup(o => o.GetUserIsCheckingOwnQualification()).Returns(false);

        var sut = GetSut();
        var result = await sut.Index(qualificationId);

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as QualificationDetailsModel;
        model.Should().NotBeNull();
    }

    [TestMethod]
    public async Task Index_QualificationHasAdditionalQuestionsButNoneAnswered_RedirectTotTheAdditionalQuestionsPage()
    {
        const string qualificationId = "qualificationId";

        var qualificationDetailsModel = new QualificationDetailsModel
                                        {
                                            QualificationId = qualificationId, AdditionalRequirementAnswers = [],
                                            Content = new DetailsPageModel()
                                        };

        _mockQualificationDetailsService.Setup(x => x.GetQualificationById(It.IsAny<string>()))
                                        .ReturnsAsync(new Qualification(qualificationId, "Name", "Awarding Org", 3));
        _mockQualificationDetailsService.Setup(o => o.GetFilteredQualifications())
                                        .ReturnsAsync(new List<Qualification> { DummyQualification });
        _mockQualificationDetailsService
            .Setup(x => x.GetQualificationDetailsPage(false, true, 3, 6, 2001, 5, 2019, It.IsAny<Qualification>()))
            .ReturnsAsync(DummyDetailsPage);

        _mockQualificationDetailsService.Setup(x => x.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(x => x.MapDetails(It.IsAny<Qualification>(),
                                                                 It.IsAny<QualificationDetailsPage>(), false))
                                        .ReturnsAsync(qualificationDetailsModel);
        _mockQualificationDetailsService
            .Setup(x => x.DoAdditionalAnswersMatchQuestions(It.IsAny<QualificationDetailsModel>())).Returns(true);
        _mockQualificationDetailsService.Setup(o => o.GetLevelOfQualification()).Returns(3);
        _mockQualificationDetailsService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((6, 2001));
        _mockQualificationDetailsService.Setup(o => o.GetWhenWasQualificationAwarded()).Returns((5, 2019));
        _mockQualificationDetailsService.Setup(o => o.GetUserIsCheckingOwnQualification()).Returns(false);
        _mockQualificationDetailsService
            .Setup(o => o.MapAdditionalRequirementAnswers(It.IsAny<List<AdditionalRequirementQuestion>>()))
            .Returns([]);

        var sut = GetSut();

        var result = await sut.Index(qualificationId);

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("CheckAdditionalRequirements");
        resultType.RouteValues.Should().Contain("qualificationId", qualificationId);
        resultType.RouteValues.Should().Contain("questionIndex", 1);
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task Index_QualificationHasAdditionalQuestionsButAnswers_ContainsQtsQuestion(
        bool userAnswerIsFullAndRelevant)
    {
        const string qualificationId = "qualificationId";

        var qualification = new Qualification(qualificationId, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())
                            {
                                AdditionalRequirementQuestions =
                                [
                                    new AdditionalRequirementQuestion
                                    {
                                        Question = "QtsQuestion",
                                        Sys = new SystemProperties
                                              {
                                                  Id = AdditionalRequirementQuestions
                                                      .QtsQuestion
                                              }
                                    },

                                    new AdditionalRequirementQuestion
                                    {
                                        Question = "Question 1",
                                        Sys = new SystemProperties
                                              {
                                                  Id = "abcde"
                                              }
                                    }
                                ]
                            };
        var qtsQuestion =
            qualification.AdditionalRequirementQuestions.First(o => o.Sys.Id == AdditionalRequirementQuestions
                                                                        .QtsQuestion);
        var details = new QualificationDetailsModel
                      {
                          AdditionalRequirementAnswers =
                          [
                              new AdditionalRequirementAnswerModel
                              {
                                  Question = qtsQuestion.Question
                              },
                              new AdditionalRequirementAnswerModel
                              {
                                  Question = "Question 1"
                              }
                          ],
                          Content = new DetailsPageModel()
                      };

        var notQtsAnswer = details.AdditionalRequirementAnswers.First(o => o.Question == "Question 1");
        _mockQualificationDetailsService.Setup(x => x.GetQualificationById(It.IsAny<string>()))
                                        .ReturnsAsync(qualification);
        _mockQualificationDetailsService.Setup(o => o.GetFilteredQualifications())
                                        .ReturnsAsync(new List<Qualification> { qualification });
        _mockQualificationDetailsService
            .Setup(x => x.GetQualificationDetailsPage(false, true, 3, 6, 2001, 5, 2019, It.IsAny<Qualification>()))
            .ReturnsAsync(new QualificationDetailsPage());
        _mockQualificationDetailsService.Setup(x => x.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(x => x.MapDetails(It.IsAny<Qualification>(),
                                                                 It.IsAny<QualificationDetailsPage>(), false))
                                        .ReturnsAsync(details);
        _mockQualificationDetailsService.Setup(x => x.QualificationContainsQtsQuestion(It.IsAny<Qualification>()))
                                        .Returns(true);
        _mockQualificationDetailsService
            .Setup(x => x.UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(qualification,
                                                                                details.AdditionalRequirementAnswers))
            .Returns(userAnswerIsFullAndRelevant);
        _mockQualificationDetailsService.Setup(o => o.GetLevelOfQualification()).Returns(3);
        _mockQualificationDetailsService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((6, 2001));
        _mockQualificationDetailsService.Setup(o => o.GetWhenWasQualificationAwarded()).Returns((5, 2019));
        _mockQualificationDetailsService.Setup(o => o.GetUserIsCheckingOwnQualification()).Returns(false);
        _mockQualificationDetailsService
            .Setup(o => o.MapAdditionalRequirementAnswers(It.IsAny<List<AdditionalRequirementQuestion>>()))
            .Returns(details.AdditionalRequirementAnswers);

        var sut = GetSut();

        var result = await sut.Index(qualificationId);

        _mockQualificationDetailsService
            .Verify(o => o.UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(qualification,
                     details.AdditionalRequirementAnswers),
                    Times.Once);

        if (userAnswerIsFullAndRelevant)
        {
            details.AdditionalRequirementAnswers.Should().NotContain(notQtsAnswer);
            result.Should().NotBeNull();
            _mockQualificationDetailsService
                .Verify(o => o.RemainingAnswersIndicateFullAndRelevant(It.IsAny<QualificationDetailsModel>(),
                                                                       qtsQuestion), Times.Never);
        }
        else
        {
            _mockQualificationDetailsService
                .Verify(o => o.RemainingAnswersIndicateFullAndRelevant(It.IsAny<QualificationDetailsModel>(),
                                                                       qtsQuestion), Times.Once);
            details.AdditionalRequirementAnswers.Should().Contain(notQtsAnswer);
        }
    }

    [TestMethod]
    public async Task Index_FullAndRelevant_CallsSuccessCorrectly()
    {
        const string qualificationId = "qualificationId";

        var details = new QualificationDetailsModel
                      {
                          Content = new DetailsPageModel(),
                          RatioRequirements = new RatioRequirementModel
                                              {
                                                  ApprovedForLevel2 = QualificationApprovalStatus.Approved,
                                                  ApprovedForLevel3 = QualificationApprovalStatus.Approved,
                                                  ApprovedForLevel6 = QualificationApprovalStatus.Approved
                                              },
                          AdditionalRequirementAnswers =
                          [
                              new AdditionalRequirementAnswerModel
                              {
                                  Question = "Question 1"
                              }
                          ]
                      };

        _mockQualificationDetailsService.Setup(x => x.GetQualificationById(It.IsAny<string>()))
                                        .ReturnsAsync(new Qualification(qualificationId, "Name", "Awarding Org", 3));

        _mockQualificationDetailsService.Setup(o => o.GetFilteredQualifications())
                                        .ReturnsAsync(new List<Qualification>
                                                      {
                                                          new Qualification(qualificationId,
                                                                            It.IsAny<string>(),
                                                                            It.IsAny<string>(),
                                                                            It.IsAny<int>())
                                                      });
        _mockQualificationDetailsService.Setup(x => x.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(x => x.MapDetails(It.IsAny<Qualification>(),
                                                                 It.IsAny<QualificationDetailsPage>(), false))
                                        .ReturnsAsync(details);
        _mockQualificationDetailsService.Setup(o => o.GetLevelOfQualification()).Returns(3);
        _mockQualificationDetailsService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((6, 2001));
        _mockQualificationDetailsService.Setup(o => o.GetWhenWasQualificationAwarded()).Returns((5, 2019));
        _mockQualificationDetailsService.Setup(o => o.GetUserIsCheckingOwnQualification()).Returns(false);
        _mockQualificationDetailsService
            .Setup(o => o.GetQualificationDetailsPage(false, true, 3, 6, 2001, 5, 2019, It.IsAny<Qualification>()))
            .ReturnsAsync(DummyDetailsPage);
        _mockQualificationDetailsService
            .Setup(o => o.MapAdditionalRequirementAnswers(It.IsAny<List<AdditionalRequirementQuestion>>()))
            .Returns(details.AdditionalRequirementAnswers);
        _mockQualificationDetailsService
            .Setup(x => x.AnswersIndicateNotFullAndRelevant(It.IsAny<List<AdditionalRequirementAnswerModel>>()))
            .Returns(false);

        var sut = GetSut();

        await sut.Index(qualificationId);

        _mockQualificationDetailsService
            .Verify(o => o.MapDetails(It.IsAny<Qualification>(), It.IsAny<QualificationDetailsPage>(), true), Times.Once);
        _mockQualificationDetailsService
            .Verify(o => o.SetQualificationResultSuccessDetails(It.IsAny<QualificationDetailsModel>(),
                                                                It.IsAny<DetailsPageLabels>()),
                    Times.Once);
    }

    [TestMethod]
    public async Task Index_NotFullAndRelevant_CallsFailCorrectly()
    {
        const string qualificationId = "qualificationId";

        var details = new QualificationDetailsModel
                      {
                          Content = new DetailsPageModel(),
                          RatioRequirements = new RatioRequirementModel
                                              {
                                                  ApprovedForLevel2 = QualificationApprovalStatus.NotApproved,
                                                  ApprovedForLevel3 = QualificationApprovalStatus.NotApproved,
                                                  ApprovedForLevel6 = QualificationApprovalStatus.NotApproved
                                              },
                          AdditionalRequirementAnswers =
                          [
                              new AdditionalRequirementAnswerModel
                              {
                                  Question = "Question 1"
                              }
                          ]
                      };

        _mockQualificationDetailsService.Setup(x => x.GetQualificationById(It.IsAny<string>()))
                                        .ReturnsAsync(new Qualification(qualificationId, "Name", "Awarding Org", 3));

        _mockQualificationDetailsService.Setup(o => o.GetFilteredQualifications())
                                        .ReturnsAsync(new List<Qualification>
                                                      {
                                                          new Qualification(qualificationId,
                                                                            It.IsAny<string>(),
                                                                            It.IsAny<string>(),
                                                                            It.IsAny<int>())
                                                      });
        _mockQualificationDetailsService.Setup(x => x.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(x => x.MapDetails(It.IsAny<Qualification>(),
                                                                 It.IsAny<QualificationDetailsPage>(), false))
                                        .ReturnsAsync(details);
        _mockQualificationDetailsService.Setup(o => o.GetLevelOfQualification()).Returns(3);
        _mockQualificationDetailsService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((6, 2001));
        _mockQualificationDetailsService.Setup(o => o.GetWhenWasQualificationAwarded()).Returns((5, 2019));
        _mockQualificationDetailsService.Setup(o => o.GetUserIsCheckingOwnQualification()).Returns(false);
        _mockQualificationDetailsService
            .Setup(o => o.GetQualificationDetailsPage(false, false, 3, 6, 2001, 5, 2019, It.IsAny<Qualification>()))
            .ReturnsAsync(DummyDetailsPage);
        _mockQualificationDetailsService
            .Setup(o => o.MapAdditionalRequirementAnswers(It.IsAny<List<AdditionalRequirementQuestion>>()))
            .Returns(details.AdditionalRequirementAnswers);
        _mockQualificationDetailsService
            .Setup(x => x.AnswersIndicateNotFullAndRelevant(It.IsAny<List<AdditionalRequirementAnswerModel>>()))
            .Returns(true);

        var sut = GetSut();

        await sut.Index(qualificationId);

        _mockQualificationDetailsService
            .Verify(o => o.MapDetails(It.IsAny<Qualification>(), It.IsAny<QualificationDetailsPage>(), false), Times.Once);
        _mockQualificationDetailsService
            .Verify(o => o.SetQualificationResultFailureDetails(It.IsAny<QualificationDetailsModel>(),
                                                                It.IsAny<DetailsPageLabels>()),
                    Times.Once);
    }

    [TestMethod]
    public void QualificationResultModelSetsCorrectly()
    {
        const string heading = "heading";
        const string messageHeading = "messageHeading";
        const string messageBody = "messageBody";
        const bool isFullAndRelevant = true;
        var model = new QualificationResultModel
                    {
                        Heading = heading,
                        MessageHeading = messageHeading,
                        MessageBody = messageBody,
                        IsFullAndRelevant = isFullAndRelevant
                    };

        model.Heading.Should().Be(heading);
        model.MessageHeading.Should().Be(messageHeading);
        model.MessageBody.Should().Be(messageBody);
        model.IsFullAndRelevant.Should().Be(isFullAndRelevant);
    }

    [TestMethod]
    public void SortRatioRows_OrderByQualificationApprovedStatus_ReturnsExpectedOrder()
    {
        var model = new QualificationDetailsModel
                    {
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForLevel3 = QualificationApprovalStatus.Approved,
                                                ApprovedForLevel2 = QualificationApprovalStatus.Approved,
                                                ApprovedForLevel6 = QualificationApprovalStatus.Approved,
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved,
                                            }
                    };

        var orderedRows = model.OrderRatioRows();

        orderedRows.Where(x => x.ApprovalStatus == QualificationApprovalStatus.Approved).Should()
                   .BeInDescendingOrder(x => x.Level);
    }

    [TestMethod]
    public void SortRatioRows_OrderByQualificationMixedtatus_ReturnsExpectedOrder()
    {
        var model = new QualificationDetailsModel
                    {
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForLevel3 = QualificationApprovalStatus.PossibleRouteAvailable,
                                                ApprovedForLevel2 = QualificationApprovalStatus.FurtherActionRequired,
                                                ApprovedForLevel6 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved,
                                            }
                    };

        var orderedRows = model.OrderRatioRows();

        orderedRows.Where(x => x.ApprovalStatus == QualificationApprovalStatus.Approved)
                   .Should().BeInDescendingOrder(x => x.Level);

        orderedRows.Where(x => x.ApprovalStatus != QualificationApprovalStatus.Approved)
                   .Should().BeInAscendingOrder(x => x.Level);

        orderedRows.ElementAt(0).ApprovalStatus.Should().Be(QualificationApprovalStatus.Approved);
        orderedRows.ElementAt(0).RatioId.Should().Be("Unqualified");
        orderedRows.ElementAt(1).ApprovalStatus.Should().Be(QualificationApprovalStatus.FurtherActionRequired);
        orderedRows.ElementAt(1).RatioId.Should().Be("Level2");
        orderedRows.ElementAt(2).ApprovalStatus.Should().Be(QualificationApprovalStatus.PossibleRouteAvailable);
        orderedRows.ElementAt(2).RatioId.Should().Be("Level3");
        orderedRows.ElementAt(3).ApprovalStatus.Should().Be(QualificationApprovalStatus.NotApproved);
        orderedRows.ElementAt(3).RatioId.Should().Be("Level6");
    }
}