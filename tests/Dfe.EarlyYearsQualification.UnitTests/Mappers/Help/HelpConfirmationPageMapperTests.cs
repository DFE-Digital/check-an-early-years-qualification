using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers.Help;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers.Help;

[TestClass]
public class HelpConfirmationPageMapperTests
{
    [TestMethod]
    public async Task MapConfirmationPageContentToViewModelAsync_MapsToViewModel()
    {
        var mockContentParser = new Mock<IGovUkContentParser>();

        var mockFeedbackFormPageMapper = new Mock<IFeedbackFormPageMapper>();

        var docContent = "The Check an early years qualification team will reply to your message within 5 working days. Complex cases may take longer.\r\nWe may need to contact you for more information before we can respond.\r\n";
        
        var docContentHTML = ContentfulContentHelper.Paragraph(docContent);
        
        var postFeedbackFormContent = "Post Feedback Form Content";

        var postFeedbackFormContentHTML = ContentfulContentHelper.Paragraph(postFeedbackFormContent);
        
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == docContentHTML)))
                         .ReturnsAsync(docContent);
        
        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == postFeedbackFormContentHTML)))
                         .ReturnsAsync(postFeedbackFormContent);
        
        var content = new HelpConfirmationPage
        {
            SuccessMessage = "Message sent",
            BodyHeading = "What happens next",
            Body = docContentHTML,
            SuccessMessageFollowingText = "Your message was successfully sent to the Check an early years qualification team.",
            ReturnToHomepageLink = new NavigationLink
            {
                DisplayText = "Return to the homepage",
                Href = "/"
            },
            PostFeedbackFormContent = postFeedbackFormContentHTML,
        };

        var result = await new HelpConfirmationPageMapper(mockContentParser.Object, mockFeedbackFormPageMapper.Object).MapConfirmationPageContentToViewModelAsync(content);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ConfirmationPageViewModel>();

        result.SuccessMessage.Should().Be(content.SuccessMessage);
        result.BodyHeading.Should().Be(content.BodyHeading);
        result.Body.Should().Be(docContent);
        result.PostFeedbackFormContent.Should().Be(postFeedbackFormContent);
    }
}