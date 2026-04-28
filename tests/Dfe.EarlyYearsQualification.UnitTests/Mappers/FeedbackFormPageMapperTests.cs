using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class FeedbackFormPageMapperTests
{
    [TestMethod]
    public async Task Map_PassInParameters_ReturnsModel()
    {
        const string postHeadingContent = "post heading content";
        var pageData = new FeedbackFormPage
                       {
                           Heading = "Heading",
                           CtaButtonText = "Continue",
                           BackButton = new NavigationLink
                                        {
                                            Href = "/"
                                        },
                           Questions = AddQuestions(),
                           PostHeadingContent = ContentfulContentHelper.Paragraph(postHeadingContent)
                       };

        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(pageData.PostHeadingContent)).ReturnsAsync(postHeadingContent);
        var mapper = new FeedbackFormPageMapper(mockContentParser.Object);
        var result = await mapper.Map(pageData);
        
        result.Should().NotBeNull();
        result.Heading.Should().Be(pageData.Heading);
        result.CtaButtonText.Should().Be(pageData.CtaButtonText);
        result.BackButton.Should().NotBeNull();
        result.BackButton.Href.Should().Be(pageData.BackButton.Href);
        result.Questions.Should().HaveCount(2);
        result.Questions[0].Should().NotBeNull();

        var question0 = result.Questions[0] as FeedbackFormQuestionRadioModel;
        question0.Should().NotBeNull();
        question0.Question.Should().Match((pageData.Questions[0] as FeedbackFormQuestionRadio)!.Question);
        question0.OptionsItems.Count.Should().Be((pageData.Questions[0] as FeedbackFormQuestionRadio)!.Options.Count);

        var question1 = result.Questions[1] as FeedbackFormQuestionTextAreaModel;
        question1.Should().NotBeNull();
        question1.Question.Should().Match((pageData.Questions[1] as FeedbackFormQuestionTextArea)!.Question);
        question1.HintText.Should().Be((pageData.Questions[1] as FeedbackFormQuestionTextArea)!.HintText);

        result.QuestionList.Should().HaveCount(2);
    }

    private static List<IFeedbackFormQuestion> AddQuestions()
    {
        var questions = new List<IFeedbackFormQuestion>
                        {
                            new FeedbackFormQuestionRadio
                            {
                                Question = "Radio question",
                                Options = AddOptions()
                            },
                            new FeedbackFormQuestionTextArea
                            {
                                Question = "Text area question",
                                HintText = "Text area hint"
                            }
                        };

        return questions;
    }

    private static List<IOptionItem> AddOptions()
    {
        return [
                   new Option
                   {
                       Hint = "Option 1 hint", Label = "Option 1 Label",
                       Value = "Option 1 Value"
                   },
                   new Option
                   {
                       Hint = "Option 2 hint", Label = "Option 2 Label",
                       Value = "Option 2 Value"
                   }
               ];
    }
}