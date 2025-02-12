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
    private static Qualification DummyQualification => new(It.IsAny<string>(),
                                                           It.IsAny<string>(),
                                                           It.IsAny<string>(),
                                                           It.IsAny<int>()
                                                          );

    private static DetailsPage DummyDetailsPage => new();

    private static QualificationDetailsModel DummyDetails => new() { Content = new DetailsPageModel() };

    private readonly Mock<ILogger<QualificationDetailsController>> _mockLogger = new();
    private readonly Mock<IQualificationDetailsService> _mockQualificationDetailsService = new();

    private QualificationDetailsController GetSut() => new(_mockLogger.Object,
                                                           _mockQualificationDetailsService.Object)
                                                       {
                                                           ControllerContext = new ControllerContext
                                                                               {
                                                                                   HttpContext = new DefaultHttpContext()
                                                                               }
                                                       };

    [TestMethod]
    public async Task Index_NullId_Returns400BadRequest()
    {
        const string qualificationId = null!;
        var sut = GetSut();

        var result = await sut.Index(qualificationId!);

        result.Should().BeOfType<BadRequestResult>();
        var resultType = result as BadRequestResult;
        resultType.Should().NotBeNull();
        resultType!.StatusCode.Should().Be(400);
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
        resultType!.StatusCode.Should().Be(400);
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
        _mockQualificationDetailsService.Setup(o => o.HasStartDate()).Returns(true);

        var sut = GetSut();

        _ = await sut.Index(qualificationId);

        _mockQualificationDetailsService.Verify(o => o.GetDetailsPage(), Times.Once);
    }

    [TestMethod]
    public async Task Index_DetailsPage_IsNull_RedirectsToError()
    {
        const string qualificationId = "qualificationId";
        _mockQualificationDetailsService.Setup(o => o.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(o => o.GetDetailsPage()).ReturnsAsync((DetailsPage)null!);

        var sut = GetSut();

        var result = await sut.Index(qualificationId);

        _mockLogger.VerifyError("No content for the qualification details page");
        result.VerifyRedirect("Index", "Error");
    }

    [TestMethod]
    public async Task Index_Calls_QualificationDetailsService_GetQualification()
    {
        const string qualificationId = "qualificationId";
        _mockQualificationDetailsService.Setup(o => o.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(o => o.GetDetailsPage()).ReturnsAsync(DummyDetailsPage);

        var sut = GetSut();

        _ = await sut.Index(qualificationId);

        _mockQualificationDetailsService.Verify(o => o.GetQualification(qualificationId), Times.Once);
    }

    [TestMethod]
    public async Task Index_Qualification_IsNull_RedirectsToError()
    {
        const string qualificationId = "qualificationId";
        _mockQualificationDetailsService.Setup(o => o.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(o => o.GetDetailsPage()).ReturnsAsync(DummyDetailsPage);
        _mockQualificationDetailsService.Setup(o => o.GetQualification(qualificationId)).ReturnsAsync((Qualification)null!);

        var sut = GetSut();

        var result = await sut.Index(qualificationId);

        _mockLogger.VerifyError($"Could not find details for qualification with ID: qualificationId");
        result.VerifyRedirect("Index", "Error");
    }

    [TestMethod]
    public async Task Index_Calls_QualificationDetailsService_MapDetails()
    {
        const string qualificationId = "qualificationId";
        _mockQualificationDetailsService.Setup(o => o.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(o => o.GetDetailsPage()).ReturnsAsync(DummyDetailsPage);
        _mockQualificationDetailsService.Setup(o => o.GetQualification(qualificationId)).ReturnsAsync(DummyQualification);

        var sut = GetSut();
        try
        {
            _ = await sut.Index(qualificationId);
        }
        catch
        {
            //ignored
        }
        finally
        {
            _mockQualificationDetailsService.Verify(o => o.MapDetails(It.IsAny<Qualification>(), It.IsAny<DetailsPage>()), Times.Once);
        }
    }

    [TestMethod]
    public async Task Index_ValidateAdditionalQuestions_Valid_Calls_QualificationDetailsService_CheckRatioRequirements()
    {
        const string qualificationId = "qualificationId";
        _mockQualificationDetailsService.Setup(o => o.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(o => o.GetDetailsPage()).ReturnsAsync(DummyDetailsPage);
        _mockQualificationDetailsService.Setup(o => o.GetQualification(qualificationId)).ReturnsAsync(DummyQualification);
        _mockQualificationDetailsService.Setup(o => o.MapDetails(It.IsAny<Qualification>(), It.IsAny<DetailsPage>())).ReturnsAsync(DummyDetails);

        var sut = GetSut();
        _ = await sut.Index(qualificationId);

        _mockQualificationDetailsService.Verify(o => o.CheckRatioRequirements(It.IsAny<Qualification>(), It.IsAny<QualificationDetailsModel>()), Times.Once);
    }

    [TestMethod]
    public async Task Index_ValidateAdditionalQuestions_Valid_Calls_QualificationLevel3OrAboveMightBeRelevantAtLevel2()
    {
        const string qualificationId = "qualificationId";
        _mockQualificationDetailsService.Setup(o => o.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(o => o.GetDetailsPage()).ReturnsAsync(DummyDetailsPage);
        _mockQualificationDetailsService.Setup(o => o.GetQualification(qualificationId)).ReturnsAsync(DummyQualification);
        _mockQualificationDetailsService.Setup(o => o.MapDetails(It.IsAny<Qualification>(), It.IsAny<DetailsPage>())).ReturnsAsync(DummyDetails);

        var sut = GetSut();
        _ = await sut.Index(qualificationId);

        _mockQualificationDetailsService.Verify(o => o.QualificationLevel3OrAboveMightBeRelevantAtLevel2(It.IsAny<QualificationDetailsModel>(), It.IsAny<Qualification>()), Times.Once);
    }

    [TestMethod]
    public async Task Index_ValidateAdditionalQuestions_Valid_ReturnsView()
    {
        const string qualificationId = "qualificationId";
        _mockQualificationDetailsService.Setup(o => o.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(o => o.GetDetailsPage()).ReturnsAsync(DummyDetailsPage);
        _mockQualificationDetailsService.Setup(o => o.GetQualification(qualificationId)).ReturnsAsync(DummyQualification);
        _mockQualificationDetailsService.Setup(o => o.MapDetails(It.IsAny<Qualification>(), It.IsAny<DetailsPage>())).ReturnsAsync(DummyDetails);

        var sut = GetSut();
        var result = await sut.Index(qualificationId);

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as QualificationDetailsModel;
        model.Should().NotBeNull();
    }

    [TestMethod]
    public async Task Index_ValidateAdditionalQuestions_InValid_Calls_QualificationDetailsService_QualificationLevel3OrAboveMightBeRelevantAtLevel2()
    {
        const string qualificationId = "qualificationId";
        var details = new QualificationDetailsModel { AdditionalRequirementAnswers = [new AdditionalRequirementAnswerModel()], Content = new DetailsPageModel() };
        _mockQualificationDetailsService.Setup(o => o.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(o => o.GetDetailsPage()).ReturnsAsync(DummyDetailsPage);
        _mockQualificationDetailsService.Setup(o => o.GetQualification(qualificationId)).ReturnsAsync(DummyQualification);
        _mockQualificationDetailsService.Setup(o => o.MapDetails(It.IsAny<Qualification>(), It.IsAny<DetailsPage>())).ReturnsAsync(details);
        _mockQualificationDetailsService.Setup(o => o.AnswersIndicateNotFullAndRelevant(It.IsAny<List<AdditionalRequirementAnswerModel>>())).Returns(true);

        var sut = GetSut();
        _ = await sut.Index(qualificationId);

        _mockQualificationDetailsService.Verify(o => o.QualificationLevel3OrAboveMightBeRelevantAtLevel2(It.IsAny<QualificationDetailsModel>(), It.IsAny<Qualification>()), Times.Once);
    }

    [TestMethod]
    public async Task Index_ValidateAdditionalQuestions_InValid_Returns_View()
    {
        const string qualificationId = "qualificationId";
        var details = new QualificationDetailsModel { AdditionalRequirementAnswers = [new AdditionalRequirementAnswerModel()], Content = new DetailsPageModel() };
        _mockQualificationDetailsService.Setup(o => o.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(o => o.GetDetailsPage()).ReturnsAsync(DummyDetailsPage);
        _mockQualificationDetailsService.Setup(o => o.GetQualification(qualificationId)).ReturnsAsync(DummyQualification);
        _mockQualificationDetailsService.Setup(o => o.MapDetails(It.IsAny<Qualification>(), It.IsAny<DetailsPage>())).ReturnsAsync(details);
        _mockQualificationDetailsService.Setup(o => o.AnswersIndicateNotFullAndRelevant(It.IsAny<List<AdditionalRequirementAnswerModel>>())).Returns(true);

        var sut = GetSut();
        var result = await sut.Index(qualificationId);

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as QualificationDetailsModel;
        model.Should().NotBeNull();
    }

    [TestMethod]
    public async Task Index_QualificationHasAdditionalQuestionsButNoneAnswered_RedirectTotTheAdditionalQuestionsPage()
    {
        const string qualificationId = "qualificationId";

        var qualificationDetailsModel = new QualificationDetailsModel { QualificationId = qualificationId, AdditionalRequirementAnswers = [], Content = new DetailsPageModel() };

        _mockQualificationDetailsService.Setup(x => x.GetQualification(qualificationId)).ReturnsAsync(DummyQualification);
        _mockQualificationDetailsService.Setup(x => x.GetDetailsPage()).ReturnsAsync(DummyDetailsPage);

        _mockQualificationDetailsService.Setup(x => x.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(x => x.MapDetails(It.IsAny<Qualification>(), It.IsAny<DetailsPage>())).ReturnsAsync(qualificationDetailsModel);
        _mockQualificationDetailsService.Setup(x => x.DoAdditionalAnswersMatchQuestions(It.IsAny<QualificationDetailsModel>())).Returns(true);

        var sut = GetSut();

        var result = await sut.Index(qualificationId);

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("CheckAdditionalRequirements");
        resultType.RouteValues.Should().Contain("qualificationId", qualificationId);
        resultType.RouteValues.Should().Contain("questionIndex", 1);
    }

    [TestMethod]
    public async Task Index_QualificationHasAdditionalQuestionsButAnswersAreNotCorrect_MarkAsNotRelevantAndReturn()
    {
        const string qualificationId = "qualificationId";

        var details = new QualificationDetailsModel { AdditionalRequirementAnswers = [], Content = new DetailsPageModel() };

        _mockQualificationDetailsService.Setup(x => x.GetQualification(qualificationId)).ReturnsAsync(DummyQualification);
        _mockQualificationDetailsService.Setup(x => x.GetDetailsPage()).ReturnsAsync(new DetailsPage());
        _mockQualificationDetailsService.Setup(x => x.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(x => x.MapDetails(It.IsAny<Qualification>(), It.IsAny<DetailsPage>())).ReturnsAsync(details);
        _mockQualificationDetailsService.Setup(x => x.AnswersIndicateNotFullAndRelevant(It.IsAny<List<AdditionalRequirementAnswerModel>>())).Returns(true);

        var sut = GetSut();

        var result = await sut.Index(qualificationId);

        _mockQualificationDetailsService.Verify(o => o.QualificationContainsQtsQuestion(It.IsAny<Qualification>()), Times.Once);
        _mockQualificationDetailsService.Verify(o => o.DoAdditionalAnswersMatchQuestions(It.IsAny<QualificationDetailsModel>()), Times.Once);
        _mockQualificationDetailsService.Verify(o => o.AnswersIndicateNotFullAndRelevant(It.IsAny<List<AdditionalRequirementAnswerModel>>()), Times.Once);
        _mockQualificationDetailsService.Verify(o => o.MarkAsNotFullAndRelevant(It.IsAny<RatioRequirementModel>()), Times.Once);
        _mockQualificationDetailsService.Verify(o => o.QualificationLevel3OrAboveMightBeRelevantAtLevel2(It.IsAny<QualificationDetailsModel>(), It.IsAny<Qualification>()), Times.Once);

        result.Should().NotBeNull();
        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task Index_QualificationHasAdditionalQuestionsButAnswers_ContainsQtsQuestion(bool userAnswerIsFullAndRelevant)
    {
        const string qualificationId = "qualificationId";

        var qualification = new Qualification(qualificationId, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())
                            {
                                AdditionalRequirementQuestions = new List<AdditionalRequirementQuestion>
                                                                 {
                                                                     new()
                                                                     {
                                                                         Question = "QtsQuestion",
                                                                         Sys = new SystemProperties
                                                                               {
                                                                                   Id = AdditionalRequirementQuestions.QtsQuestion
                                                                               }
                                                                     },

                                                                     new()
                                                                     {
                                                                         Question = "Question 1",
                                                                         Sys = new SystemProperties
                                                                               {
                                                                                   Id = "abcde"
                                                                               }
                                                                     }
                                                                 }
                            };
        var qtsQuestion = qualification.AdditionalRequirementQuestions.First(o => o.Sys.Id == AdditionalRequirementQuestions.QtsQuestion);
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

        _mockQualificationDetailsService.Setup(x => x.GetQualification(qualificationId)).ReturnsAsync(qualification);
        _mockQualificationDetailsService.Setup(x => x.GetDetailsPage()).ReturnsAsync(new DetailsPage());
        _mockQualificationDetailsService.Setup(x => x.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(x => x.MapDetails(It.IsAny<Qualification>(), It.IsAny<DetailsPage>())).ReturnsAsync(details);
        _mockQualificationDetailsService.Setup(x => x.QualificationContainsQtsQuestion(It.IsAny<Qualification>())).Returns(true);
        _mockQualificationDetailsService.Setup(x => x.UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(qualification, details.AdditionalRequirementAnswers)).Returns(userAnswerIsFullAndRelevant);

        var sut = GetSut();

        var result = await sut.Index(qualificationId);

        _mockQualificationDetailsService.Verify(o => o.UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(qualification, details.AdditionalRequirementAnswers), Times.Once);

        if (userAnswerIsFullAndRelevant)
        {
            details.AdditionalRequirementAnswers.Should().NotContain(notQtsAnswer);
            result.Should().NotBeNull();
            _mockQualificationDetailsService.Verify(o => o.RemainingAnswersIndicateFullAndRelevant(details, qtsQuestion), Times.Never);
        }
        else
        {
            _mockQualificationDetailsService.Verify(o => o.RemainingAnswersIndicateFullAndRelevant(details, qtsQuestion), Times.Once);
            details.AdditionalRequirementAnswers.Should().Contain(notQtsAnswer);
        }
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task Index_CallsSuccessOrFail_Correctly(bool ratioIsNotFullAndRelevant)
    {
        const string qualificationId = "qualificationId";

        var details = new QualificationDetailsModel
                      {
                          Content = new DetailsPageModel(),
                          RatioRequirements = new RatioRequirementModel
                                              {
                                                  ApprovedForLevel2 = ratioIsNotFullAndRelevant ? QualificationApprovalStatus.NotApproved : QualificationApprovalStatus.Approved,
                                                  ApprovedForLevel3 = ratioIsNotFullAndRelevant ? QualificationApprovalStatus.NotApproved : QualificationApprovalStatus.Approved,
                                                  ApprovedForLevel6 = ratioIsNotFullAndRelevant ? QualificationApprovalStatus.NotApproved : QualificationApprovalStatus.Approved
                                              }
                      };

        _mockQualificationDetailsService.Setup(x => x.GetQualification(qualificationId)).ReturnsAsync(new Qualification(qualificationId, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
        _mockQualificationDetailsService.Setup(x => x.GetDetailsPage()).ReturnsAsync(new DetailsPage());
        _mockQualificationDetailsService.Setup(x => x.HasStartDate()).Returns(true);
        _mockQualificationDetailsService.Setup(x => x.MapDetails(It.IsAny<Qualification>(), It.IsAny<DetailsPage>())).ReturnsAsync(details);
        var sut = GetSut();

        await sut.Index(qualificationId);

        if (ratioIsNotFullAndRelevant)
        {
            _mockQualificationDetailsService.Verify(o => o.SetQualificationResultFailureDetails(It.IsAny<QualificationDetailsModel>(), It.IsAny<DetailsPage>()), Times.Once);
        }
        else
        {
            _mockQualificationDetailsService.Verify(o => o.SetQualificationResultSuccessDetails(It.IsAny<QualificationDetailsModel>(), It.IsAny<DetailsPage>()), Times.Once);
        }
    }
}