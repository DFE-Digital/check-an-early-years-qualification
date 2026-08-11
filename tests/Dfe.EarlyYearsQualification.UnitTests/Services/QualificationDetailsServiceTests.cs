using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.QualificationDetails;
using Dfe.EarlyYearsQualification.Web.Services.QualificationSearch;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class QualificationDetailsServiceTests
{
    private Mock<IGovUkContentParser> _mockContentParser = new Mock<IGovUkContentParser>();
    private Mock<IContentService> _mockContentService = new Mock<IContentService>();
    private Mock<ILogger<QualificationDetailsService>> _mockLogger = new Mock<ILogger<QualificationDetailsService>>();
    private Mock<IUserJourneyCookieService> _mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
    private Mock<IPlaceholderUpdater> _mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();
    private Mock<IQualificationDetailsMapper> _mockQualificationDetailsMapper = new Mock<IQualificationDetailsMapper>();
    private Mock<IQualificationSearchService> _mockQualificationSearchService = new Mock<IQualificationSearchService>();

    private QualificationDetailsService GetSut()
    {
        return new QualificationDetailsService(
                                               _mockLogger.Object,
                                               _mockContentService.Object,
                                               _mockContentParser.Object,
                                               _mockUserJourneyCookieService.Object,
                                               _mockPlaceholderUpdater.Object,
                                               _mockQualificationDetailsMapper.Object,
                                               _mockQualificationSearchService.Object
                                              );
    }

    [TestInitialize]
    public void Initialize()
    {
        _mockLogger = new Mock<ILogger<QualificationDetailsService>>();
        _mockContentService = new Mock<IContentService>();
        _mockContentParser = new Mock<IGovUkContentParser>();
        _mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        _mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();
        _mockQualificationDetailsMapper = new Mock<IQualificationDetailsMapper>();
    }

    [TestMethod]
    public async Task GetFilteredQualifications_Calls_SearchService_GetFilteredQualifications()
    {
        _ = await GetSut().GetFilteredQualifications();

        _mockQualificationSearchService.Verify(o => o.GetFilteredQualifications(), Times.Once);
    }
    
    [TestMethod]
    public async Task GetFilteredQualifications_CallsWithOverride_SearchService_GetFilteredQualifications()
    {
        const string searchCriteriaOverride = "override";
        _ = await GetSut().GetFilteredQualifications(searchCriteriaOverride);

        _mockQualificationSearchService.Verify(o => o.GetFilteredQualifications(searchCriteriaOverride), Times.Once);
    }
    
    [TestMethod]
    public async Task GetQualificationById_Calls_SearchService_GetQualification()
    {
        const string qualificationId = "ABC-123";
        _ = await GetSut().GetQualificationById(qualificationId);

        _mockQualificationSearchService.Verify(o => o.GetQualificationById(qualificationId), Times.Once);
    }

    [TestMethod]
    public async Task GetDetailsPage_QualificationIsADegree_GetDetailsPage()
    {
        var sut = GetSut();

        var qualification = new Qualification("TST001", "Qual Name", "Awarding Org", 6)
                            { IsTheQualificationADegree = true };

        _ = await sut.GetQualificationDetailsPage(false, false, 3, 6, 2001, 5, 2019, qualification);

        _mockContentService.Verify(o => o.GetQualificationDetailsPage(false, false, 3, 6, 2001, 5, 2019, true, false),
                                   Times.Once);
    }

    [TestMethod]
    public async Task GetDetailsPage_QualificationIsApprovedAtLevel6_GetDetailsPage()
    {
        var sut = GetSut();

        var qualification = new Qualification("TST001", "Qual Name", "Awarding Org", 6)
                            { IsAutomaticallyApprovedAtLevel6 = true };

        _ = await sut.GetQualificationDetailsPage(false, false, 3, 6, 2001, 5, 2019, qualification);

        _mockContentService.Verify(o => o.GetQualificationDetailsPage(false, false, 3, 6, 2001, 5, 2019,false, true),
                                   Times.Once);
    }

    [TestMethod]
    public async Task GetDetailsPage_QualificationIsNotADegree_GetDetailsPage()
    {
        var sut = GetSut();

        var qualification = new Qualification("TST001", "Qual Name", "Awarding Org", 3);

        _ = await sut.GetQualificationDetailsPage(false, false, 3, 6, 2001, 5, 2019, qualification);

        _mockContentService.Verify(o => o.GetQualificationDetailsPage(false, false, 3, 6, 2001, 5, 2019, false, false),
                                   Times.Once);
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
    [DataRow(true, "no", true)]
    [DataRow(true, "yes", false)]
    [DataRow(false, "no", false)]
    [DataRow(false, "yes", true)]
    public void AnswersIndicateNotFullAndRelevant(bool fullAndRelevant, string answer, bool expectedResult)
    {
        var additionalRequirementsAnswers = new List<AdditionalRequirementAnswerModel>
                                            {
                                                new AdditionalRequirementAnswerModel
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
                                                {
                                                    new AdditionalRequirementAnswerModel
                                                    { Question = qts.Question, Answer = answer }
                                                };
        var sut = GetSut();

        var result =
            sut.UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(qualification, additionalRequirementAnswerModels);

        result.Should().Be(expectedResult);
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
                                                 new AdditionalRequirementQuestion
                                                 {
                                                     Question = "Question 1",
                                                     AnswerToBeFullAndRelevant = true,
                                                     ConfirmationStatement = "confirmation statement 1"
                                                 },
                                                 new AdditionalRequirementQuestion
                                                 {
                                                     Question = "Question 2",
                                                     AnswerToBeFullAndRelevant = false,
                                                     ConfirmationStatement = "confirmation statement 2"
                                                 },
                                                 new AdditionalRequirementQuestion
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
                           new AdditionalRequirementAnswerModel
                           {
                               Question = "Question 1",
                               AnswerToBeFullAndRelevant = true,
                               ConfirmationStatement = "confirmation statement 1",
                               Answer = "Answer 1"
                           },
                           new AdditionalRequirementAnswerModel
                           {
                               Question = "Question 2",
                               AnswerToBeFullAndRelevant = false,
                               ConfirmationStatement = "confirmation statement 2",
                               Answer = "Answer 2"
                           },
                           new AdditionalRequirementAnswerModel
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
        const string requirements = "requirements";
        const bool hasMultipleQualificationsWithSameName = false;
        const bool isFullAndRelevant = true;
        var requirementsText = new Document { NodeType = requirements };
        var backButton = new NavigationLink { Href = "backButton" };
        var qualification =
            new Qualification(qualificationId, qualificationName, awardingOrganisationTitle, qualificationLevel)
            { FromWhichYear = "FromWhichYear" };
        var detailsPage = new QualificationDetailsPage
                          {
                              RequirementsText = requirementsText,
                              Labels = new DetailsPageLabels
                                       {
                                           BackButton = backButton
                                       }
                          };

        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((startMonth, startYear));
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationAwarded()).Returns((awardMonth, awardYear));

        _mockQualificationSearchService.Setup(x => x.GetFilteredQualifications(It.IsAny<string>())).ReturnsAsync(new List<Qualification>());
        _mockQualificationDetailsMapper
            .Setup(x => x.Map(qualification, detailsPage, backButton,
                              It.IsAny<List<AdditionalRequirementAnswerModel>>(), dateStarted, dateAwarded,
                              hasMultipleQualificationsWithSameName, isFullAndRelevant))
            .ReturnsAsync(new QualificationDetailsModel());

        var sut = GetSut();
        var result = await sut.MapDetails(qualification, detailsPage, isFullAndRelevant, null);

        result.Should().NotBeNull();
        _mockQualificationDetailsMapper.Verify(x => x.Map(qualification, detailsPage, backButton,
                                                          It.IsAny<List<AdditionalRequirementAnswerModel>>(),
                                                          dateStarted, dateAwarded, hasMultipleQualificationsWithSameName, isFullAndRelevant),
                                               Times.Once);
    }

    [TestMethod]
    public async Task MapDetails_StartDateBeforeSeptember2014_PassesBeforeString()
    {
        const string qualificationId = "qualificationId";
        const string qualificationName = "qualificationName";
        const string awardingOrganisationTitle = "awardingOrganisationTitle";
        const int qualificationLevel = 1;
        const bool hasMultipleQualificationsWithSameName = false;
        const bool isFullAndRelevant = true;
        var backButton = new NavigationLink { Href = "backButton" };
        var qualification =
            new Qualification(qualificationId, qualificationName, awardingOrganisationTitle, qualificationLevel)
            { FromWhichYear = "FromWhichYear" };
        var detailsPage = new QualificationDetailsPage
                          {
                              RequirementsText = new Document(),
                              Labels = new DetailsPageLabels
                                       {
                                           BackButton = backButton
                                       }
                          };

        // Start date: August 2014 => before September 2014
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((8, 2014));
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationAwarded()).Returns((null, null));
        _mockQualificationSearchService.Setup(x => x.GetFilteredQualifications(It.IsAny<string>())).ReturnsAsync(new List<Qualification>());
        _mockQualificationDetailsMapper
            .Setup(x => x.Map(qualification, detailsPage, backButton,
                              It.IsAny<List<AdditionalRequirementAnswerModel>>(), "Before 1 September 2014", string.Empty,
                              hasMultipleQualificationsWithSameName, isFullAndRelevant))
            .ReturnsAsync(new QualificationDetailsModel());

        var sut = GetSut();

        var result = await sut.MapDetails(qualification, detailsPage, isFullAndRelevant, null);

        result.Should().NotBeNull();
        _mockQualificationDetailsMapper.Verify(x => x.Map(qualification, detailsPage, backButton,
                                                          It.IsAny<List<AdditionalRequirementAnswerModel>>(),
                                                          "Before 1 September 2014", string.Empty, 
                                                          hasMultipleQualificationsWithSameName, isFullAndRelevant),
                                               Times.Once);
    }

    [TestMethod]
    public async Task MapDetails_PassesAdditionalRequirementAnswersToMapper()
    {
        const string qualificationId = "qualificationId";
        const string qualificationName = "qualificationName";
        const string awardingOrganisationTitle = "awardingOrganisationTitle";
        const int qualificationLevel = 1;
        const bool hasMultipleQualificationsWithSameName = false;
        const bool isFullAndRelevant = true;
        var backButton = new NavigationLink { Href = "backButton" };

        var q1 = new AdditionalRequirementQuestion
                 {
                     Question = "Q1",
                     AnswerToBeFullAndRelevant = true,
                     ConfirmationStatement = "confirm"
                 };

        var qualification = new Qualification(qualificationId, qualificationName, awardingOrganisationTitle, qualificationLevel)
                            { AdditionalRequirementQuestions = [ q1 ] };

        var detailsPage = new QualificationDetailsPage
                          {
                              RequirementsText = new Document(),
                              Labels = new DetailsPageLabels
                                       {
                                           BackButton = backButton
                                       }
                          };

        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((null, null));
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationAwarded()).Returns((null, null));

        var userAnswers = new Dictionary<string, string> { { "Q1", "yes" } };
        _mockUserJourneyCookieService.Setup(o => o.GetAdditionalQuestionsAnswers()).Returns(userAnswers);

        _mockQualificationSearchService.Setup(x => x.GetFilteredQualifications(It.IsAny<string>())).ReturnsAsync(new List<Qualification>());
        _mockQualificationDetailsMapper
            .Setup(x => x.Map(It.IsAny<Qualification>(), It.IsAny<QualificationDetailsPage>(), It.IsAny<NavigationLink?>(),
                              It.IsAny<List<AdditionalRequirementAnswerModel>>(), It.IsAny<string>(), It.IsAny<string>(),
                              hasMultipleQualificationsWithSameName, isFullAndRelevant))
            .ReturnsAsync(new QualificationDetailsModel());
        
        var additionalRequirementAnswerModels = new List<AdditionalRequirementAnswerModel>
                                             {
                                                 new AdditionalRequirementAnswerModel
                                                 {
                                                     Answer = "yes",
                                                     Question = "Q1"
                                                 }
                                             };

        var sut = GetSut();

        var result = await sut.MapDetails(qualification, detailsPage, isFullAndRelevant, additionalRequirementAnswerModels);

        result.Should().NotBeNull();

        _mockQualificationDetailsMapper.Verify(x => x.Map(
            qualification,
            detailsPage,
            backButton,
            It.Is<List<AdditionalRequirementAnswerModel>>(list => list.Count == 1 && list[0].Question == "Q1" && list[0].Answer == "yes"),
            It.IsAny<string>(),
            It.IsAny<string>(),
            hasMultipleQualificationsWithSameName, isFullAndRelevant), Times.Once);
    }

    [TestMethod]
    public async Task SetRatiosText_IsFullAndRelevant_ShowsApprovedText()
    {
        const string ratiosTextNotFullAndRelevant = "Not approved";
        const string ratiosTextL3PlusNotFrBetweenSep14Aug19 = "Not approved L3+ between Sep14 and Aug19";
        var ratiosTextNotFullAndRelevantDoc = new Document { NodeType = ratiosTextNotFullAndRelevant };
        var ratiosTextL3PlusNotFrBetweenSep14Aug19Doc =
            new Document { NodeType = ratiosTextL3PlusNotFrBetweenSep14Aug19 };
        _mockContentParser.Setup(o => o.ToHtml(ratiosTextNotFullAndRelevantDoc))
                          .ReturnsAsync(ratiosTextNotFullAndRelevant);
        _mockContentParser.Setup(o => o.ToHtml(ratiosTextL3PlusNotFrBetweenSep14Aug19Doc))
                          .ReturnsAsync(ratiosTextL3PlusNotFrBetweenSep14Aug19);
        var detailsPageContent = new QualificationDetailsPage
                                 {
                                     Labels = new DetailsPageLabels
                                              {
                                                  RatiosTextNotFullAndRelevant = ratiosTextNotFullAndRelevantDoc,
                                                  RatiosTextL3PlusNotFrBetweenSep14Aug19 =
                                                      ratiosTextL3PlusNotFrBetweenSep14Aug19Doc
                                              }
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

        await sut.SetRatioText(model, detailsPageContent.Labels);

        model.Content.Should().NotBeNull();
    }

    [TestMethod]
    public async Task SetRatiosText_IsNotFullAndRelevantAndOutsideOfAug19_ShowsNotApprovedText()
    {
        const string ratiosTextNotFullAndRelevant = "Not approved";
        const string ratiosTextL3PlusNotFrBetweenSep14Aug19 = "Not approved L3+ between Sep14 and Aug19";
        var ratiosTextNotFullAndRelevantDoc = new Document { NodeType = ratiosTextNotFullAndRelevant };
        var ratiosTextL3PlusNotFrBetweenSep14Aug19Doc =
            new Document { NodeType = ratiosTextL3PlusNotFrBetweenSep14Aug19 };
        _mockContentParser.Setup(o => o.ToHtml(ratiosTextNotFullAndRelevantDoc))
                          .ReturnsAsync(ratiosTextNotFullAndRelevant);
        _mockContentParser.Setup(o => o.ToHtml(ratiosTextL3PlusNotFrBetweenSep14Aug19Doc))
                          .ReturnsAsync(ratiosTextL3PlusNotFrBetweenSep14Aug19);
        var detailsPageContent = new QualificationDetailsPage
                                 {
                                     Labels = new DetailsPageLabels
                                              {
                                                  RatiosTextNotFullAndRelevant = ratiosTextNotFullAndRelevantDoc,
                                                  RatiosTextL3PlusNotFrBetweenSep14Aug19 =
                                                      ratiosTextL3PlusNotFrBetweenSep14Aug19Doc
                                              }
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

        await sut.SetRatioText(model, detailsPageContent.Labels);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().Be(ratiosTextNotFullAndRelevant);
    }

    [TestMethod]
    public void SetQualificationResultSuccessDetails_ShowsSuccessText()
    {
        var detailsPageContent = new QualificationDetailsPage
                                 {
                                     Labels = new DetailsPageLabels
                                              {
                                                  QualificationResultHeading = "Result heading",
                                                  QualificationResultFrMessageHeading = "Message heading",
                                                  QualificationResultFrMessageBody = "Message body"
                                              }
                                 };

        var model = new QualificationDetailsModel
                    {
                        Content = new DetailsPageModel()
                    };

        var sut = GetSut();

        sut.SetQualificationResultSuccessDetails(model, detailsPageContent.Labels);

        model.Content.Should().NotBeNull();
        model.Content.QualificationResultHeading.Should().Be(detailsPageContent.Labels.QualificationResultHeading);
        model.Content.QualificationResultMessageHeading.Should()
             .Be(detailsPageContent.Labels.QualificationResultFrMessageHeading);
        model.Content.QualificationResultMessageBody.Should()
             .Be(detailsPageContent.Labels.QualificationResultFrMessageBody);
    }

    [TestMethod]
    public void SetQualificationResultFailureDetails_IsNotFullAndRelevantAndOutsideOfAug19_ShowsCorrectText()
    {
        var detailsPageContent = new QualificationDetailsPage
                                 {
                                     Labels = new DetailsPageLabels
                                              {
                                                  QualificationResultHeading = "Result heading",
                                                  QualificationResultNotFrMessageHeading = "Message heading",
                                                  QualificationResultNotFrMessageBody = "Message body"
                                              }
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

        sut.SetQualificationResultFailureDetails(model, detailsPageContent.Labels);

        model.Content.Should().NotBeNull();
        model.Content.QualificationResultHeading.Should().Be(detailsPageContent.Labels.QualificationResultHeading);
        model.Content.QualificationResultMessageHeading.Should()
             .Be(detailsPageContent.Labels.QualificationResultNotFrMessageHeading);
        model.Content.QualificationResultMessageBody.Should()
             .Be(detailsPageContent.Labels.QualificationResultNotFrMessageBody);
    }

    [TestMethod]
    public void SetQualificationResultFailureDetails_IsNotFullAndRelevantAndL3BetweenSep14AndAug19_ShowsCorrectText()
    {
        var detailsPageContent = new QualificationDetailsPage
                                 {
                                     Labels = new DetailsPageLabels
                                              {
                                                  QualificationResultHeading = "Result heading",
                                                  QualificationResultNotFrL3MessageHeading = "Message heading",
                                                  QualificationResultNotFrL3MessageBody = "Message body"
                                              }
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

        sut.SetQualificationResultFailureDetails(model, detailsPageContent.Labels);

        model.Content.Should().NotBeNull();
        model.Content.QualificationResultHeading.Should().Be(detailsPageContent.Labels.QualificationResultHeading);
        model.Content.QualificationResultMessageHeading.Should()
             .Be(detailsPageContent.Labels.QualificationResultNotFrL3MessageHeading);
        model.Content.QualificationResultMessageBody.Should()
             .Be(detailsPageContent.Labels.QualificationResultNotFrL3MessageBody);
    }

    [TestMethod]
    public void
        SetQualificationResultFailureDetails_IsNotFullAndRelevantAndL3BetweenSep14AndAug19_Level_6_ShowsCorrectText()
    {
        var detailsPageContent = new QualificationDetailsPage
                                 {
                                     Labels = new DetailsPageLabels
                                              {
                                                  QualificationResultHeading = "Result heading",
                                                  QualificationResultNotFrL3OrL6MessageHeading = "Message heading",
                                                  QualificationResultNotFrL3OrL6MessageBody = "Message body"
                                              }
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

        sut.SetQualificationResultFailureDetails(model, detailsPageContent.Labels);

        model.Content.Should().NotBeNull();
        model.Content.QualificationResultHeading.Should().Be(detailsPageContent.Labels.QualificationResultHeading);
        model.Content.QualificationResultMessageHeading.Should()
             .Be(detailsPageContent.Labels.QualificationResultNotFrL3OrL6MessageHeading);
        model.Content.QualificationResultMessageBody.Should()
             .Be(detailsPageContent.Labels.QualificationResultNotFrL3OrL6MessageBody);
    }

    [TestMethod]
    public async Task SetRatiosText_L2_NotFullAndRelevant_ShowNotFullAndRelevantText()
    {
        const string ratiosTextNotFullAndRelevant = "Not approved";
        var ratiosTextNotFullAndRelevantDoc = new Document { NodeType = ratiosTextNotFullAndRelevant };
        _mockContentParser.Setup(o => o.ToHtml(ratiosTextNotFullAndRelevantDoc))
                          .ReturnsAsync(ratiosTextNotFullAndRelevant);
        var detailsPageContent = new QualificationDetailsPage
                                 {
                                     Labels = new DetailsPageLabels
                                              {
                                                  RatiosTextNotFullAndRelevant = ratiosTextNotFullAndRelevantDoc,
                                              }
                                 };

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

        await sut.SetRatioText(model, detailsPageContent.Labels);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().Be(ratiosTextNotFullAndRelevant);
    }

    [TestMethod]
    public async Task SetRatiosText_IsFullAndRelevantAndL2BeforeJune2016_ShowNoText()
    {
        var detailsPageContent = new QualificationDetailsPage();

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

        await sut.SetRatioText(model, detailsPageContent.Labels);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().BeNull();
    }

    [TestMethod]
    [DataRow(3)]
    [DataRow(4)]
    [DataRow(5)]
    public async Task SetRatiosText_IsFullAndRelevantAwardedBeforeSept2014_ShowsNoText(int level)
    {
        var detailsPageContent = new QualificationDetailsPage();

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

        await sut.SetRatioText(model, detailsPageContent.Labels);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().BeNull();
    }

    [TestMethod]
    [DataRow(6)]
    [DataRow(7)]
    public async Task SetRatiosText_IsFullAndRelevantForAllLevels_ShowNoText(int level)
    {
        var detailsPageContent = new QualificationDetailsPage();

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

        await sut.SetRatioText(model, detailsPageContent.Labels);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().BeNull();
    }

    [TestMethod]
    [DataRow(6)]
    [DataRow(7)]
    public async Task SetRatiosText_IsFullAndRelevantForAllLevelsButL6AwardedBeforeSeptember2014_ShowNoText(int level)
    {
        var detailsPageContent = new QualificationDetailsPage();

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

        await sut.SetRatioText(model, detailsPageContent.Labels);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().BeNull();
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
        var detailsPageContent = new QualificationDetailsPage
                                 {
                                     Labels = new DetailsPageLabels
                                              {
                                                  RatiosTextNotFullAndRelevant = ratiosTextNotFullAndRelevantDoc,
                                                  RatiosTextL3Ebr = l3EbrDoc
                                              }
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

        await sut.SetRatioText(model, detailsPageContent.Labels);

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
        var detailsPageContent = new QualificationDetailsPage
                                 {
                                     Labels = new DetailsPageLabels
                                              {
                                                  RatiosTextNotFullAndRelevant = ratiosTextNotFullAndRelevantDoc,
                                                  RatiosTextL3Ebr = l3EbrDoc
                                              }
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

        await sut.SetRatioText(model, detailsPageContent.Labels);

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
    public async Task
        SetRatiosText_IsNotFullAndRelevantStartedBetweenSeptember2014AndSeptember2019_NotFandRL3AndL3EBR(int level)
    {
        const string ratiosTextNotFullAndRelevantBetweenDates = "Not approved between dates";
        var ratiosTextNotFullAndRelevantBetweenDatesDoc =
            new Document { NodeType = ratiosTextNotFullAndRelevantBetweenDates };
        _mockContentParser.Setup(o => o.ToHtml(ratiosTextNotFullAndRelevantBetweenDatesDoc))
                          .ReturnsAsync(ratiosTextNotFullAndRelevantBetweenDates);

        const string l3Ebr = "l3 Ebr";
        var l3EbrDoc = new Document { NodeType = l3Ebr };
        _mockContentParser.Setup(o => o.ToHtml(l3EbrDoc))
                          .ReturnsAsync(l3Ebr);
        var detailsPageContent = new QualificationDetailsPage
                                 {
                                     Labels = new DetailsPageLabels
                                              {
                                                  RatiosTextL3PlusNotFrBetweenSep14Aug19 =
                                                      ratiosTextNotFullAndRelevantBetweenDatesDoc,
                                                  RatiosTextL3Ebr = l3EbrDoc
                                              }
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

        await sut.SetRatioText(model, detailsPageContent.Labels);

        model.Content.Should().NotBeNull();
        model.Content.RatiosText.Should().Be(ratiosTextNotFullAndRelevantBetweenDates);
        model.Content.RatiosAdditionalInfoText.Should().Be(l3Ebr);
    }

    [TestMethod]
    [DataRow(Options.Yes, true)]
    [DataRow(Options.No, false)]
    public void GetUserIsCheckingOwnQualification_Calls_UserJourneyCookieService_GetIsUserCheckingTheirOwnQualification(
        string input, bool expected)
    {
        _mockUserJourneyCookieService.Setup(o => o.GetIsUserCheckingTheirOwnQualification())
                                     .Returns(input);

        var result = GetSut().GetUserIsCheckingOwnQualification();

        result.Should().Be(expected);
        _mockUserJourneyCookieService.Verify(o => o.GetIsUserCheckingTheirOwnQualification(), Times.Once);
    }

    [TestMethod]
    public void GetLevelOfQualification_Calls_UserJourneyCookieService_GetLevelOfQualification()
    {
        _ = GetSut().GetLevelOfQualification();

        _mockUserJourneyCookieService.Verify(o => o.GetLevelOfQualification(), Times.Once);
    }

    [TestMethod]
    public void GetWhenWasQualificationStarted_Calls_UserJourneyCookieService_GetWhenWasQualificationStarted()
    {
        _ = GetSut().GetWhenWasQualificationStarted();

        _mockUserJourneyCookieService.Verify(o => o.GetWhenWasQualificationStarted(), Times.Once);
    }
}