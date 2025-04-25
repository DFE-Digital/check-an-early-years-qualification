using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.QualificationDetails;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class QualificationDetailsServiceTests
{
    private Mock<IGovUkContentParser> _mockContentParser = new();
    private Mock<IContentService> _mockContentService = new();
    private Mock<ILogger<QualificationDetailsService>> _mockLogger = new();
    private Mock<IQualificationsRepository> _mockRepository = new();
    private Mock<IUserJourneyCookieService> _mockUserJourneyCookieService = new();

    private QualificationDetailsService GetSut()
    {
        return new QualificationDetailsService(
                                               _mockLogger.Object,
                                               _mockRepository.Object,
                                               _mockContentService.Object,
                                               _mockContentParser.Object,
                                               _mockUserJourneyCookieService.Object
                                              );
    }

    [TestInitialize]
    public void Initialize()
    {
        _mockLogger = new Mock<ILogger<QualificationDetailsService>>();
        _mockRepository = new Mock<IQualificationsRepository>();
        _mockContentService = new Mock<IContentService>();
        _mockContentParser = new Mock<IGovUkContentParser>();
        _mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
    }

    [TestMethod]
    public async Task GetQualification_Calls_Repository_GetById()
    {
        const string qualificationId = "qualificationId";
        var sut = GetSut();

        _ = await sut.GetQualification(qualificationId);

        _mockRepository.Verify(o => o.GetById(qualificationId), Times.Once);
    }

    [TestMethod]
    public async Task GetDetailsPage_Calls_Content_GetDetailsPage()
    {
        var sut = GetSut();

        _ = await sut.GetDetailsPage();

        _mockContentService.Verify(o => o.GetDetailsPage(), Times.Once);
    }

    [TestMethod]
    public void HasStartDate_Calls_Cookies_GetWhenQualificationStarted()
    {
        var sut = GetSut();

        _ = sut.HasStartDate();

        _mockUserJourneyCookieService.Verify(o => o.GetWhenWasQualificationStarted(), Times.Once);
    }

    [TestMethod]
    [DataRow(null, null)]
    [DataRow(1, null)]
    [DataRow(null, 1)]
    public void HasStartDate_NullDates_ReturnsFalse(int? month, int? year)
    {
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((month, year));
        var sut = GetSut();

        var result = sut.HasStartDate();

        result.Should().BeFalse();
    }

    [TestMethod]
    [DataRow(1, 1)]
    public void HasStartDate_GotDates_ReturnsTrue(int? month, int? year)
    {
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((month, year));
        var sut = GetSut();

        var result = sut.HasStartDate();

        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task GetFeedbackBannerToHtml_NullBanner_ReturnsNull()
    {
        FeedbackBanner? feedbackBanner = null;
        var sut = GetSut();

        var result = await sut.GetFeedbackBannerBodyToHtml(feedbackBanner);

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetFeedbackBannerToHtml_GotBanner_CallsContentParser()
    {
        const string expectedContent = "<h1>Feedback banner</h1>";
        var feedbackBanner = new FeedbackBanner { Body = new Document() };
        _mockContentParser.Setup(o => o.ToHtml(feedbackBanner.Body)).ReturnsAsync(expectedContent);
        var sut = GetSut();

        var result = await sut.GetFeedbackBannerBodyToHtml(feedbackBanner);

        _mockContentParser.Verify(o => o.ToHtml(feedbackBanner.Body), Times.Once);
        result.Should().BeEquivalentTo(expectedContent);
    }

    [TestMethod]
    public void QualificationContainsQtsQuestion_NullQuestions_ReturnsFalse()
    {
        var qualification = new Qualification(It.IsAny<string>(),
                                              It.IsAny<string>(),
                                              It.IsAny<string>(),
                                              It.IsAny<int>()
                                             )
                            {
                                AdditionalRequirementQuestions = null
                            };

        var sut = GetSut();

        var result = sut.QualificationContainsQtsQuestion(qualification);

        result.Should().BeFalse();
    }

    [TestMethod]
    [DataRow("abcde", false)]
    [DataRow(AdditionalRequirementQuestions.QtsQuestion, true)]
    [DataRow("uwxyz", false)]
    public void QualificationContainsQtsQuestion_GotQuestions_ReturnsTrueIfQts(string questionId, bool expectedResult)
    {
        var qualification = new Qualification(It.IsAny<string>(),
                                              It.IsAny<string>(),
                                              It.IsAny<string>(),
                                              It.IsAny<int>()
                                             )
                            {
                                AdditionalRequirementQuestions =
                                [
                                    new AdditionalRequirementQuestion
                                    {
                                        Sys = new SystemProperties { Id = questionId }
                                    }
                                ]
                            };

        var sut = GetSut();

        var result = sut.QualificationContainsQtsQuestion(qualification);

        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void MarkAsNotFullAndRelevant_Marks_Correctly()
    {
        var model = new RatioRequirementModel();
        var sut = GetSut();

        var result = sut.MarkAsNotFullAndRelevant(model);

        result.ApprovedForLevel2.Should().Be(QualificationApprovalStatus.NotApproved);
        result.ApprovedForLevel3.Should().Be(QualificationApprovalStatus.NotApproved);
        result.ApprovedForLevel6.Should().Be(QualificationApprovalStatus.NotApproved);
        result.ApprovedForUnqualified.Should().Be(QualificationApprovalStatus.Approved);
    }

    [TestMethod]
    public void DoAdditionalAnswersMatchQuestions_NoAnswers_ReturnsTrue()
    {
        var details = new QualificationDetailsModel
                      {
                          AdditionalRequirementAnswers = []
                      };

        var sut = GetSut();

        var result = sut.DoAdditionalAnswersMatchQuestions(details);

        result.Should().BeTrue();
    }

    [TestMethod]
    [DataRow(null, true)]
    [DataRow("abcde", false)]
    [DataRow("", true)]
    [DataRow("uwxyz", false)]
    public void DoAdditionalAnswersMatchQuestions_GotAnswers_ReturnsTrueIfExists(string answer, bool expectedResult)
    {
        var details = new QualificationDetailsModel
                      {
                          AdditionalRequirementAnswers =
                          [
                              new AdditionalRequirementAnswerModel
                              {
                                  Answer = answer
                              }
                          ]
                      };

        var sut = GetSut();

        var result = sut.DoAdditionalAnswersMatchQuestions(details);

        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void CalculateBackButton_Calls_Cookies_UserHasAnsweredAdditionalQuestions()
    {
        var detailsPage = new DetailsPage();
        const string qualificationId = "qualificationId";
        var sut = GetSut();

        _ = sut.CalculateBackButton(detailsPage, qualificationId);

        _mockUserJourneyCookieService.Verify(o => o.UserHasAnsweredAdditionalQuestions(), Times.Once);
    }

    [TestMethod]
    public void CalculateBackButton_HasAnsweredAdditionalQuestions_BackToConfirmAnswersNull_ReturnsBackButton()
    {
        var detailsPage = new DetailsPage
                          {
                              BackToConfirmAnswers = null,
                              BackButton = new NavigationLink()
                          };
        const string qualificationId = "qualificationId";
        _mockUserJourneyCookieService.Setup(o => o.UserHasAnsweredAdditionalQuestions()).Returns(true);
        var sut = GetSut();

        var result = sut.CalculateBackButton(detailsPage, qualificationId);

        result.Should().Be(detailsPage.BackButton);
    }

    [TestMethod]
    public void
        CalculateBackButton_HasAnsweredAdditionalQuestions_BackToConfirmAnswersNotNull_ReturnsExpectedBackButton()
    {
        const string qualificationId = "qualificationId";
        const string expectedHref = "qualificationId LINK";

        var detailsPage = new DetailsPage
                          { BackToConfirmAnswers = new NavigationLink { Href = "$[qualification-id]$ LINK" } };

        _mockUserJourneyCookieService.Setup(o => o.UserHasAnsweredAdditionalQuestions()).Returns(true);
        var sut = GetSut();

        var result = sut.CalculateBackButton(detailsPage, qualificationId);
        result.Should().NotBeNull();
        result!.Href.Should().BeEquivalentTo(expectedHref);
    }

    [TestMethod]
    public void CalculateBackButton_NoAdditionalQuestions_Calls_Cookies_GetLevelOfQualification()
    {
        const string qualificationId = "qualificationId";

        var detailsPage = new DetailsPage();

        _mockUserJourneyCookieService.Setup(o => o.UserHasAnsweredAdditionalQuestions()).Returns(false);
        var sut = GetSut();

        _ = sut.CalculateBackButton(detailsPage, qualificationId);

        _mockUserJourneyCookieService.Verify(o => o.GetLevelOfQualification(), Times.Once);
    }

    [TestMethod]
    [DataRow(YesOrNo.No, 1, false)]
    [DataRow(YesOrNo.No, 2, false)]
    [DataRow(YesOrNo.No, 3, false)]
    [DataRow(YesOrNo.No, 4, false)]
    [DataRow(YesOrNo.No, 5, false)]
    [DataRow(YesOrNo.No, 6, true)]
    [DataRow(YesOrNo.Yes, 1, false)]
    [DataRow(YesOrNo.Yes, 2, false)]
    [DataRow(YesOrNo.Yes, 3, false)]
    [DataRow(YesOrNo.Yes, 4, false)]
    [DataRow(YesOrNo.Yes, 5, false)]
    [DataRow(YesOrNo.Yes, 6, false)]
    public void CalculateBackButton_NoAdditionalQuestions_Calls_Cookies_WasStartedBeforeSeptember2014_WhenItShould(
        YesOrNo selectedFromList, int level, bool shouldCall)
    {
        const string qualificationId = "qualificationId";

        var detailsPage = new DetailsPage();

        _mockUserJourneyCookieService.Setup(o => o.UserHasAnsweredAdditionalQuestions()).Returns(false);
        _mockUserJourneyCookieService.Setup(o => o.GetQualificationWasSelectedFromList()).Returns(selectedFromList);
        _mockUserJourneyCookieService.Setup(o => o.GetLevelOfQualification()).Returns(level);

        var sut = GetSut();

        _ = sut.CalculateBackButton(detailsPage, qualificationId);

        _mockUserJourneyCookieService.Verify(o => o.WasStartedBeforeSeptember2014(), Times.Exactly(shouldCall ? 1 : 0));
    }

    [TestMethod]
    public void
        CalculateBackButton_NoAdditionalQuestions_Lvl6NotInList_BeforeSept2014_Returns_BackToLevelSixAdviceBefore2014()
    {
        const string qualificationId = "qualificationId";
        const string backToLevelSixAdviceBefore2014 = "backToLevelSixAdviceBefore2014";

        var detailsPage = new DetailsPage
                          {
                              BackToLevelSixAdviceBefore2014 =
                                  new NavigationLink { Href = backToLevelSixAdviceBefore2014 }
                          };

        _mockUserJourneyCookieService.Setup(o => o.UserHasAnsweredAdditionalQuestions()).Returns(false);
        _mockUserJourneyCookieService.Setup(o => o.GetQualificationWasSelectedFromList()).Returns(YesOrNo.No);
        _mockUserJourneyCookieService.Setup(o => o.GetLevelOfQualification()).Returns(6);
        _mockUserJourneyCookieService.Setup(o => o.WasStartedBeforeSeptember2014()).Returns(true);

        var sut = GetSut();

        var result = sut.CalculateBackButton(detailsPage, qualificationId);
        result.Should().NotBeNull();
        result!.Href.Should().BeEquivalentTo(backToLevelSixAdviceBefore2014);
    }

    [TestMethod]
    public void CalculateBackButton_NoAdditionalQuestions_Lvl6NotInList_AfterSept2014_Returns_BackToLevelSixAdvice()
    {
        const string qualificationId = "qualificationId";
        const string backToLevelSixAdvice = "backToLevelSixAdvice";

        var detailsPage = new DetailsPage { BackToLevelSixAdvice = new NavigationLink { Href = backToLevelSixAdvice } };

        _mockUserJourneyCookieService.Setup(o => o.UserHasAnsweredAdditionalQuestions()).Returns(false);
        _mockUserJourneyCookieService.Setup(o => o.GetQualificationWasSelectedFromList()).Returns(YesOrNo.No);
        _mockUserJourneyCookieService.Setup(o => o.GetLevelOfQualification()).Returns(6);
        _mockUserJourneyCookieService.Setup(o => o.WasStartedBeforeSeptember2014()).Returns(false);

        var sut = GetSut();

        var result = sut.CalculateBackButton(detailsPage, qualificationId);
        result.Should().NotBeNull();
        result!.Href.Should().BeEquivalentTo(backToLevelSixAdvice);
    }

    [TestMethod]
    public void CalculateBackButton_NoAdditionalQuestions_NotLvl6NotInList_AfterSept2014_Returns_BackToLevelSixAdvice()
    {
        const string qualificationId = "qualificationId";
        const string backButton = "backButton";

        var detailsPage = new DetailsPage { BackButton = new NavigationLink { Href = backButton } };

        _mockUserJourneyCookieService.Setup(o => o.UserHasAnsweredAdditionalQuestions()).Returns(false);
        _mockUserJourneyCookieService.Setup(o => o.GetQualificationWasSelectedFromList()).Returns(YesOrNo.Yes);
        _mockUserJourneyCookieService.Setup(o => o.GetLevelOfQualification()).Returns(1);

        var sut = GetSut();

        var result = sut.CalculateBackButton(detailsPage, qualificationId);
        result.Should().NotBeNull();
        result!.Href.Should().BeEquivalentTo(backButton);
    }

    [TestMethod]
    [DataRow(true, "no", true)]
    [DataRow(true, "yes", false)]
    [DataRow(false, "no", false)]
    [DataRow(false, "yes", true)]
    public void AnswersIndicateNotFullAndRelevant(bool fullAndRelevant, string answer, bool expectedResult)
    {
        var additionalRequirementsAnswers = new List<AdditionalRequirementAnswerModel>
                                            {
                                                new()
                                                {
                                                    AnswerToBeFullAndRelevant = fullAndRelevant,
                                                    Answer = answer
                                                }
                                            };

        var sut = GetSut();

        var result = sut.AnswersIndicateNotFullAndRelevant(additionalRequirementsAnswers);

        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public void UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant_NoAnswers_Returns_False()
    {
        var qualification =
            new Qualification(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>());
        List<AdditionalRequirementAnswerModel> additionalRequirementAnswerModels = null!;
        var sut = GetSut();

        var result =
            sut.UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(qualification, additionalRequirementAnswerModels);

        result.Should().BeFalse();
    }

    [TestMethod]
    [DataRow("yes", true, true)]
    [DataRow("no", true, false)]
    [DataRow("yes", false, false)]
    [DataRow("no", false, true)]
    public void UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant_Returns_AnswerAsBool(
        string answer, bool qtsFullAndRelevant, bool expectedResult)
    {
        var qts = new AdditionalRequirementQuestion
                  {
                      Sys = new SystemProperties
                            {
                                Id = AdditionalRequirementQuestions.QtsQuestion
                            },
                      AnswerToBeFullAndRelevant = qtsFullAndRelevant
                  };
        var qualification =
            new Qualification(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())
            {
                AdditionalRequirementQuestions = [qts]
            };
        var additionalRequirementAnswerModels = new List<AdditionalRequirementAnswerModel>
                                                { new() { Question = qts.Question, Answer = answer } };
        var sut = GetSut();

        var result =
            sut.UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(qualification, additionalRequirementAnswerModels);

        result.Should().Be(expectedResult);
    }

    [TestMethod]
    public async Task QualificationLevel3OrAboveMightBeRelevantAtLevel2_DoesNotHitEdgeCase()
    {
        var details = new QualificationDetailsModel();
        var qualification =
            new Qualification(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>());

        var sut = GetSut();

        await sut.QualificationLevel3OrAboveMightBeRelevantAtLevel2(details, qualification);

        details.RatioRequirements.ApprovedForLevel2.Should().Be(QualificationApprovalStatus.Approved);
        _mockContentParser.Verify(o => o.ToHtml(It.IsAny<Document>()), Times.Never);
        details.RatioRequirements.RequirementsForLevel2.Should().Be(string.Empty);
        details.RatioRequirements.ShowRequirementsForLevel2ByDefault.Should().BeFalse();
    }

    [TestMethod]
    public async Task QualificationLevel3OrAboveMightBeRelevantAtLevel2_DoesHitEdgeCase()
    {
        var details = new QualificationDetailsModel
                      {
                          RatioRequirements = new RatioRequirementModel
                                              {
                                                  ApprovedForLevel2 = QualificationApprovalStatus.NotApproved,
                                                  ApprovedForLevel3 = QualificationApprovalStatus.NotApproved,
                                                  ApprovedForLevel6 = QualificationApprovalStatus.NotApproved
                                              }
                      };
        var qualification = new Qualification(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 3)
                            {
                                RatioRequirements =
                                [
                                    new RatioRequirement
                                    {
                                        RatioRequirementName = RatioRequirements.Level2RatioRequirementName
                                    }
                                ]
                            };
        const string requirementsForLevel2 = "requirementsForLevel2";

        _mockContentParser.Setup(o => o.ToHtml(It.IsAny<Document>())).ReturnsAsync(requirementsForLevel2);
        _mockUserJourneyCookieService.Setup(o => o.WasStartedBetweenSeptember2014AndAugust2019()).Returns(true);
        var sut = GetSut();

        await sut.QualificationLevel3OrAboveMightBeRelevantAtLevel2(details, qualification);

        details.RatioRequirements.ApprovedForLevel2.Should().Be(QualificationApprovalStatus.FurtherActionRequired);
        _mockContentParser.Verify(o => o.ToHtml(It.IsAny<Document>()), Times.Once);
        details.RatioRequirements.RequirementsForLevel2.Should().Be(requirementsForLevel2);
        details.RatioRequirements.ShowRequirementsForLevel2ByDefault.Should().BeTrue();
    }

    [TestMethod]
    public async Task QualificationLevel3OrAboveMightBeRelevantAtLevel2_MissingRequirements_Throws()
    {
        const string qualificationId = "qualificationId";
        var details = new QualificationDetailsModel
                      {
                          RatioRequirements = new RatioRequirementModel
                                              {
                                                  ApprovedForLevel2 = QualificationApprovalStatus.NotApproved,
                                                  ApprovedForLevel3 = QualificationApprovalStatus.NotApproved,
                                                  ApprovedForLevel6 = QualificationApprovalStatus.NotApproved
                                              }
                      };
        var qualification = new Qualification(qualificationId, It.IsAny<string>(), It.IsAny<string>(), 3);
        const string requirementsForLevel2 = "requirementsForLevel2";

        _mockContentParser.Setup(o => o.ToHtml(It.IsAny<Document>())).ReturnsAsync(requirementsForLevel2);
        _mockUserJourneyCookieService.Setup(o => o.WasStartedBetweenSeptember2014AndAugust2019()).Returns(true);
        var sut = GetSut();

        var action = async () => await sut.QualificationLevel3OrAboveMightBeRelevantAtLevel2(details, qualification);

        await action.Should().ThrowAsync<Exception>();

        _mockLogger.VerifyError("Could not find property: RequirementForLevel2BetweenSept14AndAug19 within Level 2 Ratio Requirements for qualification: qualificationId");
    }

    [TestMethod]
    public void MapAdditionalRequirementAnswers_Null_ReturnsNull()
    {
        List<AdditionalRequirementQuestion> additionalRequirementQuestions = null!;

        var sut = GetSut();

        var result = sut.MapAdditionalRequirementAnswers(additionalRequirementQuestions);

        result.Should().BeNull();
    }

    [TestMethod]
    public void MapAdditionalRequirementAnswers_Calls_Cookies_GetAdditionalQuestionsAnswers()
    {
        var additionalRequirementQuestions = new List<AdditionalRequirementQuestion>();

        var sut = GetSut();

        _ = sut.MapAdditionalRequirementAnswers(additionalRequirementQuestions);

        _mockUserJourneyCookieService.Verify(o => o.GetAdditionalQuestionsAnswers(), Times.Once);
    }

    [TestMethod]
    public void MapAdditionalRequirementAnswers_NullAnswers_ReturnsEmpty()
    {
        var additionalRequirementQuestions = new List<AdditionalRequirementQuestion>();

        _mockUserJourneyCookieService.Setup(o => o.GetAdditionalQuestionsAnswers())
                                     .Returns((Dictionary<string, string>)null!);
        var sut = GetSut();

        var result = sut.MapAdditionalRequirementAnswers(additionalRequirementQuestions);

        result.Should().BeEquivalentTo(new List<AdditionalRequirementAnswerModel>());
    }

    [TestMethod]
    public void MapAdditionalRequirementAnswers_MapsCorrectly()
    {
        var additionalRequirementQuestions = new List<AdditionalRequirementQuestion>
                                             {
                                                 new()
                                                 {
                                                     Question = "Question 1",
                                                     AnswerToBeFullAndRelevant = true,
                                                     ConfirmationStatement = "confirmation statement 1"
                                                 },
                                                 new()
                                                 {
                                                     Question = "Question 2",
                                                     AnswerToBeFullAndRelevant = false,
                                                     ConfirmationStatement = "confirmation statement 2"
                                                 },
                                                 new()
                                                 {
                                                     Question = "Question 3",
                                                     AnswerToBeFullAndRelevant = true,
                                                     ConfirmationStatement = "confirmation statement 3"
                                                 }
                                             };

        var userAnswers = new Dictionary<string, string>
                          {
                              { "Question 1", "Answer 1" },
                              { "Question 2", "Answer 2" },
                              { "Question 3", "Answer 3" }
                          };

        var expected = new List<AdditionalRequirementAnswerModel>
                       {
                           new()
                           {
                               Question = "Question 1",
                               AnswerToBeFullAndRelevant = true,
                               ConfirmationStatement = "confirmation statement 1",
                               Answer = "Answer 1"
                           },
                           new()
                           {
                               Question = "Question 2",
                               AnswerToBeFullAndRelevant = false,
                               ConfirmationStatement = "confirmation statement 2",
                               Answer = "Answer 2"
                           },
                           new()
                           {
                               Question = "Question 3",
                               AnswerToBeFullAndRelevant = true,
                               ConfirmationStatement = "confirmation statement 3",
                               Answer = "Answer 3"
                           }
                       };

        _mockUserJourneyCookieService.Setup(o => o.GetAdditionalQuestionsAnswers()).Returns(userAnswers);
        var sut = GetSut();

        var result = sut.MapAdditionalRequirementAnswers(additionalRequirementQuestions);

        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void RemainingAnswersIndicateFullAndRelevant_FullAndRelevant_ReturnsExpected()
    {
        var qtsQuestion = new AdditionalRequirementQuestion { Question = "Qts" };
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
                                  AnswerToBeFullAndRelevant = true,
                                  Answer = "yes"
                              }
                          ]
                      };
        var sut = GetSut();

        var result = sut.RemainingAnswersIndicateFullAndRelevant(details, qtsQuestion);

        result.isFullAndRelevant.Should().BeTrue();
        result.details.RatioRequirements.ShowRequirementsForLevel6ByDefault.Should().BeTrue();
    }

    [TestMethod]
    public void RemainingAnswersIndicateFullAndRelevant_NotFullAndRelevant_ReturnsExpected()
    {
        var qtsQuestion = new AdditionalRequirementQuestion { Question = "Qts" };
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
                                  AnswerToBeFullAndRelevant = true,
                                  Answer = "no"
                              }
                          ]
                      };
        var sut = GetSut();

        var result = sut.RemainingAnswersIndicateFullAndRelevant(details, qtsQuestion);

        result.isFullAndRelevant.Should().BeFalse();
        result.details.RatioRequirements.ShowRequirementsForLevel6ByDefault.Should().BeTrue();
    }

    [TestMethod]
    public async Task CheckLevel6Requirements_ChecksCorrectly()
    {
        var qualification = new Qualification(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 6)
                            {
                                RatioRequirements =
                                [
                                    new RatioRequirement
                                    {
                                        RatioRequirementName = RatioRequirements.Level6RatioRequirementName
                                    }
                                ]
                            };
        var details = new QualificationDetailsModel();

        _mockUserJourneyCookieService.Setup(o => o.WasStartedBeforeSeptember2014()).Returns(true);

        var sut = GetSut();

        var result = await sut.CheckLevel6Requirements(qualification, details);

        result.RatioRequirements.ApprovedForLevel2.Should().Be(QualificationApprovalStatus.NotApproved);
        result.RatioRequirements.ApprovedForLevel3.Should().Be(QualificationApprovalStatus.NotApproved);
        result.RatioRequirements.ApprovedForLevel6.Should().Be(QualificationApprovalStatus.NotApproved);
        result.RatioRequirements.ApprovedForUnqualified.Should().Be(QualificationApprovalStatus.Approved);

        _mockContentParser.Verify(o => o.ToHtml(It.IsAny<Document>()), Times.Once);
    }

    [TestMethod]
    [DataRow(null, null, "", null, null, "")]
    [DataRow(null, 2024, "", null, null, "")]
    [DataRow(1, null, "", null, null, "")]
    [DataRow(1, 2024, "January 2024", null, null, "")]
    [DataRow(null, null, "", null, null, "")]
    [DataRow(null, null, "", null, 2024, "")]
    [DataRow(null, null, "", 1, null, "")]
    [DataRow(null, null, "", 1, 2024, "January 2024")]
    public async Task MapDetails_(int? startMonth, int? startYear, string dateStarted, int? awardMonth, int? awardYear,
                                  string dateAwarded)
    {
        const string qualificationId = "qualificationId";
        const string qualificationName = "qualificationName";
        const string awardingOrganisationTitle = "awardingOrganisationTitle";
        const int qualificationLevel = 1;
        const string checkAnotherQualification = "checkAnotherQualification";
        const string furtherInfo = "furtherInfo";
        const string requirements = "requirements";
        const string feedback = "feedback";
        var checkAnotherQualificationText = new Document { NodeType = checkAnotherQualification };
        var furtherInfoText = new Document { NodeType = furtherInfo };
        var requirementsText = new Document { NodeType = requirements };
        var feedbackText = new Document { NodeType = feedback };
        var feedbackBanner = new FeedbackBanner { Body = feedbackText };
        var backButton = new NavigationLink { Href = "backbutton" };
        var qualification =
            new Qualification(qualificationId, qualificationName, awardingOrganisationTitle, qualificationLevel)
            { FromWhichYear = "FromWhichYear" };
        var detailsPage = new DetailsPage
                          {
                              CheckAnotherQualificationText = checkAnotherQualificationText,
                              FurtherInfoText = furtherInfoText,
                              RequirementsText = requirementsText,
                              FeedbackBanner = feedbackBanner,
                              BackButton = backButton
                          };

        _mockContentParser.Setup(o => o.ToHtml(checkAnotherQualificationText)).ReturnsAsync(checkAnotherQualification);
        _mockContentParser.Setup(o => o.ToHtml(furtherInfoText)).ReturnsAsync(furtherInfo);
        _mockContentParser.Setup(o => o.ToHtml(requirementsText)).ReturnsAsync(requirements);
        _mockContentParser.Setup(o => o.ToHtml(feedbackText)).ReturnsAsync(feedback);
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((startMonth, startYear));
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationAwarded()).Returns((awardMonth, awardYear));

        var sut = GetSut();
        var result = await sut.MapDetails(qualification, detailsPage);

        _mockContentParser.Verify(o => o.ToHtml(checkAnotherQualificationText), Times.Once);
        _mockContentParser.Verify(o => o.ToHtml(furtherInfoText), Times.Once);
        _mockContentParser.Verify(o => o.ToHtml(requirementsText), Times.Once);
        _mockContentParser.Verify(o => o.ToHtml(feedbackText), Times.Once);

        result.QualificationId.Should().Be(qualificationId);
        result.QualificationLevel.Should().Be(qualificationLevel);
        result.QualificationName.Should().Be(qualificationName);
        result.AwardingOrganisationTitle.Should().Be(awardingOrganisationTitle);
        result.FromWhichYear.Should().Be(qualification.FromWhichYear);
        result.BackButton!.Href.Should().Be(backButton.Href);
        result.AdditionalRequirementAnswers.Should().BeNullOrEmpty();
        result.DateStarted.Should().Be(dateStarted);
        result.DateAwarded.Should().Be(dateAwarded);

        var content = result.Content!;
        content.CheckAnotherQualificationText.Should().Be(checkAnotherQualification);
        content.FurtherInfoText.Should().Be(furtherInfo);
        content.RequirementsText.Should().Be(requirements);
        content.FeedbackBanner!.Body.Should().Be(feedback);
    }

    [TestMethod]
    public async Task CheckRatioRequirements_Calls_Cookies_WasStartedBeforeSeptember2014()
    {
        const bool wasStartedBeforeSeptember2014 = true;
        const string qualificationId = "qualificationId";
        const string qualificationName = "qualificationName";
        const string awardingOrganisationTitle = "awardingOrganisationTitle";
        const int qualificationLevel = 2;
        var qualification =
            new Qualification(qualificationId, qualificationName, awardingOrganisationTitle, qualificationLevel)
            {
                RatioRequirements =
                [
                    new RatioRequirement
                    {
                        RatioRequirementName = RatioRequirements.Level2RatioRequirementName
                    },
                    new RatioRequirement
                    {
                        RatioRequirementName = RatioRequirements.Level3RatioRequirementName
                    },
                    new RatioRequirement
                    {
                        RatioRequirementName = RatioRequirements.Level6RatioRequirementName
                    },
                    new RatioRequirement
                    {
                        RatioRequirementName = RatioRequirements.UnqualifiedRatioRequirementName
                    }
                ]
            };
        var qualificationDetails = new QualificationDetailsModel();

        _mockUserJourneyCookieService.Setup(o => o.WasStartedBeforeSeptember2014())
                                     .Returns(wasStartedBeforeSeptember2014);

        var sut = GetSut();

        await sut.CheckRatioRequirements(qualification, qualificationDetails);

        _mockUserJourneyCookieService.Verify(o => o.WasStartedBeforeSeptember2014(), Times.Once);
    }

    [TestMethod]
    public async Task CheckRatioRequirements_AutomaticallyApproved()
    {
        const bool wasStartedBeforeSeptember2014 = true;
        const string qualificationId = "qualificationId";
        const string qualificationName = "qualificationName";
        const string awardingOrganisationTitle = "awardingOrganisationTitle";
        const int qualificationLevel = 2;
        var qualification =
            new Qualification(qualificationId, qualificationName, awardingOrganisationTitle, qualificationLevel)
            {
                IsAutomaticallyApprovedAtLevel6 = true,
                RatioRequirements =
                [
                    new RatioRequirement
                    {
                        RatioRequirementName = RatioRequirements.Level2RatioRequirementName,
                        RequirementForQtsEtcBefore2014 = new Document()
                    },
                    new RatioRequirement
                    {
                        RatioRequirementName = RatioRequirements.Level3RatioRequirementName,
                        RequirementForQtsEtcBefore2014 = new Document()
                    },
                    new RatioRequirement
                    {
                        RatioRequirementName = RatioRequirements.Level6RatioRequirementName,
                        RequirementForQtsEtcBefore2014 = new Document()
                    },
                    new RatioRequirement
                    {
                        RatioRequirementName = RatioRequirements.UnqualifiedRatioRequirementName,
                        RequirementForQtsEtcBefore2014 = new Document()
                    }
                ]
            };
        var qualificationDetails = new QualificationDetailsModel();

        _mockUserJourneyCookieService.Setup(o => o.WasStartedBeforeSeptember2014())
                                     .Returns(wasStartedBeforeSeptember2014);

        var sut = GetSut();

        await sut.CheckRatioRequirements(qualification, qualificationDetails);

        _mockUserJourneyCookieService.Verify(o => o.WasStartedBeforeSeptember2014(), Times.Once);
    }

    [TestMethod]
    public async Task SetRatiosText_IsFullAndRelevant_ShowsApprovedText()
    {
        const string ratiosText = "Approved ratio text";
        const string ratiosTextNotFullAndRelevant = "Not approved";
        const string ratiosTextL3PlusNotFrBetweenSep14Aug19 = "Not approved L3+ between Sep14 and Aug19";
        var ratiosTextDoc = new Document { NodeType = ratiosText };
        var ratiosTextNotFullAndRelevantDoc = new Document { NodeType = ratiosTextNotFullAndRelevant };
        var ratiosTextL3PlusNotFrBetweenSep14Aug19Doc =
            new Document { NodeType = ratiosTextL3PlusNotFrBetweenSep14Aug19 };
        _mockContentParser.Setup(o => o.ToHtml(ratiosTextDoc)).ReturnsAsync(ratiosText);
        _mockContentParser.Setup(o => o.ToHtml(ratiosTextNotFullAndRelevantDoc))
                          .ReturnsAsync(ratiosTextNotFullAndRelevant);
        _mockContentParser.Setup(o => o.ToHtml(ratiosTextL3PlusNotFrBetweenSep14Aug19Doc))
                          .ReturnsAsync(ratiosTextL3PlusNotFrBetweenSep14Aug19);
        var detailsPageContent = new DetailsPage
                                 {
                                     RatiosText = ratiosTextDoc,
                                     RatiosTextNotFullAndRelevant = ratiosTextNotFullAndRelevantDoc,
                                     RatiosTextL3PlusNotFrBetweenSep14Aug19 = ratiosTextL3PlusNotFrBetweenSep14Aug19Doc
                                 };

        var model = new QualificationDetailsModel
                    {
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForLevel2 = QualificationApprovalStatus.Approved,
                                                ApprovedForLevel3 = QualificationApprovalStatus.Approved,
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved
                                            },
                        Content = new DetailsPageModel()
                    };

        var sut = GetSut();

        await sut.SetRatioText(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().Be(ratiosText);
    }

    [TestMethod]
    public async Task SetRatiosText_IsNotFullAndRelevantAndL3BetweenSep14AndAug19_ShowsNotApprovedText()
    {
        const string ratiosText = "Approved ratio text";
        const string ratiosTextNotFullAndRelevant = "Not approved";
        const string ratiosTextL3PlusNotFrBetweenSep14Aug19 = "Not approved L3+ between Sep14 and Aug19";
        var ratiosTextDoc = new Document { NodeType = ratiosText };
        var ratiosTextNotFullAndRelevantDoc = new Document { NodeType = ratiosTextNotFullAndRelevant };
        var ratiosTextL3PlusNotFrBetweenSep14Aug19Doc =
            new Document { NodeType = ratiosTextL3PlusNotFrBetweenSep14Aug19 };
        _mockContentParser.Setup(o => o.ToHtml(ratiosTextDoc)).ReturnsAsync(ratiosText);
        _mockContentParser.Setup(o => o.ToHtml(ratiosTextNotFullAndRelevantDoc))
                          .ReturnsAsync(ratiosTextNotFullAndRelevant);
        _mockContentParser.Setup(o => o.ToHtml(ratiosTextL3PlusNotFrBetweenSep14Aug19Doc))
                          .ReturnsAsync(ratiosTextL3PlusNotFrBetweenSep14Aug19);
        var detailsPageContent = new DetailsPage
                                 {
                                     RatiosText = ratiosTextDoc,
                                     RatiosTextNotFullAndRelevant = ratiosTextNotFullAndRelevantDoc,
                                     RatiosTextL3PlusNotFrBetweenSep14Aug19 = ratiosTextL3PlusNotFrBetweenSep14Aug19Doc
                                 };

        var model = new QualificationDetailsModel
                    {
                        QualificationLevel = 3,
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForLevel6 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel2 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel3 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved
                                            },
                        Content = new DetailsPageModel()
                    };

        _mockUserJourneyCookieService.Setup(x => x.WasStartedBetweenSeptember2014AndAugust2019()).Returns(true);

        var sut = GetSut();

        await sut.SetRatioText(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().Be(ratiosTextL3PlusNotFrBetweenSep14Aug19);
    }

    [TestMethod]
    public async Task SetRatiosText_IsNotFullAndRelevantAndOutsideOfAug19_ShowsNotApprovedText()
    {
        const string ratiosText = "Approved ratio text";
        const string ratiosTextNotFullAndRelevant = "Not approved";
        const string ratiosTextL3PlusNotFrBetweenSep14Aug19 = "Not approved L3+ between Sep14 and Aug19";
        var ratiosTextDoc = new Document { NodeType = ratiosText };
        var ratiosTextNotFullAndRelevantDoc = new Document { NodeType = ratiosTextNotFullAndRelevant };
        var ratiosTextL3PlusNotFrBetweenSep14Aug19Doc =
            new Document { NodeType = ratiosTextL3PlusNotFrBetweenSep14Aug19 };
        _mockContentParser.Setup(o => o.ToHtml(ratiosTextDoc)).ReturnsAsync(ratiosText);
        _mockContentParser.Setup(o => o.ToHtml(ratiosTextNotFullAndRelevantDoc))
                          .ReturnsAsync(ratiosTextNotFullAndRelevant);
        _mockContentParser.Setup(o => o.ToHtml(ratiosTextL3PlusNotFrBetweenSep14Aug19Doc))
                          .ReturnsAsync(ratiosTextL3PlusNotFrBetweenSep14Aug19);
        var detailsPageContent = new DetailsPage
                                 {
                                     RatiosText = ratiosTextDoc,
                                     RatiosTextNotFullAndRelevant = ratiosTextNotFullAndRelevantDoc,
                                     RatiosTextL3PlusNotFrBetweenSep14Aug19 = ratiosTextL3PlusNotFrBetweenSep14Aug19Doc
                                 };

        var model = new QualificationDetailsModel
                    {
                        QualificationLevel = 3,
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForLevel6 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel2 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel3 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved
                                            },
                        Content = new DetailsPageModel()
                    };

        _mockUserJourneyCookieService.Setup(x => x.WasStartedBetweenSeptember2014AndAugust2019()).Returns(false);

        var sut = GetSut();

        await sut.SetRatioText(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().Be(ratiosTextNotFullAndRelevant);
    }

    [TestMethod]
    public void SetQualificationResultSuccessDetails_ShowsSuccessText()
    {
        var detailsPageContent = new DetailsPage
                                 {
                                     QualificationResultHeading = "Result heading",
                                     QualificationResultFrMessageHeading = "Message heading",
                                     QualificationResultFrMessageBody = "Message body"
                                 };

        var model = new QualificationDetailsModel
                    {
                        Content = new DetailsPageModel()
                    };

        var sut = GetSut();

        sut.SetQualificationResultSuccessDetails(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.QualificationResultHeading.Should().Be(detailsPageContent.QualificationResultHeading);
        model.Content.QualificationResultMessageHeading.Should()
             .Be(detailsPageContent.QualificationResultFrMessageHeading);
        model.Content.QualificationResultMessageBody.Should().Be(detailsPageContent.QualificationResultFrMessageBody);
    }

    [TestMethod]
    public void SetQualificationResultFailureDetails_IsNotFullAndRelevantAndOutsideOfAug19_ShowsCorrectText()
    {
        var detailsPageContent = new DetailsPage
                                 {
                                     QualificationResultHeading = "Result heading",
                                     QualificationResultNotFrMessageHeading = "Message heading",
                                     QualificationResultNotFrMessageBody = "Message body"
                                 };

        var model = new QualificationDetailsModel
                    {
                        QualificationLevel = 3,
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForLevel6 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel2 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel3 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved
                                            },
                        Content = new DetailsPageModel()
                    };

        _mockUserJourneyCookieService.Setup(x => x.WasStartedBetweenSeptember2014AndAugust2019()).Returns(false);

        var sut = GetSut();

        sut.SetQualificationResultFailureDetails(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.QualificationResultHeading.Should().Be(detailsPageContent.QualificationResultHeading);
        model.Content.QualificationResultMessageHeading.Should()
             .Be(detailsPageContent.QualificationResultNotFrMessageHeading);
        model.Content.QualificationResultMessageBody.Should()
             .Be(detailsPageContent.QualificationResultNotFrMessageBody);
    }

    [TestMethod]
    public void SetQualificationResultFailureDetails_IsNotFullAndRelevantAndL3BetweenSep14AndAug19_ShowsCorrectText()
    {
        var detailsPageContent = new DetailsPage
                                 {
                                     QualificationResultHeading = "Result heading",
                                     QualificationResultNotFrL3MessageHeading = "Message heading",
                                     QualificationResultNotFrL3MessageBody = "Message body"
                                 };

        var model = new QualificationDetailsModel
                    {
                        QualificationLevel = 3,
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForLevel6 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel2 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel3 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved
                                            },
                        Content = new DetailsPageModel()
                    };

        _mockUserJourneyCookieService.Setup(x => x.WasStartedBetweenSeptember2014AndAugust2019()).Returns(true);

        var sut = GetSut();

        sut.SetQualificationResultFailureDetails(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.QualificationResultHeading.Should().Be(detailsPageContent.QualificationResultHeading);
        model.Content.QualificationResultMessageHeading.Should()
             .Be(detailsPageContent.QualificationResultNotFrL3MessageHeading);
        model.Content.QualificationResultMessageBody.Should()
             .Be(detailsPageContent.QualificationResultNotFrL3MessageBody);
    }

    [TestMethod]
    public void SetQualificationResultFailureDetails_IsNotFullAndRelevantAndL3BetweenSep14AndAug19_Level_6_ShowsCorrectText()
    {
        var detailsPageContent = new DetailsPage
                                 {
                                     QualificationResultHeading = "Result heading",
                                     QualificationResultNotFrL3OrL6MessageHeading = "Message heading",
                                     QualificationResultNotFrL3OrL6MessageBody = "Message body"
                                 };

        var model = new QualificationDetailsModel
                    {
                        QualificationLevel = 6,
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForLevel6 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel2 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel3 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved
                                            },
                        Content = new DetailsPageModel()
                    };

        _mockUserJourneyCookieService.Setup(x => x.WasStartedBetweenSeptember2014AndAugust2019()).Returns(true);

        var sut = GetSut();

        sut.SetQualificationResultFailureDetails(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.QualificationResultHeading.Should().Be(detailsPageContent.QualificationResultHeading);
        model.Content.QualificationResultMessageHeading.Should()
             .Be(detailsPageContent.QualificationResultNotFrL3OrL6MessageHeading);
        model.Content.QualificationResultMessageBody.Should()
             .Be(detailsPageContent.QualificationResultNotFrL3OrL6MessageBody);
    }

    [TestMethod]
    public async Task SetRatiosText_L2_NotFullAndRelevant_ShowNoText()
    {
        var detailsPageContent = new DetailsPage();

        var model = new QualificationDetailsModel
                    {
                        QualificationLevel = 2,
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForLevel6 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel3 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel2 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved
                                            },
                        Content = new DetailsPageModel()
                    };

        var sut = GetSut();

        await sut.SetRatioText(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().Be(string.Empty);
    }

    [TestMethod]
    public async Task SetRatiosText_IsFullAndRelevantAndL2BeforeJune2016_ShowNoText()
    {
        var detailsPageContent = new DetailsPage();

        var model = new QualificationDetailsModel
                    {
                        QualificationLevel = 2,
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved
                                            },
                        Content = new DetailsPageModel()
                    };

        _mockUserJourneyCookieService.Setup(x => x.WasAwardedBeforeJune2016()).Returns(true);

        var sut = GetSut();

        await sut.SetRatioText(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().Be(string.Empty);
    }

    [TestMethod]
    public async Task SetRatiosText_IsFullAndRelevantAndL2InJune2016_ShowsMayNeedRequirements()
    {
        const string mayNeedPfaText = "May need PFA";
        var mayNeedPfaDoc = new Document { NodeType = mayNeedPfaText };
        _mockContentParser.Setup(o => o.ToHtml(mayNeedPfaDoc)).ReturnsAsync(mayNeedPfaText);

        var detailsPageContent = new DetailsPage
                                 {
                                     RatiosTextMaybePfa = mayNeedPfaDoc
                                 };

        var model = new QualificationDetailsModel
                    {
                        QualificationLevel = 2,
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved
                                            },
                        Content = new DetailsPageModel()
                    };

        _mockUserJourneyCookieService.Setup(x => x.WasAwardedInJune2016()).Returns(true);

        var sut = GetSut();

        await sut.SetRatioText(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().Be(mayNeedPfaText);
    }

    [TestMethod]
    public async Task SetRatiosText_IsFullAndRelevantAndL2AfterJune2016_ShowsWillNeedRequirements()
    {
        const string willNeedPfaText = "Will need PFA";
        var willNeedPfaDoc = new Document { NodeType = willNeedPfaText };
        _mockContentParser.Setup(o => o.ToHtml(willNeedPfaDoc)).ReturnsAsync(willNeedPfaText);

        var detailsPageContent = new DetailsPage
                                 {
                                     RatiosTextPfa = willNeedPfaDoc
                                 };

        var model = new QualificationDetailsModel
                    {
                        QualificationLevel = 2,
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved
                                            },
                        Content = new DetailsPageModel()
                    };

        _mockUserJourneyCookieService.Setup(x => x.WasAwardedAfterJune2016()).Returns(true);

        var sut = GetSut();

        await sut.SetRatioText(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().Be(willNeedPfaText);
    }

    [TestMethod]
    [DataRow(3)]
    [DataRow(4)]
    [DataRow(5)]
    public async Task SetRatiosText_IsFullAndRelevantAwardedBeforeSept2014_ShowsNoText(int level)
    {
        var detailsPageContent = new DetailsPage();

        var model = new QualificationDetailsModel
                    {
                        QualificationLevel = level,
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForLevel3 = QualificationApprovalStatus.Approved,
                                                ApprovedForLevel2 = QualificationApprovalStatus.Approved,
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved
                                            },
                        Content = new DetailsPageModel()
                    };

        _mockUserJourneyCookieService.Setup(x => x.WasAwardedBeforeSeptember2014()).Returns(true);

        var sut = GetSut();

        await sut.SetRatioText(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().Be(string.Empty);
    }

    [TestMethod]
    [DataRow(3)]
    [DataRow(4)]
    [DataRow(5)]
    public async Task SetRatiosText_IsFullAndRelevantAwardedOnOrAfterSept2014_ShowsMayNeedRequirements(int level)
    {
        const string mayNeedPfaText = "May need PFA";
        var mayNeedPfaDoc = new Document { NodeType = mayNeedPfaText };
        _mockContentParser.Setup(o => o.ToHtml(mayNeedPfaDoc)).ReturnsAsync(mayNeedPfaText);

        var detailsPageContent = new DetailsPage
                                 {
                                     RatiosTextMaybePfa = mayNeedPfaDoc
                                 };
        var model = new QualificationDetailsModel
                    {
                        QualificationLevel = level,
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForLevel3 = QualificationApprovalStatus.Approved,
                                                ApprovedForLevel2 = QualificationApprovalStatus.Approved,
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved
                                            },
                        Content = new DetailsPageModel()
                    };

        _mockUserJourneyCookieService.Setup(x => x.WasAwardedOnOrAfterSeptember2014()).Returns(true);

        var sut = GetSut();

        await sut.SetRatioText(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().Be(mayNeedPfaText);
    }

    [TestMethod]
    [DataRow(6)]
    [DataRow(7)]
    public async Task SetRatiosText_IsFullAndRelevantForAllLevels_ShowNoText(int level)
    {
        var detailsPageContent = new DetailsPage();

        var model = new QualificationDetailsModel
                    {
                        QualificationLevel = level,
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForLevel6 = QualificationApprovalStatus.Approved,
                                                ApprovedForLevel3 = QualificationApprovalStatus.Approved,
                                                ApprovedForLevel2 = QualificationApprovalStatus.Approved,
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved
                                            },
                        Content = new DetailsPageModel()
                    };

        var sut = GetSut();

        await sut.SetRatioText(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().Be(string.Empty);
    }

    [TestMethod]
    [DataRow(6)]
    [DataRow(7)]
    public async Task SetRatiosText_IsFullAndRelevantForAllLevelsButL6AwardedBeforeSeptember2014_ShowNoText(int level)
    {
        var detailsPageContent = new DetailsPage();

        var model = new QualificationDetailsModel
                    {
                        QualificationLevel = level,
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForLevel6 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel3 = QualificationApprovalStatus.Approved,
                                                ApprovedForLevel2 = QualificationApprovalStatus.Approved,
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved
                                            },
                        Content = new DetailsPageModel()
                    };

        _mockUserJourneyCookieService.Setup(x => x.WasAwardedBeforeSeptember2014()).Returns(true);
        _mockUserJourneyCookieService.Setup(x => x.WasAwardedOnOrAfterSeptember2014()).Returns(false);

        var sut = GetSut();

        await sut.SetRatioText(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().Be(string.Empty);
    }

    [TestMethod]
    [DataRow(6)]
    [DataRow(7)]
    public async Task SetRatiosText_IsFullAndRelevantForAllLevelsButL6AwardedOnOrAfterSeptember2014_ShowsMayNeedRequirements(int level)
    {
        const string mayNeedPfaText = "May need PFA";
        var mayNeedPfaDoc = new Document { NodeType = mayNeedPfaText };
        _mockContentParser.Setup(o => o.ToHtml(mayNeedPfaDoc)).ReturnsAsync(mayNeedPfaText);

        var detailsPageContent = new DetailsPage
                                 {
                                     RatiosTextMaybePfa = mayNeedPfaDoc
                                 };
        var model = new QualificationDetailsModel
                    {
                        QualificationLevel = level,
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForLevel6 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel3 = QualificationApprovalStatus.Approved,
                                                ApprovedForLevel2 = QualificationApprovalStatus.Approved,
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved
                                            },
                        Content = new DetailsPageModel()
                    };

        _mockUserJourneyCookieService.Setup(x => x.WasAwardedBeforeSeptember2014()).Returns(false);
        _mockUserJourneyCookieService.Setup(x => x.WasAwardedOnOrAfterSeptember2014()).Returns(true);

        var sut = GetSut();

        await sut.SetRatioText(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().Be(mayNeedPfaText);
    }

    [TestMethod]
    [DataRow(3)]
    [DataRow(4)]
    [DataRow(5)]
    [DataRow(6)]
    [DataRow(7)]
    public async Task SetRatiosText_IsNotFullAndRelevantStartedBeforeSeptember2014_NotFandRAndL3EBR(int level)
    {
        const string ratiosTextNotFullAndRelevant = "Not approved";
        var ratiosTextNotFullAndRelevantDoc = new Document { NodeType = ratiosTextNotFullAndRelevant };
        _mockContentParser.Setup(o => o.ToHtml(ratiosTextNotFullAndRelevantDoc))
                          .ReturnsAsync(ratiosTextNotFullAndRelevant);

        const string l3Ebr = "l3 Ebr";
        var l3EbrDoc = new Document { NodeType = l3Ebr };
        _mockContentParser.Setup(o => o.ToHtml(l3EbrDoc))
                          .ReturnsAsync(l3Ebr);
        var detailsPageContent = new DetailsPage
                                 {
                                     RatiosTextNotFullAndRelevant = ratiosTextNotFullAndRelevantDoc,
                                     RatiosTextL3Ebr = l3EbrDoc
                                 };
        var model = new QualificationDetailsModel
                    {
                        QualificationLevel = level,
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForLevel6 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel3 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel2 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved
                                            },
                        Content = new DetailsPageModel()
                    };

        _mockUserJourneyCookieService.Setup(x => x.WasStartedBeforeSeptember2014()).Returns(true);
        _mockUserJourneyCookieService.Setup(x => x.WasStartedOnOrAfterSeptember2019()).Returns(false);

        var sut = GetSut();

        await sut.SetRatioText(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().Be(ratiosTextNotFullAndRelevant);
        model.Content.RatiosAdditionalInfoText.Should().Be(l3Ebr);
    }
    
    [TestMethod]
    [DataRow(3)]
    [DataRow(4)]
    [DataRow(5)]
    [DataRow(6)]
    [DataRow(7)]
    public async Task SetRatiosText_IsNotFullAndRelevantStartedOnOrAfterSeptember2019_NotFandRAndL3EBR(int level)
    {
        const string ratiosTextNotFullAndRelevant = "Not approved";
        var ratiosTextNotFullAndRelevantDoc = new Document { NodeType = ratiosTextNotFullAndRelevant };
        _mockContentParser.Setup(o => o.ToHtml(ratiosTextNotFullAndRelevantDoc))
                          .ReturnsAsync(ratiosTextNotFullAndRelevant);

        const string l3Ebr = "l3 Ebr";
        var l3EbrDoc = new Document { NodeType = l3Ebr };
        _mockContentParser.Setup(o => o.ToHtml(l3EbrDoc))
                          .ReturnsAsync(l3Ebr);
        var detailsPageContent = new DetailsPage
                                 {
                                     RatiosTextNotFullAndRelevant = ratiosTextNotFullAndRelevantDoc,
                                     RatiosTextL3Ebr = l3EbrDoc
                                 };
        var model = new QualificationDetailsModel
                    {
                        QualificationLevel = level,
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForLevel6 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel3 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel2 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved
                                            },
                        Content = new DetailsPageModel()
                    };

        _mockUserJourneyCookieService.Setup(x => x.WasStartedBeforeSeptember2014()).Returns(false);
        _mockUserJourneyCookieService.Setup(x => x.WasStartedOnOrAfterSeptember2019()).Returns(true);

        var sut = GetSut();

        await sut.SetRatioText(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().Be(ratiosTextNotFullAndRelevant);
        model.Content.RatiosAdditionalInfoText.Should().Be(l3Ebr);
    }
    
    [TestMethod]
    [DataRow(3)]
    [DataRow(4)]
    [DataRow(5)]
    [DataRow(6)]
    [DataRow(7)]
    public async Task SetRatiosText_IsNotFullAndRelevantStartedBetweenSeptember2014AndSeptember2019_NotFandRL3AndL3EBR(int level)
    {
        const string ratiosTextNotFullAndRelevantBetweenDates = "Not approved between dates";
        var ratiosTextNotFullAndRelevantBetweenDatesDoc = new Document { NodeType = ratiosTextNotFullAndRelevantBetweenDates };
        _mockContentParser.Setup(o => o.ToHtml(ratiosTextNotFullAndRelevantBetweenDatesDoc))
                          .ReturnsAsync(ratiosTextNotFullAndRelevantBetweenDates);

        const string l3Ebr = "l3 Ebr";
        var l3EbrDoc = new Document { NodeType = l3Ebr };
        _mockContentParser.Setup(o => o.ToHtml(l3EbrDoc))
                          .ReturnsAsync(l3Ebr);
        var detailsPageContent = new DetailsPage
                                 {
                                     RatiosTextL3PlusNotFrBetweenSep14Aug19 = ratiosTextNotFullAndRelevantBetweenDatesDoc,
                                     RatiosTextL3Ebr = l3EbrDoc
                                 };
        var model = new QualificationDetailsModel
                    {
                        QualificationLevel = level,
                        RatioRequirements = new RatioRequirementModel
                                            {
                                                ApprovedForLevel6 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel3 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForLevel2 = QualificationApprovalStatus.NotApproved,
                                                ApprovedForUnqualified = QualificationApprovalStatus.Approved
                                            },
                        Content = new DetailsPageModel()
                    };

        _mockUserJourneyCookieService.Setup(x => x.WasStartedBetweenSeptember2014AndAugust2019()).Returns(true);

        var sut = GetSut();

        await sut.SetRatioText(model, detailsPageContent);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().Be(ratiosTextNotFullAndRelevantBetweenDates);
        model.Content.RatiosAdditionalInfoText.Should().Be(l3Ebr);
    }
}