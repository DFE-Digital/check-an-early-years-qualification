using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class DropdownQuestionMapperTests
{
    [TestMethod]
    public async Task Map_PassInParameters_ReturnsModel()
    {
        const string actionName = "action";
        const string controllerName = "controller";
        const string additionalInformationBodyHtml = "additionalInformationBodyHtml";
        const string selectedAwardingOrganisation = "selectedAwardingOrganisation";
        const bool selectedNotOnTheList = true;
        
        var question = new DropdownQuestionPage
                       {
                           CtaButtonText = "Button text",
                           ErrorMessage = "Error message",
                           Question = "Question",
                           DropdownHeading = "Dropdown heading",
                           NotInListText = "Not in the list text",
                           BackButton = new NavigationLink
                                        {
                                            DisplayText = "Back",
                                            OpenInNewTab = true,
                                            Href = "/"
                                        },
                           DefaultText = "Default text",
                           ErrorBannerHeading = "Error banner heading",
                           ErrorBannerLinkText = "Error banner link text",
                           AdditionalInformationHeader = "Additional information header",
                           AdditionalInformationBody = ContentfulContentHelper.Paragraph(additionalInformationBodyHtml)
                       };
        
        var uniqueAwardingOrganisations = new List<string> { "awarding org A", "awarding org B" };

        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(question.AdditionalInformationBody)).ReturnsAsync(additionalInformationBodyHtml);
        var mapper = new DropdownQuestionMapper(mockContentParser.Object);

        var result = await mapper.Map(new DropdownQuestionModel(), question, actionName, controllerName,
                                                uniqueAwardingOrganisations.Order(),
                                                selectedAwardingOrganisation, selectedNotOnTheList);

        result.Should().NotBeNull();
        result.ActionName.Should().BeSameAs(actionName);
        result.ControllerName.Should().BeSameAs(controllerName);
        result.CtaButtonText.Should().BeSameAs(question.CtaButtonText);
        result.ErrorMessage.Should().BeSameAs(question.ErrorMessage);
        result.Question.Should().BeSameAs(question.Question);
        result.DropdownHeading.Should().BeSameAs(question.DropdownHeading);
        result.NotInListText.Should().BeSameAs(question.NotInListText);
        result.BackButton.Should().BeEquivalentTo(question.BackButton, options => options.Excluding(x => x.Sys));
        result.Values.Should().NotBeNull();
        result.Values.Count.Should().Be(3);
        result.Values[0].Text.Should().BeSameAs(question.DefaultText);
        result.Values[0].Value.Should().BeEmpty();
        result.Values[1].Text.Should().BeSameAs(uniqueAwardingOrganisations[0]);
        result.Values[1].Value.Should().BeSameAs(uniqueAwardingOrganisations[0]);
        result.Values[2].Text.Should().BeSameAs(uniqueAwardingOrganisations[1]);
        result.Values[2].Value.Should().BeSameAs(uniqueAwardingOrganisations[1]);
        result.ErrorBannerHeading.Should().BeSameAs(question.ErrorBannerHeading);
        result.ErrorBannerLinkText.Should().BeSameAs(question.ErrorBannerLinkText);
        result.AdditionalInformationHeader.Should().BeSameAs(question.AdditionalInformationHeader);
        result.AdditionalInformationBody.Should().BeSameAs(additionalInformationBodyHtml);
        result.NotInTheList.Should().BeTrue();
        result.SelectedValue.Should().BeSameAs(selectedAwardingOrganisation);
    }
}