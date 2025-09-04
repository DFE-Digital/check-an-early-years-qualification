using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class PreCheckPageMapperTests
{
    [TestMethod]
    public async Task Map_PassInData_ReturnsExpectedModel()
    {
        const string postHeaderContent = "Post header content";
        var preCheckPage = new PreCheckPage
                           {
                               Header = "Header",
                               BackButton = new NavigationLink
                                            {
                                                DisplayText = "Back"
                                            },
                               Question = "Question",
                               Options = [new Option {Label = "First"}],
                               InformationMessage = "Information message",
                               CtaButtonText = "Continue",
                               ErrorBannerHeading = "Error banner heading",
                               ErrorMessage = "Error message",
                               PostHeaderContent = ContentfulContentHelper.Paragraph(postHeaderContent)
                           };

        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(preCheckPage.PostHeaderContent)).ReturnsAsync(postHeaderContent);
        var mapper = new PreCheckPageMapper(mockContentParser.Object);
        var result = await mapper.Map(preCheckPage);

        result.Should().NotBeNull();
        result.Header.Should().BeSameAs(preCheckPage.Header);
        result.BackButton.Should().NotBeNull();
        result.BackButton!.DisplayText.Should().BeSameAs(preCheckPage.BackButton.DisplayText);
        result.Question.Should().BeSameAs(preCheckPage.Question);
        result.OptionsItems.Should().NotBeEmpty();
        result.OptionsItems.Count.Should().Be(1);
        result.OptionsItems[0].As<OptionModel>();
        (result.OptionsItems[0] as OptionModel)!.Label.Should().BeSameAs((preCheckPage.Options[0] as Option)!.Label);
        result.InformationMessage.Should().BeSameAs(preCheckPage.InformationMessage);
        result.CtaButtonText.Should().BeSameAs(preCheckPage.CtaButtonText);
        result.ErrorBannerHeading.Should().BeSameAs(preCheckPage.ErrorBannerHeading);
        result.ErrorMessage.Should().BeSameAs(preCheckPage.ErrorMessage);
    }
}