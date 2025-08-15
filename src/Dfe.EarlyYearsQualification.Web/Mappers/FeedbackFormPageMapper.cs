using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class FeedbackFormPageMapper
{
    public static FeedbackFormPageModel Map(FeedbackFormPage feedbackFormPage, string postHeadingContent)
    {
        var model = new FeedbackFormPageModel
               {
                   Heading = feedbackFormPage.Heading,
                   BackButton = NavigationLinkMapper.Map(feedbackFormPage.BackButton),
                   CtaButtonText = feedbackFormPage.CtaButtonText,
                   ErrorBannerHeading = feedbackFormPage.ErrorBannerHeading,
                   PostHeadingContent = postHeadingContent,
                   Questions = MapQuestions(feedbackFormPage.Questions)
               };
        
        model.Questions.ForEach(x => model.QuestionList.Add(new FeedbackFormQuestionListModel { Question = (x as BaseFeedbackFormQuestionModel)!.Question}));

        return model;
    }

    private static List<IFeedbackFormQuestionModel> MapQuestions(List<IFeedbackFormQuestion> questions)
    {
        var results = new List<IFeedbackFormQuestionModel>();
        foreach (var question in questions)
        {
            if (question.GetType() == typeof(FeedbackFormQuestionRadio))
            {
                results.Add(MapRadioQuestion(question as FeedbackFormQuestionRadio));
                continue;
            }
            if (question.GetType() == typeof(FeedbackFormQuestionRadioAndInput))
            {
                results.Add(MapRadioAndInputQuestion(question as FeedbackFormQuestionRadioAndInput));
                continue;
            }
            if (question.GetType() == typeof(FeedbackFormQuestionTextArea))
            {
                results.Add(MapTextAreaQuestion(question as FeedbackFormQuestionTextArea));
            }
        }
        return results;
    }

    private static FeedbackFormQuestionTextAreaModel MapTextAreaQuestion(FeedbackFormQuestionTextArea? question)
    {
        return new FeedbackFormQuestionTextAreaModel
               {
                   Question = question!.Question,
                   HintText = question.HintText,
                   ErrorMessage = question.ErrorMessage
               };
    }

    private static FeedbackFormQuestionRadioAndInputModel MapRadioAndInputQuestion(FeedbackFormQuestionRadioAndInput? question)
    {
        return new FeedbackFormQuestionRadioAndInputModel
               {
                   Question = question!.Question,
                   OptionsItems = OptionItemMapper.Map(question.Options),
                   ErrorMessage = question.ErrorMessage,
                   ErrorMessageForInput = question.ErrorMessageForInput,
                   InputHeading = question.InputHeading,
                   InputHeadingHintText = question.InputHeadingHintText
               };
    }

    private static FeedbackFormQuestionRadioModel MapRadioQuestion(FeedbackFormQuestionRadio? question)
    {
        return new FeedbackFormQuestionRadioModel
               {
                   Question = question!.Question,
                   ErrorMessage = question.ErrorMessage,
                   OptionsItems = OptionItemMapper.Map(question.Options)
               };
    }
}