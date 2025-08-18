using System.ComponentModel.DataAnnotations;
using System.Text;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.Web.Services.FeedbackForm;

public class FeedbackFormService(IUserJourneyCookieService userJourneyCookieService) : IFeedbackFormService
{
    public ErrorSummaryModel ValidateQuestions(FeedbackFormPage feedbackFormPage, FeedbackFormPageModel model)
    {
        var errorSummaryModel = new ErrorSummaryModel
                                {
                                    ErrorBannerHeading = feedbackFormPage.ErrorBannerHeading,
                                    ErrorSummaryLinks = []
                                };

        var mandatoryQuestions =
            feedbackFormPage.Questions.Where(x => (x as BaseFeedbackFormQuestion)!.IsTheQuestionMandatory).ToList();

        foreach (var question in mandatoryQuestions)
        {
            var baseQuestion = question as BaseFeedbackFormQuestion;
            var answeredQuestionIndex = model.QuestionList.FindIndex(x => x.Question == baseQuestion!.Question);
            var answeredQuestion = model.QuestionList[answeredQuestionIndex];

            if (question.GetType() == typeof(FeedbackFormQuestionTextArea))
            {
                ValidateTextAreaQuestion(question as FeedbackFormQuestionTextArea, errorSummaryModel, answeredQuestion,
                                         answeredQuestionIndex);
            }
            else if (question.GetType() == typeof(FeedbackFormQuestionRadio))
            {
                ValidateRadioQuestion(question as FeedbackFormQuestionRadio, errorSummaryModel, answeredQuestion,
                                      answeredQuestionIndex);
            }
            else if (question.GetType() == typeof(FeedbackFormQuestionRadioAndInput))
            {
                ValidateRadioAndInputQuestion(question as FeedbackFormQuestionRadioAndInput, errorSummaryModel,
                                              answeredQuestion, answeredQuestionIndex);
            }
            // Replace the answeredQuestion object
            model.QuestionList.RemoveAt(answeredQuestionIndex);
            model.QuestionList.Insert(answeredQuestionIndex, answeredQuestion);
        }

        return errorSummaryModel;
    }

    public string ConvertQuestionListToString(FeedbackFormPageModel model)
    {
        var sb = new StringBuilder();

        foreach (var question in model.QuestionList)
        {
            // Uses markdown to create a heading
            sb.AppendLine($"## {question.Question}");
            sb.AppendLine(question.Answer);
            if (!string.IsNullOrWhiteSpace(question.AdditionalInfo))
            {
                sb.AppendLine(question.AdditionalInfo);
            }
            // Creates a horizontal rule between questions
            sb.AppendLine();
            sb.AppendLine("---");
        }

        return sb.ToString();
    }

    public void SetDefaultAnswers(FeedbackFormPage feedbackFormPage, FeedbackFormPageModel model)
    {
        var hasUserGotEverythingTheyNeededToday = userJourneyCookieService.GetHasUserGotEverythingTheyNeededToday();
        if (string.IsNullOrEmpty(hasUserGotEverythingTheyNeededToday)) return;
        var question = feedbackFormPage.Questions.FirstOrDefault(x => (x as BaseFeedbackFormQuestion)!.Sys.Id == FeedbackFormQuestions.DidYouGetEverythingYouNeededToday);
        if (question is BaseFeedbackFormQuestion baseQuestion && model.QuestionList.Any(x => x.Question == baseQuestion.Question))
        {
            model.QuestionList.FirstOrDefault(x => x.Question == baseQuestion.Question)!.Answer = hasUserGotEverythingTheyNeededToday;
        }
    }

    private static void ValidateTextAreaQuestion(FeedbackFormQuestionTextArea? question, ErrorSummaryModel errorSummaryModel,
                                                 FeedbackFormQuestionListModel answeredQuestion, int answeredQuestionIndex)
    {
        if (string.IsNullOrEmpty(answeredQuestion.Answer) && question is not null)
        {
            SetErrorOnAnsweredQuestion(answeredQuestion, question.ErrorMessage);
            AddErrorMessageToModel(errorSummaryModel, question.ErrorMessage, $"{answeredQuestionIndex}_textArea");
        }
    }

    private static void ValidateRadioQuestion(FeedbackFormQuestionRadio? question, ErrorSummaryModel errorSummaryModel,
                                              FeedbackFormQuestionListModel answeredQuestion, int answeredQuestionIndex)
    {
        if (string.IsNullOrEmpty(answeredQuestion.Answer) && question is not null)
        {
            SetErrorOnAnsweredQuestion(answeredQuestion, question.ErrorMessage);
            AddErrorMessageToModel(errorSummaryModel, question.ErrorMessage,
                                   $"{answeredQuestionIndex}_{(question.Options[0] as Option)!.Value}");
        }
    }

    private void ValidateRadioAndInputQuestion(FeedbackFormQuestionRadioAndInput? question,
                                                      ErrorSummaryModel errorSummaryModel,
                                                      FeedbackFormQuestionListModel answeredQuestion,
                                                      int answeredQuestionIndex)
    {
        if (question is null) return;
        
        var firstRadioOption = (question.Options[0] as Option)!;
        if (string.IsNullOrEmpty(answeredQuestion.Answer))
        {
            SetErrorOnAnsweredQuestion(answeredQuestion, question.ErrorMessage);
            AddErrorMessageToModel(errorSummaryModel, question.ErrorMessage,
                                   $"{answeredQuestionIndex}_{firstRadioOption.Value}");
        }
        else if (string.IsNullOrEmpty(answeredQuestion.AdditionalInfo) &&
                 answeredQuestion.Answer == firstRadioOption.Value)
        {
            SetErrorOnAnsweredQuestion(answeredQuestion, question.ErrorMessageForInput);
            AddErrorMessageToModel(errorSummaryModel, question.ErrorMessageForInput,
                                   $"{answeredQuestionIndex}_additionalInfo");
        }
        else if (!string.IsNullOrEmpty(answeredQuestion.AdditionalInfo)
                 && answeredQuestion.Answer == firstRadioOption.Value
                 && question.ValidateInputAsAnEmailAddress
                 && !new EmailAddressAttribute().IsValid(answeredQuestion.AdditionalInfo))
        {
            SetErrorOnAnsweredQuestion(answeredQuestion, question.ErrorMessageForInvalidEmailFormat);
            AddErrorMessageToModel(errorSummaryModel, question.ErrorMessageForInvalidEmailFormat,
                                   $"{answeredQuestionIndex}_additionalInfo");
        }

        if (question.Sys.Id == FeedbackFormQuestions.WouldYouLikeToBeContactedAboutResearch 
            && answeredQuestion.Answer == firstRadioOption.Value
            && !string.IsNullOrEmpty(answeredQuestion.AdditionalInfo))
        {
            userJourneyCookieService.SetHasSubmittedEmailAddressInFeedbackFormQuestion(true);
        }
        else
        {
            userJourneyCookieService.SetHasSubmittedEmailAddressInFeedbackFormQuestion(false);
        }
    }

    private static void AddErrorMessageToModel(ErrorSummaryModel model, string questionErrorMessage, string elementId)
    {
        model.ErrorSummaryLinks.Add(new ErrorSummaryLink
                                    {
                                        ErrorBannerLinkText = questionErrorMessage,
                                        ElementLinkId = elementId
                                    });
    }
    
    private static void SetErrorOnAnsweredQuestion(FeedbackFormQuestionListModel answeredQuestion, string errorMessage)
    {
        answeredQuestion.HasError = true;
        answeredQuestion.ErrorMessage = errorMessage;
    }
}