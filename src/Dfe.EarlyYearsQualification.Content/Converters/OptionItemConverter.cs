using Dfe.EarlyYearsQualification.Content.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dfe.EarlyYearsQualification.Content.Converters;

public class OptionItemConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(IOptionItem);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
                                    JsonSerializer serializer)
    {
        var jo = JObject.Load(reader);

        var hasLabel = jo.ContainsKey("label") || jo.ContainsKey("Label");

        IOptionItem model = new Divider();

        if (hasLabel)
        {
            model = new Option();
        }

        var contentTypeId = jo["sys"]?["contentType"]?["sys"]?["id"]?.ToString();
        if (string.Equals(contentTypeId, "radioButtonWithDateInput"))
        {
            model = new RadioButtonAndDateInput();
        }

        serializer.Populate(jo.CreateReader(), model);
        return model;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}