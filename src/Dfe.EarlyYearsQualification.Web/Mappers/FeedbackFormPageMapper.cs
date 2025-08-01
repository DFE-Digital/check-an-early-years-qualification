using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class FeedbackFormPageMapper
{
    public static GiveFeedbackPageModel Map(FeedbackFormPage feedbackFormPage, string postHeadingContent)
    {
        return new GiveFeedbackPageModel
               {
                   Heading = feedbackFormPage.Heading,
                   BackButton = NavigationLinkMapper.Map(feedbackFormPage.BackButton),
                   CtaButtonText = feedbackFormPage.CtaButtonText,
                   ErrorBannerHeading = feedbackFormPage.ErrorBannerHeading,
                   PostHeadingContent = postHeadingContent,
                   Questions = MapQuestions(feedbackFormPage.Questions)
               };
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
                //results.Add(MapRadioAndInputQuestion(question as FeedbackFormQuestionRadioAndInput));
                continue;
            }
            if (question.GetType() == typeof(FeedbackFormQuestionTextArea))
            {
                //results.Add(MapTextAreaQuestion(question as FeedbackFormQuestionTextArea));
            }
        }
        return results;
    }

    private static IFeedbackFormQuestionModel MapTextAreaQuestion(FeedbackFormQuestionTextArea? question)
    {
        throw new NotImplementedException();
    }

    private static IFeedbackFormQuestionModel MapRadioAndInputQuestion(FeedbackFormQuestionRadioAndInput? question)
    {
        throw new NotImplementedException();
    }

    private static IFeedbackFormQuestionModel MapRadioQuestion(FeedbackFormQuestionRadio? question)
    {
        return new FeedbackFormQuestionRadioModel
               {
                   Question = question!.Question,
                   IsTheQuestionMandatory = question.IsTheQuestionMandatory,
                   ErrorMessage = question.ErrorMessage,
                   OptionsItems = OptionItemMapper.Map(question.Options)
               };
    }
}