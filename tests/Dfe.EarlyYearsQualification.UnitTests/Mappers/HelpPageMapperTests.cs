using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class HelpPageMapperTests
{
    [TestMethod]
    public void Map_MapsToModel()
    {
        const string postHeadingContent = "This is the post heading text";
        var helpPage = new HelpPage
                       {
                           Heading = "Help Page Heading",
                           PostHeadingContent = ContentfulContentHelper.Paragraph(postHeadingContent),
                           EmailAddressHeading = "Enter your email address (optional)",
                           EmailAddressHintText =
                               "If you do not enter your email address we will not be able to contact you in relation to your enquiry",
                           ReasonForEnquiryHeading = "Choose the reason of your enquiry",
                           ReasonForEnquiryHintText = "Select one option",
                           EnquiryReasons =
                           [
                               new EnquiryOption
                               { Label = "Option 1", Value = "Option 1" },
                               new EnquiryOption
                               { Label = "Option 2", Value = "Option 2" },
                               new EnquiryOption
                               { Label = "Option 3", Value = "Option 3" }
                           ],
                           AdditionalInformationHeading = "Provide further information about your enquiry",
                           AdditionalInformationHintText =
                               "Provide details about the qualification you are checking for or the specific issue you are experiencing with the service.",
                           AdditionalInformationWarningText =
                               "Do not include personal information, for example the name of the qualification holder",
                           CtaButtonText = "Send message",
                           BackButton = new NavigationLink
                                        {
                                            DisplayText = "Home",
                                            Href = "/",
                                            OpenInNewTab = false
                                        },
                           ErrorBannerHeading = "There is a problem",
                           InvalidEmailAddressErrorMessage = "Enter a valid email address",
                           NoEnquiryOptionSelectedErrorMessage = "Select one option",
                           FurtherInformationErrorMessage = "Enter further information about your enquiry"
                       };

        var result = HelpPageMapper.Map(helpPage, postHeadingContent);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<HelpPageModel>();
        result!.Heading.Should().Be("Help Page Heading");
        result.PostHeadingContent.Should().Be(postHeadingContent);
        result.EmailAddressHeading.Should().Be("Enter your email address (optional)");
        result.EmailAddressHintText.Should().Be("If you do not enter your email address we will not be able to contact you in relation to your enquiry");
        result.ReasonForEnquiryHeading.Should().Be("Choose the reason of your enquiry");
        result.ReasonForEnquiryHintText.Should().Be("Select one option");
        result.EnquiryReasons.Should().NotBeNull();
        result.EnquiryReasons.Count.Should().Be(3);
        result.EnquiryReasons[0].Label.Should().Be("Option 1");
        result.EnquiryReasons[0].Value.Should().Be("Option 1");
        result.EnquiryReasons[1].Label.Should().Be("Option 2");
        result.EnquiryReasons[1].Value.Should().Be("Option 2");
        result.EnquiryReasons[2].Label.Should().Be("Option 3");
        result.EnquiryReasons[2].Value.Should().Be("Option 3");
        result.AdditionalInformationHeading.Should().Be("Provide further information about your enquiry");
        result.AdditionalInformationHintText.Should().Be("Provide details about the qualification you are checking for or the specific issue you are experiencing with the service.");
        result.AdditionalInformationWarningText.Should().Be("Do not include personal information, for example the name of the qualification holder");
        result.CtaButtonText.Should().Be("Send message");
        result.BackButton.Should().BeEquivalentTo(new NavigationLinkModel
                                                  {
                                                      DisplayText = "Home",
                                                      OpenInNewTab = false,
                                                      Href = "/"
                                                  });
        result.ErrorBannerHeading.Should().Be("There is a problem");
        result.InvalidEmailAddressErrorMessage.Should().Be("Enter a valid email address");
        result.NoEnquiryOptionSelectedErrorMessage.Should().Be("Select one option");
        result.AdditionalInformationErrorMessage.Should().Be("Enter further information about your enquiry");
    }
}