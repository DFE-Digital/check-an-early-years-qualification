using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class CookiesPageMapperTests
{
    [TestMethod]
    public async Task Map_PassInParameters_ReturnsModel()
    {
        const string bodyContentHtml = "Body content";
        const string successBannerContentHtml = "Success banner content";
        var cookiesPage = new CookiesPage
                          {
                              Heading = "Heading",
                              Options = [new Option { Label = "Label", Value = "Value", Hint = "Hint" }],
                              ButtonText = "Button text",
                              ErrorText = "Error text",
                              FormHeading = "Form heading",
                              SuccessBannerHeading = "Success banner heading",
                              BackButton = new NavigationLink
                                           {
                                               DisplayText = "Back",
                                               OpenInNewTab = true,
                                               Href = "/"
                                           },
                              Body = ContentfulContentHelper.Paragraph(bodyContentHtml),
                              SuccessBannerContent = ContentfulContentHelper.Paragraph(successBannerContentHtml)
                          };

        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(cookiesPage.Body)).ReturnsAsync(bodyContentHtml);
        mockContentParser.Setup(x => x.ToHtml(cookiesPage.SuccessBannerContent)).ReturnsAsync(successBannerContentHtml);
        var mapper = new CookiesPageMapper(mockContentParser.Object);
        var result = await mapper.Map(cookiesPage);

        result.Should().NotBeNull();
        result.Heading.Should().BeSameAs(cookiesPage.Heading);
        result.BodyContent.Should().BeSameAs(bodyContentHtml);
        result.Options.Should().NotBeNull();
        result.Options.Count.Should().Be(1);
        result.Options[0].Label.Should().BeSameAs(cookiesPage.Options[0].Label);
        result.Options[0].Value.Should().BeSameAs(cookiesPage.Options[0].Value);
        result.Options[0].Hint.Should().BeSameAs(cookiesPage.Options[0].Hint);
        result.ButtonText.Should().BeSameAs(cookiesPage.ButtonText);
        result.SuccessBannerContent.Should().BeSameAs(successBannerContentHtml);
        result.SuccessBannerHeading.Should().BeSameAs(cookiesPage.SuccessBannerHeading);
        result.ErrorText.Should().BeSameAs(cookiesPage.ErrorText);
        result.FormHeading.Should().BeSameAs(cookiesPage.FormHeading);
        result.BackButton.Should().BeEquivalentTo(cookiesPage.BackButton, options => options.Excluding(x => x.Sys));
    }
}