using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class FeedbackFormPageMapper(IGovUkContentParser contentParser) : IFeedbackFormPageMapper
{
    public async Task<FeedbackFormPageModel> Map(FeedbackFormPage feedbackFormPage)
    {
        var postHeadingContent = await contentParser.ToHtml(feedbackFormPage.PostHeadingContent);
        var model = new FeedbackFormPageModel
               {
                   Heading = feedbackFormPage.Heading,
                   BackButton = NavigationLinkMapper.Map(feedbackFormPage.BackButton),
                   CtaButtonText = feedbackFormPage.CtaButtonText,
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
                   HintText = question.HintText
               };
    }

    private static FeedbackFormQuestionRadioModel MapRadioQuestion(FeedbackFormQuestionRadio? question)
    {
        return new FeedbackFormQuestionRadioModel
               {
                   Question = question!.Question,
                   OptionsItems = OptionItemMapper.Map(question.Options)
               };
    }
}