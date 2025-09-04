using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class FeedbackFormConfirmationPageMapperTests
{
    [TestMethod]
    public async Task Map_PassInParameters_ReturnsModel()
    {
        const string bodyHtml = "<p>body</p>";
        const string optionalEmailBodyHtml = "<p>optional email body</p>";
        var pageData = new FeedbackFormConfirmationPage
                       { 
                           SuccessMessage = "Success",
                           OptionalEmailHeading = "Optional heading",
                           ReturnToHomepageLink = new NavigationLink { Href = "/" },
                           Body = ContentfulContentHelper.Paragraph(bodyHtml),
                           OptionalEmailBody = ContentfulContentHelper.Paragraph(optionalEmailBodyHtml)
                       };

        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(pageData.Body)).ReturnsAsync(bodyHtml);
        mockContentParser.Setup(x => x.ToHtml(pageData.OptionalEmailBody)).ReturnsAsync(optionalEmailBodyHtml);
        var mapper = new FeedbackFormConfirmationPageMapper(mockContentParser.Object);
        var result = await mapper.Map(pageData);
        
        result.Should().NotBeNull();
        result.SuccessMessage.Should().Match(pageData.SuccessMessage);
        result.OptionalEmailHeading.Should().Match(pageData.OptionalEmailHeading);
        result.ReturnToHomepageLink.Should().NotBeNull();
        result.ReturnToHomepageLink.Href.Should().Match(pageData.ReturnToHomepageLink.Href);
        result.Body.Should().Be(bodyHtml);
        result.OptionalEmailBody.Should().Be(optionalEmailBodyHtml);
    }
}