using System.ComponentModel.DataAnnotations;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/give-feedback")]
public class GiveFeedbackController(
    ILogger<GiveFeedbackController> logger,
    IContentService contentService,
    IGovUkContentParser contentParser,
    IUserJourneyCookieService userJourneyCookieService,
    INotificationService notificationService)
    : ServiceController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var feedbackFormPage = await contentService.GetFeedbackFormPage();
        if (feedbackFormPage is null) RedirectToAction("Index", "Error");

        var model = FeedbackFormPageMapper.Map(feedbackFormPage!,
                                               await contentParser.ToHtml(feedbackFormPage!.PostHeadingContent));
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Post(FeedbackFormPageModel model)
    {
        // Get the page content
        var feedbackFormPage = await contentService.GetFeedbackFormPage();
        if (feedbackFormPage is null) RedirectToAction("Index", "Error");

        // Ensure that mandatory questions have been answered
        var errorSummaryModel = ValidateQuestions(feedbackFormPage!, model);

        if (errorSummaryModel.ErrorSummaryLinks.Count != 0)
        {
            var mappedModel = FeedbackFormPageMapper.Map(feedbackFormPage!,
                                                         await contentParser.ToHtml(feedbackFormPage!.PostHeadingContent));
            mappedModel.HasError = true;
            mappedModel.ErrorSummaryModel = errorSummaryModel;
            mappedModel.QuestionList = model.QuestionList; 
            return View("Get", mappedModel);
        }

        // if we have all required information send email through Notify
        // redirect to confirmation page
        return RedirectToAction("Get");
    }

    private ErrorSummaryModel ValidateQuestions(FeedbackFormPage feedbackFormPage, FeedbackFormPageModel model)
    {
        var errorSummaryModel = new ErrorSummaryModel
                                {
                                    ErrorBannerHeading = feedbackFormPage.ErrorBannerHeading,
                                    ErrorSummaryLinks = new List<ErrorSummaryLink>()
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

    private void ValidateTextAreaQuestion(FeedbackFormQuestionTextArea? question, ErrorSummaryModel errorSummaryModel,
                                          FeedbackFormQuestionListModel answeredQuestion, int answeredQuestionIndex)
    {
        if (string.IsNullOrEmpty(answeredQuestion.Answer) && question is not null)
        {
            SetErrorOnAnsweredQuestion(answeredQuestion, question.ErrorMessage);
            AddErrorMessageToModel(errorSummaryModel, question.ErrorMessage, $"{answeredQuestionIndex}_textArea");
        }
    }

    private void ValidateRadioQuestion(FeedbackFormQuestionRadio? question, ErrorSummaryModel errorSummaryModel,
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
        if (question is not null)
        {
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