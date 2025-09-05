using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Mappers.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers.Help;

[TestClass]
public class HelpGetHelpPageMapperTests
{
    [TestMethod]
    public async Task MapGetHelpPageContentToViewModelAsync_MapsToViewModel()
    {
        var mockContentParser = new Mock<IGovUkContentParser>();

        var postHeadingContent = "Use this form to ask a question about a qualification or report a problem with the service or the information it provides.\r\nWe aim to respond to all queries within 5 working days. Complex cases may take longer.\r\n";

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).Returns(() => Task.FromResult(postHeadingContent));

        var content = new GetHelpPage
        {
            Heading = "Get help with the Check an early years qualification service",
            PostHeadingContent = ContentfulContentHelper.Paragraph(postHeadingContent),
            ReasonForEnquiryHeading = "Why are you contacting us?",
            CtaButtonText = "Continue",
            BackButton = new NavigationLink
            {
                DisplayText = "Home",
                Href = "/",
                OpenInNewTab = false
            },
            ErrorBannerHeading = "There is a problem",
            NoEnquiryOptionSelectedErrorMessage = "Select one option",
            EnquiryReasons =
            [
                new EnquiryOption
                { Label = "I have a question about a qualification", Value = nameof(HelpFormEnquiryReasons.QuestionAboutAQualification)},
                new EnquiryOption
                { Label = "I am experiencing an issue with the service", Value = nameof(HelpFormEnquiryReasons.IssueWithTheService) }
            ]
        };

        var enquiryReasons = new List<Option>
                             {
                                 new() { Label = "I have a question about a qualification", Value = nameof(HelpFormEnquiryReasons.QuestionAboutAQualification) },
                                 new() { Label = "I am experiencing an issue with the service", Value = nameof(HelpFormEnquiryReasons.IssueWithTheService) },
                             };

        var result = await new HelpGetHelpPageMapper(mockContentParser.Object).MapGetHelpPageContentToViewModelAsync(content);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<GetHelpPageViewModel>();

        result.Heading.Should().Be(content.Heading);
        result.PostHeadingContent.Should().Be(postHeadingContent);
        result.ReasonForEnquiryHeading.Should().Be(content.ReasonForEnquiryHeading);

        result.EnquiryReasons.Should().NotBeNull();
        result.EnquiryReasons.Count.Should().Be(2);
        result.EnquiryReasons.First().Label.Should().Be(enquiryReasons.First().Label);
        result.EnquiryReasons.Last().Value.Should().Be(enquiryReasons.Last().Value);
        result.EnquiryReasons.First().Label.Should().Be(enquiryReasons.First().Label);
        result.EnquiryReasons.Last().Value.Should().Be(enquiryReasons.Last().Value);

        result.CtaButtonText.Should().Be("Continue");
        result.BackButton.Should().BeEquivalentTo(new NavigationLinkModel
        {
            DisplayText = "Home",
            OpenInNewTab = false,
            Href = "/"
        });
        result.ErrorBannerHeading.Should().Be("There is a problem");
        result.NoEnquiryOptionSelectedErrorMessage.Should().Be("Select one option");

        result.ErrorBannerHeading.Should().Be(content.ErrorBannerHeading);
        result.NoEnquiryOptionSelectedErrorMessage.Should().Be(content.NoEnquiryOptionSelectedErrorMessage);
        result.CtaButtonText.Should().Be(content.CtaButtonText);
        result.ErrorBannerHeading.Should().Be(content.ErrorBannerHeading);
        result.HasNoEnquiryOptionSelectedError.Should().BeFalse();
        result.HasValidationErrors.Should().BeFalse();
    }
}