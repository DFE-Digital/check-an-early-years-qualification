using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class RadioQuestionMapperTests
{
    [TestMethod]
    public void Map_PassInParameters_ReturnsModel()
    {
        var question = new RadioQuestionPage
                       {
                           Question = "Question",
                           Options =
                           [
                               new Option { Label = "Label", Value = "Value", Hint = "Hint " },
                               new Divider { Text = "Or" }
                           ],
                           CtaButtonText = "Button text",
                           ErrorMessage = "Error message",
                           AdditionalInformationHeader = "Additional information header",
                           BackButton = new NavigationLink
                                        {
                                            DisplayText = "Back",
                                            OpenInNewTab = true,
                                            Href = "/"
                                        },
                           ErrorBannerHeading = "Error banner heading",
                           ErrorBannerLinkText = "Error banner link text"
                       };
        const string actionName = "action";
        const string controllerName = "controller";
        const string additionalInformationBodyHtml = "additional info body";
        const string selectedAnswer = "selected answer";

        var result = RadioQuestionMapper.Map(new RadioQuestionModel(), question, actionName, controllerName,
                                             additionalInformationBodyHtml, selectedAnswer);

        result.Should().NotBeNull();
        result.Question.Should().BeSameAs(question.Question);
        result.OptionsItems.Should().NotBeNull();
        result.OptionsItems.Count.Should().Be(2);
        result.OptionsItems[0].Should().BeOfType<OptionModel>();
        result.OptionsItems[0].Should().BeEquivalentTo((Option)question.Options[0]);
        result.OptionsItems[1].Should().BeOfType<DividerModel>();
        result.OptionsItems[1].Should().BeEquivalentTo((Divider)question.Options[1]);
        result.ActionName.Should().BeSameAs(actionName);
        result.ControllerName.Should().BeSameAs(controllerName);
        result.ErrorMessage.Should().BeSameAs(question.ErrorMessage);
        result.AdditionalInformationHeader.Should().BeSameAs(question.AdditionalInformationHeader);
        result.AdditionalInformationBody.Should().BeSameAs(additionalInformationBodyHtml);
        result.BackButton.Should().BeEquivalentTo(question.BackButton, options => options.Excluding(x => x.Sys));
        result.ErrorBannerHeading.Should().BeSameAs(question.ErrorBannerHeading);
        result.ErrorBannerLinkText.Should().BeSameAs(question.ErrorBannerLinkText);
        result.Option.Should().BeSameAs(selectedAnswer);
    }
}