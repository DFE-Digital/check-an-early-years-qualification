using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Mappers;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class FeedbackFormConfirmationPageMapperTests
{
    [TestMethod]
    public void Map_PassInParameters_ReturnsModel()
    {
        var pageData = new FeedbackFormConfirmationPage
                       { 
                           SuccessMessage = "Success",
                           OptionalEmailHeading = "Optional heading",
                           ReturnToHomepageLink = new NavigationLink() { Href = "/" } 
                       };
        const string bodyHtml = "<p>body</p>";
        const string optionalEmailBodyHtml = "<p>optional email body</p>";
        
        var result = FeedbackFormConfirmationPageMapper.Map(pageData, bodyHtml, optionalEmailBodyHtml);
        
        result.Should().NotBeNull();
        result.SuccessMessage.Should().Match(pageData.SuccessMessage);
        result.OptionalEmailHeading.Should().Match(pageData.OptionalEmailHeading);
        result.ReturnToHomepageLink.Should().NotBeNull();
        result.ReturnToHomepageLink.Href.Should().Match(pageData.ReturnToHomepageLink.Href);
        result.Body.Should().Be(bodyHtml);
        result.OptionalEmailBody.Should().Be(optionalEmailBodyHtml);
    }
}