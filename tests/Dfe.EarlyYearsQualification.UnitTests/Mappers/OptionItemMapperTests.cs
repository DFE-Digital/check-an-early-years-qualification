using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Helpers;

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

    [TestMethod]
    public void Map_PassInRadioButtonAndDateInput_ReturnsRadioButtonAndDateInputModel()
    {
        var options = new List<IOptionItem>
                      {
                          new RadioButtonAndDateInput
                          {
                              Label = "Radio label",
                              Value = "radio-value",
                              Hint = "radio-hint",
                              StartedQuestion = new DateQuestion
                                                {
                                                    ErrorMessage = "Start error",
                                                    MonthLabel = "Month",
                                                    YearLabel = "Year",
                                                    QuestionHeader = "When did it start?",
                                                    QuestionHint = "Enter start date"
                                                }
                          }
                      };

        var result = OptionItemMapper.Map(options);

        result.Should().NotBeNull();
        result.Count.Should().Be(1);
        result[0].Should().BeAssignableTo<RadioButtonAndDateInputModel>();

        var model = result[0] as RadioButtonAndDateInputModel;
        model.Should().NotBeNull();
        model!.Label.Should().Be("Radio label");
        model.Value.Should().Be("radio-value");
        model.Hint.Should().Be("radio-hint");

        model.Question.Should().NotBeNull();
        model.Question!.MonthId.Should().Be("Month");
        model.Question.YearId.Should().Be("Year");
        model.Question.ErrorMessage.Should().Be("Start error");
        model.Question.MonthLabel.Should().Be("Month");
        model.Question.YearLabel.Should().Be("Year");
        model.Question.QuestionHeader.Should().Be("When did it start?");
        model.Question.QuestionHint.Should().Be("Enter start date");
        model.Question.Prefix.Should().Be("question");
        model.Question.QuestionId.Should().Be(StringFormattingHelper.ToHtmlId("When did it start?"));
    }
}