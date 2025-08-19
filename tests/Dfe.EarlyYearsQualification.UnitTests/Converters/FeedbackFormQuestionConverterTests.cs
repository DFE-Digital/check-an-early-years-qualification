using Dfe.EarlyYearsQualification.Content.Converters;
using Dfe.EarlyYearsQualification.Content.Entities;
using Newtonsoft.Json;

namespace Dfe.EarlyYearsQualification.UnitTests.Converters;

[TestClass]
public class FeedbackFormQuestionConverterTests
{
    [TestMethod]
    public void CanConvert_PassInNonFeedbackFormQuestionType_ReturnsFalse()
    {
        var result = new FeedbackFormQuestionConverter().CanConvert(typeof(PhaseBanner));
        result.Should().BeFalse();
    }

    [TestMethod]
    public void CanConvert_PassInFeedbackFormQuestionType_ReturnsTrue()
    {
        var result = new FeedbackFormQuestionConverter().CanConvert(typeof(IFeedbackFormQuestion));
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ReadJson_PassInObjectContainingInputHeading_ReturnsFeedbackFormQuestionRadioAndInput()
    {
        var question = new FeedbackFormQuestionRadioAndInput { InputHeading = "Test" };
        var json = JsonConvert.SerializeObject(question);
        JsonReader reader = new JsonTextReader(new StringReader(json));
        while (reader.TokenType == JsonToken.None)
            if (!reader.Read())
                break;

        var result =
            new FeedbackFormQuestionConverter().ReadJson(reader, typeof(IFeedbackFormQuestion), null, JsonSerializer.CreateDefault());

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<FeedbackFormQuestionRadioAndInput>();
        var data = result as FeedbackFormQuestionRadioAndInput;
        data!.InputHeading.Should().Match(question.InputHeading);
    }
    
    [TestMethod]
    public void ReadJson_PassInObjectContainingOptions_ReturnsFeedbackFormQuestionRadio()
    {
        var question = new FeedbackFormQuestionRadio();
        var json = JsonConvert.SerializeObject(question);
        JsonReader reader = new JsonTextReader(new StringReader(json));
        while (reader.TokenType == JsonToken.None)
            if (!reader.Read())
                break;

        var result =
            new FeedbackFormQuestionConverter().ReadJson(reader, typeof(IFeedbackFormQuestion), null, JsonSerializer.CreateDefault());

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<FeedbackFormQuestionRadio>();
    }

    [TestMethod]
    public void ReadJson_PassInObjectNotContainingInputHeadingOrOptions_ReturnsFeedbackFormQuestionTextArea()
    {
        var question = new FeedbackFormQuestionTextArea { Question = "Test" };
        var json = JsonConvert.SerializeObject(question);
        JsonReader reader = new JsonTextReader(new StringReader(json));
        while (reader.TokenType == JsonToken.None)
            if (!reader.Read())
                break;

        var result =
            new FeedbackFormQuestionConverter().ReadJson(reader, typeof(IFeedbackFormQuestion), null, JsonSerializer.CreateDefault());

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<FeedbackFormQuestionTextArea>();
        var data = result as FeedbackFormQuestionTextArea;
        data!.Question.Should().Match(question.Question);
    }

    [TestMethod]
    public void WriteJson_ShouldThrowException()
    {
        var action = () => new FeedbackFormQuestionConverter().WriteJson(new JsonTextWriter(new StringWriter()), null,
                                                                         JsonSerializer.CreateDefault());

        action.Should().Throw<NotImplementedException>();
    }
}