using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.FeedbackForm;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class FeedbackFormServiceTests
{
    [TestMethod]
    public void ValidateQuestions_PassInMandatoryQuestions_NotAnswered_ReturnsErrorLinks()
    {
        var feedbackFormPage = CreateFeedbackFormPageModel(false);
        var questionList = new List<FeedbackFormQuestionListModel>
                           {
                               new FeedbackFormQuestionListModel
                               {
                                   Question = (feedbackFormPage.Questions[0] as BaseFeedbackFormQuestion)!.Question
                               },
                               new FeedbackFormQuestionListModel
                               {
                                   Question = (feedbackFormPage.Questions[1] as BaseFeedbackFormQuestion)!.Question
                               },
                               new FeedbackFormQuestionListModel
                               {
                                   Question = (feedbackFormPage.Questions[2] as BaseFeedbackFormQuestion)!.Question
                               }
                           };
        var model = CreateFeedbackFormPageModel(questionList);
        
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        
        var service = new FeedbackFormService(mockUserJourneyCookieService.Object);
        
        var result = service.ValidateQuestions(feedbackFormPage, model);
        
        result.Should().NotBeNull();
        result.ErrorBannerHeading.Should().Match(feedbackFormPage.ErrorBannerHeading);
        result.ErrorSummaryLinks.Should().NotBeNull();
        result.ErrorSummaryLinks.Count.Should().Be(3);
        
        var radioButtonErrorSummaryLink = result.ErrorSummaryLinks[0];
        radioButtonErrorSummaryLink.Should().NotBeNull();
        radioButtonErrorSummaryLink.ElementLinkId.Should().Be("0_yes");
        radioButtonErrorSummaryLink.ErrorBannerLinkText.Should().Be((feedbackFormPage.Questions[0] as FeedbackFormQuestionRadio)!.ErrorMessage);
        model.QuestionList[0].HasError.Should().BeTrue();
        model.QuestionList[0].ErrorMessage.Should().Match(radioButtonErrorSummaryLink.ErrorBannerLinkText);
        
        var textAreaErrorSummaryLink = result.ErrorSummaryLinks[1];
        textAreaErrorSummaryLink.Should().NotBeNull();
        textAreaErrorSummaryLink.ElementLinkId.Should().Be("1_textArea");
        textAreaErrorSummaryLink.ErrorBannerLinkText.Should().Be((feedbackFormPage.Questions[1] as FeedbackFormQuestionTextArea)!.ErrorMessage);
        model.QuestionList[1].HasError.Should().BeTrue();
        model.QuestionList[1].ErrorMessage.Should().Match(textAreaErrorSummaryLink.ErrorBannerLinkText);
        
        var radioAndInputErrorSummaryLink = result.ErrorSummaryLinks[2];
        radioAndInputErrorSummaryLink.Should().NotBeNull();
        radioAndInputErrorSummaryLink.ElementLinkId.Should().Be("2_yes");
        radioAndInputErrorSummaryLink.ErrorBannerLinkText.Should().Be((feedbackFormPage.Questions[2] as FeedbackFormQuestionRadioAndInput)!.ErrorMessage);
        model.QuestionList[2].HasError.Should().BeTrue();
        model.QuestionList[2].ErrorMessage.Should().Match(radioAndInputErrorSummaryLink.ErrorBannerLinkText);
        
        mockUserJourneyCookieService.Verify(x => x.SetHasSubmittedEmailAddressInFeedbackFormQuestion(false), Times.Once);
    }
    
    [TestMethod]
    public void ValidateQuestions_AdditionalInfoNotSet_ReturnsErrorLinks()
    {
        var feedbackFormPage = CreateFeedbackFormPageModel(false);
        var questionList = new List<FeedbackFormQuestionListModel>
                           {
                               new FeedbackFormQuestionListModel
                               {
                                   Question = (feedbackFormPage.Questions[0] as BaseFeedbackFormQuestion)!.Question,
                                   Answer = "yes"
                               },
                               new FeedbackFormQuestionListModel
                               {
                                   Question = (feedbackFormPage.Questions[1] as BaseFeedbackFormQuestion)!.Question,
                                   Answer = "text"
                               },
                               new FeedbackFormQuestionListModel
                               {
                                   Question = (feedbackFormPage.Questions[2] as BaseFeedbackFormQuestion)!.Question,
                                   Answer = "yes"
                               }
                           };
        var model = CreateFeedbackFormPageModel(questionList);
        
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        
        var service = new FeedbackFormService(mockUserJourneyCookieService.Object);
        
        var result = service.ValidateQuestions(feedbackFormPage, model);
        
        result.Should().NotBeNull();
        result.ErrorBannerHeading.Should().Match(feedbackFormPage.ErrorBannerHeading);
        result.ErrorSummaryLinks.Should().NotBeNull();
        result.ErrorSummaryLinks.Count.Should().Be(1);
        
        var radioAndInputErrorSummaryLink = result.ErrorSummaryLinks[0];
        radioAndInputErrorSummaryLink.Should().NotBeNull();
        radioAndInputErrorSummaryLink.ElementLinkId.Should().Be("2_additionalInfo");
        radioAndInputErrorSummaryLink.ErrorBannerLinkText.Should().Be((feedbackFormPage.Questions[2] as FeedbackFormQuestionRadioAndInput)!.ErrorMessageForInput);
        model.QuestionList[2].HasError.Should().BeTrue();
        model.QuestionList[2].ErrorMessage.Should().Match(radioAndInputErrorSummaryLink.ErrorBannerLinkText);
        
        mockUserJourneyCookieService.Verify(x => x.SetHasSubmittedEmailAddressInFeedbackFormQuestion(false), Times.Once);
    }
    
    [TestMethod]
    public void ValidateQuestions_AdditionalInfoInvalidFormat_ReturnsErrorLinks()
    {
        var feedbackFormPage = CreateFeedbackFormPageModel(false);
        var questionList = new List<FeedbackFormQuestionListModel>
                           {
                               new FeedbackFormQuestionListModel
                               {
                                   Question = (feedbackFormPage.Questions[0] as BaseFeedbackFormQuestion)!.Question,
                                   Answer = "yes"
                               },
                               new FeedbackFormQuestionListModel
                               {
                                   Question = (feedbackFormPage.Questions[1] as BaseFeedbackFormQuestion)!.Question,
                                   Answer = "text"
                               },
                               new FeedbackFormQuestionListModel
                               {
                                   Question = (feedbackFormPage.Questions[2] as BaseFeedbackFormQuestion)!.Question,
                                   Answer = "yes",
                                   AdditionalInfo = "testing"
                               }
                           };
        var model = CreateFeedbackFormPageModel(questionList);
        
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        
        var service = new FeedbackFormService(mockUserJourneyCookieService.Object);
        
        var result = service.ValidateQuestions(feedbackFormPage, model);
        
        result.Should().NotBeNull();
        result.ErrorBannerHeading.Should().Match(feedbackFormPage.ErrorBannerHeading);
        result.ErrorSummaryLinks.Should().NotBeNull();
        result.ErrorSummaryLinks.Count.Should().Be(1);
        
        var radioAndInputErrorSummaryLink = result.ErrorSummaryLinks[0];
        radioAndInputErrorSummaryLink.Should().NotBeNull();
        radioAndInputErrorSummaryLink.ElementLinkId.Should().Be("2_additionalInfo");
        radioAndInputErrorSummaryLink.ErrorBannerLinkText.Should().Be((feedbackFormPage.Questions[2] as FeedbackFormQuestionRadioAndInput)!.ErrorMessageForInvalidEmailFormat);
        model.QuestionList[2].HasError.Should().BeTrue();
        model.QuestionList[2].ErrorMessage.Should().Match(radioAndInputErrorSummaryLink.ErrorBannerLinkText);
        
        mockUserJourneyCookieService.Verify(x => x.SetHasSubmittedEmailAddressInFeedbackFormQuestion(false), Times.Once);
    }
    
    [TestMethod]
    public void ValidateQuestions_AnsweredWouldYouLikeToBeContactedAboutResearchQuestion_SetsCookieValueToTrue()
    {
        var feedbackFormPage = CreateFeedbackFormPageModel(true);
        var questionList = new List<FeedbackFormQuestionListModel>
                           {
                               new FeedbackFormQuestionListModel
                               {
                                   Question = (feedbackFormPage.Questions[0] as BaseFeedbackFormQuestion)!.Question,
                                   Answer = "yes"
                               },
                               new FeedbackFormQuestionListModel
                               {
                                   Question = (feedbackFormPage.Questions[1] as BaseFeedbackFormQuestion)!.Question,
                                   Answer = "text"
                               },
                               new FeedbackFormQuestionListModel
                               {
                                   Question = (feedbackFormPage.Questions[2] as BaseFeedbackFormQuestion)!.Question,
                                   Answer = "yes",
                                   AdditionalInfo = "testing@test.com"
                               }
                           };
        var model = CreateFeedbackFormPageModel(questionList);
        
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        
        var service = new FeedbackFormService(mockUserJourneyCookieService.Object);
        
        var result = service.ValidateQuestions(feedbackFormPage, model);
        
        result.Should().NotBeNull();
        result.ErrorBannerHeading.Should().Match(feedbackFormPage.ErrorBannerHeading);
        result.ErrorSummaryLinks.Should().BeEmpty();
        
        mockUserJourneyCookieService.Verify(x => x.SetHasSubmittedEmailAddressInFeedbackFormQuestion(true), Times.Once);
    }

    [TestMethod]
    public void ConvertQuestionListToString_PassInModel_ReturnsExpectedResult()
    {
        const string question = "Question";
        const string answer = "Answer";
        const string additionalInfo = "AdditionalInfo";

        const string expectedResult = $"""
            ## {question}
            {answer}
            {additionalInfo}
            
            ---
            
            """;
        
        var model = new FeedbackFormPageModel
                    {
                        Heading = "",
                        CtaButtonText = "",
                        ErrorBannerHeading = "",
                        QuestionList =
                        [
                            new FeedbackFormQuestionListModel
                            {
                                Question = question,
                                Answer = answer,
                                AdditionalInfo = additionalInfo
                            }
                        ]
                    };
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var service = new FeedbackFormService(mockUserJourneyCookieService.Object);
        var result = service.ConvertQuestionListToString(model);
        result.Should().NotBeNull();
        result.Should().Match(expectedResult);
    }
    
    [TestMethod]
    public void ConvertQuestionListToString_NoAdditionalInfo_ReturnsExpectedResult()
    {
        const string question = "Question";
        const string answer = "Answer";

        const string expectedResult = $"""
            ## {question}
            {answer}
            
            ---
            
            """;
        
        var model = new FeedbackFormPageModel
                    {
                        Heading = "",
                        CtaButtonText = "",
                        ErrorBannerHeading = "",
                        QuestionList =
                        [
                            new FeedbackFormQuestionListModel
                            {
                                Question = question,
                                Answer = answer
                            }
                        ]
                    };
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var service = new FeedbackFormService(mockUserJourneyCookieService.Object);
        var result = service.ConvertQuestionListToString(model);
        result.Should().NotBeNull();
        result.Should().Match(expectedResult);
    }

    [TestMethod]
    public void SetDefaultAnswers_CookieValueIsNull_AnswerNotSet()
    {
        var feedbackFormPage = CreateFeedbackFormPageModel(true);
        var questionList = new List<FeedbackFormQuestionListModel>
                           {
                               new FeedbackFormQuestionListModel
                               {
                                   Question = (feedbackFormPage.Questions[0] as BaseFeedbackFormQuestion)!.Question
                               }
                           };
        var model = CreateFeedbackFormPageModel(questionList);
        
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        mockUserJourneyCookieService.Setup(x => x.GetHasUserGotEverythingTheyNeededToday()).Returns(string.Empty);
        
        var service = new FeedbackFormService(mockUserJourneyCookieService.Object);
        
        service.SetDefaultAnswers(feedbackFormPage, model);
        
        model.QuestionList.First().Answer.Should().BeNullOrEmpty();
    }
    
    [TestMethod]
    public void SetDefaultAnswers_CookieValueIsSet_QuestionsListDoesntIncludeQuestion_AnswerNotSet()
    {
        var feedbackFormPage = CreateFeedbackFormPageModel(true);
        var questionList = new List<FeedbackFormQuestionListModel>
                           {
                               new FeedbackFormQuestionListModel
                               {
                                   Question = "Test question doesn't match the required question"
                               }
                           };
        var model = CreateFeedbackFormPageModel(questionList);
        
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        mockUserJourneyCookieService.Setup(x => x.GetHasUserGotEverythingTheyNeededToday()).Returns("yes");
        
        var service = new FeedbackFormService(mockUserJourneyCookieService.Object);
        
        service.SetDefaultAnswers(feedbackFormPage, model);
        
        model.QuestionList.First().Answer.Should().BeNullOrEmpty();
    }
    
    [TestMethod]
    public void SetDefaultAnswers_CookieValueIsSet_QuestionsListContainsQuestion_ModelValueChanged()
    {
        const string response = "yes";
        var feedbackFormPage = CreateFeedbackFormPageModel(true);
        var questionList = new List<FeedbackFormQuestionListModel>
                           {
                               new FeedbackFormQuestionListModel
                               {
                                   Question = (feedbackFormPage.Questions[0] as BaseFeedbackFormQuestion)!.Question
                               }
                           };
        var model = CreateFeedbackFormPageModel(questionList);
        
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        mockUserJourneyCookieService.Setup(x => x.GetHasUserGotEverythingTheyNeededToday()).Returns(response);
        
        var service = new FeedbackFormService(mockUserJourneyCookieService.Object);
        
        service.SetDefaultAnswers(feedbackFormPage, model);
        
        model.QuestionList.First().Answer.Should().Be(response);
    }

    private static FeedbackFormPage CreateFeedbackFormPageModel(bool setSystemId)
    {
        return new FeedbackFormPage
               {
                   BackButton = new NavigationLink(),
                   CtaButtonText = "Continue",
                   ErrorBannerHeading = "Error banner heading",
                   Heading = "Heading",
                   Questions =
                   [
                       new FeedbackFormQuestionRadio
                       {
                           Sys = new SystemProperties
                                 {
                                     Id = FeedbackFormQuestions.DidYouGetEverythingYouNeededToday
                                 },
                           IsTheQuestionMandatory = true,
                           Question = "Radio Question",
                           ErrorMessage = "Radio question error message",
                           Options = [
                                        new Option
                                        {
                                            Value = "yes"
                                        }
                                     ]
                       },
                       new FeedbackFormQuestionTextArea
                       {
                           IsTheQuestionMandatory = true,
                           Question = "Text Area Question",
                           ErrorMessage = "Text area error message"
                       },
                       new FeedbackFormQuestionRadioAndInput
                       {
                           Sys = new SystemProperties
                                 {
                                     Id = setSystemId ? FeedbackFormQuestions.WouldYouLikeToBeContactedAboutResearch : string.Empty
                                 },
                           IsTheQuestionMandatory = true,
                           Question = "Radio and Input Question",
                           ErrorMessage = "Radio and Input Question error message",
                           ErrorMessageForInput = "Input error message",
                           ErrorMessageForInvalidEmailFormat = "Input format error message",
                           ValidateInputAsAnEmailAddress = true,
                           Options = [
                                         new Option
                                         {
                                             Value = "yes"
                                         }
                                     ]
                       }
                   ]
               };
    }

    private static FeedbackFormPageModel CreateFeedbackFormPageModel(List<FeedbackFormQuestionListModel> questionList)
    {
        return new FeedbackFormPageModel
               {
                   Heading = "Heading",
                   CtaButtonText = "Continue",
                   ErrorBannerHeading = "Error banner heading",
                   QuestionList = questionList
               };
    }
}