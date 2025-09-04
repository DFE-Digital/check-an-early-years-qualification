using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class ChallengePageMapperTests
{
    [TestMethod]
    public async Task Map_PassInParameters_ReturnsModel()
    {
        const string footerContentHtml = "This is the footer content";
        const string mainContentHtml = "This is the main content";
        var model = new ChallengePageModel();
        var content = new ChallengePage
                      {
                          InputHeading = "Input heading",
                          MainHeading = "Main heading",
                          SubmitButtonText = "Submit button text",
                          ShowPasswordButtonText = "Show password button text",
                          FooterContent = ContentfulContentHelper.Paragraph(footerContentHtml),
                          MainContent = ContentfulContentHelper.Paragraph(mainContentHtml)
                      };
        const string sanitisedReferralAddress = "address";
        

        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == content.FooterContent))).ReturnsAsync(footerContentHtml);
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == content.MainContent))).ReturnsAsync(mainContentHtml);
        
        var mapper = new ChallengePageMapper(mockContentParser.Object);
        var result = await mapper.Map(model, content, sanitisedReferralAddress);

        result.Should().NotBeNull();
        result.RedirectAddress.Should().BeSameAs(sanitisedReferralAddress);
        result.FooterContent.Should().BeSameAs(footerContentHtml);
        result.InputHeading.Should().BeSameAs(content.InputHeading);
        result.MainContent.Should().BeSameAs(mainContentHtml);
        result.MainHeading.Should().BeSameAs(content.MainHeading);
        result.SubmitButtonText.Should().BeSameAs(content.SubmitButtonText);
        result.ShowPasswordButtonText.Should().BeSameAs(content.ShowPasswordButtonText);
    }
}