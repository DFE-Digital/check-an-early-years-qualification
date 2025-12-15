using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Mappers.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers.Help;

[TestClass]
public class HelpProvideDetailsPageMapperTests
{
    [TestMethod]
    [DataRow(HelpFormEnquiryReasons.QuestionAboutAQualification)]
    [DataRow(HelpFormEnquiryReasons.IssueWithTheService)]
    public void MapProvideDetailsPageContentToViewModel_MapsToViewModel(string reasonForEnquiring)
    {
        var content = new HelpProvideDetailsPage
                      {
                          Heading = "How can we help you?",
                          PostHeadingContent =
                              "Give as much detail as you can. This helps us give you the right support.",
                          CtaButtonText = "Continue",
                          BackButtonToGetHelpPage = new NavigationLink
                                                    {
                                                        DisplayText =
                                                            "Back to get help with the Check an early years qualification service",
                                                        Href = "/help/get-help",
                                                        OpenInNewTab = false
                                                    },
                          BackButtonToQualificationDetailsPage = new NavigationLink
                                                                 {
                                                                     DisplayText =
                                                                         "Back to what are the qualification details",
                                                                     Href = "/help/qualification-details",
                                                                     OpenInNewTab = false
                                                                 },
                          AdditionalInformationWarningText = "Do not include any personal information",
                          AdditionalInformationErrorMessage = "Provide information about how we can help you",
                          ErrorBannerHeading = "There is a problem"
                      };

        var result =
            new HelpProvideDetailsPageMapper().MapProvideDetailsPageContentToViewModel(content, reasonForEnquiring);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ProvideDetailsPageViewModel>();

        result.Heading.Should().Be(content.Heading);
        result.PostHeadingContent.Should().Be(content.PostHeadingContent);
        result.CtaButtonText.Should().Be("Continue");

        if (reasonForEnquiring == HelpFormEnquiryReasons.QuestionAboutAQualification)
        {
            result.BackButton.Should().BeEquivalentTo(
                                                      new NavigationLinkModel
                                                      {
                                                          DisplayText = "Back to what are the qualification details",
                                                          Href = "/help/qualification-details",
                                                          OpenInNewTab = false
                                                      }
                                                     );
        }

        if (reasonForEnquiring == HelpFormEnquiryReasons.IssueWithTheService)
        {
            result.BackButton.Should().BeEquivalentTo(
                                                      new NavigationLinkModel
                                                      {
                                                          DisplayText =
                                                              "Back to get help with the Check an early years qualification service",
                                                          Href = "/help/get-help",
                                                          OpenInNewTab = false
                                                      }
                                                     );
        }

        result.ErrorBannerHeading.Should().Be(content.ErrorBannerHeading);
        result.AdditionalInformationErrorMessage.Should().Be(content.AdditionalInformationErrorMessage);
        result.Heading.Should().Be(content.Heading);
        result.CtaButtonText.Should().Be(content.CtaButtonText);
        result.ErrorBannerHeading.Should().Be(content.ErrorBannerHeading);
        result.PostHeadingContent.Should().Be(content.PostHeadingContent);
        result.HasAdditionalInformationError.Should().BeFalse();
        result.HasValidationErrors.Should().BeFalse();
    }
}