using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers.Help;

[TestClass]
public class HelpEmailAddressPageMapperTests
{
    [TestMethod]
    public void MapEmailAddressPageContentToViewModel_MapsToViewModel()
    {
        var mockContentParser = new Mock<IGovUkContentParser>();

        var content = new HelpEmailAddressPage()
        {
            Heading = "What is your email address?",
            InvalidEmailAddressErrorMessage = "Enter an email address in the correct format, for example name@example.com",
            NoEmailAddressEnteredErrorMessage = "Enter an email address",
            BackButton = new NavigationLink
            {
                DisplayText = "Back to how can we help you",
                Href = "/help/provide-details",
                OpenInNewTab = false
            },
            CtaButtonText = "Send message",
            ErrorBannerHeading = "There is a problem",
            PostHeadingContent = "We will only use this email address to reply to your message"
        };

        var result = new HelpEmailAddressPageMapper().MapEmailAddressPageContentToViewModel(content);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<EmailAddressPageViewModel>();

        result.Heading.Should().Be(content.Heading);
        result.BackButton.DisplayText.Should().Be(content.BackButton.DisplayText);
        result.BackButton.Href.Should().Be(content.BackButton.Href);
        result.CtaButtonText.Should().Be(content.CtaButtonText);
        result.ErrorBannerHeading.Should().Be(content.ErrorBannerHeading);
        result.PostHeadingContent.Should().Be(content.PostHeadingContent);
        result.HasEmailAddressError.Should().BeFalse();
        result.HasValidationErrors.Should().BeFalse();
    }
}