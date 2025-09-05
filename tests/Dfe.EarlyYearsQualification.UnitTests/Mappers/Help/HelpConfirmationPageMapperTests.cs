using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers.Help;

[TestClass]
public class HelpConfirmationPageMapperTests
{
    [TestMethod]
    public async Task MapConfirmationPageContentToViewModelAsync_MapsToViewModel()
    {
        var mockContentParser = new Mock<IGovUkContentParser>();

        var docContent = "The Check an early years qualification team will reply to your message within 5 working days. Complex cases may take longer.\r\nWe may need to contact you for more information before we can respond.\r\n";

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).Returns(() => Task.FromResult(docContent));

        var content = new HelpConfirmationPage
        {
            SuccessMessage = "Message sent",
            BodyHeading = "What happens next",
            Body = ContentfulContentHelper.Paragraph(docContent),
            FeedbackComponent = new FeedbackComponent
            {
                Header = "Give feedback",
                Body = ContentfulContentHelper.Paragraph("Your feedback matters and will help us improve the service.")
            },
            SuccessMessageFollowingText = "Your message was successfully sent to the Check an early years qualification team.",
            ReturnToHomepageLink = new NavigationLink
            {
                DisplayText = "Return to the homepage",
                Href = "/"
            }
        };

        var result = await new HelpConfirmationPageMapper(mockContentParser.Object).MapConfirmationPageContentToViewModelAsync(content);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ConfirmationPageViewModel>();

        result.SuccessMessage.Should().Be(content.SuccessMessage);
        result.BodyHeading.Should().Be(content.BodyHeading);
        result.Body.Should().Be(docContent);
    }
}