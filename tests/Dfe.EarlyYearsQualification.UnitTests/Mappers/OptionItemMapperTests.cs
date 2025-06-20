using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class OptionItemMapperTests
{
    [TestMethod]
    public void Map_PassInDetails_ReturnsExpected()
    {
        var options = new List<IOptionItem>
                      {
                          new Option
                          {
                              Label = "Option Label",
                              Value = "Option Value",
                              Hint = "Option Hint"
                          },
                          new Divider
                          {
                              Text = "Divider text"
                          }
                      };

        var result = OptionItemMapper.Map(options);

        result.Should().NotBeNull();
        result.Count.Should().Be(2);
        result[0].Should().BeAssignableTo<OptionModel>();
        result[1].Should().BeAssignableTo<DividerModel>();

        var optionModel = result[0] as OptionModel;
        optionModel.Should().NotBeNull();
        optionModel!.Label.Should().BeSameAs((options[0] as Option)!.Label);
        optionModel.Value.Should().BeSameAs((options[0] as Option)!.Value);
        optionModel.Hint.Should().BeSameAs((options[0] as Option)!.Hint);
        
        var dividerModel = result[1] as DividerModel;
        dividerModel.Should().NotBeNull();
        dividerModel!.Text.Should().BeSameAs((options[1] as Divider)!.Text);
    }
}