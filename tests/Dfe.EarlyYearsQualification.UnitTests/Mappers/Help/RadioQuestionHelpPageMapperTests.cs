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
public class RadioQuestionHelpPageMapperTests
{
    [TestMethod]
    public async Task MapRadioQuestionHelpPageContentToViewModelAsync_MapsToViewModel()
    {
        var mockContentParser = new Mock<IGovUkContentParser>();

        var postHeadingContent = "Use this form to ask a question about a qualification or report a problem with the service or the information it provides.\r\nWe aim to respond to all queries within 5 working days. Complex cases may take longer.\r\n";

        var postRadioButtonContent = "Use this form to ask a question about a qualification or report a problem with the service or the information it provides.\r\nWe aim to respond to all queries within 5 working days. Complex cases may take longer.\r\n";

        var content = new RadioQuestionHelpPage
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
            Options =
            [
                new Option
                { Label = "I need a copy of the qualification certificate or transcript", Value = nameof(HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript) },
                new Option
                { Label = "I do not know what level the qualification is", Value = nameof(HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs) },
                new Option
                { Label = "I want to check whether a course is approved before I enrol", Value = nameof(HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol) },
                new Option
                { Label = "I have a question about a qualification", Value = nameof(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification), Hint = "Some hint text"},
                new Option
                { Label = "I am experiencing an issue with the service", Value = nameof(HelpFormEnquiryReasons.GetHelp.IssueWithTheService) }
            ]
        };

        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == content.PostHeadingContent)))
                 .ReturnsAsync(postHeadingContent);

        mockContentParser.Setup(x => x.ToHtml(It.Is<Document>(d => d == content.PostRadioButtonContent)))
         .ReturnsAsync(postRadioButtonContent);

        var result = await new RadioQuestionHelpPageMapper(mockContentParser.Object).MapRadioQuestionHelpPageContentToViewModelAsync(content);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<RadioQuestionHelpPageViewModel>();

        result.Heading.Should().Be(content.Heading);
        result.PostHeadingContent.Should().Be(postHeadingContent);
        result.ReasonForEnquiryHeading.Should().Be(content.ReasonForEnquiryHeading);

        result.Options.Should().NotBeNull();
        result.Options.Count.Should().Be(5);

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
        result.PostRadioButtonContent.Should().Be(postRadioButtonContent);
    }
}