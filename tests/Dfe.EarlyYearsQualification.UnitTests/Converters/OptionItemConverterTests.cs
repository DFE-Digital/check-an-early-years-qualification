using Dfe.EarlyYearsQualification.Content.Converters;
using Dfe.EarlyYearsQualification.Content.Entities;
using Newtonsoft.Json;

namespace Dfe.EarlyYearsQualification.UnitTests.Converters;

[TestClass]
public class OptionItemConverterTests
{
    [TestMethod]
    public void CanConvert_PassInNonOptionItemType_ReturnsFalse()
    {
        var result = new OptionItemConverter().CanConvert(typeof(PhaseBanner));
        result.Should().BeFalse();
    }

    [TestMethod]
    public void CanConvert_PassInOptionItemType_ReturnsTrue()
    {
        var result = new OptionItemConverter().CanConvert(typeof(IOptionItem));
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ReadJson_PassInObjectContainingLabel_ReturnsOption()
    {
        var option = new Option { Label = "Test" };
        var json = JsonConvert.SerializeObject(option);
        JsonReader reader = new JsonTextReader(new StringReader(json));
        while (reader.TokenType == JsonToken.None)
            if (!reader.Read())
                break;

        var result =
            new OptionItemConverter().ReadJson(reader, typeof(IOptionItem), null, JsonSerializer.CreateDefault());

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Option>();
        var data = result as Option;
        data!.Label.Should().Match(option.Label);
    }

    [TestMethod]
    public void ReadJson_PassInObjectContaininglabel_ReturnsOption()
    {
        var option = new {
            label = "Test"
        };

        var json = JsonConvert.SerializeObject(option);
        JsonReader reader = new JsonTextReader(new StringReader(json));
        while (reader.TokenType == JsonToken.None)
            if (!reader.Read())
                break;

        var result =
            new OptionItemConverter().ReadJson(reader, typeof(IOptionItem), null, JsonSerializer.CreateDefault());

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Option>();
        var data = result as Option;
        data!.Label.Should().Match(option.label);
    }

    [TestMethod]
    public void ReadJson_PassInObjectNotContainingLabel_ReturnsDivider()
    {
        var divider = new Divider { Text = "Or" };
        var json = JsonConvert.SerializeObject(divider);
        JsonReader reader = new JsonTextReader(new StringReader(json));
        while (reader.TokenType == JsonToken.None)
            if (!reader.Read())
                break;

        var result =
            new OptionItemConverter().ReadJson(reader, typeof(IOptionItem), null, JsonSerializer.CreateDefault());

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Divider>();
        var data = result as Divider;
        data!.Text.Should().Match(divider.Text);
    }

    [TestMethod]
    public void WriteJson_ShouldThrowException()
    {
        var action = () => new OptionItemConverter().WriteJson(new JsonTextWriter(new StringWriter()), null,
                                                               JsonSerializer.CreateDefault());

        action.Should().Throw<NotImplementedException>();
    }
}