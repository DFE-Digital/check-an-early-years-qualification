using Dfe.EarlyYearsQualification.Content.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dfe.EarlyYearsQualification.Content.Converters;

public class FeedbackFormQuestionConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(IFeedbackFormQuestion);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jo = JObject.Load(reader);
        IFeedbackFormQuestion model;
        
        if (jo.ContainsKey("inputHeading") || jo.ContainsKey("InputHeading"))
        {
            model = new FeedbackFormQuestionRadioAndInput();
        }
        else if (jo.ContainsKey("options") || jo.ContainsKey("Options"))
        {
            model = new FeedbackFormQuestionRadio();
        }
        else
        {
            model = new FeedbackFormQuestionTextArea();
        }
        
        serializer.Populate(jo.CreateReader(), model);
        return model;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}