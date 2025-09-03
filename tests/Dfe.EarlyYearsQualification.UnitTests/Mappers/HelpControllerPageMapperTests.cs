using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class HelpControllerPageMapperTests
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
                { Label = "I have a question about a qualification", Value = "QuestionAboutAQualification" },
                new EnquiryOption
                { Label = "I am experiencing an issue with the service", Value = "IssueWithTheService" }
            ]
        };

        var enquiryReasons = new List<Option>
                             {
                                 new() { Label = "I have a question about a qualification", Value = "QuestionAboutAQualification" },
                                 new() { Label = "I am experiencing an issue with the service", Value = "IssueWithTheService" },
                             };

        var result = await HelpControllerPageMapper.MapGetHelpPageContentToViewModelAsync(content, mockContentParser.Object);

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

    [TestMethod]
    public void MapQualificationDetailsContentToViewModel_MapsToViewModel()
    {
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var content = new HelpQualificationDetailsPage()
        {
            Heading = "What are the qualification details?",
            PostHeadingContent = "We need to know the following qualification details to quickly and accurately respond to any questions you may have.",
            CtaButtonText = "Continue",
            BackButton = new NavigationLink
            {
                DisplayText = "Back to get help with the Check an early years qualification service",
                Href = "/help/get-help",
                OpenInNewTab = false
            },
            QualificationNameHeading = "Qualification name",
            QualificationNameErrorMessage = "Enter the qualification name",
            AwardingOrganisationHeading = "Awarding organisation",
            AwardingOrganisationErrorMessage = "Enter the awarding organisation",
            ErrorBannerHeading = "There is a problem",
            AwardedDateIsAfterStartedDateErrorText = "The awarded date must be after the started date",
            StartDateQuestion = new DateQuestion
            {
                MonthLabel = "Month",
                YearLabel = "Year",
                QuestionHeader = "Start date",
                QuestionHint = null,
                ErrorBannerLinkText = "Enter the month and year that the qualification was started",
                ErrorMessage = "Enter the month and year that the qualification was started",
                FutureDateErrorBannerLinkText = "The date the qualification was started must be in the past",
                FutureDateErrorMessage = "The date the qualification was started must be in the past",
                MissingMonthErrorMessage = "Enter the month that the qualification was started",
                MissingYearErrorMessage = "Enter the year that the qualification was started",
                MissingMonthBannerLinkText = "Enter the month that the qualification was started",
                MissingYearBannerLinkText = "Enter the year that the qualification was started",
                MonthOutOfBoundsErrorLinkText = "The month the qualification was started must be between 1 and 12",
                MonthOutOfBoundsErrorMessage = "The month the qualification was started must be between 1 and 12",
                YearOutOfBoundsErrorLinkText = "The year the qualification was started must be between 1900 and $[actual-year]$",
                YearOutOfBoundsErrorMessage = "The year the qualification was started must be between 1900 and $[actual-year]$"
            },
            AwardedDateQuestion = new DateQuestion
            {
                MonthLabel = "Month",
                YearLabel = "Year",
                QuestionHeader = "Award date",
                QuestionHint = null,
                ErrorBannerLinkText = "Enter the month and year that the qualification was awarded",
                ErrorMessage = "Enter the date the qualification was awarded",
                FutureDateErrorBannerLinkText = "The date the qualification was awarded must be in the past",
                FutureDateErrorMessage = "The date the qualification was awarded must be in the past",
                MissingMonthErrorMessage = "Enter the month that the qualification was awarded",
                MissingYearErrorMessage = "Enter the year that the qualification was awarded",
                MissingMonthBannerLinkText = "Enter the month that the qualification was awarded",
                MissingYearBannerLinkText = "Enter the year that the qualification was awarded",
                MonthOutOfBoundsErrorLinkText = "The month the qualification was awarded must be between 1 and 12",
                MonthOutOfBoundsErrorMessage = "The month the qualification was awarded must be between 1 and 12",
                YearOutOfBoundsErrorLinkText = "The year the qualification was awarded must be between 1900 and $[actual-year]$",
                YearOutOfBoundsErrorMessage = "The year the qualification was awarded must be between 1900 and $[actual-year]$"
            },

        };

        var viewModel = new QualificationDetailsPageViewModel()
        {
            QuestionModel = new DatesQuestionModel()
            {
                StartedQuestion = new DateQuestionModel()
                {
                    QuestionHeader = content.StartDateQuestion.QuestionHeader,
                    MonthLabel = content.StartDateQuestion.MonthLabel,
                    YearLabel = content.StartDateQuestion.YearLabel,
                    SelectedMonth = 1,
                    SelectedYear = 2000
                },
                AwardedQuestion = new DateQuestionModel()
                {
                    QuestionHeader = content.AwardedDateQuestion.QuestionHeader,
                    MonthLabel = content.AwardedDateQuestion.MonthLabel,
                    YearLabel = content.AwardedDateQuestion.YearLabel,
                    SelectedMonth = 1,
                    SelectedYear = 2002
                }
            }
        };

        var modelState = new ModelStateDictionary();

        var result = HelpControllerPageMapper.MapQualificationDetailsContentToViewModel(viewModel, content, null, modelState, mockPlaceholderUpdater.Object);

        result.Should().NotBeNull();

        result.AwardingOrganisationErrorMessage.Should().Be(content.AwardingOrganisationErrorMessage);
        result.AwardingOrganisationHeading.Should().Be(content.AwardingOrganisationHeading);
        result.BackButton.Should().BeEquivalentTo(new NavigationLinkModel
        {
            DisplayText = "Back to get help with the Check an early years qualification service",
            Href = "/help/get-help",
            OpenInNewTab = false
        });

        result.CtaButtonText.Should().Be("Continue");
        result.ErrorBannerHeading.Should().Be("There is a problem");
        result.Heading.Should().Be(content.Heading);
        result.PostHeadingContent.Should().Be(content.PostHeadingContent);
        result.QualificationNameErrorMessage.Should().Be(content.QualificationNameErrorMessage);
        result.QualificationNameHeading.Should().Be(content.QualificationNameHeading);
        result.HasAwardingOrganisationError.Should().BeFalse();
        result.HasQualificationNameError.Should().BeFalse();
        result.HasValidationErrors.Should().BeFalse();
        result.QuestionModel.Should().NotBeNull();

        result.QuestionModel.StartedQuestion.Should().NotBeNull();
        result.QuestionModel.StartedQuestion.QuestionHeader.Should().Be(content.StartDateQuestion.QuestionHeader);
        result.QuestionModel.StartedQuestion.QuestionHint.Should().BeNullOrEmpty();
        result.QuestionModel.StartedQuestion.MonthLabel.Should().Be(content.StartDateQuestion.MonthLabel);
        result.QuestionModel.StartedQuestion.YearLabel.Should().Be(content.StartDateQuestion.YearLabel);
        result.QuestionModel.StartedQuestion.SelectedMonth.Should().Be(1);
        result.QuestionModel.StartedQuestion.SelectedYear.Should().Be(2000);

        result.QuestionModel.AwardedQuestion.Should().NotBeNull();
        result.QuestionModel.AwardedQuestion.QuestionHeader.Should().Be(content.AwardedDateQuestion.QuestionHeader);
        result.QuestionModel.AwardedQuestion.QuestionHint.Should().BeNullOrEmpty();
        result.QuestionModel.AwardedQuestion.MonthLabel.Should().Be(content.AwardedDateQuestion.MonthLabel);
        result.QuestionModel.AwardedQuestion.YearLabel.Should().Be(content.AwardedDateQuestion.YearLabel);
        result.QuestionModel.AwardedQuestion.SelectedMonth.Should().Be(1);
        result.QuestionModel.AwardedQuestion.SelectedYear.Should().Be(2002);
    }

    [TestMethod]
    [DataRow("Question about a qualification")]
    [DataRow("Issue with the service")]
    public void MapProvideDetailsPageContentToViewModel_MapsToViewModel(string reasonForEnquiring)
    {
        var content = new HelpProvideDetailsPage()
        {
            Heading = "How can we help you?",
            PostHeadingContent = "Give as much detail as you can. This helps us give you the right support.",
            CtaButtonText = "Continue",
            BackButtonToGetHelpPage = new NavigationLink
            {
                DisplayText = "Back to get help with the Check an early years qualification service",
                Href = "/help/get-help",
                OpenInNewTab = false
            },
            BackButtonToQualificationDetailsPage = new NavigationLink
            {
                DisplayText = "Back to what are the qualification details",
                Href = "/help/qualification-details",
                OpenInNewTab = false
            },
            AdditionalInformationWarningText = "Do not include any personal information",
            AdditionalInformationErrorMessage = "Provide information about how we can help you",
            ErrorBannerHeading = "There is a problem"
        };

        var result = HelpControllerPageMapper.MapProvideDetailsPageContentToViewModel(content, reasonForEnquiring);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ProvideDetailsPageViewModel>();

        result.Heading.Should().Be(content.Heading);
        result.PostHeadingContent.Should().Be(content.PostHeadingContent);
        result.CtaButtonText.Should().Be("Continue");

        if (reasonForEnquiring == "Question about a qualification")
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

        if (reasonForEnquiring == "Issue with the service")
        {
            result.BackButton.Should().BeEquivalentTo(
                new NavigationLinkModel
                {
                    DisplayText = "Back to get help with the Check an early years qualification service",
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

        var result = HelpControllerPageMapper.MapEmailAddressPageContentToViewModel(content);

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

        var result = await HelpControllerPageMapper.MapConfirmationPageContentToViewModelAsync(content, mockContentParser.Object);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ConfirmationPageViewModel>();

        result.SuccessMessage.Should().Be(content.SuccessMessage);
        result.BodyHeading.Should().Be(content.BodyHeading);
        result.Body.Should().Be(docContent);
    }
}